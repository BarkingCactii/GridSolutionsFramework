//******************************************************************************************************
//  Common.cs - Gbtc
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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;

namespace GSF.PhasorProtocols.IEC61850_90_5_Goose
{
    #region [ Enumerations ]

    /// <summary>
    /// IEC 61850-90-5 frame types enumeration.
    /// </summary>
    [Serializable]
    public enum FrameType : ushort
    {
        /// <summary>
        /// 000 Data frame.
        /// </summary>
        DataFrame = (ushort)Bits.Nil,
        /// <summary>
        /// 011 Configuration frame.
        /// </summary>
        ConfigurationFrame = (ushort)(Bits.Bit04 | Bits.Bit05),
        /// <summary>
        /// 100 Command frame.
        /// </summary>
        CommandFrame = (ushort)Bits.Bit06,
        /// <summary>
        /// Version number mask.
        /// </summary>
        VersionNumberMask = (ushort)(Bits.Bit00 | Bits.Bit01 | Bits.Bit02 | Bits.Bit03)
    }

    /// <summary>
    /// IEC 61850-90-5 session types.
    /// </summary>
    public enum SessionType : byte
    {
        /// <summary>
        /// Tunnelled values.
        /// </summary>
        Tunnelled = 0xA0,
        /// <summary>
        /// Goose values.
        /// </summary>
        Goose = 0xA1,
        /// <summary>
        /// Sampled values.
        /// </summary>
        SampledValues = 0xA2
    }

    /// <summary>
    /// IEC 61850-90-5 signature algorithm.
    /// </summary>
    public enum SignatureAlgorithm : byte
    {
        /// <summary>
        /// No algorithm used
        /// </summary>
        None,
        /// <summary>
        /// SHA 256/80
        /// </summary>
        Sha80,
        /// <summary>
        /// SHA 256/128
        /// </summary>
        Sha128,
        /// <summary>
        /// SHA 256/256
        /// </summary>
        Sha256,
        /// <summary>
        /// AES GMAC 64
        /// </summary>
        Aes64,
        /// <summary>
        /// AES GMAC 128
        /// </summary>
        Aes128
    }

    /// <summary>
    /// IEC 61850-90-5 security algorithm.
    /// </summary>
    public enum SecurityAlgorithm : byte
    {
        /// <summary>
        /// No algorithm used
        /// </summary>
        None,
        /// <summary>
        /// AES 128
        /// </summary>
        Aes128,
        /// <summary>
        /// AES 256
        /// </summary>
        Aes256
    }

    /// <summary>
    /// Sampled value tags.
    /// </summary>
    public enum SampledValueTag : byte
    {
        /// <summary>
        /// Sampled value protocol data unit.
        /// </summary>
        SvPdu = 0x60,
        /// <summary>
        /// Number of ASDUs. 
        /// </summary>
        AsduCount = 0x80,
        /// <summary>
        /// Sequence of ASDU.
        /// </summary>
        SequenceOfAsdu = 0xA2,
        /// <summary>
        /// ASDU sequence number.
        /// </summary>
        AsduSequence = 0x30,
        /// <summary>
        /// Multicast sampled value identifier.
        /// </summary>
        MsvID = 0x80,
        /// <summary>
        /// Data set.
        /// </summary>
        Dataset = 0x81,
        /// <summary>
        /// Sample Count
        /// </summary>
        SmpCnt = 0x82,
        /// <summary>
        /// Configuration revision.
        /// </summary>
        ConfRev = 0x83,
        /// <summary>
        /// Local refresh time.
        /// </summary>
        RefrTm = 0x84,
        /// <summary>
        /// Samples are synchronized.
        /// </summary>
        SmpSynch = 0x85,
        /// <summary>
        /// Sample rate.
        /// </summary>
        SmpRate = 0x86,
        /// <summary>
        /// Data samples.
        /// </summary>
        Samples = 0x87,
        /// <summary>
        /// Sample mod.
        /// </summary>
        SmpMod = 0x88,
        /// <summary>
        /// UTC Timestamp.
        /// </summary>
        UtcTimestamp = 0x89
    }

    /// <summary>
    /// Goose tags.
    /// </summary>
    public enum GooseTag : byte
    {
        /// <summary>
        /// Goose protocol data unit.
        /// </summary>
        GPdu = 0x61,
        /// <summary>
        /// Reference to Goose Control Block
        /// </summary>
        GocbRef = 0x80,
        /// <summary>
        /// Time allowed to live
        /// </summary>
        TimeAllowedToLive = 0x81,
        /// <summary>
        /// Data Set
        /// </summary>
        DatSet = 0x82,
        /// <summary>
        /// Goose ID
        /// </summary>
        GoId = 0x83,
        /// <summary>
        /// Local refresh time.
        /// </summary>
        RefrTm = 0x84,
        /// <summary>
        /// State Number
        /// </summary>
        StNum = 0x85,
        /// <summary>
        /// Sequence Number
        /// </summary>
        SqNum = 0x86,
        /// <summary>
        /// Test.
        /// </summary>
        Test = 0x87,
        /// <summary>
        /// Configuration revision.
        /// </summary>
        ConfRev = 0x88,
        /// <summary>
        /// Needs commissioning.
        /// </summary>
        NdsCom = 0x89,
        /// <summary>
        /// Number of Data Set Entries
        /// </summary>
        NumDatSetEntries =  0x8A,
        /// <summary>
        /// ALl data
        /// </summary>
        AllData = 0xAB
    }


