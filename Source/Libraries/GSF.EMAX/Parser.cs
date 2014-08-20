﻿//******************************************************************************************************
//  Parser.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  06/17/2012 - J. Ritchie Carroll
//       Generated original version of source code.
//  12/13/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;
using System.Diagnostics;
using System.IO;
using GSF.IO;

namespace GSF.EMAX
{
    /// <summary>
    /// EMAX data file(s) parser.
    /// </summary>
    public class Parser : IDisposable
    {
        #region [ Members ]

        // Fields
        private ControlFile m_controlFile;
        private string m_fileName;
        private bool m_disposed;
        private FileStream[] m_fileStreams;
        private int m_streamIndex;
        private DateTime m_timestamp;
        private double[] m_values;
        private ushort[] m_eventGroups;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Releases the unmanaged resources before the <see cref="Parser"/> object is reclaimed by <see cref="GC"/>.
        /// </summary>
        ~Parser()
        {
            Dispose(false);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets associated EMAX control file for this <see cref="Parser"/>.
        /// </summary>
        /// <remarks>
        /// This is similar in function to a COMTRADE schema file.
        /// </remarks>
        public ControlFile ControlFile
        {
            get
            {
                return m_controlFile;
            }
            set
            {
                m_controlFile = value;

                if ((object)m_controlFile != null)
                {
                    if (m_controlFile.AnalogChannelCount > 0)
                        m_values = new double[m_controlFile.AnalogChannelCount];
                    else
                        throw new InvalidOperationException("Invalid control file: total analog channels defined in control file is zero.");

                    if (m_controlFile.EventGroupCount > 0)
                        m_eventGroups = new ushort[m_controlFile.EventGroupCount];
                    else
                        throw new InvalidOperationException("Invalid control file: total event groups defined in control file is zero.");
                }
                else
                {
                    m_values = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets EMAX data filename. If there are more than one data files in a set (e.g., RCL/RCU), this should be set to first file name in the set, e.g., DATA123.RCL.
        /// </summary>
        public string FileName
        {
            get
            {
                return m_fileName;
            }
            set
            {
                m_fileName = value;
            }
        }

        /// <summary>
        /// Gets timestamp of current record.
        /// </summary>
        public DateTime Timestamp
        {
            get
            {
                return m_timestamp;
            }
        }

        /// <summary>
        /// Gets values of current record.
        /// </summary>
        public double[] Values
        {
            get
            {
                return m_values;
            }
        }

        /// <summary>
        /// Gets event groups for current record.
        /// </summary>
        public ushort[] EventGroups
        {
            get
            {
                return m_eventGroups;
            }
        }
        #endregion

        #region [ Methods ]

        /// <summary>
        /// Releases all the resources used by the <see cref="Parser"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Parser"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    if (disposing)
                    {
                        CloseFiles();
                    }
                }
                finally
                {
                    m_disposed = true;  // Prevent duplicate dispose.
                }
            }
        }

        /// <summary>
        /// Opens all EMAX data file streams.
        /// </summary>
        public void OpenFiles()
        {
            if (string.IsNullOrWhiteSpace(m_fileName))
                throw new InvalidOperationException("Initial EMAX data file name was not specified, cannot open file(s).");

            if (!File.Exists(m_fileName))
                throw new FileNotFoundException(string.Format("Specified EMAX data file \"{0}\" was not found, cannot open file(s).", m_fileName));

            string extension = FilePath.GetExtension(m_fileName);
            string[] fileNames;

            if (extension.Equals(".RCU", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Please specify the .RCL file instead of the .RCU as the initial file - the .RCU will be automatically loaded: " + m_fileName);

            if (extension.Equals(".RCD", StringComparison.OrdinalIgnoreCase))
            {
                // RCD files mean there is only one card in the system
                fileNames = new string[1];
                fileNames[0] = m_fileName;
            }
            else if (extension.Equals(".RCL", StringComparison.OrdinalIgnoreCase))
            {
                // RCL files mean there are two cards in the system - one low (RCL) and one high (RCU)
                fileNames = new string[2];
                fileNames[0] = m_fileName;
                fileNames[1] = Path.Combine(FilePath.GetDirectoryName(m_fileName), FilePath.GetFileNameWithoutExtension(m_fileName) + ".RCU");
            }
            else
            {
                throw new InvalidOperationException("Specified file name does not have a valid EMAX extension: " + m_fileName);
            }

            // Create a new file stream for each file
            m_fileStreams = new FileStream[fileNames.Length];
            byte[] skipBytes = new byte[2];

            for (int i = 0; i < fileNames.Length; i++)
            {
                m_fileStreams[i] = new FileStream(fileNames[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                m_fileStreams[i].Read(skipBytes, 0, 2);
            }

            m_streamIndex = 0;
        }

        /// <summary>
        /// Closes all EMAX data file streams.
        /// </summary>
        public void CloseFiles()
        {
            if ((object)m_fileStreams != null)
            {
                foreach (FileStream fileStream in m_fileStreams)
                {
                    if ((object)fileStream != null)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                }
            }

            m_fileStreams = null;
        }

        /// <summary>
        /// Reads next EMAX record.
        /// </summary>
        /// <returns><c>true</c> if read succeeded; otherwise <c>false</c> if end of data set was reached.</returns>
        public bool ReadNext()
        {
            if ((object)m_fileStreams == null)
                throw new InvalidOperationException("EMAX data files are not open, cannot read next record.");

            if ((object)m_controlFile == null)
                throw new InvalidOperationException("No EMAX schema has been defined, cannot read records.");

            if (m_streamIndex > m_fileStreams.Length)
                throw new EndOfStreamException("All EMAX data has been read, cannot read more records.");

            byte[] buffer = null;

            try
            {
                FileStream currentFile = m_fileStreams[m_streamIndex];
                int recordLength = m_controlFile.FrameLength;
                double multiplier = double.Parse(m_controlFile.AnalogChannelSettings.mult_factor);
                ushort[] clockWords = new ushort[4];
                ushort value;
                int index = 0;

                buffer = BufferPool.TakeBuffer(recordLength);

                // Read next record from file
                int bytesRead = currentFile.Read(buffer, 0, recordLength);

                // See if we have reached the end of this file
                if (bytesRead == 0)
                {
                    m_streamIndex++;

                    // There is more to read if there is another file
                    return m_streamIndex < m_fileStreams.Length && ReadNext();
                }

                if (bytesRead == recordLength)
                {
#if DEBUG
                    // HACK: Attempt to find / parse record level timestamp...
                    DateTime sourceTime = ControlFile.Header.Timestamp;
                    Debug.WriteLine("Target time = {0:dd-MMM-yyyy HH:mm:ss.ffffff}", sourceTime);
                    DateTime parsedTime;
                    ushort temp;

                    // Perform full scan through each byte in buffer reading forwards and backwards
                    for (int i = 0; i < recordLength - 8; i++)
                    {
                        clockWords[0] = BigEndian.ToUInt16(buffer, i);
                        clockWords[1] = BigEndian.ToUInt16(buffer, i + 2);
                        clockWords[2] = BigEndian.ToUInt16(buffer, i + 4);
                        clockWords[3] = BigEndian.ToUInt16(buffer, i + 6);

                        parsedTime = (new Timestamp(ControlFile.Header.Timestamp, clockWords)).Value;

                        if (parsedTime.DayOfYear == sourceTime.DayOfYear && parsedTime.Year == sourceTime.Year && parsedTime.Hour == sourceTime.Hour)
                            Debug.WriteLine("Day-of-year match found @Index {0} [0x{0:X}] Clock word 0 = [0x{1:X}] => BE={2:dd-MMM-yyyy HH:mm:ss.ffffff}", i, clockWords[0], parsedTime);

                        temp = clockWords[0];
                        clockWords[0] = clockWords[3];
                        clockWords[3] = temp;
                        temp = clockWords[1];
                        clockWords[1] = clockWords[2];
                        clockWords[2] = temp;

                        parsedTime = (new Timestamp(ControlFile.Header.Timestamp, clockWords)).Value;

                        if (parsedTime.DayOfYear == sourceTime.DayOfYear && parsedTime.Year == sourceTime.Year && parsedTime.Hour == sourceTime.Hour)
                            Debug.WriteLine("Day-of-year match found (reverse words) @Index {0} [0x{0:X}] Clock word 0 = [0x{1:X}] => BE={2:dd-MMM-yyyy HH:mm:ss.ffffff}", i, clockWords[0], parsedTime);

                        clockWords[0] = LittleEndian.ToUInt16(buffer, i);
                        clockWords[1] = LittleEndian.ToUInt16(buffer, i + 2);
                        clockWords[2] = LittleEndian.ToUInt16(buffer, i + 4);
                        clockWords[3] = LittleEndian.ToUInt16(buffer, i + 6);

                        parsedTime = (new Timestamp(ControlFile.Header.Timestamp, clockWords)).Value;

                        if (parsedTime.DayOfYear == sourceTime.DayOfYear && parsedTime.Year == sourceTime.Year && parsedTime.Hour == sourceTime.Hour)
                            Debug.WriteLine("Day-of-year match found @Index {0} [0x{0:X}] Clock word 0 = [0x{1:X}] => LE={2:dd-MMM-yyyy HH:mm:ss.ffffff}", i, clockWords[0], parsedTime);

                        temp = clockWords[0];
                        clockWords[0] = clockWords[3];
                        clockWords[3] = temp;
                        temp = clockWords[1];
                        clockWords[1] = clockWords[2];
                        clockWords[2] = temp;

                        parsedTime = (new Timestamp(ControlFile.Header.Timestamp, clockWords)).Value;

                        if (parsedTime.DayOfYear == sourceTime.DayOfYear && parsedTime.Year == sourceTime.Year && parsedTime.Hour == sourceTime.Hour)
                            Debug.WriteLine("Day-of-year match found (reverse words) @Index {0} [0x{0:X}] Clock word 0 = [0x{1:X}] => LE={2:dd-MMM-yyyy HH:mm:ss.ffffff}", i, clockWords[0], parsedTime);

                        parsedTime = (new UnixTimeTag(BigEndian.ToUInt32(buffer, i))).ToDateTime();

                        if (parsedTime.DayOfYear == sourceTime.DayOfYear && parsedTime.Year == sourceTime.Year && parsedTime.Hour == sourceTime.Hour)
                            Debug.WriteLine("Day-of-year match found (Unix Time Tag) @Index {0} [0x{0:X}] Clock word 0 = [0x{1:X}] => BE={2:dd-MMM-yyyy HH:mm:ss.ffffff}", i, clockWords[0], parsedTime);

                        parsedTime = (new UnixTimeTag(LittleEndian.ToUInt32(buffer, i))).ToDateTime();

                        if (parsedTime.DayOfYear == sourceTime.DayOfYear && parsedTime.Year == sourceTime.Year && parsedTime.Hour == sourceTime.Hour)
                            Debug.WriteLine("Day-of-year match found (Unix Time Tag) @Index {0} [0x{0:X}] Clock word 0 = [0x{1:X}] => LE={2:dd-MMM-yyyy HH:mm:ss.ffffff}", i, clockWords[0], parsedTime);

                    }
#endif
                    // Parse all analog record values
                    for (int i = 0; i < m_values.Length; i++)
                    {
                        // Read next value
                        value = LittleEndian.ToUInt16(buffer, index);

                        if (ControlFile.DataSize == DataSize.Bits12)
                            value >>= 4;

                        // TODO: Determine proper scalar
                        m_values[i] = value;  // * multiplier;
                        index += 2;
                    }

                    // Read event group values (first set)
                    for (int i = 0; i < 4; i++)
                    {
                        m_eventGroups[i] = LittleEndian.ToUInt16(buffer, index);
                        index += 2;
                    }

                    // Read timestamp from next four word values
                    for (int i = 0; i < 4; i++)
                    {
                        clockWords[i] = LittleEndian.ToUInt16(buffer, index);
                        index += 2;
                    }

                    m_timestamp = (new Timestamp(ControlFile.Header.Timestamp, clockWords)).Value;

                    if (ControlFile.ConfiguredAnalogChannels > 32 && ControlFile.SystemSettings.samples_per_second <= 5760)
                    {
                        // Read next set of event group values
                        for (int i = 4; i < 8; i++)
                        {
                            m_eventGroups[i] = LittleEndian.ToUInt16(buffer, index);
                            index += 2;
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("Failed to read enough bytes from EMAX file for a record as defined by control file - possible control file/data file mismatch or file corruption.");
                }
            }
            finally
            {
                if ((object)buffer != null)
                    BufferPool.ReturnBuffer(buffer);
            }

            return true;
        }

        #endregion
    }
}