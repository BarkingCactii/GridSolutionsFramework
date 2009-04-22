//*******************************************************************************************************
//  CommonFrameHeader.cs
//  Copyright © 2009 - TVA, all rights reserved - Gbtc
//
//  Build Environment: C#, Visual Studio 2008
//  Primary Developer: James R Carroll
//      Office: PSO TRAN & REL, CHATTANOOGA - MR BK-C
//       Phone: 423/751-4165
//       Email: jrcarrol@tva.gov
//
//  Code Modification History:
//  -----------------------------------------------------------------------------------------------------
//  11/12/2004 - James R Carroll
//       Generated original version of source code.
//
//*******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using PCS.Measurements;
using PCS.Parsing;

namespace PCS.PhasorProtocols.BpaPdcStream
{
    /// <summary>
    /// Represents the common header for all BPA PDCstream frames of data.
    /// </summary>
    [Serializable()]
    public class CommonFrameHeader : ICommonHeader<FrameType>, IChannelFrame
    {
        #region [ Members ]

        // Constants

        /// <summary>
        /// Total fixed length of <see cref="CommonFrameHeader"/>.
        /// </summary>
        public const ushort FixedLength = 4;

        // Fields
        private byte m_packetNumber;
        private ushort m_wordCount;
        private IChannelParsingState m_state;
        private Dictionary<string, string> m_attributes;
        private object m_tag;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="CommonFrameHeader"/> from specified parameters.
        /// </summary>
        /// <param name="packetNumber">The BPA PDCstream packet number, configuration frame is packet zero.</param>
        public CommonFrameHeader(byte packetNumber)
        {
            m_packetNumber = packetNumber;
        }

        /// <summary>
        /// Creates a new <see cref="CommonFrameHeader"/> from given <paramref name="binaryImage"/>.
        /// </summary>
        /// <param name="configurationFrame">BPA PDCstream <see cref="ConfigurationFrame1"/> if already parsed.</param>
        /// <param name="binaryImage">Buffer that contains data to parse.</param>
        /// <param name="startIndex">Start index into buffer where valid data begins.</param>
        public CommonFrameHeader(ConfigurationFrame configurationFrame, byte[] binaryImage, int startIndex)
        {
            if (binaryImage[startIndex] != PhasorProtocols.Common.SyncByte)
                throw new InvalidOperationException("Bad data stream, expected sync byte 0xAA as first byte in BPA PDCstream frame, got " + binaryImage[startIndex].ToString("X").PadLeft(2, '0'));

            // Get packet number and word count
            m_packetNumber = binaryImage[startIndex + 1];
            m_wordCount = EndianOrder.BigEndian.ToUInt16(binaryImage, startIndex + 2);
        }

