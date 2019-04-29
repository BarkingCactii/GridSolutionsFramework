﻿//******************************************************************************************************
//  DataFrame.cs - Gbtc
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
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Xml;
using GSF.IO;
using GSF.Parsing;
using GSF.Units.EE;

namespace GSF.PhasorProtocols.IEC61850_90_5
{
    /// <summary>
    /// Represents the IEC 61850-90-5 implementation of a <see cref="IDataFrame"/> that can be sent or received.
    /// </summary>
    [Serializable]
    public class DataFrame : DataFrameBase, ISupportSourceIdentifiableFrameImage<SourceChannel, FrameType>
    {
        #region [ Members ]

        // Nested Types

        // Defines possible signals types for an ETR file
        private enum SignalType
        {
            // Current phase magnitude.
            IPHM = 1,
            // Current phase angle.
            IPHA = 2,
            // Voltage phase magnitude.
            VPHM = 3,
            // Voltage phase angle.
            VPHA = 4,
            // Frequency.
            FREQ = 5,
            // Frequency delta (dF/dt).
            DFDT = 6,
            // Analog value.
            ALOG = 7,
            // Status flags.
            FLAG = 8,
            // Digital value.
            DIGI = 9,
            // Undefined signal.
            NONE = -1
        }

        // Fields
        private CommonFrameHeader m_frameHeader;
        private uint m_qualityFlags;
        private string m_msvID;
        private readonly int m_asduCount;
        private readonly byte[][] m_asduImages;
        private byte[] m_binaryImage;
        private ushort m_sampleCount;
        private uint m_configurationRevision;
        private byte m_sampleSynchronization;
        private ushort m_sampleRate;
        private ushort m_idCode;
        private string m_stationName;
        private ConfigurationFrame m_configurationFrame;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="DataFrame"/>.
        /// </summary>
        /// <remarks>
        /// This constructor is used by <see cref="FrameImageParserBase{TTypeIdentifier,TOutputType}"/> to parse an IEC 61850-90-5 data frame.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DataFrame()
            : base(new DataCellCollection(), 0, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="DataFrame"/> from specified parameters.
        /// </summary>
        /// <param name="timestamp">The exact timestamp, in <see cref="Ticks"/>, of the data represented by this <see cref="DataFrame"/>.</param>
        /// <param name="configurationFrame">The <see cref="ConfigurationFrame"/> associated with this <see cref="DataFrame"/>.</param>
        /// <param name="msvID">MSVID to use for <see cref="DataFrame"/>.</param>
        /// <param name="asduCount">ASDU count.</param>
        /// <param name="asduImages">Concentrator's ASDU image cache.</param>
        /// <param name="configurationRevision">Configuration revision.</param>
        /// <remarks>
        /// This constructor is used by a consumer to generate an IEC 61850-90-5 data frame.
        /// </remarks>
        public DataFrame(Ticks timestamp, ConfigurationFrame configurationFrame, string msvID, int asduCount, byte[][] asduImages, uint configurationRevision)
            : base(new DataCellCollection(), timestamp, configurationFrame)
        {
            m_msvID = msvID;
            m_asduCount = asduCount;
            m_asduImages = asduImages;
            m_configurationRevision = configurationRevision;
        }

        /// <summary>
        /// Creates a new <see cref="DataFrame"/> from serialization parameters.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> with populated with data.</param>
        /// <param name="context">The source <see cref="StreamingContext"/> for this deserialization.</param>
        protected DataFrame(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Deserialize data frame
            m_frameHeader = (CommonFrameHeader)info.GetValue("frameHeader", typeof(CommonFrameHeader));
            m_msvID = info.GetString("msvID");
            m_sampleCount = info.GetUInt16("sampleCount");
            m_configurationRevision = info.GetUInt32("configurationRevision");
            SampleSynchronization = info.GetByte("sampleSynchronization");
            m_sampleRate = info.GetUInt16("sampleRate");
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets reference to the <see cref="DataCellCollection"/> for this <see cref="DataFrame"/>.
        /// </summary>
        public new DataCellCollection Cells
        {
            get
            {
                return base.Cells as DataCellCollection;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ConfigurationFrame"/> associated with this <see cref="DataFrame"/>.
        /// </summary>
        public new ConfigurationFrame ConfigurationFrame
        {
            get
            {
                return base.ConfigurationFrame as ConfigurationFrame;
            }
            set
            {
                base.ConfigurationFrame = value;
            }
        }

        /// <summary>
        /// Gets or sets exact timestamp, in ticks, of the data represented by this <see cref="DataFrame"/>.
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
        /// Gets the identifier that is used to identify the IEC 61850-90-5 frame.
        /// </summary>
        public FrameType TypeID
        {
            get
            {
                return IEC61850_90_5.FrameType.DataFrame;
            }
        }

        /// <summary>
        /// Gets or sets IEC 61850-90-5 sample synchronization state.
        /// </summary>
        public byte SampleSynchronization
        {
            get
            {
                return m_sampleSynchronization;
            }
            set
            {
                m_sampleSynchronization = value;
            }
        }

        /// <summary>
        /// Gets or sets sample count for the <see cref="DataFrame"/>.
        /// </summary>
        public ushort SampleCount
        {
            get
            {
                return m_sampleCount;
            }
            set
            {
                m_sampleCount = value;
            }
        }

        /// <summary>
        /// Gets or sets current <see cref="CommonFrameHeader"/>.
        /// </summary>
        public CommonFrameHeader CommonHeader
        {
            get
            {
                // Make sure frame header exists - using base class timestamp to
                // prevent recursion (m_frameHeader doesn't exist yet)
                if ((object)m_frameHeader == null)
                {
                    m_frameHeader = new CommonFrameHeader(ConfigurationFrame, TypeID, base.IDCode, base.Timestamp, m_msvID, m_asduCount, m_configurationRevision)
                    {
                        ConfigurationFrame = ConfigurationFrame
                    };
                }

                return m_frameHeader;
            }
            set
            {
                m_frameHeader = value;

                if (m_frameHeader != null)
                {
                    State = m_frameHeader.State as IDataFrameParsingState;
                    base.Timestamp = m_frameHeader.Timestamp;
                    m_qualityFlags = ((uint)m_frameHeader.TimeQualityFlags | (uint)m_frameHeader.TimeQualityIndicatorCode);

                    // Reference header MSVID in data frame if it's defined
                    if ((object)m_frameHeader.MsvID != null)
                        m_msvID = m_frameHeader.MsvID;
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
        /// Gets or sets protocol specific quality flags for this <see cref="DataFrame"/>.
        /// </summary>
        public override uint QualityFlags
        {
            get
            {
                return m_qualityFlags;
            }
            set
            {
                m_qualityFlags = value;

                // Set time quality flags
                TimeQualityFlags = (TimeQualityFlags)(m_qualityFlags & ~(uint)TimeQualityFlags.TimeQualityIndicatorCodeMask);

                // Set time quality indicator code
                TimeQualityIndicatorCode = (TimeQualityIndicatorCode)(m_qualityFlags & (uint)TimeQualityFlags.TimeQualityIndicatorCodeMask);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="TimeQualityFlags"/> associated with this <see cref="DataFrame"/>.
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
        /// Gets or sets the <see cref="TimeQualityIndicatorCode"/> associated with this <see cref="DataFrame"/>.
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
        /// Gets the length of the header image.
        /// </summary>
        protected override int HeaderLength
        {
            get
            {
                return CommonHeader.Length;
            }
        }

        /// <summary>
        /// Gets the binary header image of the <see cref="DataFrame"/> object.
        /// </summary>
        protected override byte[] HeaderImage
        {
            get
            {
                return CommonHeader.BinaryImage;
            }
        }

        /// <summary>
        /// Gets the length of the <see cref="BinaryImage"/>.
        /// </summary>
        public override int BinaryLength
        {
            get
            {
                if ((object)m_binaryImage == null)
                {
                    // If parsing instead of publishing, we just return parsed frame length
                    if ((object)m_asduImages == null)
                        return CommonHeader.FrameLength;

                    m_binaryImage = GenerateBinaryImage();
                }

                if ((object)m_binaryImage != null)
                    return m_binaryImage.Length;

                return 0;
            }
        }

        /// <summary>
        /// Gets the binary image of this <see cref="DataFrame"/> object.
        /// </summary>
        public override byte[] BinaryImage
        {
            get
            {
                return m_binaryImage;
            }
        }

        /// <summary>
        /// Gets the length of the <see cref="FooterImage"/>.
        /// </summary>
        protected override int FooterLength
        {
            get
            {
                if (CommonHeader.SecurityAlgorithm != SecurityAlgorithm.None)
                    return 64;

                return 0;
            }
        }

        /// <summary>
        /// Gets the binary footer image of the <see cref="DataFrame"/> object.
        /// </summary>
        protected override byte[] FooterImage
        {
            get
            {
                int length = FooterLength;

                if (length > 0)
                {
                    SignatureAlgorithm algorithm = CommonHeader.SignatureAlgorithm;
                    byte[] buffer = new byte[length];

                    // Set signature tag
                    buffer[0] = 0x85;

                    // KeyID in common header is technically a lookup into derived rotating keys, but all implementations are using dummy key for now
                    HMAC hmac = (byte)algorithm <= (byte)SignatureAlgorithm.Sha256 ? (HMAC)(new CommonFrameHeader.ShaHmac(Common.DummyKey)) : (HMAC)(new CommonFrameHeader.AesHmac(Common.DummyKey));

                    switch (algorithm)
                    {
                        case SignatureAlgorithm.None:
                            break;
                        case SignatureAlgorithm.Aes64:
                            Buffer.BlockCopy(hmac.ComputeHash(BodyImage, 0, BodyLength).BlockCopy(0, 8), 0, buffer, 1, 8);
                            break;
                        case SignatureAlgorithm.Sha80:
                            Buffer.BlockCopy(hmac.ComputeHash(BodyImage, 0, BodyLength).BlockCopy(0, 10), 0, buffer, 1, 10);
                            break;
                        case SignatureAlgorithm.Sha128:
                        case SignatureAlgorithm.Aes128:
                            Buffer.BlockCopy(hmac.ComputeHash(BodyImage, 0, BodyLength).BlockCopy(0, 16), 0, buffer, 1, 16);
                            break;
                        case SignatureAlgorithm.Sha256:
                            Buffer.BlockCopy(hmac.ComputeHash(BodyImage, 0, BodyLength).BlockCopy(0, 32), 0, buffer, 1, 32);
                            break;
                        default:
                            throw new NotSupportedException(string.Format("IEC 61850-90-5 signature algorithm \"{0}\" is not currently supported: ", algorithm));
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}"/> of string based property names and values for the <see cref="DataFrame"/> object.
        /// </summary>
        public override Dictionary<string, string> Attributes
        {
            get
            {
                Dictionary<string, string> baseAttributes = base.Attributes;

                CommonHeader.AppendHeaderAttributes(baseAttributes);
                baseAttributes.Add("MSVID", m_msvID.ToNonNullString());
                baseAttributes.Add("Sample Count", m_sampleCount.ToString());
                baseAttributes.Add("Configuration Revision", m_configurationRevision.ToString());
                baseAttributes.Add("Sample Synchronization", SampleSynchronization + ": " + (SampleSynchronization == 0 ? "Not Synchronized" : "Synchronized"));
                baseAttributes.Add("Sample Rate", m_sampleRate.ToString());

                return baseAttributes;
            }
        }

        #endregion

        #region [ Methods ]

        // Does the work of generating binary image - this is a separate function since
        // the binary length cannot be calculated in advance due variable tag lengths
        private byte[] GenerateBinaryImage()
        {
            CommonFrameHeader header = CommonHeader;
            ConfigurationFrame configurationFrame = ConfigurationFrame;
            byte[] asduImage = null;
            int index = 0;

            // Get a buffer with extra room for variable length tags to hold new image 
            asduImage = new byte[100 + configurationFrame.GetCalculatedSampleLength() + m_msvID.Length];

            //
            // Generate current ASDU image
            //

            // Encode place holder for ASDU sequence tag
            ushort asduSequence = (ushort)(header.AsduLength - 8);
            asduSequence.EncodeTagLength(SampledValueTag.AsduSequence, asduImage, ref index);

            // Encode MSVID value
            m_msvID.EncodeTagValue(SampledValueTag.MsvID, asduImage, ref index);

            // Encode sample count
            m_sampleCount.EncodeTagValue(SampledValueTag.SmpCnt, asduImage, ref index);

            // Encode configuration revision
            m_configurationRevision.EncodeTagValue(SampledValueTag.ConfRev, asduImage, ref index);

            // Encode timestamp
            ulong timestamp = Word.MakeQuadWord(header.SecondOfCentury, (uint)(((uint)header.FractionOfSecond << 8) | (uint)header.TimeQualityFlags));
            timestamp.EncodeTagValue(SampledValueTag.RefrTm, asduImage, ref index);

            // Defaulting sample synchronization state to true - not sure what value this has
            m_sampleSynchronization = 0x01;
            m_sampleSynchronization.EncodeTagValue(SampledValueTag.SmpSynch, asduImage, ref index);

            // Can optionally encode sample rate here - seems like a waste of space so we skip this
            //m_sampleRate.EncodeTagValue(SampledValueTag.SmpRate, bodyImage, ref index);

            // Encode sample length
            ushort sampleLength = (ushort)configurationFrame.GetCalculatedSampleLength();
            sampleLength.EncodeTagLength(SampledValueTag.Samples, asduImage, ref index);

            // Copy in base image
            Cells.BinaryImage().CopyImage(asduImage, ref index, sampleLength);

            // Update actual ASDU length in header for proper tag size encoding
            header.AsduLength = (ushort)index;

            // Encode actual ASDU sequence length - wireshark uses this value to locate ASDU data
            index = 0;
            asduSequence = (ushort)(header.AsduLength - 4);
            asduSequence.EncodeTagLength(SampledValueTag.AsduSequence, asduImage, ref index);

            // Next two tags, sample mod and UTC timestamp, are optional so we skip them to conserve bandwidth

            //
            // Manage ASDU image queue
            //

            // Cascade old ASDU images up one in buffer (newest last)
            for (int i = 0; i < m_asduCount - 1; i++)
            {
                m_asduImages[i] = m_asduImages[i + 1];
            }

            // Cache current ASDU image into image cache
            m_asduImages[m_asduCount - 1] = asduImage.BlockCopy(0, header.AsduLength);

            // Make sure ASDU image buffers are initialized (first time through they will all be null)
            for (int i = 0; i < m_asduCount - 1; i++)
            {
                if ((object)m_asduImages[i] == null)
                    m_asduImages[i] = m_asduImages[m_asduCount - 1];
            }

            //
            // Create combined ASDU primary buffer image
            //

            // Reset index for use in primary buffer
            index = 0;

            // Generate header image
            byte[] headerImage = header.BinaryImage;

            // Create a buffer large enough to hold combined ASDU images plus header
            byte[] buffer = new byte[headerImage.Length + m_asduImages.Sum(image => image.Length)];

            // Copy in common header
            headerImage.CopyImage(buffer, ref index, header.Length);

            // Publish each ASDU image in outgoing frame (e.g, DataCell(t-2), DataCell(t-1), DataCell(t))
            for (int i = 0; i < m_asduCount; i++)
            {
                m_asduImages[i].CopyImage(buffer, ref index, m_asduImages[i].Length);
            }

            return buffer;
        }

        /// <summary>
        /// Parses the binary image.
        /// </summary>
        /// <param name="buffer">Binary image to parse.</param>
        /// <param name="startIndex">Start index into <paramref name="buffer"/> to begin parsing.</param>
        /// <param name="length">Length of valid data within <paramref name="buffer"/>.</param>
        /// <returns>The length of the data that was parsed.</returns>
        public override int ParseBinaryImage(byte[] buffer, int startIndex, int length)
        {
            // Overrides base class behavior, ASDUs can generally be parsed even without configuration.
            /*
            while (startIndex < buffer.Length)
            {
                if (buffer[startIndex] == 0x01 && buffer[startIndex + 1] == Common.CltpTag)
                    break;

                startIndex++;
                length--;
            }
            */
            buffer.ValidateParameters(startIndex, length);
            CommonFrameHeader header = CommonHeader;
            IDataFrameParsingState state = State;
            IConfigurationFrame configurationFrame = state.ConfigurationFrame;

            // Make sure configuration frame gets assigned before parsing begins, if available...
            if (configurationFrame != null)
                ConfigurationFrame = configurationFrame as ConfigurationFrame;

            //int tagLength, index = startIndex;

            // Header has already been parsed, skip past it
            //index += header.Length;

            // Get reference to configuration frame, if available
            m_configurationFrame = ConfigurationFrame;

            // Check session type
            if (header.SessionID == SessionType.SampledValues)
            {
                ParseSampledValues(buffer, header, startIndex, length);
            }
            else if (header.SessionID == SessionType.Goose)
            {
                ParseGoose(buffer, header, startIndex, length);
            }
            // We're not parsing any of the items remaining in the footer...
            return header.FrameLength;
        }

        private void ParseSampledValues(byte[] buffer, CommonFrameHeader header, int startIndex, int length)
        {
            int tagLength;
            int index = startIndex + header.Length;

            // Parse each ASDU in incoming frame (e.g, DataCell(t-2), DataCell(t-1), DataCell(t))
            for (int i = 0; i < header.AsduCount; i++)
            {
                // Handle redundant ASDU - last one parsed will be newest and exposed normally
                if (i > 0)
                {
                    if (header.ParseRedundantASDUs)
                    {
                        // Create a new data frame to hold redundant ASDU data
                        DataFrame dataFrame = new DataFrame
                        {
                            ConfigurationFrame = m_configurationFrame,
                            CommonHeader = header
                        };

                        // Add a copy of the current data cell to the new frame 
                        foreach (IDataCell cell in Cells)
                        {
                            dataFrame.Cells.Add(cell);
                        }

                        // Publish new data frame with redundant ASDU data
                        header.PublishFrame(dataFrame);
                    }

                    // Clear any existing ASDU values from this data frame
                    Cells.Clear();
                }

                // Validate ASDU sequence tag exists and skip past it
                buffer.ValidateTag(SampledValueTag.AsduSequence, ref index);

                // Parse MSVID value
                m_msvID = buffer.ParseStringTag(SampledValueTag.MsvID, ref index);

                // If formatted according to implementation agreement, MSVID value will contain an ID code and station name
                if (!string.IsNullOrWhiteSpace(m_msvID))
                {
                    int underscoreIndex = m_msvID.IndexOf("_");

                    if (underscoreIndex > 0)
                    {
                        if (!ushort.TryParse(m_msvID.Substring(0, underscoreIndex), out m_idCode))
                        {
                            m_idCode = 1;
                            m_stationName = m_msvID;
                        }
                        else
                        {
                            m_stationName = m_msvID.Substring(underscoreIndex + 1);
                        }
                    }
                    else
                    {
                        m_idCode = 1;
                        m_stationName = m_msvID;
                    }
                }
                else
                {
                    m_idCode = 1;
                    m_stationName = "IEC61850Dataset";
                }

                //// Dataset name has been removed as per implementation agreement
                //// Parse dataset name
                //m_dataSet = buffer.ParseStringTag(SampledValueTag.Dataset, ref index);

                // Parse sample count
                m_sampleCount = buffer.ParseUInt16Tag(SampledValueTag.SmpCnt, ref index);

                // Parse configuration revision
                m_configurationRevision = buffer.ParseUInt32Tag(SampledValueTag.ConfRev, ref index);

                // Parse refresh time
                if ((SampledValueTag)buffer[index] != SampledValueTag.RefrTm)
                    throw new InvalidOperationException("Encountered out-of-sequence or unknown sampled value tag: 0x" + buffer[startIndex].ToString("X").PadLeft(2, '0'));

                index++;
                tagLength = buffer.ParseTagLength(ref index);

                if (tagLength < 8)
                    throw new InvalidOperationException(string.Format("Unexpected length for \"{0}\" tag: {1}", SampledValueTag.RefrTm, tagLength));

                uint secondOfCentury = BigEndian.ToUInt32(buffer, index);
                UInt24 fractionOfSecond = BigEndian.ToUInt24(buffer, index + 4);
                index += 7;

                // Get whole seconds of timestamp
                long timestamp = (new UnixTimeTag((decimal)secondOfCentury)).ToDateTime().Ticks;

                // Add fraction seconds of timestamp
                decimal fractionalSeconds = fractionOfSecond / (decimal)header.Timebase;
                timestamp += (long)(fractionalSeconds * (decimal)Ticks.PerSecond);

                // Apply parsed timestamp to common header
                header.Timestamp = timestamp;
                header.TimeQualityFlags = (TimeQualityFlags)buffer[index++];

                // Parse sample synchronization state
                SampleSynchronization = buffer.ParseByteTag(SampledValueTag.SmpSynch, ref index);

                // Parse optional sample rate
                if ((SampledValueTag)buffer[index] == SampledValueTag.SmpRate)
                    m_sampleRate = buffer.ParseUInt16Tag(SampledValueTag.SmpRate, ref index);

                // Validate that next tag is for sample values
                if ((SampledValueTag)buffer[index] != SampledValueTag.Samples)
                    throw new InvalidOperationException("Encountered out-of-sequence or unknown sampled value tag: 0x" + buffer[startIndex].ToString("X").PadLeft(2, '0'));

                index++;
                tagLength = buffer.ParseTagLength(ref index);

                // Attempt to derive a configuration if none is defined
                if ((object)m_configurationFrame == null)
                {
                    // If requested, attempt to load configuration from an associated ETR file
                    if (header.UseETRConfiguration)
                        ParseETRConfiguration();

                    // If we still have no configuration, see if a "guess" is requested
                    if ((object)m_configurationFrame == null && header.GuessConfiguration)
                        GuessAtConfiguration(tagLength);
                }

                if ((object)m_configurationFrame == null)
                {
                    // If the configuration is still unavailable, skip past sample values - don't know the details otherwise
                    index += tagLength;
                }
                else
                {
                    // See if sample size validation should by bypassed
                    if (header.IgnoreSampleSizeValidationFailures)
                    {
                        base.ParseBodyImage(buffer, index, length - (index - startIndex));
                        index += tagLength;
                    }
                    else
                    {
                        // Validate that sample size matches current configuration
                        if (tagLength != m_configurationFrame.GetCalculatedSampleLength())
                            throw new InvalidOperationException("Configuration does match data sample size - cannot parse data");

                        // Parse standard synchrophasor sequence
                        index += (ushort)base.ParseBodyImage(buffer, index, length - (index - startIndex));
                    }
                }

                // Skip past optional sample mod tag, if defined
                if ((SampledValueTag)buffer[startIndex] == SampledValueTag.SmpMod)
                    buffer.ValidateTag(SampledValueTag.SmpMod, ref index);

                // Skip past optional UTC timestamp tag, if defined
                if ((SampledValueTag)buffer[startIndex] == SampledValueTag.UtcTimestamp)
                    buffer.ValidateTag(SampledValueTag.UtcTimestamp, ref index);
            }
        }

        private void ParseGoose(byte[] buffer, CommonFrameHeader header, int startIndex, int length)
        {
            int tagLength;

            Common.Dump("ParseGoose");
            //Common.Dump(buffer);

            /*
            // buffers don't always seem to start with goosePdu 0x01 tag, so advance to this
            while ( startIndex < buffer.Length )
            {
                if (buffer[startIndex] == 0x01 && buffer[startIndex+1] == Common.CltpTag)
                    break;

                startIndex++;
            }
            */
            //"somewhere else this is in error"
            int index = startIndex + header.Length;// - 2;// - 3;

            Common.Dump(buffer, index, "Data, headerLength=" + header.Length.ToString());

            // Validate and Skip a bunch of useless tags
            buffer.SkipTag(GooseTag.GocbRef, ref index);
            buffer.SkipTag(GooseTag.TimeAllowedToLive, ref index);
            buffer.SkipTag(GooseTag.DatSet, ref index);

            // Currently parse GoID string to use in place of msvID (potentially DatSet might be a better reference?)
            m_msvID = buffer.ParseStringTag((SampledValueTag)GooseTag.GoId, ref index);

            // Parse refresh time (same as SV)
            if ((GooseTag)buffer[index] != GooseTag.RefrTm)
            {
                Common.Dump(buffer, index, "startindex = " + startIndex.ToString());
                throw new InvalidOperationException("Encountered out-of-sequence or unknown goose tag: 0x" + buffer[startIndex].ToString("X").PadLeft(2, '0'));
            }

            index++;
            tagLength = buffer.ParseTagLength(ref index);

            if (tagLength < 8)
                throw new InvalidOperationException(string.Format("Unexpected length for \"{0}\" tag: {1}", GooseTag.RefrTm, tagLength));

            uint secondOfCentury = BigEndian.ToUInt32(buffer, index);
            UInt24 fractionOfSecond = BigEndian.ToUInt24(buffer, index + 4);
            index += 7;

            // Get whole seconds of timestamp
            long timestamp = (new UnixTimeTag((decimal)secondOfCentury)).ToDateTime().Ticks;

            // Add fraction seconds of timestamp
            decimal fractionalSeconds = fractionOfSecond / (decimal)header.Timebase;
            timestamp += (long)(fractionalSeconds * (decimal)Ticks.PerSecond);

            // Apply parsed timestamp to common header
            header.Timestamp = timestamp;
            header.TimeQualityFlags = (TimeQualityFlags)buffer[index++];

            // skip stNum tag
            buffer.SkipTag(GooseTag.StNum, ref index);

            // Validate sqNum tag and subsititute as sample count
            int countLength = buffer.ValidateTag(GooseTag.SqNum, ref index);
            if (countLength < 2)
            {
                m_sampleCount = ushort.Parse(buffer[index++].ToString("X"), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                index--;
                index--;
                m_sampleCount = buffer.ParseUInt16Tag(SampledValueTag.SmpRate, ref index);
            }

            // skip several more tags
            buffer.SkipTag(GooseTag.Test, ref index);
            buffer.SkipTag(GooseTag.ConfRev, ref index);
            buffer.SkipTag(GooseTag.NdsCom, ref index);
            buffer.SkipTag(GooseTag.NumDatSetEntries, ref index);

            // Confirm next tag is actual data
            int dataLength = buffer.SkipTag(GooseTag.AllData, ref index);

            // Extract data without tags to new buffer
            byte[] dataBuffer = buffer.ExtractGooseData(index - dataLength, dataLength);
            int numDataBytes = 0;

            if ((object)m_configurationFrame == null)
            {
                if (header.UseETRConfiguration)
                {
                    // need to do something with numDataBytes
                    numDataBytes = ParseXmlConfig();
                }
                else
                    throw new InvalidOperationException("No configuration available, data will be ignored");
            }
            if ((object)m_configurationFrame != null)
            {
                // Validate that sample size matches current configuration
                /*
                if (dataBuffer.Length != m_configurationFrame.GetCalculatedGooseLength())
                {
                    // If it doesn't check configuration hasn't changed
                    numDataBytes = ParseXmlConfig();
                }
                
                if (dataBuffer.Length != m_configurationFrame.GetCalculatedGooseLength())
                {
                    // (Last chance) Possible frequnecy data is missing which is included in calculated lenght by default
                    //if (dataBuffer.Length != m_configurationFrame.GetCalculatedGooseLength() - 8)
                    // i dont think possible to check TESTn
//                    throw new InvalidOperationException(String.Format("Configuration does not match data sample size - Expected {0}, but received {1} bytes from packet and {2} bytes from XML calculation", m_configurationFrame.GetCalculatedGooseLength(), dataBuffer.Length, numDataBytes));
                }
                */
                // Parse the sequence (same as SV)
                base.ParseBodyImage(dataBuffer, 0, dataBuffer.Length);
            }

            // skip final stNum tag
            buffer.SkipTag(GooseTag.StNum, ref index);


        }

        // XML READER
        public int ParseXmlConfig()
        {
            /*
             * Method 3
            var xmlString = File.ReadAllText(m_msvID + ".XML");
            var stringReader = new StringReader(xmlString);
            var dsSet = new DataSet();
            dsSet.ReadXml(stringReader);
            */

            // return 0;

            int numDataBytes = 0;
            bool statusDefined = false;


            ConfigurationFrame configFrame = new ConfigurationFrame(Common.Timebase, 1, DateTime.UtcNow.Ticks, m_sampleRate);
            ConfigurationCell configCell = new ConfigurationCell(configFrame, (ushort)(m_idCode + configFrame.Cells.Count), LineFrequency.Hz60)
            {
                StationName = m_stationName + (configFrame.Cells.Count + 1)
            };

//            ConfigurationCell configCell = new ConfigurationCell(configFrame, (ushort)(1 + configFrame.Cells.Count), LineFrequency.Hz50);
            String exceptionMessage = "";
            try
            {
                Common.Dump("ParseXmlConfig " + m_msvID + ".XML" );

                // new xdoc instance 
                XmlDocument xDoc = new XmlDocument();

                //load up the xml from the location 
                xDoc.Load(m_msvID + ".XML");

                // cycle through each child noed 
                foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
                {
                    // first node is the url ... have to go to nexted loc node 
                    foreach (XmlNode locNode in node)
                    {
                        Common.Dump(String.Format("{0}->{1},{2}->{3}", "node.Name", node.Name, "locNode.Value", locNode.Value));
                        switch (node.Name)
                        {
                            case "FLAG":
                                {
                                    numDataBytes += 1;
                                 //   configCell.DigitalDefinitions.Add(new DigitalDefinition(configCell, locNode.Value, 0, 1));
                                    
                                    statusDefined = true;
                                    break;
                                }
                            case "VPHA":
                                {
                                    numDataBytes += 5;
                                    PhasorDefinition phasor = new PhasorDefinition(configCell, locNode.Value, 1, 0.0D, PhasorType.Voltage, null);
                                    phasor.Label = "A VPHA Label";
                                    configCell.PhasorDefinitions.Add(phasor);// new PhasorDefinition(configCell, locNode.Value, 1, 0.0D, PhasorType.Voltage, null));

                                    break;
                                }
                            case "IPHA":
                                {
                                    numDataBytes += 5;
                                    configCell.PhasorDefinitions.Add(new PhasorDefinition(configCell, locNode.Value, 1, 0.0D, PhasorType.Current, null));
                                    
                                    break;
                                }
                            case "FREQ":
                                {
                                    numDataBytes += 5;
                                    break;
                                }
                            case "DFDT":
                                {
                                    numDataBytes += 5;
                                    configCell.FrequencyDefinition = new FrequencyDefinition(configCell, "Frequency");
                                    break;
                                }
                            case "ALOG":
                                {
                                    numDataBytes += 5;
                                    configCell.AnalogDefinitions.Add(new AnalogDefinition(configCell, locNode.Value, 1, 0.0D, AnalogType.SinglePointOnWave));
                                    break;
                                }
                            case "DIGI":
                                {
                                    numDataBytes += 5;
                                    configCell.DigitalDefinitions.Add(new DigitalDefinition(configCell, locNode.Value, 0, 1));
                                    break;
                                }
                            case "STRING":
                                {
                                    // can't be determined
                                    DigitalDefinition digital = new DigitalDefinition(configCell, locNode.Value, 0, 1);
                                    digital.Label = "String";
                                    configCell.DigitalDefinitions.Add(digital);

                                    numDataBytes += 0;
                                    break;
                                }
                            case "BOOL":
                                {
                                    DigitalDefinition digital = new DigitalDefinition(configCell, locNode.Value, 0, 1);
                                    digital.Label = "hahahaha";
                                   configCell.DigitalDefinitions.Add(digital);

                                  //  numDataBytes += 5;
                                  //  configCell.AnalogDefinitions.Add(new AnalogDefinition(configCell, locNode.Value, 1, 0.0D, AnalogType.SinglePointOnWave));

                                    numDataBytes += 1;
                                    break;
                                }
                            default:
                                {
                                    exceptionMessage += "Unexpected signal type aaaaencountered: " + node.Name + Environment.NewLine;
                                    // dont throw exception so we can process as much as possible
                                    //throw new InvalidOperationException("Unexpected signal type encountered: " + node.Name);
                                    break;
                                }
                        }
                    }
                }
            }


            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }


            if ( !String.IsNullOrEmpty(exceptionMessage))
            {
                Common.Dump(exceptionMessage);
          //      throw new InvalidOperationException(exceptionMessage); TEST
            }

            /*
             * JOELs method
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.DtdProcessing = DtdProcessing.Parse;
                        XmlReader reader = XmlReader.Create(m_msvID + ".XML", settings);
                        reader.MoveToContent();
                        String elementType = "NONE", lastElementType = "NONE";
                        string label;
                        bool statusDefined = false;
                        ConfigurationFrame configFrame = new ConfigurationFrame(Common.Timebase, 1, DateTime.UtcNow.Ticks, m_sampleRate);
                        ConfigurationCell configCell = new ConfigurationCell(configFrame, (ushort)(1 + configFrame.Cells.Count), LineFrequency.Hz50)
                        {
                            StationName = m_msvID
                        };
                        bool badOrder = false;
                        // Parse the file and display each of the nodes.
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    elementType = reader.Name;
                                    break;
                                case XmlNodeType.Text:
                                    label = reader.Value;
                                    switch (elementType)
                                    {
                                        case "FLAG":
                                            badOrder = lastElementType != "NONE";
                                            statusDefined = true;
                                            break;
                                        case "VPHA":
                                        case "IPHA":
                                            badOrder = (lastElementType != "FLAG" && lastElementType != "VPHA" && lastElementType != "IPHA");
                                            PhasorDefinition phasor = new PhasorDefinition(configCell, label, 1, 0.0D, elementType == "VPHA" ? PhasorType.Voltage : PhasorType.Current, null);
                                            configCell.PhasorDefinitions.Add(phasor);
                                            break;
                                        case "FREQ":
                                            badOrder = (lastElementType != "VPHA" && lastElementType != "IPHA" && lastElementType != "FLAG");
                                            break;
                                        case "DFDT":
                                            badOrder = lastElementType != "FREQ";
                                            configCell.FrequencyDefinition = new FrequencyDefinition(configCell, "Frequency");
                                            break;
                                        case "ALOG":
                                            badOrder = (lastElementType == "DIGI");
                                            AnalogDefinition analog = new AnalogDefinition(configCell, label, 1, 0.0D, AnalogType.SinglePointOnWave);
                                            configCell.AnalogDefinitions.Add(analog);
                                            break;
                                        case "DIGI":
                                            DigitalDefinition digital = new DigitalDefinition(configCell, label, 0, 1);
                                            configCell.DigitalDefinitions.Add(digital);
                                            break;
                                        default:
                                            throw new InvalidOperationException("Unexpected signal type encountered: " + elementType);
                                    }
                                    lastElementType = elementType;
                                    break;
                                default:
                                    break;
                            }
                            if (badOrder)
                                throw new InvalidOperationException(string.Format("Invalid signal order encountered - {0} cannot follow {1}. Standard synchrophasor order is: status flags, one or more phasor magnitude/angle pairs, frequency, dF/dt, optional analogs, optional digitals", elementType, lastElementType));
                        }
            */
            if (!statusDefined)
                throw new InvalidOperationException("No status flag signal was defined.");

            // Add cell to configuration frame
            configFrame.Cells.Add(configCell);

            // Publish configuration frame
//            PublishNewGooseConfigurationFrame(configFrame);
            PublishNewConfigurationFrame(configFrame);

            return numDataBytes;
        }

        // Attempt to parse an associated ETR configuration
        private void ParseETRConfiguration()
        {
            if (!string.IsNullOrWhiteSpace(m_msvID))
            {
                // See if an associated ETR file exists
                string etrFileName = m_msvID + ".etr";
                string etrFilePath = FilePath.GetAbsolutePath(etrFileName);
                bool foundETRFile = File.Exists(etrFilePath);

                if (!foundETRFile)
                {
                    // Also test for ETR in configuration cache folder
                    etrFilePath = FilePath.GetAbsolutePath(string.Format("ConfigurationCache{0}{1}", Path.DirectorySeparatorChar, etrFileName));
                    foundETRFile = File.Exists(etrFilePath);
                }

                if (foundETRFile)
                {
                    try
                    {
                        StreamReader reader = new StreamReader(etrFilePath);
                        SignalType signalType, lastSignalType = SignalType.NONE;
                        string label;
                        bool statusDefined = false;
                        bool endOfFile;
                        int magnitudeSignals = 0;
                        int angleSignals = 0;

                        ConfigurationFrame configFrame = new ConfigurationFrame(Common.Timebase, 1, DateTime.UtcNow.Ticks, m_sampleRate);

                        do
                        {
                            bool badOrder = false;

                            ConfigurationCell configCell = new ConfigurationCell(configFrame, (ushort)(m_idCode + configFrame.Cells.Count), LineFrequency.Hz60)
                            {
                                StationName = m_stationName + (configFrame.Cells.Count + 1)
                            };

                            // Keep parsing records until there are no more...
                            while (ParseNextSampleDefinition(reader, out signalType, out label, out endOfFile))
                            {
                                // If ETR is defining a new device, exit and handle current device
                                if (signalType == SignalType.FLAG && statusDefined)
                                {
                                    badOrder = (lastSignalType != SignalType.DFDT && lastSignalType != SignalType.ALOG && lastSignalType != SignalType.DIGI);
                                    lastSignalType = SignalType.FLAG;
                                    break;
                                }

                                // Validate signal order
                                switch (signalType)
                                {
                                    case SignalType.FLAG:
                                        badOrder = lastSignalType != SignalType.NONE;
                                        statusDefined = true;
                                        break;
                                    case SignalType.VPHM:
                                    case SignalType.IPHM:
                                        badOrder = (lastSignalType != SignalType.FLAG && lastSignalType != SignalType.VPHA && lastSignalType != SignalType.IPHA);
                                        PhasorDefinition phasor = new PhasorDefinition(configCell, label, 1, 0.0D, signalType == SignalType.VPHM ? PhasorType.Voltage : PhasorType.Current, null);
                                        configCell.PhasorDefinitions.Add(phasor);
                                        magnitudeSignals++;
                                        break;
                                    case SignalType.VPHA:
                                        badOrder = lastSignalType != SignalType.VPHM;
                                        angleSignals++;
                                        break;
                                    case SignalType.IPHA:
                                        badOrder = lastSignalType != SignalType.IPHM;
                                        angleSignals++;
                                        break;
                                    case SignalType.FREQ:
                                        badOrder = (lastSignalType != SignalType.VPHA && lastSignalType != SignalType.IPHA);
                                        break;
                                    case SignalType.DFDT:
                                        badOrder = lastSignalType != SignalType.FREQ;
                                        configCell.FrequencyDefinition = new FrequencyDefinition(configCell, "Frequency");
                                        break;
                                    case SignalType.ALOG:
                                        badOrder = (lastSignalType != SignalType.DFDT && lastSignalType != SignalType.ALOG);
                                        AnalogDefinition analog = new AnalogDefinition(configCell, label, 1, 0.0D, AnalogType.SinglePointOnWave);
                                        configCell.AnalogDefinitions.Add(analog);
                                        break;
                                    case SignalType.DIGI:
                                        badOrder = (lastSignalType != SignalType.DFDT && lastSignalType != SignalType.ALOG && lastSignalType != SignalType.DIGI);
                                        DigitalDefinition digital = new DigitalDefinition(configCell, label, 0, 1);
                                        configCell.DigitalDefinitions.Add(digital);
                                        break;
                                    default:
                                        throw new InvalidOperationException("Unxpected signal type enecountered: " + signalType);
                                }

                                lastSignalType = signalType;
                            }

                            if (badOrder)
                                throw new InvalidOperationException(string.Format("Invalid signal order encountered - {0} cannot follow {1}. Standard synchrophasor order is: status flags, one or more phasor magnitude/angle pairs, frequency, dF/dt, optional analogs, optional digitals", signalType, lastSignalType));

                            if (!statusDefined)
                                throw new InvalidOperationException("No status flag signal was defined.");

                            if (configCell.PhasorDefinitions.Count == 0)
                                throw new InvalidOperationException("No phasor magnitude/angle signal pairs were defined.");

                            if (magnitudeSignals != angleSignals)
                                throw new InvalidOperationException("Phasor magnitude/angle signal pair mismatch - there must be a one-to-one definition between angle and magnitude signals.");

                            if (configCell.FrequencyDefinition == null)
                                throw new InvalidOperationException("No frequency and dF/dt signal pair was defined.");

                            // Add cell to configuration frame
                            configFrame.Cells.Add(configCell);

                            // Reset counters
                            magnitudeSignals = 0;
                            angleSignals = 0;
                        }
                        while (!endOfFile);

                        // Publish configuration frame
                        PublishNewConfigurationFrame(configFrame);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(string.Format("Failed to parse associated ETR configuration \"{0}\": {1}", etrFilePath, ex.Message), ex);
                    }
                }
            }
        }

        // Complex function used to read next signal type and label from the ETR file...
        // Note that current parsing depends on sample tag name format defined in the IEC 61850-90-5 implementation agreement
        private static bool ParseNextSampleDefinition(StreamReader reader, out SignalType signalType, out string label, out bool endOfFile)
        {
            string signalLabel, dataType, signalDetail;
            bool result = false;

            signalType = SignalType.NONE;
            label = null;

            // Attempt to read signal definition and label line
            signalLabel = reader.ReadLine();

            if (signalLabel != null)
            {
                // Attempt to reader data type line
                dataType = reader.ReadLine();

                if (dataType != null)
                {
                    // Clean up data type
                    dataType = dataType.Trim().ToLower();

                    int index = signalLabel.IndexOf("-");

                    // Get defined signal label
                    label = signalLabel.Substring(index + 1).Trim();

                    // See if signal type contains ST
                    index = signalLabel.IndexOf(".ST.");

                    if (index > 0)
                    {
                        // Get detail portion of signal type label
                        signalDetail = signalLabel.Substring(index + 4);

                        // Status or digital value
                        if (signalDetail.StartsWith("Ind1"))
                        {
                            // Status word value
                            signalType = SignalType.FLAG;

                            if (dataType != "i2")
                                throw new InvalidOperationException(string.Format("Invalid data type size {0} specified for signal type {1} parsed from {2}", dataType, signalType, signalLabel));

                            result = true;
                        }
                        else if (signalDetail.StartsWith("Ind2"))
                        {
                            // Digital value
                            signalType = SignalType.DIGI;

                            if (dataType != "i2")
                                throw new InvalidOperationException(string.Format("Invalid data type size {0} specified for signal type {1} parsed from {2}", dataType, signalType, signalLabel));

                            result = true;
                        }
                        else
                        {
                            // Unable to determine signal type
                            throw new InvalidOperationException(string.Format("Unable to determine ETR signal type for {0} ({1})", signalLabel, dataType));
                        }
                    }
                    else
                    {
                        // See if signal type contains MX
                        index = signalLabel.IndexOf(".MX.");

                        if (index > 0)
                        {
                            // Get detail portion of signal type label
                            signalDetail = signalLabel.Substring(index + 4);

                            // Frequency or phasor value
                            if (signalDetail.StartsWith("HzRte"))
                            {
                                // dF/dt value
                                signalType = SignalType.DFDT;

                                if (dataType != "f4")
                                    throw new InvalidOperationException(string.Format("Invalid data type size {0} specified for signal type {1} parsed from {2}", dataType, signalType, signalLabel));

                                result = true;
                            }
                            else if (signalDetail.StartsWith("Hz"))
                            {
                                // Frequency value
                                signalType = SignalType.FREQ;

                                if (dataType != "f4")
                                    throw new InvalidOperationException(string.Format("Invalid data type size {0} specified for signal type {1} parsed from {2}", dataType, signalType, signalLabel));

                                result = true;
                            }
                            else if (signalDetail.StartsWith("PhV") || signalDetail.StartsWith("SeqV"))
                            {
                                if (signalDetail.Contains(".mag."))
                                {
                                    // Voltage phase magnitude
                                    signalType = SignalType.VPHM;
                                }
                                else if (signalDetail.Contains(".ang."))
                                {
                                    // Voltage phase angle
                                    signalType = SignalType.VPHA;
                                }
                                else
                                {
                                    // Unable to determine signal type
                                    throw new InvalidOperationException(string.Format("Unable to determine ETR signal type for {0} ({1})", signalLabel, dataType));
                                }

                                if (dataType != "f4")
                                    throw new InvalidOperationException(string.Format("Invalid data type size {0} specified for signal type {1} parsed from {2}", dataType, signalType, signalLabel));

                                result = true;
                            }
                            else if (signalDetail.StartsWith("SeqA") || signalDetail.StartsWith("A"))
                            {
                                if (signalDetail.Contains(".mag."))
                                {
                                    // Current phase magnitude
                                    signalType = SignalType.IPHM;
                                }
                                else if (signalDetail.Contains(".ang."))
                                {
                                    // Current phase angle
                                    signalType = SignalType.IPHA;
                                }
                                else
                                {
                                    // Unable to determine signal type
                                    throw new InvalidOperationException(string.Format("Unable to determine ETR signal type for {0} ({1})", signalLabel, dataType));
                                }

                                if (dataType != "f4")
                                    throw new InvalidOperationException(string.Format("Invalid data type size {0} specified for signal type {1} parsed from {2}", dataType, signalType, signalLabel));

                                result = true;
                            }
                            else
                            {
                                // Unable to determine signal type
                                throw new InvalidOperationException(string.Format("Unable to determine ETR signal type for {0} ({1})", signalLabel, dataType));
                            }
                        }
                        else
                        {
                            // Assuming anything else is an Analog value
                            signalType = SignalType.ALOG;

                            if (dataType != "f4")
                                throw new InvalidOperationException(string.Format("Invalid data type size {0} specified for assumed analog signal type parsed from {1}", dataType, signalLabel));

                            result = true;
                        }
                    }
                }
            }

            endOfFile = !result;
            return result;
        }

        // Attempt to guess at the configuration
        private void GuessAtConfiguration(int sampleLength)
        {
            // Removed fixed length for 2-byte status, 4-byte frequency and 4-byte dF/dt
            int test = sampleLength - 10;

            // Assume remaining even 8-byte pairs are phasor values (i.e., 4-byte magnitude and 4-byte angle)
            int phasors = test / 8;
            test -= phasors * 8;

            // Assume remaining even 2-byte items are digital values
            int digitals = test / 2;
            test -= digitals * 2;

            // If no bytes remain, we'll assume this distribution as a guess configuration
            if (test == 0)
            {
                // Just assume some details for a configuration frame
                ConfigurationFrame configFrame = new ConfigurationFrame(Common.Timebase, 1, DateTime.UtcNow.Ticks, m_sampleRate);
                ConfigurationCell configCell = new ConfigurationCell(configFrame, m_idCode, LineFrequency.Hz60)
                {
                    StationName = m_stationName
                };

                // Add phasors
                for (int i = 0; i < phasors; i++)
                {
                    PhasorType type = i < phasors / 2 ? PhasorType.Voltage : PhasorType.Current;
                    PhasorDefinition phasor = new PhasorDefinition(configCell, "Phasor " + (i + 1), 1, 0.0D, type, null);
                    configCell.PhasorDefinitions.Add(phasor);
                }

                // Add frequency
                configCell.FrequencyDefinition = new FrequencyDefinition(configCell, "Frequency");

                // Add digitals
                for (int i = 0; i < digitals; i++)
                {
                    DigitalDefinition digital = new DigitalDefinition(configCell, "Digital " + (i + 1), 0, 1);
                    configCell.DigitalDefinitions.Add(digital);
                }

                configFrame.Cells.Add(configCell);
                PublishNewConfigurationFrame(configFrame);
            }
        }

        public void PublishNewGooseConfigurationFrame(ConfigurationFrame configFrame)
        {
            // Cache new configuration
            m_configurationFrame = configFrame;

            // Cache new associated configuration frame
            ConfigurationFrame = configFrame;

            // Update the frame level parsing state
            bool trustHeaderLength = true;
            bool validateCheckSum = true;

            if ((object)State != null)
            {
                trustHeaderLength = State.TrustHeaderLength;
                validateCheckSum = State.ValidateCheckSum;
            }

            DataFrameParsingState parsingState = new DataFrameParsingState(CommonHeader.FrameLength, configFrame, DataCell.CreateNewCell, trustHeaderLength, validateCheckSum);
            CommonHeader.State = parsingState;
            State = parsingState;

            // Update local associated configuration cells
            for (int i = 0; i < configFrame.Cells.Count; i++)
            {
                ConfigurationCell configCell = configFrame.Cells[i];

                // Update associated configuration cell
                if (Cells.Count < i + 1)
                    Cells.Add(new IEC61850_90_5.DataCell(this, new IEC61850_90_5.ConfigurationCell(this.ConfigurationFrame)));

                Cells[i].ConfigurationCell = configCell;

                // Update local parsing state with new configuration info
                Cells[i].State = new DataCellParsingState(
                    configCell,
                    PhasorValue.CreateNewValue,
                    FrequencyValue.CreateNewValue,
                    AnalogValue.CreateNewValue,
                    DigitalValue.CreateNewValue);
            }

            // Publish the configuration frame to the rest of the system
            if (CommonHeader.PublishFrame != null)
                CommonHeader.PublishFrame(configFrame);
        }

        // Exposes a newly created configuration frame
        public void PublishNewConfigurationFrame(ConfigurationFrame configFrame)
        {
            // Cache new configuration
            m_configurationFrame = configFrame;

            // Cache new associated configuration frame
            ConfigurationFrame = configFrame;

            // Update the frame level parsing state
            bool trustHeaderLength = true;
            bool validateCheckSum = true;

            if ((object)State != null)
            {
                trustHeaderLength = State.TrustHeaderLength;
                validateCheckSum = State.ValidateCheckSum;
            }

            DataFrameParsingState parsingState = new DataFrameParsingState(CommonHeader.FrameLength, configFrame, DataCell.CreateNewCell, trustHeaderLength, validateCheckSum);
            CommonHeader.State = parsingState;
            State = parsingState;

            // Update local associated configuration cells
            for (int i = 0; i < Cells.Count; i++)
            {
                ConfigurationCell configCell = configFrame.Cells[i];

                // Update associated configuration cell
                Cells[i].ConfigurationCell = configCell;

                // Update local parsing state with new configuration info
                Cells[i].State = new DataCellParsingState(
                    configCell,
                    PhasorValue.CreateNewValue,
                    FrequencyValue.CreateNewValue,
                    AnalogValue.CreateNewValue,
                    DigitalValue.CreateNewValue);
            }

            // Publish the configuration frame to the rest of the system
            if ( CommonHeader.PublishFrame != null)
                CommonHeader.PublishFrame(configFrame);
        }

        /// <summary>
        /// Determines if checksum in the <paramref name="buffer"/> is valid.
        /// </summary>
        /// <param name="buffer">Buffer image to validate.</param>
        /// <param name="startIndex">Start index into <paramref name="buffer"/> to perform checksum.</param>
        /// <returns>Flag that determines if checksum over <paramref name="buffer"/> is valid.</returns>
        protected override bool ChecksumIsValid(byte[] buffer, int startIndex)
        {
            // IEC 61850-90-5 data frame CRC is checked during header parsing
            return true;
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
            throw new NotImplementedException("Checksum for IEC 61850-90-5 data frames are calculated when header is parsed, see CommonFrameHeader.");
        }

        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination <see cref="StreamingContext"/> for this serialization.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            // Serialize data frame
            info.AddValue("frameHeader", m_frameHeader, typeof(CommonFrameHeader));
            info.AddValue("msvID", m_msvID);
            info.AddValue("sampleCount", m_sampleCount);
            info.AddValue("configurationRevision", m_configurationRevision);
            info.AddValue("sampleSynchronization", SampleSynchronization);
            info.AddValue("sampleRate", m_sampleRate);
        }

        #endregion
    }
}