    ///
    ///
    ///
    public enum DataType : byte
    {
        /// <summary>
        /// Array.
        /// </summary>
        array = 0x81,
        /// <summary>
        /// Structure.
        /// </summary>
        structure = 0xA2,
        /// <summary>
        /// Boolean.
        /// </summary>
        boolean = 0x83,
        /// <summary>
        /// bit string.
        /// </summary>
        bitString = 0x84,
        /// <summary>
        /// Integer.
        /// </summary>
        integer = 0x85,
        /// <summary>
        /// Unsigned.
        /// </summary>
        unsigned = 0x86,
        /// <summary>
        /// Floating Point.
        /// </summary>
        floatingPoint = 0x87,
        /// <summary>
        /// Octet String.
        /// </summary>
        octetString = 0x89,
        /// <summary>
        /// Visible string.
        /// </summary>
        visibleStirng = 0x8A,
        /// <summary>
        /// Time of day.
        /// </summary>
        timeOfDay = 0x8C,
        /// <summary>
        /// BCD.
        /// </summary>
        BCD = 0x8D,
        /// <summary>
        /// Boolean Array.
        /// </summary>
        booleanArray = 0x8E,
        /// <summary>
        /// UTC Timestamp.
        /// </summary>
        utcTime = 0x91

    }

    public enum MeasurementType : byte
    {
        Flag = 0x01,
        Vpha = 0x02,
        Ipha = 0x03,
        Freq = 0x04,
        Dfdt = 0x05,
        Alog = 0x06,
        Digi = 0x07
    }

    /// <summary>
    /// Protocol draft revision numbers enumeration.
    /// </summary>
    [Serializable]
    public enum DraftRevision : byte
    {
        /// <summary>
        /// Draft 6.0.
        /// </summary>
        Draft6 = 0,
        /// <summary>
        /// Draft 7.0 (Version 1.0).
        /// </summary>
        Draft7 = 1
    }

    /// <summary>
    /// Data format flags enumeration.
    /// </summary>
    [Flags, Serializable]
    public enum FormatFlags : ushort
    {
        /// <summary>
        /// Frequency value format (Set = float, Clear = integer).
        /// </summary>
        Frequency = (ushort)Bits.Bit03,
        /// <summary>
        /// Analog value format (Set = float, Clear = integer).
        /// </summary>
        Analog = (ushort)Bits.Bit02,
        /// <summary>
        /// Phasor value format (Set = float, Clear = integer).
        /// </summary>
        Phasors = (ushort)Bits.Bit01,
        /// <summary>
        /// Phasor coordinate format (Set = polar, Clear = rectangular).
        /// </summary>
        Coordinates = (ushort)Bits.Bit00,
        /// <summary>
        /// Unsed format bits mask.
        /// </summary>
        UnusedMask = unchecked(ushort.MaxValue & (ushort)~(Bits.Bit00 | Bits.Bit01 | Bits.Bit02 | Bits.Bit03)),
        /// <summary>
        /// No flags.
        /// </summary>
        NoFlags = (ushort)Bits.Nil
    }

    /// <summary>
    /// Time quality flags enumeration.
    /// </summary>
    [Flags, Serializable]
    public enum TimeQualityFlags : uint
    {
        /// <summary>
        /// Reserved bit.
        /// </summary>
        Reserved = (uint)Bits.Bit31,
        /// <summary>
        /// Leap second direction – 0 for add, 1 for delete.
        /// </summary>
        LeapSecondDirection = (uint)Bits.Bit30,
        /// <summary>
        /// Leap second occurred – set in the first second after the leap second occurs and remains set for 24 hours.
        /// </summary>
        LeapSecondOccurred = (uint)Bits.Bit29,
        /// <summary>
        /// Leap second pending – set before a leap second occurs and cleared in the second after the leap second occurs.
        /// </summary>
        LeapSecondPending = (uint)Bits.Bit28,
        /// <summary>
        /// Time quality indicator code mask.
        /// </summary>
        TimeQualityIndicatorCodeMask = (uint)(Bits.Bit27 | Bits.Bit26 | Bits.Bit25 | Bits.Bit24),
        /// <summary>
        /// No flags.
        /// </summary>
        NoFlags = (uint)Bits.Nil
    }