        /// <summary>
        /// Creates a new <see cref="CommonFrameHeader"/> from serialization parameters.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> with populated with data.</param>
        /// <param name="context">The source <see cref="StreamingContext"/> for this deserialization.</param>
        protected CommonFrameHeader(SerializationInfo info, StreamingContext context)
        {
            // Deserialize common frame header
            m_packetNumber = info.GetByte("packetNumber");
            m_wordCount = info.GetUInt16("wordCount");
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the BPA PDCstream specific frame type of this frame.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This returns the protocol specific frame classification which uniquely identifies the frame type.
        /// </para>
        /// <para>
        /// This is the <see cref="ICommonHeader{TTypeIdentifier}.TypeID"/> implementation.
        /// </para>
        /// </remarks>
        public FrameType TypeID
        {
            get
            {
                return (m_packetNumber == 0 ? BpaPdcStream.FrameType.ConfigurationFrame : BpaPdcStream.FrameType.DataFrame);
            }
        }

        /// <summary>
        /// Gets or sets the BPA PDCstream packet number of this frame - set to 00 for configuration frame
        /// </summary>
        public byte PacketNumber
        {
            get
            {
                return m_packetNumber;
            }
            set
            {
                m_packetNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets the BPA PDCstream frame length of this frame.
        /// </summary>
        public ushort FrameLength
        {
            get
            {
                return (ushort)(2 * m_wordCount);
            }
            set
            {
                m_wordCount = (ushort)(value / 2);
            }
        }

        /// <summary>
        /// Gets or sets the BPA PDcstream word count.
        /// </summary>
        public ushort WordCount
        {
            get
            {
                return m_wordCount;
            }
            set
            {
                m_wordCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the BPA PDcstream packet flag.
        /// </summary>
        public byte PacketFlag
        {
            get
            {
                return m_packetNumber;
            }
            set
            {
                m_packetNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets the length of the data in the BPA PDCstream frame (i.e., the <see cref="FrameLength"/> minus the header length and checksum: <see cref="FrameLength"/> - 8).
        /// </summary>
        public ushort DataLength
        {
            get
            {
                // Data length will be frame length minus common header length minus crc16
                return (ushort)(FrameLength - FixedLength - 2);
            }
            set
            {
                if (value > Common.MaximumDataLength)
                    throw new OverflowException("Data length value cannot exceed " + Common.MaximumDataLength);
                else
                    FrameLength = (ushort)(value + FixedLength + 2);
            }
        }

        /// <summary>
        /// Gets or sets the parsing state for the <see cref="CommonFrameHeader"/> object.
        /// </summary>
        public IChannelParsingState State
        {
            get
            {
                return m_state;
            }
            set
            {
                m_state = value;
            }
        }

        // Gets or sets any additional state information - satifies ICommonHeader<FrameType>.State interface property
        object ICommonHeader<FrameType>.State
        {
            get
            {
                return m_state;
            }
            set
            {
                m_state = value as IChannelParsingState;
            }
        }

        /// <summary>
        /// Gets the fundamental frame type of this frame.
        /// </summary>
        /// <remarks>
        /// Frames are generally classified as data, configuration or header frames. This returns the general frame classification.
        /// </remarks>
        public FundamentalFrameType FrameType
        {
            get
            {
                // Translate BPA PDCstream specific frame type to fundamental frame type
                switch (TypeID)
                {
                    case BpaPdcStream.FrameType.DataFrame:
                        return FundamentalFrameType.DataFrame;
                    case BpaPdcStream.FrameType.ConfigurationFrame:
                        return FundamentalFrameType.ConfigurationFrame;
                    default:
                        return FundamentalFrameType.Undetermined;
                }
            }
        }

        /// <summary>
        /// Gets the length of the <see cref="BinaryImage"/>.
        /// </summary>
        public int BinaryLength
        {
            get
            {
                return FixedLength;
            }
        }

        /// <summary>
        /// Gets the binary image of the common header portion of this frame.
        /// </summary>
        public byte[] BinaryImage
        {
            get
            {
                byte[] buffer = new byte[FixedLength];

                buffer[0] = PhasorProtocols.Common.SyncByte;
                buffer[1] = m_packetNumber;
                EndianOrder.BigEndian.CopyBytes(m_wordCount, buffer, 2);

                return buffer;
            }
        }

        /// <summary>
        /// Determines if <see cref="IChannelFrame"/> is only partially parsed.
        /// </summary>
        /// <remarks>
        /// This frame is not complete - it only represents the parsed common "header" for frames.
        /// </remarks>
        public bool IsPartial
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}"/> of string based property names and values for the <see cref="CommonFrameHeader"/> object.
        /// </summary>
        public Dictionary<string, string> Attributes
        {
            get
            {
                // Create a new attributes dictionary or clear the contents of any existing one
                if (m_attributes == null)
                    m_attributes = new Dictionary<string, string>();
                else
                    m_attributes.Clear();

                m_attributes.Add("Derived Type", this.GetType().Name);
                m_attributes.Add("Binary Length", FixedLength.ToString());
                m_attributes.Add("Total Cells", "0");
                m_attributes.Add("Fundamental Frame Type", (int)FrameType + ": " + FrameType);
                m_attributes.Add("ID Code", "undefined");
                m_attributes.Add("Is Partial Frame", IsPartial.ToString());
                m_attributes.Add("Published", "n/a");
                m_attributes.Add("Ticks", "undefined");
                m_attributes.Add("Timestamp", "n/a");
                AppendHeaderAttributes(m_attributes);

                return m_attributes;
            }
        }

        /// <summary>
        /// User definable object used to hold a reference associated with the <see cref="IChannel"/> object.
        /// </summary>
        public object Tag
        {
            get
            {
                return m_tag;
            }
            set
            {
                m_tag = value;
            }
        }

        #region [ IChannelFrame Implementation ]

        ushort IChannelFrame.IDCode
        {
            get
            {
                return 0;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        object IChannelFrame.Cells
        {
            get
            {
                return null;
            }
        }

        int ISupportBinaryImage.Initialize(byte[] binaryImage, int startIndex, int length)
        {
            // The common frame header is parsed during construction
            throw new NotImplementedException();
        }

        int IFrame.PublishedMeasurements
        {
            get
            {
                return 0;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        IDictionary<MeasurementKey, IMeasurement> IFrame.Measurements
        {
            get
            {
                return null;
            }
        }

        bool IFrame.Published
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        IMeasurement IFrame.LastSortedMeasurement
        {
            get
            {
                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="IFrame"/>.
        /// </summary>
        /// <param name="other">An <see cref="IFrame"/> value to compare to this instance.</param>
        /// <returns>
        /// True if <paramref name="other"/> has the same value as this instance; otherwise, False.
        /// </returns>
        public bool Equals(IFrame other)
        {
            return (CompareTo(other) == 0);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="IFrame"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">An <see cref="IFrame"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        public int CompareTo(IFrame other)
        {
            return (this as IFrame).Timestamp.CompareTo(other.Timestamp);
        }

        /// <summary>
        /// Compares this instance to a specified object and returns an indication of their relative values.
        /// </summary>
        /// <param name="obj">An object to compare, or null.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        /// <exception cref="ArgumentException">value is not an <see cref="IFrame"/>.</exception>
        public int CompareTo(object obj)
        {
            IFrame other = obj as IFrame;

            if (other != null)
                return CompareTo(other);

            throw new ArgumentException("Frame can only be compared with other IFrames...");
        }

        /// <summary>
        /// Appends header specific attributes to <paramref name="attributes"/> dictionary.
        /// </summary>
        /// <param name="attributes">Dictionary to append header specific attributes to.</param>
        internal void AppendHeaderAttributes(Dictionary<string, string> attributes)
        {
            attributes.Add("Frame Type", (ushort)TypeID + ": " + TypeID);
            attributes.Add("Frame Length", FrameLength.ToString());
            attributes.Add("Packet Number", PacketNumber.ToString());
            attributes.Add("Word Count", WordCount.ToString());
        }

        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination <see cref="StreamingContext"/> for this serialization.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Serialize unique common frame header values
            info.AddValue("packetNumber", m_packetNumber);
            info.AddValue("wordCount", m_wordCount);
        }

        #endregion
    }
}

// TODO: Delete old code...

////*******************************************************************************************************
////  CommonFrameHeader.vb - BPA PDCstream Common frame header functions
////  Copyright © 2008 - TVA, all rights reserved - Gbtc
////
////  Build Environment: VB.NET, Visual Studio 2008
////  Primary Developer: J. Ritchie Carroll, Operations Data Architecture [TVA]
////      Office: COO - TRNS/PWR ELEC SYS O, CHATTANOOGA, TN - MR 2W-C
////       Phone: 423/751-2827
////       Email: jrcarrol@tva.gov
////
////  Code Modification History:
////  -----------------------------------------------------------------------------------------------------
////  11/12/2004 - J. Ritchie Carroll
////       Initial version of source generated
////
////*******************************************************************************************************

//using System;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//using PCS;
//using PCS.Parsing;
//using PCS.Measurements;

//namespace PCS.PhasorProtocols
//{
//    namespace BpaPdcStream
//    {
//        // This class generates and parses a frame header specfic to BPA PDCstream
//        [CLSCompliant(false), Serializable()]
//        public sealed class CommonFrameHeader
//        {
//            #region " Internal Common Frame Header Instance Class "

//            // This class is used to temporarily hold parsed frame header
//            private class CommonFrameHeaderInstance : ICommonFrameHeader
//            {
//                private byte m_packetNumber;
//                private short m_wordCount;
//                private ushort m_idCode;
//                private short m_sampleNumber;
//                private long m_ticks;
//                private Dictionary<string, string> m_attributes;
//                private object m_tag;

//                public CommonFrameHeaderInstance()
//                {
//                }

//                protected CommonFrameHeaderInstance(SerializationInfo info, StreamingContext context)
//                {
//                    throw (new NotImplementedException());
//                }

//                public System.Type DerivedType
//                {
//                    get
//                    {
//                        return this.GetType();
//                    }
//                }

//                public FrameType FrameType
//                {
//                    get
//                    {
//                        return ((FrameType)m_packetNumber == BpaPdcStream.FrameType.ConfigurationFrame ? BpaPdcStream.FrameType.ConfigurationFrame : BpaPdcStream.FrameType.DataFrame);
//                    }
//                }

//                FundamentalFrameType IChannelFrame.FrameType
//                {
//                    get
//                    {
//                        return this.FundamentalFrameType;
//                    }
//                }

//                public FundamentalFrameType FundamentalFrameType
//                {
//                    get
//                    {
//                        // Translate BPA PDCstream specific frame type to fundamental frame type
//                        switch (FrameType)
//                        {
//                            case BpaPdcStream.FrameType.ConfigurationFrame:
//                                return FundamentalFrameType.ConfigurationFrame;
//                            case BpaPdcStream.FrameType.DataFrame:
//                                return FundamentalFrameType.DataFrame;
//                            default:
//                                return FundamentalFrameType.Undetermined;
//                        }
//                    }
//                }

//                public ushort FrameLength
//                {
//                    get
//                    {
//                        return (ushort)(2 * m_wordCount);
//                    }
//                }

//                public byte PacketNumber
//                {
//                    get
//                    {
//                        return this.PacketFlag;
//                    }
//                    set
//                    {
//                        this.PacketFlag = value;
//                    }
//                }

//                public byte PacketFlag
//                {
//                    get
//                    {
//                        return m_packetNumber;
//                    }
//                    set
//                    {
//                        m_packetNumber = value;
//                    }
//                }

//                public short WordCount
//                {
//                    get
//                    {
//                        return m_wordCount;
//                    }
//                    set
//                    {
//                        m_wordCount = value;
//                    }
//                }

//                public short SampleNumber
//                {
//                    get
//                    {
//                        return m_sampleNumber;
//                    }
//                    set
//                    {
//                        m_sampleNumber = value;
//                    }
//                }

//                public ushort IDCode
//                {
//                    get
//                    {
//                        return m_idCode;
//                    }
//                    set
//                    {
//                        m_idCode = value;
//                    }
//                }

//                public long Ticks
//                {
//                    get
//                    {
//                        return m_ticks;
//                    }
//                    set
//                    {
//                        m_ticks = value;
//                    }
//                }

//                public IMeasurement LastSortedMeasurement
//                {
//                    get
//                    {
//                        return null;
//                    }
//                    set
//                    {
//                        throw (new NotImplementedException());
//                    }
//                }

//                public byte[] BinaryImage
//                {
//                    get
//                    {
//                        throw (new NotImplementedException());
//                    }
//                }

//                public ushort BinaryLength
//                {
//                    get
//                    {
//                        return 0;
//                    }
//                }

//                public void ParseBinaryImage(IChannelParsingState state, byte[] binaryImage, int startIndex)
//                {

//                    throw (new NotImplementedException());

//                }

//                public object Cells
//                {
//                    get
//                    {
//                        return null;
//                    }
//                }

//                public bool Published
//                {
//                    get
//                    {
//                        return false;
//                    }
//                    set
//                    {
//                        throw (new NotImplementedException());
//                    }
//                }

//                // This frame is not complete - it only represents the parsed common "header" for frames
//                public bool IsPartial
//                {
//                    get
//                    {
//                        return true;
//                    }
//                }

//                public DateTime Timestamp
//                {
//                    get
//                    {
//                        return new DateTime(m_ticks);
//                    }
//                }

//                public UnixTimeTag TimeTag
//                {
//                    get
//                    {
//                        return new UnixTimeTag(Timestamp);
//                    }
//                }

//                public bool Equals(IFrame other)
//                {

//                    return (CompareTo(other) == 0);

//                }

//                public int CompareTo(IFrame other)
//                {

//                    return m_ticks.CompareTo(other.Ticks);

//                }

//                public int CompareTo(object obj)
//                {
//                    IFrame other = obj as IFrame;
//                    if (other != null) return CompareTo(other);
//                    throw new ArgumentException("Frame can only be compared with other IFrames...");
//                }

//                IDictionary<MeasurementKey, IMeasurement> IFrame.Measurements
//                {
//                    get
//                    {
//                        return null;
//                    }
//                }

//                int IFrame.PublishedMeasurements
//                {
//                    get
//                    {
//                        return 0;
//                    }
//                    set
//                    {
//                        throw (new NotImplementedException());
//                    }
//                }

//                public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
//                {
//                    throw (new NotImplementedException());
//                }

//                public Dictionary<string, string> Attributes
//                {
//                    get
//                    {
//                        // Create a new attributes dictionary or clear the contents of any existing one
//                        if (m_attributes == null)
//                        {
//                            m_attributes = new Dictionary<string, string>();
//                        }
//                        else
//                        {
//                            m_attributes.Clear();
//                        }

//                        m_attributes.Add("Derived Type", DerivedType.Name);
//                        m_attributes.Add("Binary Length", BinaryLength.ToString());
//                        m_attributes.Add("Total Cells", "0");
//                        m_attributes.Add("Fundamental Frame Type", (int)FundamentalFrameType + ": " + FundamentalFrameType);
//                        m_attributes.Add("ID Code", IDCode.ToString());
//                        m_attributes.Add("Is Partial Frame", IsPartial.ToString());
//                        m_attributes.Add("Published", Published.ToString());
//                        m_attributes.Add("Ticks", Ticks.ToString());
//                        m_attributes.Add("Timestamp", Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));
//                        m_attributes.Add("Frame Type", (int)FrameType + ": " + FrameType);
//                        m_attributes.Add("Frame Length", FrameLength.ToString());
//                        m_attributes.Add("Packet Flag", m_packetNumber.ToString());
//                        m_attributes.Add("Word Count", m_wordCount.ToString());
//                        m_attributes.Add("Sample Number", m_sampleNumber.ToString());

//                        return m_attributes;
//                    }
//                }

//                public object Tag
//                {
//                    get
//                    {
//                        return m_tag;
//                    }
//                    set
//                    {
//                        m_tag = value;
//                    }
//                }

//            }

//            #endregion

//            public const ushort BinaryLength = 4;

//            private CommonFrameHeader()
//            {
//                // This class contains only global functions and is not meant to be instantiated
//            }

//            // Note: in order to parse timestamp from data frame, this parse procedure needs six more bytes above and beyond common frame header binary length
//            public static ICommonFrameHeader ParseBinaryImage(ConfigurationFrame configurationFrame, bool parseWordCountFromByte, byte[] binaryImage, int startIndex)
//            {
//                if (binaryImage[startIndex] != PhasorProtocols.Common.SyncByte)
//                {
//                    throw (new InvalidOperationException("Bad data stream, expected sync byte AA as first byte in BPA PDCstream frame, got " + binaryImage[startIndex].ToString("X").PadLeft(2, '0')));
//                }

//                CommonFrameHeaderInstance commonFrameHeader = new CommonFrameHeaderInstance();

//                // Parse out packet flags and word count information...
//                commonFrameHeader.PacketFlag = binaryImage[startIndex + 1];

//                // Some older streams have a bad word count (e.g., the NYISO data stream has a 0x01 as the third byte
//                // in the stream - this should be a 0x00 to make the word count come out correctly).  The following
//                // compensates for this erratic behavior
//                if (parseWordCountFromByte)
//                {
//                    commonFrameHeader.WordCount = binaryImage[startIndex + 3];
//                }
//                else
//                {
//                    commonFrameHeader.WordCount = EndianOrder.BigEndian.ToInt16(binaryImage, startIndex + 2);
//                }

//                if (commonFrameHeader.FrameType == FrameType.ConfigurationFrame)
//                {
//                    // We just assume current timestamp for configuration frames since they don't provide one
//                    commonFrameHeader.Ticks = DateTime.UtcNow.Ticks;
//                }
//                else
//                {
//                    // Next six bytes in data frame is the timestamp - so we go ahead and get it
//                    uint secondOfCentury = EndianOrder.BigEndian.ToUInt32(binaryImage, startIndex + 4);
//                    commonFrameHeader.SampleNumber = EndianOrder.BigEndian.ToInt16(binaryImage, startIndex + 8);

//                    if (configurationFrame == null)
//                    {
//                        // Until configuration is available, we make a guess at time tag type - this will just be
//                        // used for display purposes until a configuration frame arrives.  If second of century
//                        // is greater than 3155673600 (SOC value for NTP timestamp 1/1/2007), then this is likely
//                        // an NTP time stamp (else this is a Unix time tag for the year 2069 - not likely).
//                        if (secondOfCentury > 3155673600)
//                        {
//                            commonFrameHeader.Ticks = (new NtpTimeTag(secondOfCentury)).ToDateTime().Ticks;
//                        }
//                        else
//                        {
//                            commonFrameHeader.Ticks = (new UnixTimeTag(secondOfCentury)).ToDateTime().Ticks;
//                        }
//                    }
//                    else
//                    {
//                        if (configurationFrame.RevisionNumber == RevisionNumber.Revision0)
//                        {
//                            commonFrameHeader.Ticks = (new NtpTimeTag(secondOfCentury)).ToDateTime().Ticks + (commonFrameHeader.SampleNumber * (long)configurationFrame.TicksPerFrame);
//                        }
//                        else
//                        {
//                            commonFrameHeader.Ticks = (new UnixTimeTag(secondOfCentury)).ToDateTime().Ticks + (commonFrameHeader.SampleNumber * (long)configurationFrame.TicksPerFrame);
//                        }
//                    }
//                }

//                return (ICommonFrameHeader)commonFrameHeader;
//            }

//            public static byte[] BinaryImage(ICommonFrameHeader frameHeader)
//            {
//                byte[] buffer = new byte[BinaryLength];

//                buffer[0] = PhasorProtocols.Common.SyncByte;
//                buffer[1] = frameHeader.PacketNumber;
//                EndianOrder.BigEndian.CopyBytes(frameHeader.WordCount, buffer, 2);

//                return buffer;
//            }

//            public static void Clone(ICommonFrameHeader sourceFrameHeader, ICommonFrameHeader destinationFrameHeader)
//            {
//                destinationFrameHeader.PacketNumber = sourceFrameHeader.PacketNumber;
//                destinationFrameHeader.WordCount = sourceFrameHeader.WordCount;
//                destinationFrameHeader.Ticks = sourceFrameHeader.Ticks;
//                destinationFrameHeader.SampleNumber = sourceFrameHeader.SampleNumber;
//            }
//        }
//    }
//}
