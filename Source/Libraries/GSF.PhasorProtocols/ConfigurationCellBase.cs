﻿//******************************************************************************************************
//  ConfigurationCellBase.cs - Gbtc
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
//  01/14/2005 - J. Ritchie Carroll
//       Generated original version of source code.
//  12/17/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GSF.Parsing;
using GSF.Units.EE;

namespace GSF.PhasorProtocols
{
    /// <summary>
    /// Represents the protocol independent common implementation of all configuration elements for cells in a <see cref="IConfigurationFrame"/>.
    /// </summary>
    [Serializable]
    public abstract class ConfigurationCellBase : ChannelCellBase, IConfigurationCell
    {
        #region [ Members ]

        // Fields
        private string m_stationName;
        private string m_idLabel;
        private readonly PhasorDefinitionCollection m_phasorDefinitions;
        private IFrequencyDefinition m_frequencyDefinition;
        private readonly AnalogDefinitionCollection m_analogDefinitions;
        private readonly DigitalDefinitionCollection m_digitalDefinitions;
        private LineFrequency m_nominalFrequency;
        private AngleFormat m_angleFormat = AngleFormat.Radians;
        private ushort m_revisionCount;
        private int m_hashCode;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="ConfigurationCellBase"/> from specified parameters.
        /// </summary>
        /// <param name="parent">The reference to parent <see cref="IConfigurationFrame"/> of this <see cref="ConfigurationCellBase"/>.</param>
        /// <param name="idCode">The numeric ID code for this <see cref="ConfigurationCellBase"/>.</param>
        /// <param name="maximumPhasors">Sets the maximum number of phasors for the <see cref="PhasorDefinitions"/> collection.</param>
        /// <param name="maximumAnalogs">Sets the maximum number of phasors for the <see cref="AnalogDefinitions"/> collection.</param>
        /// <param name="maximumDigitals">Sets the maximum number of phasors for the <see cref="DigitalDefinitions"/> collection.</param>
        protected ConfigurationCellBase(IConfigurationFrame parent, ushort idCode, int maximumPhasors, int maximumAnalogs, int maximumDigitals)
            : base(parent, idCode)
        {
            m_nominalFrequency = LineFrequency.Hz60; // Defaulting to 60Hz
            // We convert maximum counts to last valid indices (count - 1):
            m_phasorDefinitions = new PhasorDefinitionCollection(maximumPhasors - 1);
            m_analogDefinitions = new AnalogDefinitionCollection(maximumAnalogs - 1);
            m_digitalDefinitions = new DigitalDefinitionCollection(maximumDigitals - 1);
        }