    /// <summary>
    /// Time quality indicator code enumeration.
    /// </summary>
    [Serializable]
    public enum TimeQualityIndicatorCode : uint
    {
        /// <summary>
        /// 1111 - 0xF:	Fault--clock failure, time not reliable.
        /// </summary>
        Failure = (uint)(Bits.Bit27 | Bits.Bit26 | Bits.Bit25 | Bits.Bit24),
        /// <summary>
        /// 1011 - 0xB:	Clock unlocked, time within 10^1 s.
        /// </summary>
        UnlockedWithin10Seconds = (uint)(Bits.Bit27 | Bits.Bit25 | Bits.Bit24),
        /// <summary>
        /// 1010 - 0xA:	Clock unlocked, time within 10^0 s.
        /// </summary>
        UnlockedWithin1Second = (uint)(Bits.Bit27 | Bits.Bit25),
        /// <summary>
        /// 1001 - 0x9: Clock unlocked, time within 10^-1 s.
        /// </summary>
        UnlockedWithinPoint1Seconds = (uint)(Bits.Bit27 | Bits.Bit24),
        /// <summary>
        /// 1000 - 0x8: Clock unlocked, time within 10^-2 s.
        /// </summary>
        UnlockedWithinPoint01Seconds = (uint)Bits.Bit27,
        /// <summary>
        /// 0111 - 0x7: Clock unlocked, time within 10^-3 s.
        /// </summary>
        UnlockedWithinPoint001Seconds = (uint)(Bits.Bit26 | Bits.Bit25 | Bits.Bit24),
        /// <summary>
        /// 0110 - 0x6: Clock unlocked, time within 10^-4 s.
        /// </summary>
        UnlockedWithinPoint0001Seconds = (uint)(Bits.Bit26 | Bits.Bit25),
        /// <summary>
        /// 0101 - 0x5: Clock unlocked, time within 10^-5 s.
        /// </summary>
        UnlockedWithinPoint00001Seconds = (uint)(Bits.Bit26 | Bits.Bit24),
        /// <summary>
        /// 0100 - 0x4: Clock unlocked, time within 10^-6 s.
        /// </summary>
        UnlockedWithinPoint000001Seconds = (uint)Bits.Bit26,
        /// <summary>
        /// 0011 - 0x3: Clock unlocked, time within 10^-7 s.
        /// </summary>
        UnlockedWithinPoint0000001Seconds = (uint)(Bits.Bit25 | Bits.Bit24),
        /// <summary>
        /// 0010 - 0x2: Clock unlocked, time within 10^-8 s.
        /// </summary>
        UnlockedWithinPoint00000001Seconds = (uint)Bits.Bit25,
        /// <summary>
        /// 0001 - 0x1: Clock unlocked, time within 10^-9 s.
        /// </summary>
        UnlockedWithinPoint000000001Seconds = (uint)Bits.Bit24,
        /// <summary>
        /// 0000 - 0x0: Normal operation, clock locked.
        /// </summary>
        Locked = 0
    }

    /// <summary>
    /// Status flags enumeration.
    /// </summary>
    [Flags, Serializable]
    public enum StatusFlags : ushort
    {
        /// <summary>
        /// Data is valid (0 when device data is valid, 1 when invalid or device is in test mode).
        /// </summary>
        DataIsValid = (ushort)Bits.Bit15,
        /// <summary>
        /// Device error including configuration error, 0 when no error.
        /// </summary>
        DeviceError = (ushort)Bits.Bit14,
        /// <summary>
        /// Device synchronization error, 0 when in sync.
        /// </summary>
        DeviceSynchronizationError = (ushort)Bits.Bit13,
        /// <summary>
        /// Data sorting type, 0 by timestamp, 1 by arrival.
        /// </summary>
        DataSortingType = (ushort)Bits.Bit12,
        /// <summary>
        /// Device trigger detected, 0 when no trigger.
        /// </summary>
        DeviceTriggerDetected = (ushort)Bits.Bit11,
        /// <summary>
        /// Configuration changed, set to 1 for one minute when configuration changed.
        /// </summary>
        ConfigurationChanged = (ushort)Bits.Bit10,
        /// <summary>
        /// Reserved bits for security, presently set to 0.
        /// </summary>
        ReservedFlags = (ushort)(Bits.Bit09 | Bits.Bit08 | Bits.Bit07 | Bits.Bit06),
        /// <summary>
        /// Unlocked time mask.
        /// </summary>
        UnlockedTimeMask = (ushort)(Bits.Bit05 | Bits.Bit04),
        /// <summary>
        /// Trigger reason mask.
        /// </summary>
        TriggerReasonMask = (ushort)(Bits.Bit03 | Bits.Bit02 | Bits.Bit01 | Bits.Bit00),
        /// <summary>
        /// No flags.
        /// </summary>
        NoFlags = (ushort)Bits.Nil
    }

    /// <summary>
    /// Unlocked time enumeration.
    /// </summary>
    [Serializable]
    public enum UnlockedTime : byte
    {
        /// <summary>
        /// Sync locked, best quality.
        /// </summary>
        SyncLocked = (byte)Bits.Nil,
        /// <summary>
        /// Unlocked for 10 seconds.
        /// </summary>
        UnlockedFor10Seconds = (byte)Bits.Bit04,
        /// <summary>
        /// Unlocked for 100 seconds.
        /// </summary>
        UnlockedFor100Seconds = (byte)Bits.Bit05,
        /// <summary>
        /// Unlocked for over 1000 seconds.
        /// </summary>
        UnlockedForOver1000Seconds = (byte)(Bits.Bit05 | Bits.Bit04)
    }

