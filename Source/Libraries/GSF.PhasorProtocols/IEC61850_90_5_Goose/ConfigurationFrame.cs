﻿//******************************************************************************************************
//  ConfigurationFrame.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  04/19/2012 - J. Ritchie Carroll
//       Generated original version of source code.
//  12/17/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using GSF.IO.Checksums;
using GSF.Parsing;
using System.IO;
using System.Reflection;

namespace GSF.PhasorProtocols.IEC61850_90_5_Goose
{
    /// <summary>
    /// Represents the IEC 61850-90-5 implementation of a <see cref="IConfigurationFrame"/> that can be sent or received.
    /// </summary>
    [Serializable]
    public class ConfigurationFrame : ConfigurationFrameBase, ISupportSourceIdentifiableFrameImage<SourceChannel, FrameType>
    {
        #region [ Members ]

        // Constants
        private const int FixedHeaderLength = CommonFrameHeader.FixedLength + 6;

        // Fields
        private CommonFrameHeader m_frameHeader;
        private uint m_timebase;
        private int m_calculatedSampleLength;
        private int m_calculatedGooseLength;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="ConfigurationFrame"/>.
        /// </summary>
        /// <remarks>
        /// This constructor is used by <see cref="FrameImageParserBase{TTypeIdentifier,TOutputType}"/> to parse an IEC 61850-90-5 configuration frame.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ConfigurationFrame()
            : base(0, new ConfigurationCellCollection(), 0, 0)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ConfigurationFrame"/> from specified parameters.
        /// </summary>
        /// <param name="timebase">Timebase to use for fraction second resolution.</param>
        /// <param name="idCode">The ID code of this <see cref="ConfigurationFrame"/>.</param>
        /// <param name="timestamp">The exact timestamp, in <see cref="Ticks"/>, of the data represented by this <see cref="ConfigurationFrame"/>.</param>
        /// <param name="frameRate">The defined frame rate of this <see cref="ConfigurationFrame"/>.</param>
        /// <remarks>
        /// This constructor is used by a consumer to generate an IEC 61850-90-5 configuration frame.
        /// </remarks>
        public ConfigurationFrame(uint timebase, ushort idCode, Ticks timestamp, ushort frameRate)
            : base(idCode, new ConfigurationCellCollection(), timestamp, frameRate)
        {
            this.Timebase = timebase;
        }

