﻿//******************************************************************************************************
//  StringExtensions.cs - Gbtc
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
//  10/27/2016 - Steven E. Chisholm
//       Split this class. GSF.StringExtensions is now located in the GSF Shared Project. 
//       What remains is code that cannot be hosted inside a SQL CLR process.
//
//******************************************************************************************************

#if MONO
using System;
#else
using System.Data.Entity.Design.PluralizationServices;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
#endif

namespace GSF
{
    //------------------------------------------------------------------------------------------------------------
    // Note: This code has been moved to the GSF.Core shared project. Only put string extension methods here
    //       that cannot be hosted inside of a SQL CLR process.
    //------------------------------------------------------------------------------------------------------------

    /// <summary>Defines extension functions related to string manipulation.</summary>
    public static partial class StringExtensions
    {
#if !MONO
        private static readonly PluralizationService s_pluralizationService;

        [SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.Collections.Generic.Dictionary`2<System.StringComparison,System.StringComparer>.set_Item(System.StringComparison,System.StringComparer)")]
        static StringExtensions()
        {
            // Pluralization service currently only supports English, if other languages are supported in the
            // future, cached services can use to a concurrent dictionary keyed on LCID of culture
            s_pluralizationService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
        }
#endif
        /// <summary>
        /// Returns the singular form of the specified word.
        /// </summary>
        /// <param name="value">The word to be made singular.</param>
        /// <returns>The singular form of the input parameter.</returns>
        public static string ToSingular(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
#if MONO
            return value.IsSingular() ? value : value.Substring(0, value.Length - 1);
#else
            return s_pluralizationService.Singularize(value);
#endif
        }

        /// <summary>
        /// Determines whether the specified word is singular.
        /// </summary>
        /// <param name="value">The word to be analyzed.</param>
        /// <returns><c>true</c> if the word is singular; otherwise, <c>false</c>.</returns>
        public static bool IsSingular(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
#if MONO
            return !value.IsPlural();
#else
            return s_pluralizationService.IsSingular(value);
#endif
        }

        /// <summary>
        /// Returns the plural form of the specified word.
        /// </summary>
        /// <param name="value">The word to be made plural.</param>
        /// <returns>The plural form of the input parameter.</returns>
        public static string ToPlural(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
#if MONO
            return value.IsPlural() ? value : $"{value}s";
#else
            return s_pluralizationService.Pluralize(value);
#endif
        }

        /// <summary>
        /// Determines whether the specified word is plural.
        /// </summary>
        /// <param name="value">The word to be analyzed.</param>
        /// <returns><c>true</c> if the word is plural; otherwise, <c>false</c>.</returns>
        public static bool IsPlural(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
#if MONO
            return value.EndsWith("s", StringComparison.OrdinalIgnoreCase);
#else
            return s_pluralizationService.IsPlural(value);
#endif
        }
    }
}