    /// <summary>
    /// Trigger reason enumeration.
    /// </summary>
    [Serializable]
    public enum TriggerReason : byte
    {
        /// <summary>
        /// 1111 Vendor defined trigger 8.
        /// </summary>
        VendorDefinedTrigger8 = (byte)(Bits.Bit03 | Bits.Bit02 | Bits.Bit01 | Bits.Bit00),
        /// <summary>
        /// 1110 Vendor defined trigger 7.
        /// </summary>
        VendorDefinedTrigger7 = (byte)(Bits.Bit03 | Bits.Bit02 | Bits.Bit01),
        /// <summary>
        /// 1101 Vendor defined trigger 6.
        /// </summary>
        VendorDefinedTrigger6 = (byte)(Bits.Bit03 | Bits.Bit02 | Bits.Bit00),
        /// <summary>
        /// 1100 Vendor defined trigger 5.
        /// </summary>
        VendorDefinedTrigger5 = (byte)(Bits.Bit03 | Bits.Bit02),
        /// <summary>
        /// 1011 Vendor defined trigger 4.
        /// </summary>
        VendorDefinedTrigger4 = (byte)(Bits.Bit03 | Bits.Bit01 | Bits.Bit00),
        /// <summary>
        /// 1010 Vendor defined trigger 3.
        /// </summary>
        VendorDefinedTrigger3 = (byte)(Bits.Bit03 | Bits.Bit01),
        /// <summary>
        /// 1001 Vendor defined trigger 2.
        /// </summary>
        VendorDefinedTrigger2 = (byte)(Bits.Bit03 | Bits.Bit00),
        /// <summary>
        /// 1000 Vendor defined trigger 1.
        /// </summary>
        VendorDefinedTrigger1 = (byte)Bits.Bit03,
        /// <summary>
        /// 0111 Digital.
        /// </summary>
        Digital = (byte)(Bits.Bit02 | Bits.Bit01 | Bits.Bit00),
        /// <summary>
        /// 0101 df/dt high.
        /// </summary>
        DfDtHigh = (byte)(Bits.Bit02 | Bits.Bit00),
        /// <summary>
        /// 0011 Phase angle difference.
        /// </summary>
        PhaseAngleDifference = (byte)(Bits.Bit01 | Bits.Bit00),
        /// <summary>
        /// 0001 Magnitude low.
        /// </summary>
        MagnitudeLow = (byte)Bits.Bit00,
        /// <summary>
        /// 0110 Reserved.
        /// </summary>
        Reserved = (byte)(Bits.Bit02 | Bits.Bit01),
        /// <summary>
        /// 0100 Frequency high/low.
        /// </summary>
        FrequencyHighOrLow = (byte)Bits.Bit02,
        /// <summary>
        /// 0010 Magnitude high.
        /// </summary>
        MagnitudeHigh = (byte)Bits.Bit01,
        /// <summary>
        /// 0000 Manual.
        /// </summary>
        Manual = (byte)Bits.Nil
    }

    #endregion

    /// <summary>
    /// Common IEC 61850-90-5 declarations and functions.
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Timebase used by IEC 61850-90-5 protocol implementation.
        /// </summary>
        public const uint Timebase = 16777216;

        /// <summary>
        /// Marker for a connectionless transport protocol tag in IEC 61850-90-5 data frames.
        /// </summary>
        public const byte CltpTag = 0x40;

        /// <summary>
        /// Common session header size.
        /// </summary>
        public const byte SessionHeaderSize = 0x18;

        /// <summary>
        /// Size of keys.
        /// </summary>
        public const int KeySize = 4;

        /// <summary>
        /// Temporary key used by IEC 61850-90-5 draft implementations.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static byte[] DummyKey = 
        {
            0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x66, 0x77, 0x88,
            0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x66, 0x77, 0x88,
            0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x66, 0x77, 0x88,
            0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x66, 0x77, 0x88,
            0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x66, 0x77, 0x88,
            0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x66, 0x77, 0x88,
            0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x66, 0x77, 0x88,
            0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x01, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x66, 0x77, 0x88
        };

        /// <summary>
        /// Absolute maximum number of possible phasor values that could fit into a data frame.
        /// </summary>
        public const ushort MaximumPhasorValues = (ushort)(ushort.MaxValue / 4 - CommonFrameHeader.FixedLength - 8);

        /// <summary>
        /// Absolute maximum number of possible analog values that could fit into a data frame.
        /// </summary>
        public const ushort MaximumAnalogValues = (ushort)(ushort.MaxValue / 2 - CommonFrameHeader.FixedLength - 8);

        /// <summary>
        /// Absolute maximum number of possible digital values that could fit into a data frame.
        /// </summary>
        public const ushort MaximumDigitalValues = (ushort)(ushort.MaxValue / 2 - CommonFrameHeader.FixedLength - 8);

        /// <summary>
        /// Absolute maximum data length (in bytes) that could fit into any frame.
        /// </summary>
        public const ushort MaximumDataLength = (ushort)(ushort.MaxValue - CommonFrameHeader.FixedLength - 2);

        /// <summary>
        /// Absolute maximum number of bytes of extended data that could fit into a command frame.
        /// </summary>
        public const ushort MaximumExtendedDataLength = (ushort)(MaximumDataLength - 2);