        /// <summary>
        /// Creates a new <see cref="ConfigurationFrame"/> from serialization parameters.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> with populated with data.</param>
        /// <param name="context">The source <see cref="StreamingContext"/> for this deserialization.</param>
        protected ConfigurationFrame(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Deserialize configuration frame
            m_frameHeader = (CommonFrameHeader)info.GetValue("frameHeader", typeof(CommonFrameHeader));
            m_timebase = info.GetUInt32("timebase");
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets reference to the <see cref="ConfigurationCellCollection"/> for this <see cref="ConfigurationFrame"/>.
        /// </summary>
        public new ConfigurationCellCollection Cells
        {
            get
            {
                return base.Cells as ConfigurationCellCollection;
            }
        }

        /// <summary>
        /// Gets the <see cref="IEC61850_90_5_Goose.DraftRevision"/> of this <see cref="ConfigurationFrame"/>.
        /// </summary>
        public virtual DraftRevision DraftRevision
        {
            get
            {
                return DraftRevision.Draft7;
            }
        }

        /// <summary>
        /// Gets the <see cref="FrameType"/> of this <see cref="ConfigurationFrame"/>.
        /// </summary>
        public virtual FrameType TypeID
        {
            get
            {
                return IEC61850_90_5_Goose.FrameType.ConfigurationFrame;
            }
        }

        /// <summary>
        /// Gets or sets exact timestamp, in ticks, of the data represented by this <see cref="ConfigurationFrame"/>.
        /// </summary>
        /// <remarks>
        /// The value of this property represents the number of 100-nanosecond intervals that have elapsed since 12:00:00 midnight, January 1, 0001.
        /// </remarks>
        public override Ticks Timestamp
        {
            get
            {
                return CommonHeader.Timestamp;
            }
            set
            {
                // Keep timestamp updates synchrnonized...
                CommonHeader.Timestamp = value;
                base.Timestamp = value;
            }
        }

        /// <summary>
        /// Gets or sets current <see cref="CommonFrameHeader"/>.
        /// </summary>
        public CommonFrameHeader CommonHeader
        {
            get
            {
                // Make sure frame header exists
                if (m_frameHeader == null)
                    m_frameHeader = new CommonFrameHeader(this, TypeID, base.IDCode, base.Timestamp);

                return m_frameHeader;
            }
            set
            {
                m_frameHeader = value;

                if (m_frameHeader != null)
                {
                    State = m_frameHeader.State as IConfigurationFrameParsingState;
                    base.IDCode = m_frameHeader.IDCode;
                    base.Timestamp = m_frameHeader.Timestamp;
                    m_timebase = m_frameHeader.Timebase;
                }
            }
        }

        // This interface implementation satisfies ISupportFrameImage<FrameType>.CommonHeader
        ICommonHeader<FrameType> ISupportFrameImage<FrameType>.CommonHeader
        {
            get
            {
                return CommonHeader;
            }
            set
            {
                CommonHeader = value as CommonFrameHeader;
            }
        }

        /// <summary>
        /// Gets or sets the IEC 61850-90-5 protocol version of this <see cref="ConfigurationFrame"/>.
        /// </summary>
        public byte Version
        {
            get
            {
                return CommonHeader.Version;
            }
            set
            {
                CommonHeader.Version = value;
            }
        }

        /// <summary>
        /// Gets or sets the IEC 61850-90-5 resolution of fractional time stamps of this <see cref="ConfigurationFrame"/>.
        /// </summary>
        public uint Timebase
        {
            get
            {
                return m_timebase;
            }
            set
            {
                m_timebase = value;
                CommonHeader.Timebase = m_timebase;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="TimeQualityFlags"/> of this <see cref="ConfigurationFrame"/>.
        /// </summary>
        public TimeQualityFlags TimeQualityFlags
        {
            get
            {
                return CommonHeader.TimeQualityFlags;
            }
            set
            {
                CommonHeader.TimeQualityFlags = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="TimeQualityIndicatorCode"/> of this <see cref="ConfigurationFrame"/>.
        /// </summary>
        public TimeQualityIndicatorCode TimeQualityIndicatorCode
        {
            get
            {
                return CommonHeader.TimeQualityIndicatorCode;
            }
            set
            {
                CommonHeader.TimeQualityIndicatorCode = value;
            }
        }

        /// <summary>
        /// Gets the length of the <see cref="HeaderImage"/>.
        /// </summary>
        protected override int HeaderLength
        {
            get
            {
                return FixedHeaderLength;
            }
        }

        /// <summary>
        /// Gets the binary header image of the <see cref="ConfigurationFrame"/> object.
        /// </summary>
        protected override byte[] HeaderImage
        {
            get
            {
                // Make sure to provide proper frame length for use in the common header image
                unchecked
                {
                    CommonHeader.FrameLength = (ushort)BinaryLength;
                }

                byte[] buffer = new byte[FixedHeaderLength];
                int index = 0;

                CommonHeader.BinaryImage.CopyImage(buffer, ref index, CommonFrameHeader.FixedLength);
                BigEndian.CopyBytes(m_timebase, buffer, index);
                BigEndian.CopyBytes((ushort)Cells.Count, buffer, index + 4);

                return buffer;
            }
        }

        /// <summary>
        /// Gets the length of the <see cref="FooterImage"/>.
        /// </summary>
        protected override int FooterLength
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Gets the binary footer image of the <see cref="ConfigurationFrame"/> object.
        /// </summary>
        protected override byte[] FooterImage
        {
            get
            {
                byte[] buffer = new byte[2];

                BigEndian.CopyBytes(FrameRate, buffer, 0);

                return buffer;
            }
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}"/> of string based property names and values for the <see cref="ConfigurationFrame"/> object.
        /// </summary>
        public override Dictionary<string, string> Attributes
        {
            get
            {
                Dictionary<string, string> baseAttributes = base.Attributes;

                CommonHeader.AppendHeaderAttributes(baseAttributes);
                baseAttributes.Add("Draft Revision", (int)DraftRevision + ": " + DraftRevision);

                return baseAttributes;
            }
        }

        #endregion

        #region [ Methods ]

        public String PrintProperties(object obj)
        {
            String result = "";
            return PrintProperties(obj, result, 0);
        }
        public String PrintProperties(object obj, String result, int indent)
        {
            if (obj == null) return result;
            string indentString = new string(' ', indent);
            Type objType = obj.GetType();
            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object propValue = property.GetValue(obj, null);
                if (property.PropertyType.Assembly == objType.Assembly && !property.PropertyType.IsEnum)
                {
                    result += String.Format("{0}{1}:\n", indentString, property.Name);
//                    PrintProperties(propValue, result, indent + 2);
                    PrintProperties(property.Name, result, indent + 2);
                }
                else
                {
                    result += String.Format("{0}{1}: {2}\n", indentString, property.Name, propValue);
                }
            }
            return result;
        }

        private String Blah (object obj)
        {
            String result = "Hello\n";
            Type fieldsType = obj.GetType();

            // Get an array of FieldInfo objects.
            FieldInfo[] fields = fieldsType.GetFields(BindingFlags.Public
                | BindingFlags.Instance);
            // Display the values of the fields.
            //Console.WriteLine("Displaying the values of the fields of {0}:", fieldsType);
            for (int i = 0; i < fields.Length; i++)
            {
                result += String.Format("{0}: {1}\n", fields[i].Name, fields[i].GetValue(obj));

//                Console.WriteLine("   {0}:\t'{1}'",                   fields[i].Name, fields[i].GetValue(fieldsInst));
            }
            return result;
        }

        /// <summary>
        /// Gets calculated length of data samples based on known configuration.
        /// </summary>
        /// <returns>Calculated length of data samples.</returns>
        public int GetCalculatedSampleLength()
        {
            if (m_calculatedSampleLength == 0)
            {
                m_calculatedSampleLength = Cells.Sum(cell => 10 + cell.PhasorDefinitions.Count * 8 + cell.AnalogDefinitions.Count * 4 + cell.DigitalDefinitions.Count * 2);
            }

            return m_calculatedSampleLength;
        }

        public int GetCalculatedGooseLength()
        {
            if (m_calculatedGooseLength == 0)
            {
                foreach (ConfigurationCell cell in Cells)
                {
                    try
                    {
                        String result = String.Format("cell.IDCode {0}, cell.IDLabel {1}, cell.PhasorDefinitions.Count {2}, cell.AnalogDefinitions.Count {3}, cell.DigitalDefinitions.Count {4}\n", cell.IDCode, cell.IDLabel, cell.PhasorDefinitions.Count, cell.AnalogDefinitions.Count, cell.DigitalDefinitions.Count);
                        Common.Dump(result);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(String.Format("{0}:{1}:{2}\n", ex.Message, ex.StackTrace, ex.Source));
                    }
                }
                //File.AppendAllText("jeff.txt", String.Format("cell.PhasorDefinitions.Count {0} * 8, cell.AnalogDefinitions.Count {1} * 4, cell.DigitalDefinitions.Count {2} * 2"),
                //cell.PhasorDefinitions.Count,cell.AnalogDefinitions.Count, Cell.cell.DigitalDefinitions.Count);                
                // 1 byte for STAT
                // 5 bytes for FREQ
                // 5 bytes if FrequencyDefinition exists

                m_calculatedGooseLength = Cells.Sum(cell => 1 + 5 + (cell.FrequencyDefinition == null ? 0 : 5)
                    + ( cell.PhasorDefinitions.Count * 8 ) 
                    + ( cell.AnalogDefinitions.Count * 4 ) 
                    + ( cell.DigitalDefinitions.Count * 2));
            }

            Common.Dump(String.Format("GetCalculatedGooseLength() -> {0}, Freq {1}, Phasor {2}, Analog {3}, Digital {4}", 
                m_calculatedGooseLength, Cells.Sum(cell => cell.FrequencyDefinition == null ? 0 : 1), Cells.Sum( cell => cell.PhasorDefinitions.Count), Cells.Sum(cell => cell.AnalogDefinitions.Count), Cells.Sum(cell => cell.DigitalDefinitions.Count)));

            return m_calculatedGooseLength;
        }

        /// <summary>
        /// Parses the binary header image.
        /// </summary>
        /// <param name="buffer">Binary image to parse.</param>
        /// <param name="startIndex">Start index into <paramref name="buffer"/> to begin parsing.</param>
        /// <param name="length">Length of valid data within <paramref name="buffer"/>.</param>
        /// <returns>The length of the data that was parsed.</returns>
        protected override int ParseHeaderImage(byte[] buffer, int startIndex, int length)
        {
            // Skip past header that was already parsed...
            startIndex += CommonFrameHeader.FixedLength;

            m_timebase = BigEndian.ToUInt32(buffer, startIndex);
            CommonHeader.Timebase = m_timebase;
            State.CellCount = BigEndian.ToUInt16(buffer, startIndex + 4);

            return FixedHeaderLength;
        }

        /// <summary>
        /// Parses the binary footer image.
        /// </summary>
        /// <param name="buffer">Binary image to parse.</param>
        /// <param name="startIndex">Start index into <paramref name="buffer"/> to begin parsing.</param>
        /// <param name="length">Length of valid data within <paramref name="buffer"/>.</param>
        /// <returns>The length of the data that was parsed.</returns>
        protected override int ParseFooterImage(byte[] buffer, int startIndex, int length)
        {
            FrameRate = BigEndian.ToUInt16(buffer, startIndex);
            return 2;
        }

        /// <summary>
        /// Calculates checksum of given <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer image over which to calculate checksum.</param>
        /// <param name="offset">Start index into <paramref name="buffer"/> to calculate checksum.</param>
        /// <param name="length">Length of data within <paramref name="buffer"/> to calculate checksum.</param>
        /// <returns>Checksum over specified portion of <paramref name="buffer"/>.</returns>
        protected override ushort CalculateChecksum(byte[] buffer, int offset, int length)
        {
            // IEC 61850-90-5 uses CRC-CCITT to calculate checksum for frames
            return buffer.CrcCCITTChecksum(offset, length);
        }

        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination <see cref="StreamingContext"/> for this serialization.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            // Serialize configuration frame
            info.AddValue("frameHeader", m_frameHeader, typeof(CommonFrameHeader));
            info.AddValue("timebase", m_timebase);
        }

        #endregion
    }
}