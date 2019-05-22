//******************************************************************************************************
//  DataCell.cs - Gbtc
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
using System.Runtime.Serialization;

namespace GSF.PhasorProtocols.IEC61850_90_5_Goose
{
    /// <summary>
    /// Represents the IEC 61850-90-5 implementation of a <see cref="IDataCell"/> that can be sent or received.
    /// </summary>
    [Serializable]
    public class DataCell : DataCellBase
    {
        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="DataCell"/>.
        /// </summary>
        /// <param name="parent">The reference to parent <see cref="IDataFrame"/> of this <see cref="DataCell"/>.</param>
        /// <param name="configurationCell">The <see cref="IConfigurationCell"/> associated with this <see cref="DataCell"/>.</param>
        public DataCell(IDataFrame parent, IConfigurationCell configurationCell)
            : base(parent, configurationCell, 0x0000, Common.MaximumPhasorValues, Common.MaximumAnalogValues, Common.MaximumDigitalValues)
        {
            // Define new parsing state which defines constructors for key data values
            State = new DataCellParsingState(
                configurationCell,
                PhasorValue.CreateNewValue,
                IEC61850_90_5_Goose.FrequencyValue.CreateNewVariableValue, //.CreateNewVariValue.CreateNewVariValue,//.CreateNewValue,
                AnalogValue.CreateNewValue,
                DigitalValue.CreateNewValue);
        }