        /// <summary>
        /// Time quality flags mask.
        /// </summary>
        public const uint TimeQualityFlagsMask = (uint)(Bits.Bit31 | Bits.Bit30 | Bits.Bit29 | Bits.Bit28 | Bits.Bit27 | Bits.Bit26 | Bits.Bit25 | Bits.Bit24);

        /// <summary>
        /// Validates sample value tag exists and skips past it.
        /// </summary>
        /// <param name="buffer">Buffer containing sampled value tag length.</param>
        /// <param name="tag">Sampled value tag to validate.</param>
        /// <param name="index">Start index of buffer where tag length begins - will be auto-incremented.</param>
        public static int ValidateTag(this byte[] buffer, SampledValueTag tag, ref int index)
        {
            if ((SampledValueTag)buffer[index] != tag)
                throw new InvalidOperationException("Encountered out-of-sequence or unknown sampled value tag: 0x" + buffer[index].ToString("X").PadLeft(2, '0'));

            index++;
            return buffer.ParseTagLength(ref index);
        }

        /// <summary>
        /// Validates data tag exists and skips past it.
        /// </summary>
        /// <param name="buffer">Buffer containing sampled value tag length.</param>
        /// <param name="tag">data tag to validate.</param>
        /// <param name="index">Start index of buffer where tag length begins - will be auto-incremented.</param>
        public static int ValidateTag(this byte[] buffer, DataType tag, ref int index)
        {
            Common.Dump(buffer, index, "ValidateTag Type", "Data type = " + tag.ToString(), "Index = " + index.ToString());

            if ((DataType )buffer[index] != tag)
                throw new InvalidOperationException("Encountered out-of-sequence or unknown data type tag: 0x" + buffer[index].ToString("X").PadLeft(2, '0'));

            index++;
            return buffer.ParseTagLength(ref index);
        }

        /// <summary>
        /// Validates data tag exists and skips past it.
        /// </summary>
        /// <param name="buffer">Buffer containing sampled value tag length.</param>
        /// <param name="tag">Sampled value tag to validate.</param>
        /// <param name="index">Start index of buffer where tag length begins - will be auto-incremented.</param>
        public static int ValidateTag(this byte[] buffer, GooseTag tag, ref int index)
        {
            Common.Dump(buffer, index, "ValidateTag()", "GooseTag = " + tag.ToString(), "Index = " + index.ToString());

            if ((GooseTag)buffer[index] != tag)
            {
                String output = String.Format("Encountered out-of-sequence or unknown goose tag: 0x{0} should be 0x{1}. Index {2}, Tag{3}, HexDump{4}",
                    buffer[index].ToString("X").PadLeft(2, '0'), tag.ToString("X").PadLeft(2, '0'),
                    index.ToString(), tag.ToString(), BitConverter.ToString(buffer).Replace("-", " "));

                Common.Dump(buffer, index, "Out of sequence, shoule be something else, parsing tag " + tag.ToString());

                throw new InvalidOperationException(output);
            }

            index++;
            int tagLength = buffer.ParseTagLength(ref index);
           // index += tagLength;
            return tagLength;
            //return buffer.ParseTagLength(ref index);
        }

        /// <summary>
        /// Validates data tag and skips past it.
        /// </summary>
        /// <param name="buffer">Buffer containing sampled value tag length.</param>
        /// <param name="tag">Sampled value tag to validate.</param>
        /// <param name="index">Start index of buffer where tag length begins - will be auto-incremented.</param>
        public static int SkipTag(this byte[] buffer, GooseTag tag, ref int index)
        {
            int tagLength = buffer.ValidateTag(tag, ref index);
           // "should be 30"

            index += tagLength;
            return tagLength;
        }

        /// <summary>
        /// Validates and parses byte length sample value tag.
        /// </summary>
        /// <param name="buffer">Buffer containing sampled value.</param>
        /// <param name="tag">Sampled value tag to parse.</param>
        /// <param name="index">Start index of buffer where tag length begins - will be auto-incremented.</param>
        public static byte ParseByteTag(this byte[] buffer, SampledValueTag tag, ref int index)
        {
            if ((SampledValueTag)buffer[index] != tag)
                throw new InvalidOperationException("Encountered out-of-sequence or unknown sampled value tag: 0x" + buffer[index].ToString("X").PadLeft(2, '0'));

            index++;
            int tagLength = buffer.ParseTagLength(ref index);

            if (tagLength < 1)
                throw new InvalidOperationException(string.Format("Unexpected length for \"{0}\" tag: {1}", tag, tagLength));

            byte result = buffer[index];
            index += tagLength;

            return result;
        }

