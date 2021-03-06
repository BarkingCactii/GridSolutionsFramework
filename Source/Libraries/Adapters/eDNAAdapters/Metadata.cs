﻿//******************************************************************************************************
//  Metadata.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/05/2017 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using InStep.eDNA.EzDNAApiNet;

namespace eDNAAdapters
{
    /// <summary>
    /// Defines a class used to query eDNA meta-data.
    /// </summary>
    public class Metadata
    {
        #region [ Members ]

        // Fields

        /// <summary>
        /// Channel number field.
        /// </summary>
        public int ChannelNumber = -1;

        /// <summary>
        /// Site field.
        /// </summary>
        public string Site = "*";

        /// <summary>
        /// Service field.
        /// </summary>
        public string Service = "*";

        /// <summary>
        /// Short ID (a.k.a. Point ID) field.
        /// </summary>
        public string ShortID = "*";

        /// <summary>
        /// Long ID field.
        /// </summary>
        public string LongID = "*";

        /// <summary>
        /// Extended ID field.
        /// </summary>
        public string ExtendedID = "*";

        /// <summary>
        /// Description field.
        /// </summary>
        public string Description = "*";

        /// <summary>
        /// Extended description field.
        /// </summary>
        public string ExtendedDescription = "*";

        /// <summary>
        /// Point type field, "AI" or "DI".
        /// </summary>
        public string PointType = "*";

        /// <summary>
        /// Units field.
        /// </summary>
        public string Units = "*";

        /// <summary>
        /// Reference field 1.
        /// </summary>
        public string ReferenceField01 = "*";

        /// <summary>
        /// Reference field 2.
        /// </summary>
        public string ReferenceField02 = "*";

        /// <summary>
        /// Reference field 3.
        /// </summary>
        public string ReferenceField03 = "*";

        /// <summary>
        /// Reference field 4.
        /// </summary>
        public string ReferenceField04 = "*";

        /// <summary>
        /// Reference field 5.
        /// </summary>
        public string ReferenceField05 = "*";

        /// <summary>
        /// Reference field 6.
        /// </summary>
        public string ReferenceField06 = "*";

        /// <summary>
        /// Reference field 7.
        /// </summary>
        public string ReferenceField07 = "*";

        /// <summary>
        /// Reference field 8.
        /// </summary>
        public string ReferenceField08 = "*";

        /// <summary>
        /// Reference field 9.
        /// </summary>
        public string ReferenceField09 = "*";

        /// <summary>
        /// Reference field 10.
        /// </summary>
        public string ReferenceField10 = "*";

        /// <summary>
        /// Gets derived data type based on <see cref="PointType"/>.
        /// </summary>
        public DataType DataType => PointType == "DI" ? DataType.Digital : DataType.Analog;

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly object s_configOperationLock = new object();

        // Static Methods

        /// <summary>
        /// Queries eDNA meta-data for values defined in <paramref name="search"/> values.
        /// </summary>
        /// <param name="search"><see cref="Metadata"/> values to search.</param>
        /// <param name="match">Optional predicate delegate that defines the valid conditions of the elements being searched.</param>
        /// <returns>Values that match search criteria.</returns>
        /// <remarks>
        /// <para>
        /// Searches on reference fields require use of <paramref name="match"/> predicate function since the eDNA
        /// function to search meta-data, i.e., Configuration.EzSimpleFindPoints, ignores reference field values.
        /// </para>
        /// <para>
        /// Since meta-data lookups on non-key fields are a linear O(n) operation, consider a strategy that will scan
        /// and cache the full meta-data when use case demands multiple search operations or complete meta-data set.
        /// </para>
        /// </remarks>
        public static IEnumerable<Metadata> Query(Metadata search, Func<Metadata, bool> match = null)
        {
            string error;
            int key, result;

            lock (s_configOperationLock)
            {
                // Execute search - reference field value search is ignored and will return all records :(
                result = Configuration.EzSimpleFindPoints(search.Site, search.Service, search.ShortID, search.LongID,
                    search.ExtendedID, search.Description, search.ExtendedDescription, search.PointType, search.Units,
                    search.ReferenceField01, search.ReferenceField02, search.ReferenceField03, search.ReferenceField04,
                    search.ReferenceField05, search.ReferenceField06, search.ReferenceField07, search.ReferenceField08,
                    search.ReferenceField09, search.ReferenceField10, search.ChannelNumber, out key);

                if (result != 0)
                {
                    Configuration.EzSimpleFindPointsGetLastError(out error);
                    throw new EzDNAApiNetException($"Failed to execute eDNA meta-data query: {error}", result);
                }
            }

            try
            {
                int count;

                // Get search result count
                lock (s_configOperationLock)
                    count = Configuration.EzSimpleFindPointsSize(key);

                for (int i = 0; i < count; i++)
                {
                    // Create new meta-data record to hold result
                    Metadata record = new Metadata();

                    lock (s_configOperationLock)
                    {
                        // Query meta-data record values - reference fields are not returned :(
                        result = Configuration.EzSimpleFindPointsRec(key, i, out record.Site, out record.Service, out record.ShortID,
                            out record.LongID, out record.ExtendedID, out record.Description, out record.ExtendedDescription,
                            out record.PointType, out record.Units, out record.ReferenceField01, out record.ReferenceField02,
                            out record.ReferenceField03, out record.ReferenceField04, out record.ReferenceField05,
                            out record.ReferenceField06, out record.ReferenceField07, out record.ReferenceField08,
                            out record.ReferenceField09, out record.ReferenceField10, out record.ChannelNumber);

                        if (result != 0)
                        {
                            Configuration.EzSimpleFindPointsGetLastError(out error);
                            throw new EzDNAApiNetException($"Failed to read eDNA meta-data record {i} for key {key}: {error}", result);
                        }
                    }

                    string pointID = string.Format(Default.PointIDFormat, record.Site, record.Service, record.ShortID);

                    lock (s_configOperationLock)
                    {
                        // OK, now really read reference fields :p
                        result = Configuration.ReadCMRecordsRefFields(pointID, out record.ReferenceField01, out record.ReferenceField02,
                            out record.ReferenceField03, out record.ReferenceField04, out record.ReferenceField05,
                            out record.ReferenceField06, out record.ReferenceField07, out record.ReferenceField08,
                            out record.ReferenceField09, out record.ReferenceField10, 0);

                        if (result != 0)
                        {
                            Configuration.EzSimpleFindPointsGetLastError(out error);
                            throw new EzDNAApiNetException($"Failed to read reference field based meta-data for \"{pointID}\" for key {key}: {error}", result);
                        }
                    }

                    // If specified, only return for matched evaluation - all matching records returned if delegate is undefined
                    if (match?.Invoke(record) ?? true)
                        yield return record;
                }
            }
            finally
            {
                // Close search handle
                lock (s_configOperationLock)
                    Configuration.EzFindPointsRemoveKey(key);
            }
        }

        /// <summary>
        /// Gets total meta-data records.
        /// </summary>
        /// <param name="site">Site.</param>
        /// <param name="service">Service.</param>
        /// <returns>Total meta-data records.</returns>
        public static int Count(string site, string service)
        {
            string serviceType;
            int result, count, maxCount;

            lock (s_configOperationLock)
                result = Configuration.GetCMRecCnts($"{site}.{service}", out serviceType, out count, out maxCount);

            return result == 0 ? count : 0;
        }

        #endregion
    }
}