        /// <summary>
        /// Creates a new <see cref="DataCell"/> from specified parameters.
        /// </summary>
        /// <param name="parent">The reference to parent <see cref="DataFrame"/> of this <see cref="DataCell"/>.</param>
        /// <param name="configurationCell">The <see cref="ConfigurationCell"/> associated with this <see cref="DataCell"/>.</param>
        /// <param name="addEmptyValues">If <c>true</c>, adds empty values for each defined configuration cell definition.</param>
        public DataCell(DataFrame parent, ConfigurationCell configurationCell, bool addEmptyValues)
            : this(parent, configurationCell)
        {
            if (addEmptyValues)
            {
                int x;

                // Define needed phasor values
                for (x = 0; x < configurationCell.PhasorDefinitions.Count; x++)
                {
                    PhasorValues.Add(new PhasorValue(this, configurationCell.PhasorDefinitions[x]));
                }

                // Define a frequency and df/dt
                FrequencyValue = new FrequencyValue(this, configurationCell.FrequencyDefinition);

                // Define any analog values
                for (x = 0; x < configurationCell.AnalogDefinitions.Count; x++)
                {
                    AnalogValues.Add(new AnalogValue(this, configurationCell.AnalogDefinitions[x]));
                }

                // Define any digital values
                for (x = 0; x < configurationCell.DigitalDefinitions.Count; x++)
                {
                    DigitalValues.Add(new DigitalValue(this, configurationCell.DigitalDefinitions[x]));
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="DataCell"/> from serialization parameters.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> with populated with data.</param>
        /// <param name="context">The source <see cref="StreamingContext"/> for this deserialization.</param>
        protected DataCell(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the reference to parent <see cref="DataFrame"/> of this <see cref="DataCell"/>.
        /// </summary>
        public new DataFrame Parent
        {
            get
            {
                return base.Parent as DataFrame;
            }
            set
            {
                base.Parent = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ConfigurationCell"/> associated with this <see cref="DataCell"/>.
        /// </summary>
        public new ConfigurationCell ConfigurationCell
        {
            get
            {
                return base.ConfigurationCell as ConfigurationCell;
            }
            set
            {
                base.ConfigurationCell = value;
            }
        }

        /// <summary>
        /// Gets or sets status flags for this <see cref="DataCell"/>.
        /// </summary>
        public new StatusFlags StatusFlags
        {
            get
            {
                return (StatusFlags)(base.StatusFlags & ~(ushort)(StatusFlags.UnlockedTimeMask | StatusFlags.TriggerReasonMask));
            }
            set
            {
                base.StatusFlags = (ushort)((base.StatusFlags & (ushort)(StatusFlags.UnlockedTimeMask | StatusFlags.TriggerReasonMask)) | (ushort)value);
            }
        }

        /// <summary>
        /// Gets or sets unlocked time of this <see cref="DataCell"/>.
        /// </summary>
        public UnlockedTime UnlockedTime
        {
            get
            {
                return (UnlockedTime)(base.StatusFlags & (ushort)StatusFlags.UnlockedTimeMask);
            }
            set
            {
                base.StatusFlags = (ushort)((base.StatusFlags & ~(ushort)StatusFlags.UnlockedTimeMask) | (ushort)value);
                SynchronizationIsValid = (value == UnlockedTime.SyncLocked);
            }
        }

        /// <summary>
        /// Gets or sets trigger reason of this <see cref="DataCell"/>.
        /// </summary>
        public TriggerReason TriggerReason
        {
            get
            {
                return (TriggerReason)(base.StatusFlags & (short)StatusFlags.TriggerReasonMask);
            }
            set
            {
                base.StatusFlags = (ushort)((base.StatusFlags & ~(short)StatusFlags.TriggerReasonMask) | (ushort)value);
                DeviceTriggerDetected = (value != TriggerReason.Manual);
            }
        }

        /// <summary>
        /// Gets or sets flag that determines if data of this <see cref="DataCell"/> is valid.
        /// </summary>
        public override bool DataIsValid
        {
            get
            {
                // TODO: Should data be considered invalid when signature check fails? On my test device this would always be invalid since SHA is not being calculated...
                return (StatusFlags & StatusFlags.DataIsValid) == 0;
            }
            set
            {
                if (value)
                    StatusFlags = StatusFlags & ~StatusFlags.DataIsValid;
                else
                    StatusFlags = StatusFlags | StatusFlags.DataIsValid;
            }
        }

        /// <summary>
        /// Gets or sets flag that determines if timestamp of this <see cref="DataCell"/> is valid based on GPS lock.
        /// </summary>
        public override bool SynchronizationIsValid
        {
            get
            {
                // TODO: Not sure which synchronization flag should take priority here - so using both for now...
                return (StatusFlags & StatusFlags.DeviceSynchronizationError) == 0 && Parent.SampleSynchronization > 0;
            }
            set
            {
                if (value)
                    StatusFlags = StatusFlags & ~StatusFlags.DeviceSynchronizationError;
                else
                    StatusFlags = StatusFlags | StatusFlags.DeviceSynchronizationError;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="GSF.PhasorProtocols.DataSortingType"/> of this <see cref="DataCell"/>.
        /// </summary>
        public override DataSortingType DataSortingType
        {
            get
            {
                return (((StatusFlags & StatusFlags.DataSortingType) == 0) ? DataSortingType.ByTimestamp : DataSortingType.ByArrival);
            }
            set
            {
                if (value == DataSortingType.ByTimestamp)
                    StatusFlags = StatusFlags & ~StatusFlags.DataSortingType;
                else
                    StatusFlags = StatusFlags | StatusFlags.DataSortingType;
            }
        }

        /// <summary>
        /// Gets or sets flag that determines if source device of this <see cref="DataCell"/> is reporting an error.
        /// </summary>
        public override bool DeviceError
        {
            get
            {
                return (StatusFlags & StatusFlags.DeviceError) > 0;
            }
            set
            {
                if (value)
                    StatusFlags = StatusFlags | StatusFlags.DeviceError;
                else
                    StatusFlags = StatusFlags & ~StatusFlags.DeviceError;
            }
        }

        /// <summary>
        /// Gets or sets flag that determines if device trigger is detected for this <see cref="DataCell"/>.
        /// </summary>
        public bool DeviceTriggerDetected
        {
            get
            {
                return (StatusFlags & StatusFlags.DeviceTriggerDetected) > 0;
            }
            set
            {
                if (value)
                    StatusFlags = StatusFlags | StatusFlags.DeviceTriggerDetected;
                else
                    StatusFlags = StatusFlags & ~StatusFlags.DeviceTriggerDetected;
            }
        }

        /// <summary>
        /// Gets or sets flag that determines if configuration change was detected for this <see cref="DataCell"/>.
        /// </summary>
        public bool ConfigurationChangeDetected
        {
            get
            {
                return (StatusFlags & StatusFlags.ConfigurationChanged) > 0;
            }
            set
            {
                if (value)
                    StatusFlags = StatusFlags | StatusFlags.ConfigurationChanged;
                else
                    StatusFlags = StatusFlags & ~StatusFlags.ConfigurationChanged;
            }
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}"/> of string based property names and values for the <see cref="ChannelFrameBase{T}"/> object.
        /// </summary>
        public override Dictionary<string, string> Attributes
        {
            get
            {
                Dictionary<string, string> baseAttributes = base.Attributes;

                baseAttributes.Add("Unlocked Time", (int)UnlockedTime + ": " + UnlockedTime);
                baseAttributes.Add("Device Trigger Detected", DeviceTriggerDetected.ToString());
                baseAttributes.Add("Trigger Reason", (int)TriggerReason + ": " + TriggerReason);
                baseAttributes.Add("Configuration Change Detected", ConfigurationChangeDetected.ToString());

                return baseAttributes;
            }
        }

        #endregion

        #region [ Static ]

        // Static Methods

        // Delegate handler to create a new IEC 61850-90-5 data cell
        internal static IDataCell CreateNewCell(IChannelFrame parent, IChannelFrameParsingState<IDataCell> state, int index, byte[] buffer, int startIndex, out int parsedLength)
        {
            IDataFrameParsingState parsingState = state as IDataFrameParsingState;
            IConfigurationCell configurationCell = null;

            // With or without an associated configuration, we'll parse the data cell
            if (parsingState != null && parsingState.ConfigurationFrame != null)
                configurationCell = parsingState.ConfigurationFrame.Cells[index];

            IEC61850_90_5_Goose.DataCell dataCell = new IEC61850_90_5_Goose.DataCell(parent as IDataFrame, configurationCell);

#if NojaDebug
            Common.Dump(buffer, 18, "Frequency");
#endif
            parsedLength = dataCell.ParseBinaryImage(buffer, startIndex, 0);
         //   parsedLength = dataCell.ParseBodyImage(buffer, startIndex, 0);

            return dataCell;
        }

        #endregion

        /// <summary>
        /// Initializes object by parsing the specified <paramref name="buffer"/> containing a binary image.
        /// </summary>
        /// <param name="buffer">Buffer containing binary image to parse.</param>
        /// <param name="startIndex">0-based starting index in the <paramref name="buffer"/> to start parsing.</param>
        /// <param name="length">Valid number of bytes within <paramref name="buffer"/> from <paramref name="startIndex"/>.</param>
        /// <returns>The number of bytes used for initialization in the <paramref name="buffer"/> (i.e., the number of bytes parsed).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="startIndex"/> or <paramref name="length"/> is less than 0 -or- 
        /// <paramref name="startIndex"/> and <paramref name="length"/> will exceed <paramref name="buffer"/> length.
        /// </exception>
        /// <remarks>
        /// This method is not typically overridden since it is parses the header, body and footer images in sequence.
        /// </remarks>
        public override int ParseBinaryImage(byte[] buffer, int startIndex, int length)
        {
            buffer.ValidateParameters(startIndex, length);

            int index = startIndex;

            // Parse out header, body and footer images
            index += ParseHeaderImage(buffer, index, length);
            index += ParseBodyImage(buffer, index, length - (index - startIndex));
            index += ParseFooterImage(buffer, index, length - (index - startIndex));

            return (index - startIndex);
        }

        /// <summary>
        /// Parses the binary body image.
        /// </summary>
        /// <param name="buffer">Binary image to parse.</param>
        /// <param name="startIndex">Start index into <paramref name="buffer"/> to begin parsing.</param>
        /// <param name="length">Length of valid data within <paramref name="buffer"/>.</param>
        /// <returns>The length of the data that was parsed.</returns>
        protected override int ParseBodyImage(byte[] buffer, int startIndex, int length)
        {
            // Length is validated at a frame level well in advance so that low level parsing routines do not have
            // to re-validate that enough length is available to parse needed information as an optimization...
            IConfigurationCell m_configurationCell = ConfigurationCell;
            IDataCellParsingState parsingState = State;
            IPhasorValue phasorValue;
            IAnalogValue analogValue;
            IDigitalValue digitalValue;
            IFrequencyValue frequencyValue;

            int x, parsedLength, index = startIndex;
            int alogIdx = 0, digiIdx = 0, phasorIdx = 0;


            foreach ( IEC61850_90_5_Goose.DataFrame.TLV tlv in IEC61850_90_5_Goose.DataFrame.gooseDataConfiguration )
            {
                switch (tlv.MeasurementType) {
                    case MeasurementType.Alog:
                        analogValue = parsingState.CreateNewAnalogValue(this, m_configurationCell.AnalogDefinitions[alogIdx], buffer, index, out parsedLength);
                        alogIdx++;
                        index += tlv.Length;
                        // assign to base class
                        AnalogValues.Add(analogValue);
                        break;
                    case MeasurementType.Dfdt:
                        frequencyValue = parsingState.CreateNewVariableFrequencyValue(this, m_configurationCell.FrequencyDefinition, buffer, index, tlv.Length);
                        index += tlv.Length;
                        // assign to base class
                      //  FrequencyValue = frequencyValue;
                        break;
                    case MeasurementType.Digi:
                        digitalValue = parsingState.CreateNewDigitalValue(this, m_configurationCell.DigitalDefinitions[digiIdx], buffer, index, out parsedLength);
                        digiIdx++;
                        index += tlv.Length;
                        // assign to base class
                        DigitalValues.Add(digitalValue);
                        break;
                    case MeasurementType.Flag:
                        index += tlv.Length;
                        break;
                    case MeasurementType.Freq:
                        { 
                        int tmp = tlv.Length;
                        frequencyValue = parsingState.CreateNewVariableFrequencyValue(this, m_configurationCell.FrequencyDefinition, buffer, index, tlv.Length);//out parsedLength);
                        index += tlv.Length;
                        // assign to base class
                        FrequencyValue = frequencyValue;
                        break;
                        }
                    case MeasurementType.Ipha:
                        phasorValue = parsingState.CreateNewPhasorValue(this, m_configurationCell.PhasorDefinitions[phasorIdx], buffer, index, out parsedLength);
                        index += tlv.Length;
                        // assign to base class
                        PhasorValues.Add( phasorValue );
                        break;
                    case MeasurementType.Vpha:
                        phasorValue = parsingState.CreateNewPhasorValue(this, m_configurationCell.PhasorDefinitions[phasorIdx], buffer, index, out parsedLength);
                        index += tlv.Length;
                        // assign to base class
                        PhasorValues.Add(phasorValue);
                        break;
                }
            }

            return index;

            /*
            IDataCellParsingState parsingState = State;
            IPhasorValue phasorValue;
            IAnalogValue analogValue;
            IDigitalValue digitalValue;
            int x, parsedLength, index = startIndex;

            StatusFlags = BigEndian.ToUInt16(buffer, startIndex);
            index += 2;

            // By the very nature of the major phasor protocols supporting the same order of phasors, frequency, df/dt, analog and digitals
            // we are able to "automatically" parse this data out in the data cell base class - BEAUTIFUL!!!

            // Parse out phasor values
            for (x = 0; x < parsingState.PhasorCount; x++)
            {
                phasorValue = parsingState.CreateNewPhasorValue(this, m_configurationCell.PhasorDefinitions[x], buffer, index, out parsedLength);
                m_phasorValues.Add(phasorValue);
                index += parsedLength;
            }

            // Parse out frequency and dF/dt values
            m_frequencyValue = parsingState.CreateNewFrequencyValue(this, m_configurationCell.FrequencyDefinition, buffer, index, out parsedLength);
#if NojaDebug
            //Random random = new Random();
            // m_frequencyValue.Frequency = random.NextDouble() * (55.0f - 45.0f) + 45.0f;
#endif
            index += parsedLength;

            // Parse out analog values
            for (x = 0; x < parsingState.AnalogCount; x++)
            {
                analogValue = parsingState.CreateNewAnalogValue(this, m_configurationCell.AnalogDefinitions[x], buffer, index, out parsedLength);
                m_analogValues.Add(analogValue);
                index += parsedLength;
            }

            // Parse out digital values
            for (x = 0; x < parsingState.DigitalCount; x++)
            {
                digitalValue = parsingState.CreateNewDigitalValue(this, m_configurationCell.DigitalDefinitions[x], buffer, index, out parsedLength);
                m_digitalValues.Add(digitalValue);
                index += parsedLength;
            }

            // Return total parsed length
            return (index - startIndex);
            */
        }
    }
}