        /// <summary>
        /// Validates and parses 2-byte length sample value tag.
        /// </summary>
        /// <param name="buffer">Buffer containing sampled value.</param>
        /// <param name="tag">Sampled value tag to parse.</param>
        /// <param name="index">Start index of buffer where tag length begins - will be auto-incremented.</param>
        public static ushort ParseUInt16Tag(this byte[] buffer, SampledValueTag tag, ref int index)
        {
            if ((SampledValueTag)buffer[index] != tag)
                throw new InvalidOperationException("Encountered out-of-sequence or unknown sampled value tag: 0x" + buffer[index].ToString("X").PadLeft(2, '0'));

            index++;
            int tagLength = buffer.ParseTagLength(ref index);

            if (tagLength < 2)
                throw new InvalidOperationException(string.Format("Unexpected length for \"{0}\" tag: {1}", tag, tagLength));

            ushort result = BigEndian.ToUInt16(buffer, index);
            index += tagLength;

            return result;
        }

        /// <summary>
        /// Validates and parses 3-byte length sample value tag.
        /// </summary>
        /// <param name="buffer">Buffer containing sampled value.</param>
        /// <param name="tag">Sampled value tag to parse.</param>
        /// <param name="index">Start index of buffer where tag length begins - will be auto-incremented.</param>
        public static UInt24 ParseUInt24Tag(this byte[] buffer, SampledValueTag tag, ref int index)
        {
            if ((SampledValueTag)buffer[index] != tag)
                throw new InvalidOperationException("Encountered out-of-sequence or unknown sampled value tag: 0x" + buffer[index].ToString("X").PadLeft(2, '0'));

            index++;
            int tagLength = buffer.ParseTagLength(ref index);

            if (tagLength < 3)
                throw new InvalidOperationException(string.Format("Unexpected length for \"{0}\" tag: {1}", tag, tagLength));

            UInt24 result = BigEndian.ToUInt24(buffer, index);
            index += tagLength;

            return result;
        }

        /// <summary>
        /// Validates and parses 4-byte length sample value tag.
        /// </summary>
        /// <param name="buffer">Buffer containing sampled value.</param>
        /// <param name="tag">Sampled value tag to parse.</param>
        /// <param name="index">Start index of buffer where tag length begins - will be auto-incremented.</param>
        public static uint ParseUInt32Tag(this byte[] buffer, SampledValueTag tag, ref int index)
        {
            if ((SampledValueTag)buffer[index] != tag)
                throw new InvalidOperationException("Encountered out-of-sequence or unknown sampled value tag: 0x" + buffer[index].ToString("X").PadLeft(2, '0'));

            index++;
            int tagLength = buffer.ParseTagLength(ref index);

            if (tagLength < 4)
                throw new InvalidOperationException(string.Format("Unexpected length for \"{0}\" tag: {1}", tag, tagLength));

            uint result = BigEndian.ToUInt32(buffer, index);
            index += tagLength;

            return result;
        }

        /// <summary>
        /// Validates and parses 8-byte length sample value tag.
        /// </summary>
        /// <param name="buffer">Buffer containing sampled value.</param>
        /// <param name="tag">Sampled value tag to parse.</param>
        /// <param name="index">Start index of buffer where tag length begins - will be auto-incremented.</param>
        public static ulong ParseUInt64Tag(this byte[] buffer, SampledValueTag tag, ref int index)
        {
            if ((SampledValueTag)buffer[index] != tag)
                throw new InvalidOperationException("Encountered out-of-sequence or unknown sampled value tag: 0x" + buffer[index].ToString("X").PadLeft(2, '0'));

            index++;
            int tagLength = buffer.ParseTagLength(ref index);

            if (tagLength < 8)
                throw new InvalidOperationException(string.Format("Unexpected length for \"{0}\" tag: {1}", tag, tagLength));

            ulong result = BigEndian.ToUInt64(buffer, index);
            index += tagLength;

            return result;
        }

        /// <summary>
        /// Validates and parses string sample value tag.
        /// </summary>
        /// <param name="buffer">Buffer containing sampled value.</param>
        /// <param name="tag">Sampled value tag to parse.</param>
        /// <param name="index">Start index of buffer where tag length begins - will be auto-incremented.</param>
        public static string ParseStringTag(this byte[] buffer, SampledValueTag tag, ref int index)
        {
            if ((SampledValueTag)buffer[index] != tag)
                throw new InvalidOperationException("Encountered out-of-sequence or unknown sampled value tag: 0x" + buffer[index].ToString("X").PadLeft(2, '0'));

            index++;
            int tagLength = buffer.ParseTagLength(ref index);

            string result = Encoding.ASCII.GetString(buffer, index, tagLength);
            index += tagLength;

            return result;
        }