        /// <summary>
        /// Creates a new <see cref="ConfigurationCellBase"/> from serialization parameters.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> with populated with data.</param>
        /// <param name="context">The source <see cref="StreamingContext"/> for this deserialization.</param>
        protected ConfigurationCellBase(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Deserialize configuration cell values
            StationName = info.GetString("stationName");
            IDLabel = info.GetString("idLabel");
            m_phasorDefinitions = (PhasorDefinitionCollection)info.GetValue("phasorDefinitions", typeof(PhasorDefinitionCollection));
            m_frequencyDefinition = (IFrequencyDefinition)info.GetValue("frequencyDefinition", typeof(IFrequencyDefinition));
            m_analogDefinitions = (AnalogDefinitionCollection)info.GetValue("analogDefinitions", typeof(AnalogDefinitionCollection));
            m_digitalDefinitions = (DigitalDefinitionCollection)info.GetValue("digitalDefinitions", typeof(DigitalDefinitionCollection));
            m_nominalFrequency = (LineFrequency)info.GetValue("nominalFrequency", typeof(LineFrequency));
            m_revisionCount = info.GetUInt16("revisionCount");
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets a reference to the parent <see cref="IConfigurationFrame"/> for this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public new virtual IConfigurationFrame Parent
        {
            get
            {
                return base.Parent as IConfigurationFrame;
            }
            set
            {
                base.Parent = value;
            }
        }

        /// <summary>
        /// Gets or sets the parsing state for the this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public new virtual IConfigurationCellParsingState State
        {
            get
            {
                return base.State as IConfigurationCellParsingState;
            }
            set
            {
                base.State = value;
            }
        }

        /// <summary>
        /// Gets or sets the station name of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual string StationName
        {
            get
            {
                return m_stationName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "undefined";

                value = value.GetValidLabel();

                if (value.Length > MaximumStationNameLength)
                    throw new OverflowException("Station name length cannot exceed " + MaximumStationNameLength + " characters.");

                m_stationName = value;
            }
        }

        /// <summary>
        /// Gets the binary image of the <see cref="StationName"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual byte[] StationNameImage
        {
            get
            {
                return Encoding.ASCII.GetBytes(StationName.PadRight(MaximumStationNameLength));
            }
        }

        /// <summary>
        /// Gets the maximum length of the <see cref="StationName"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual int MaximumStationNameLength
        {
            get
            {
                // Typical station name length is 16 characters
                return 16;
            }
        }

        /// <summary>
        /// Gets or sets the ID label of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual string IDLabel
        {
            get
            {
                return m_idLabel;
            }
            set
            {
                if ((object)value == null)
                    value = "";

                value = value.GetValidLabel();

                if (value.Length > IDLabelLength)
                    throw new OverflowException("ID label length cannot exceed " + IDLabelLength + " characters.");

                m_idLabel = value;
            }
        }

        /// <summary>
        /// Gets the binary image of the <see cref="IDLabel"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual byte[] IDLabelImage
        {
            get
            {
                if (IDLabelLength < int.MaxValue)
                    return Encoding.ASCII.GetBytes(IDLabel.PadRight(IDLabelLength));
                else
                    return Encoding.ASCII.GetBytes(IDLabel);
            }
        }

        /// <summary>
        /// Gets the length of the <see cref="IDLabel"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual int IDLabelLength
        {
            get
            {
                // We don't restrict this for most protocols...
                return int.MaxValue;
            }
        }

        /// <summary>
        /// Gets a reference to the <see cref="PhasorDefinitionCollection"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual PhasorDefinitionCollection PhasorDefinitions
        {
            get
            {
                return m_phasorDefinitions;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DataFormat"/> for the <see cref="IPhasorDefinition"/> objects in the <see cref="PhasorDefinitions"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public abstract DataFormat PhasorDataFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="CoordinateFormat"/> for the <see cref="IPhasorDefinition"/> objects in the <see cref="PhasorDefinitions"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public abstract CoordinateFormat PhasorCoordinateFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="AngleFormat"/> for the <see cref="IPhasorDefinition"/> objects in the <see cref="PhasorDefinitions"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        /// <remarks>
        /// Base class defines default angle format since this is rarely not radians.
        /// </remarks>
        public virtual AngleFormat PhasorAngleFormat
        {
            get
            {
                return m_angleFormat;
            }
            set
            {
                m_angleFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IFrequencyDefinition"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual IFrequencyDefinition FrequencyDefinition
        {
            get
            {
                return m_frequencyDefinition;
            }
            set
            {
                m_frequencyDefinition = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DataFormat"/> of the <see cref="FrequencyDefinition"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public abstract DataFormat FrequencyDataFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the nominal <see cref="LineFrequency"/> of the <see cref="FrequencyDefinition"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual LineFrequency NominalFrequency
        {
            get
            {
                return m_nominalFrequency;
            }
            set
            {
                m_nominalFrequency = value;
            }
        }

        /// <summary>
        /// Gets a reference to the <see cref="AnalogDefinitionCollection"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual AnalogDefinitionCollection AnalogDefinitions
        {
            get
            {
                return m_analogDefinitions;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DataFormat"/> for the <see cref="IAnalogDefinition"/> objects in the <see cref="AnalogDefinitions"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public abstract DataFormat AnalogDataFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a reference to the <see cref="DigitalDefinitionCollection"/> of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual DigitalDefinitionCollection DigitalDefinitions
        {
            get
            {
                return m_digitalDefinitions;
            }
        }

        /// <summary>
        /// Gets the specified frame rate of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual ushort FrameRate
        {
            get
            {
                return Parent.FrameRate;
            }
        }

        /// <summary>
        /// Gets or sets the revision count of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        public virtual ushort RevisionCount
        {
            get
            {
                return m_revisionCount;
            }
            set
            {
                m_revisionCount = value;
            }
        }

        /// <summary>
        /// Gets the length of the <see cref="HeaderImage"/>.
        /// </summary>
        /// <remarks>
        /// Base implementation provides station name length from the header which is common to configuration frame headers in IEEE protocols.
        /// </remarks>
        protected override int HeaderLength
        {
            get
            {
                return MaximumStationNameLength;
            }
        }

        /// <summary>
        /// Gets the binary header image of the <see cref="ConfigurationCellBase"/> object.
        /// </summary>
        /// <remarks>
        /// Base implementation provides station name image from the header which is common to configuration frame headers in IEEE protocols.
        /// </remarks>
        protected override byte[] HeaderImage
        {
            get
            {
                return StationNameImage;
            }
        }

        /// <summary>
        /// Gets the length of the <see cref="BodyImage"/>.
        /// </summary>
        /// <remarks>
        /// Channel names of IEEE protocol configuration frames are common in order and type so these are defined in the base class.
        /// </remarks>
        protected override int BodyLength
        {
            get
            {
                return m_phasorDefinitions.BinaryLength + m_analogDefinitions.BinaryLength + m_digitalDefinitions.BinaryLength;
            }
        }

        /// <summary>
        /// Gets the binary body image of the <see cref="ConfigurationCellBase"/> object.
        /// </summary>
        /// <remarks>
        /// Channel names of IEEE protocol configuration frames are common in order and type so these are defined in the base class.
        /// </remarks>
        protected override byte[] BodyImage
        {
            get
            {
                byte[] buffer = new byte[BodyLength];
                int index = 0;

                // Copy in common cell images (channel names)
                m_phasorDefinitions.CopyImage(buffer, ref index);
                m_analogDefinitions.CopyImage(buffer, ref index);
                m_digitalDefinitions.CopyImage(buffer, ref index);

                return buffer;
            }
        }

        /// <summary>
        /// Gets the length of the <see cref="FooterImage"/>.
        /// </summary>
        /// <remarks>
        /// Bottom of the IEEE protocol configuration frames contain a nominal frequency definition, so base implementation exposes frequency definition as the footer.
        /// </remarks>
        protected override int FooterLength
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Gets the binary footer image of the <see cref="ConfigurationCellBase"/> object.
        /// </summary>
        /// <remarks>
        /// Bottom of the IEEE protocol configuration frames contain a nominal frequency definition, so base implementation exposes frequency definition as the footer.
        /// </remarks>
        protected override byte[] FooterImage
        {
            get
            {
                return m_frequencyDefinition.BinaryImage();
            }
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}"/> of string based property names and values for the <see cref="ConfigurationCellBase"/> object.
        /// </summary>
        public override Dictionary<string, string> Attributes
        {
            get
            {
                Dictionary<string, string> baseAttributes = base.Attributes;

                baseAttributes.Add("Station Name", StationName);
                baseAttributes.Add("ID Label", IDLabel);
                baseAttributes.Add("Phasor Coordinate Format", (int)PhasorCoordinateFormat + ": " + PhasorCoordinateFormat);
                baseAttributes.Add("Phasor Data Format", (int)PhasorDataFormat + ": " + PhasorDataFormat);
                baseAttributes.Add("Phasor Angle Format", (int)PhasorAngleFormat + ": " + PhasorAngleFormat);
                baseAttributes.Add("Frequency Data Format", (int)FrequencyDataFormat + ": " + FrequencyDataFormat);
                baseAttributes.Add("Analog Data Format", (int)AnalogDataFormat + ": " + AnalogDataFormat);
                baseAttributes.Add("Total Phasor Definitions", PhasorDefinitions.Count.ToString());
                baseAttributes.Add("Total Analog Definitions", AnalogDefinitions.Count.ToString());
                baseAttributes.Add("Total Digital Definitions", DigitalDefinitions.Count.ToString());
                baseAttributes.Add("Nominal Frequency", (int)NominalFrequency + "Hz");
                baseAttributes.Add("Revision Count", RevisionCount.ToString());
                baseAttributes.Add("Maximum Station Name Length", MaximumStationNameLength.ToString());
                baseAttributes.Add("ID Label Length", IDLabelLength.ToString());

                return baseAttributes;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Parses the binary header image.
        /// </summary>
        /// <param name="buffer">Binary image to parse.</param>
        /// <param name="startIndex">Start index into <paramref name="buffer"/> to begin parsing.</param>
        /// <param name="length">Length of valid data within <paramref name="buffer"/>.</param>
        /// <returns>The length of the data that was parsed.</returns>
        protected override int ParseHeaderImage(byte[] buffer, int startIndex, int length)
        {
            // Parse station name from header...
            int stationNameLength = MaximumStationNameLength;

            StationName = Encoding.ASCII.GetString(buffer, startIndex, stationNameLength);

            return stationNameLength;
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

            IConfigurationCellParsingState parsingState = State;
            IPhasorDefinition phasorDefinition;
            IAnalogDefinition analogDefinition;
            IDigitalDefinition digitalDefinition;
            int x, parsedLength, index = startIndex;

            // By the very nature of the IEEE protocols supporting the same order of phasor, analog and digital labels
            // we are able to "automatically" parse this data out in the configuration cell base class - BEAUTIFUL!!!

            // Parse out phasor definitions
            for (x = 0; x < parsingState.PhasorCount; x++)
            {
                phasorDefinition = parsingState.CreateNewPhasorDefinition(this, buffer, index, out parsedLength);
                m_phasorDefinitions.Add(phasorDefinition);
                index += parsedLength;
            }

            // Parse out analog definitions
            for (x = 0; x < parsingState.AnalogCount; x++)
            {
                analogDefinition = parsingState.CreateNewAnalogDefinition(this, buffer, index, out parsedLength);
                m_analogDefinitions.Add(analogDefinition);
                index += parsedLength;
            }

            // Parse out digital definitions
            for (x = 0; x < parsingState.DigitalCount; x++)
            {
                digitalDefinition = parsingState.CreateNewDigitalDefinition(this, buffer, index, out parsedLength);
                m_digitalDefinitions.Add(digitalDefinition);
                index += parsedLength;
            }

            // Return total parsed length
            return (index - startIndex);
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
            // Parse nominal frequency definition from footer...
            int parsedLength;

            m_frequencyDefinition = State.CreateNewFrequencyDefinition(this, buffer, startIndex, out parsedLength);

            return parsedLength;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare, or null.</param>
        /// <returns>
        /// True if obj is an instance of <see cref="IConfigurationCell"/> and equals the value of this instance;
        /// otherwise, False.
        /// </returns>
        /// <exception cref="ArgumentException">value is not an <see cref="IConfigurationCell"/>.</exception>
        public override bool Equals(object obj)
        {
            return Equals(obj as IConfigurationCell);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to specified <see cref="IConfigurationCell"/> value.
        /// </summary>
        /// <param name="other">A <see cref="IConfigurationCell"/> object to compare to this instance.</param>
        /// <returns>
        /// True if <paramref name="other"/> has the same value as this instance; otherwise, False.
        /// </returns>
        public bool Equals(IConfigurationCell other)
        {
            return ((object)other != null) && (CompareTo(other) == 0);
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
        /// <exception cref="ArgumentException">value is not an <see cref="IConfigurationCell"/>.</exception>
        public virtual int CompareTo(object obj)
        {
            IConfigurationCell other = obj as IConfigurationCell;

            if (other == null)
                throw new ArgumentException("ConfigurationCell can only be compared to other IConfigurationCells");

            return CompareTo(other);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="IConfigurationCell"/> object and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="other">A <see cref="IConfigurationCell"/> object to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        public virtual int CompareTo(IConfigurationCell other)
        {
            // We sort configuration cells by ID code...
            return IDCode.CompareTo(other.IDCode);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            if (m_hashCode == 0)
                m_hashCode = Guid.NewGuid().GetHashCode();

            return m_hashCode;
        }

        /// <summary>
        /// Gets the string representation of this <see cref="ConfigurationCellBase"/>.
        /// </summary>
        /// <returns>String representation of this <see cref="ConfigurationCellBase"/>.</returns>
        public override string ToString()
        {
            string stationName = StationName;

            if (!string.IsNullOrWhiteSpace(stationName))
                return stationName;

            return base.ToString();
        }

        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination <see cref="StreamingContext"/> for this serialization.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            // Serialize configuration cell values
            info.AddValue("stationName", StationName);
            info.AddValue("idLabel", IDLabel);
            info.AddValue("phasorDefinitions", m_phasorDefinitions, typeof(PhasorDefinitionCollection));
            info.AddValue("frequencyDefinition", m_frequencyDefinition, typeof(IFrequencyDefinition));
            info.AddValue("analogDefinitions", m_analogDefinitions, typeof(AnalogDefinitionCollection));
            info.AddValue("digitalDefinitions", m_digitalDefinitions, typeof(DigitalDefinitionCollection));
            info.AddValue("nominalFrequency", m_nominalFrequency, typeof(LineFrequency));
            info.AddValue("revisionCount", m_revisionCount);
        }

        #endregion
    }
}