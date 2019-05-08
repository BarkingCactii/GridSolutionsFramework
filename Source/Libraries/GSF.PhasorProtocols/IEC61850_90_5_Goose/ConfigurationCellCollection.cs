﻿//******************************************************************************************************
//  ConfigurationCellCollection.cs - Gbtc
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
using System.Runtime.Serialization;

namespace GSF.PhasorProtocols.IEC61850_90_5_Goose
{
    /// <summary>
    /// Represents a IEC 61850-90-5 implementation of a collection of <see cref="IConfigurationCell"/> objects.
    /// </summary>
    [Serializable]
    public class ConfigurationCellCollection : PhasorProtocols.ConfigurationCellCollection
    {
        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="ConfigurationCellCollection"/>.
        /// </summary>
        public ConfigurationCellCollection()
            : base(ushort.MaxValue, false)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ConfigurationCellCollection"/> from serialization parameters.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> with populated with data.</param>
        /// <param name="context">The source <see cref="StreamingContext"/> for this deserialization.</param>
        protected ConfigurationCellCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets <see cref="ConfigurationCell"/> at specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index of value to get or set.</param>
        public new ConfigurationCell this[int index]
        {
            get
            {
                return base[index] as ConfigurationCell;
            }
            set
            {
                base[index] = value;
            }
        }

        #endregion
    }
}