        /// <summary>
        /// Encodes sampled value tag with only a 16-bit length.
        /// </summary>
        /// <param name="length">Value to encode.</param>
        /// <param name="tag">Sampled value tag to encode.</param>
        /// <param name="buffer">Buffer to hold encoded sampled value.</param>
        /// <param name="index">Start index of buffer where tag will begin - will be auto-incremented.</param>
        public static void EncodeTagLength(this ushort length, SampledValueTag tag, byte[] buffer, ref int index)
        {
            buffer[index++] = (byte)tag;
            buffer[index++] = 0x80 | 2;
            buffer[index++] = (byte)((length & 0xFF00) >> 8);
            buffer[index++] = (byte)(length & 0x00FF);
        }
        /// <summary>
        /// Encodes primitive type sampled value tag.
        /// </summary>
        /// <param name="value">Value to encode.</param>
        /// <param name="tag">Sampled value tag to encode.</param>
        /// <param name="buffer">Buffer to hold encoded sampled value.</param>
        /// <param name="index">Start index of buffer where tag will begin - will be auto-incremented.</param>
        public static void EncodeTagValue<T>(this T value, SampledValueTag tag, byte[] buffer, ref int index) where T : struct, IConvertible
        {
            if (!typeof(T).IsPrimitive)
                throw new ArgumentException("Value type is not primitive", nameof(value));

            // Not sure if booleans would be encoded correctly here (due to Marshal sizeof) - also not sure
            // how IEC 61850 deals with booleans - as a result, booleans should likely be avoided.
            // I wonder if compiler is smart enough to exclude this expression in imlementations since this
            // is always false for non boolean types - where is my WHERE expression like "~bool"...
            if (typeof(T) == typeof(bool))
                throw new ArgumentOutOfRangeException(nameof(value), "Boolean encoding is currently not supported");

            ushort length = (ushort)Marshal.SizeOf(typeof(T));

            buffer[index++] = (byte)tag;
            length.EncodeTagLength(buffer, ref index);
            index += BigEndian.CopyBytes(value, buffer, index);
        }

        /// <summary>
        /// Encodes byte based sampled value tag.
        /// </summary>
        /// <param name="value">Value to encode.</param>
        /// <param name="tag">Sampled value tag to encode.</param>
        /// <param name="buffer">Buffer to hold encoded sampled value.</param>
        /// <param name="index">Start index of buffer where tag will begin - will be auto-incremented.</param>
        public static void EncodeTagValue(this byte value, SampledValueTag tag, byte[] buffer, ref int index)
        {
            const ushort length = 1;
            buffer[index++] = (byte)tag;
            length.EncodeTagLength(buffer, ref index);
            buffer[index++] = value;
        }

        /// <summary>
        /// Encodes string based sampled value tag.
        /// </summary>
        /// <param name="value">String to encode - null string will be encoded as empty string.</param>
        /// <param name="tag">Sampled value tag to encode.</param>
        /// <param name="buffer">Buffer to hold encoded sampled value.</param>
        /// <param name="index">Start index of buffer where tag will begin - will be auto-incremented.</param>
        public static void EncodeTagValue(this string value, SampledValueTag tag, byte[] buffer, ref int index)
        {
            if ((object)value == null)
                value = "";

            if (value.Length > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), "Current implementation will not encode a string larger than " + ushort.MaxValue);

            ushort length = (ushort)value.Length;

            buffer[index++] = (byte)tag;
            length.EncodeTagLength(buffer, ref index);

            if (length > 0)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, buffer, index, bytes.Length);
                index += bytes.Length;
            }
        }

        /// <summary>
        /// Extracts data froom goose strucutre to remove T-L-V elements
        /// </summary>
        /// <param name="buffer">Buffer containing goose message.</param>
        /// <param name="index">Start index of buffer where data begins</param>=
        /// <returns>Byte array containing recorded data without tags or lengths</returns>
        public static byte[] ExtractGooseData(this byte[] buffer, int idx, int length)
        {
            int startIndex = idx;
            Common.Dump(buffer, startIndex, "ExtractGooseData()", "Index = " + startIndex.ToString() + " length = " + length.ToString());
            // Create list to store data
            List<byte> gooseData = new List<byte>();

            // Form array of data types for comparison
            DataType[] dataType = Enum.GetValues(typeof(DataType)).Cast<DataType>().ToArray();

            int tlvIdx = 0;
            // Loop through to buffers end
            for ( int i = startIndex; i < startIndex + length; i++ )
            {
                foreach (DataType type in dataType)
                {
                    // If the tag is a structure, skip it.
                    // note this is not a good method of dealing with this,
                    // since the config file will be specified that this data is beind sent
                    if ((DataType)buffer[i] == DataType.structure)
                    {
                        int tagLength = buffer.ValidateTag(DataType.structure, ref i);
                        i += tagLength - 1;

                        DataFrame.gooseDataConfiguration[tlvIdx].Length = tagLength;
                        tlvIdx++;
                        break;
                    }
                    // Check if data type matches
                    else if ((DataType)buffer[i] == type)
                    {
                        // Acquire data length
                        int tagLength = buffer.ValidateTag(type, ref i);

                        //   if (type == DataType.boolean)
                        //      tagLength = 1;

                        // set the real length now
                        DataFrame.gooseDataConfiguration[tlvIdx].Length = tagLength;
                        tlvIdx++;

                        // Add data to list
                        for (int j = i; j < tagLength + i; j++)
                        {
                            gooseData.Add(buffer[j]);
                        }

                        i += tagLength - 1;
                        break;
                    }
                    else if (type == dataType.Last())
                    {
                        throw new InvalidOperationException("Encountered unknown data tag: 0x" + buffer[i].ToString("X").PadLeft(2, '0'));
                    }
                }
            }
            // Return byte array for parsing
            return gooseData.ToArray();
        }

        /// <summary>
        /// Gets decoded sample value tag length (currently limited to 16-bits).
        /// </summary>
        /// <param name="buffer">Buffer containing sampled value tag length.</param>
        /// <param name="index">Start index of buffer where tag length begins - will be auto-incremented.</param>
        /// <returns>Decoded sample value tag length.</returns>
        public static int ParseTagLength(this byte[] buffer, ref int index)
        {
            // See if high bit is set
            if ((buffer[index] & (byte)Bits.Bit07) > 0)
            {
                // Odd attempt at 7-bit encoding? Seems like a waste of bits for the
                // benefit of allowing variable length encoded 56-bit integers...
                switch (buffer[index++] & 0x7F)
                {
                    case 1:
                        return buffer[index++];
                    case 2:
                        return ((buffer[index++] & 0xFF) << 8) | (buffer[index++] & 0xFF);
                    default:
                        return 0;
                }
            }
            return buffer[index++];
        }

        /// <summary>
        /// Encodes sample value tag length (currently limited to 16-bits).
        /// </summary>
        /// <param name="length">Sample value tag length.</param>
        /// <param name="buffer">Buffer to hold encoded sampled value tag length.</param>
        /// <param name="index">Start index of buffer where tag length encoding begins - will be auto-incremented.</param>
        public static void EncodeTagLength(this ushort length, byte[] buffer, ref int index)
        {
            if (length > 0x7F)
            {
                if (length > 0xFF)
                {
                    // 16-bit length value
                    buffer[index++] = 0x80 | 2;
                    buffer[index++] = (byte)((length & 0xFF00) >> 8);
                    buffer[index++] = (byte)(length & 0x00FF);
                }
                else
                {
                    // 8-bit length value > 127
                    buffer[index++] = 0x80 | 1;
                    buffer[index++] = (byte)(length & 0xFF);
                }
            }
            else
            {
                // 8-bit length value < 128
                buffer[index++] = (byte)(length & 0xFF);
            }
        }

