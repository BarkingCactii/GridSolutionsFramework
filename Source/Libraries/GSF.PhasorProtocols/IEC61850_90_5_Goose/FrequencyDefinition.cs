﻿//******************************************************************************************************
//  FrequencyDefinition.cs - Gbtc
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
//       Modifeid Header.
//
//******************************************************************************************************

using System;
using System.Runtime.Serialization;
using GSF.Units.EE;

namespace GSF.PhasorProtocols.IEC61850_90_5_Goose
{
    /// <summary>
    /// Represents the IEC 61850-90-5 implementation of a <see cref="IFrequencyDefinition"/>.
    /// </summary>
    [Serializable]
    public class FrequencyDefinition : FrequencyDefinitionBase
    {
        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="FrequencyDefinition"/>.
        /// </summary>
        /// <param name="parent">The <see cref="IConfigurationCell"/> parent of this <see cref="FrequencyDefinition"/>.</param>
        public FrequencyDefinition(IConfigurationCell parent)
            : base(parent)
        {
            ScalingValue = 1000;
            DfDtScalingValue = 100;
        }

        /// <summary>
        /// Creates a new <see cref="FrequencyDefinition"/> from specified parameters.
        /// </summary>
        /// <param name="parent">The <see cref="ConfigurationCell"/> parent of this <see cref="FrequencyDefinition"/>.</param>
        /// <param name="label">The label of this <see cref="FrequencyDefinition"/>.</param>
        public FrequencyDefinition(ConfigurationCell parent, string label)
            : base(parent, label, 1000, 100, 0.0D)
        {
        }

        /// <summary>
        /// Creates a new <see cref="FrequencyDefinition"/> from serialization parameters.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> with populated with data.</param>
        /// <param name="context">The source <see cref="StreamingContext"/> for this deserialization.</param>
        protected FrequencyDefinition(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the <see cref="ConfigurationCell"/> parent of this <see cref="FrequencyDefinition"/>.
        /// </summary>
        public new virtual ConfigurationCell Parent
        {
            get
            {
                return base.Parent as ConfigurationCell;
            }
            set
            {
                base.Parent = value;
            }
        }

        /// <summary>
        /// Gets the length of the <see cref="BodyImage"/>.
        /// </summary>
        protected override int BodyLength
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Gets the binary body image of the <see cref="FrequencyDefinition"/> object.
        /// </summary>
        protected override byte[] BodyImage
        {
            get
            {
                return BigEndian.GetBytes((ushort)(Parent.NominalFrequency == LineFrequency.Hz50 ? Bits.Bit00 : Bits.Nil));
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Parses the binary body image.
        /// </summary>
        /// <param name="buffer">Binary image to parse.</param>
        /// <param name="startIndex">Start index into <paramref name="buffer"/> to begin parsing.</param>
        /// <param name="length">Length of valid data within <paramref name="buffer"/>.</param>
        /// <returns>The length of the data that was parsed.</returns>
        /// <remarks>
        /// The base implementation assumes that all channel definitions begin with a label as this is
        /// the general case, override functionality if this is not the case.
        /// </remarks>
        protected override int ParseBodyImage(byte[] buffer, int startIndex, int length)
        {
            Parent.NominalFrequency = ((BigEndian.ToUInt16(buffer, startIndex) & (ushort)Bits.Bit00) > 0) ? LineFrequency.Hz50 : LineFrequency.Hz60;
            return 2;
        }

        #endregion

        #region [ Static ]

        // Static Methods

        // Delegate handler to create a new IEC 61850-90-5 frequency definition
        internal static IFrequencyDefinition CreateNewDefinition(IConfigurationCell parent, byte[] buffer, int startIndex, out int parsedLength)
        {
            IFrequencyDefinition frequencyDefinition = new FrequencyDefinition(parent);

            parsedLength = frequencyDefinition.ParseBinaryImage(buffer, startIndex, 0);

            return frequencyDefinition;
        }

        #endregion
    }
}