#if NojaDebug

        static int delay = 10;

        private static String TimeStamp( out int threadId )
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
            return String.Format("{0} ({1}): ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), Thread.CurrentThread.ManagedThreadId);
        }

        private static String CreateFile(int threadId)
        {
            String fileName = String.Format("debug-{0}.txt", threadId);
            if ( File.Exists(fileName)) {
                DateTime fileTimeStamp = File.GetLastWriteTime(fileName);
                TimeSpan timeDiff = DateTime.Now - fileTimeStamp;
                if ( timeDiff.TotalSeconds > 300 )
                {
                    File.Delete(fileName);
                }
            }
            return fileName;
        }

        public static void Dump(byte[] binary)
        {
            int numberTries = 10;
            string outputStr = "";

            while (numberTries >= 0)
            {
                try
                {
                    for (int i = 0; i < binary.Length; i++)
                    {
                        outputStr += String.Format("{0} {1}", BitConverter.ToString(binary, i, 1), ((i + 1) % 16) == 0 ? Environment.NewLine : "");
                    }
                    outputStr += Environment.NewLine;
                    int threadId = 0;
                    outputStr = Common.TimeStamp(out threadId) + outputStr;
                    File.AppendAllText(Common.CreateFile(threadId), outputStr);
                    return;
                }
                catch
                {
                    // file is probably locked
                    numberTries--;
                    Thread.Sleep(delay);

                }
            }

        }

        public static void Dump(String str)
        {
            int numberTries = 10;

            while (numberTries >= 0)
            {
                try
                {
                    int threadId = 0;
                    String outputStr = Common.TimeStamp(out threadId) + str + Environment.NewLine;
                    File.AppendAllText(Common.CreateFile(threadId), outputStr);

//                    File.AppendAllText("jeff.txt", Common.TimeStamp() + str + "\n");
                    return;
                }
                catch
                {
                    // file is probably locked
                    numberTries--;
                    Thread.Sleep(delay);

                }
            }
        }

        public static void Dump(byte[] binary, int index, params string[] args)
        {
            int numberTries = 10;

            string outputStr = "";

            while (numberTries >= 0)
            {
                try
                {
                    if (args != null)
                    {
                        for (int i = 0; i < args.Length; i++)
                        {
                            String str = args[i];
                            outputStr += str + ", ";
                        }
                    }

                    if (binary != null )
                    {
                        for (int i = 0; i < binary.Length; i++ )
                        {
                            String spacer = (i == index - 1) ? "=>" : ((i == index) ? "<=" : " ");
                            outputStr += String.Format("{0}{1}{2}", BitConverter.ToString(binary, i, 1), spacer, ((i+1) % 16) == 0 ? Environment.NewLine : "");
                        }
                        //outputStr += BitConverter.ToString(binary).Replace("-", " ") + "\n";
                        outputStr += Environment.NewLine;
                    }

                    int threadId = 0;
                    outputStr = Common.TimeStamp(out threadId) + outputStr;
                    File.AppendAllText(Common.CreateFile(threadId), outputStr);

                    //File.AppendAllText("jeff.txt", Common.TimeStamp() + outputStr);
                    return;
                }
                catch
                {
                    // file is probably locked
                    numberTries--;
                    Thread.Sleep(delay);
                }
            }
        }
#endif
    }
}