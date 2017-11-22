﻿//******************************************************************************************************
//  Volume.cs - Gbtc
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
//  01/25/2008 - J. Ritchie Carroll
//       Initial version of source generated.
//  09/11/2008 - J. Ritchie Carroll
//       Converted to C#.
//  08/10/2009 - Josh L. Patterson
//       Edited Comments.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//  10/03/2017 - J. Ritchie Carroll
//       Added units enumeration with associated Convert method.
//
//******************************************************************************************************

#region [ Contributor License Agreements ]

/**************************************************************************\
   Copyright © 2009 - J. Ritchie Carroll
   All rights reserved.
  
   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:
  
      * Redistributions of source code must retain the above copyright
        notice, this list of conditions and the following disclaimer.
       
      * Redistributions in binary form must reproduce the above
        copyright notice, this list of conditions and the following
        disclaimer in the documentation and/or other materials provided
        with the distribution.
  
   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDER "AS IS" AND ANY
   EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
   IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
   PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
   OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
   OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  
\**************************************************************************/

#endregion

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace GSF.Units
{
    #region [ Enumerations ]

    /// <summary>
    /// Represents the units available for a <see cref="Volume"/> value.
    /// </summary>
    public enum VolumeUnit
    {
        /// <summary>
        /// Cubic meter volume units.
        /// </summary>
        CubicMeters,
        /// <summary>
        /// Liter volume units.
        /// </summary>
        Liters,
        /// <summary>
        /// Teaspoon volume units.
        /// </summary>
        Teaspoons,
        /// <summary>
        /// Metric teaspoon volume units.
        /// </summary>
        MetricTeaspoons,
        /// <summary>
        /// Tablespoon volume units.
        /// </summary>
        Tablespoons,
        /// <summary>
        /// Metric tablespoon volume units.
        /// </summary>
        MetricTablespoons,
        /// <summary>
        /// Cup volume units.
        /// </summary>
        Cups,
        /// <summary>
        /// Metric cup volume units.
        /// </summary>
        MetricCups,
        /// <summary>
        /// Fluid ounce volume units.
        /// </summary>
        FluidOunces,
        /// <summary>
        /// Pint volume units.
        /// </summary>
        Pints,
        /// <summary>
        /// Quart volume units.
        /// </summary>
        Quarts,
        /// <summary>
        /// Gallon volume units.
        /// </summary>
        Gallons,
        /// <summary>
        /// Cubic inch volume units.
        /// </summary>
        CubicInches,
        /// <summary>
        /// Cubic feet volume units.
        /// </summary>
        CubicFeet
    }

    #endregion

    /// <summary>
    /// Represents a volume measurement, in cubic meters, as a double-precision floating-point number.
    /// </summary>
    /// <remarks>
    /// This class behaves just like a <see cref="double"/> representing a volume in cubic meters; it is implicitly
    /// castable to and from a <see cref="double"/> and therefore can be generally used "as" a double, but it
    /// has the advantage of handling conversions to and from other volume representations, specifically
    /// liters, teaspoons, tablespoons, cubic inches, fluid ounces, cups, pints, quarts, gallons and cubic feet.
    /// Metric conversions are handled simply by applying the needed <see cref="SI"/> conversion factor, for example:
    /// <example>
    /// Convert volume, in cubic meters, to cubic kilometers:
    /// <code>
    /// public double GetCubicKilometers(Volume cubicmeters)
    /// {
    ///     return cubicmeters / SI.Kilo;
    /// }
    /// </code>
    /// This example converts teaspoons to cups:
    /// <code>
    /// public double GetCups(double teaspoons)
    /// {
    ///     return Volume.FromTeaspoons(teaspoons).ToCups();
    /// }
    /// </code>
    /// This example converts liters to fluid ounces:
    /// <code>
    /// public double GetFluidOunces(double liters)
    /// {
    ///     return Volume.FromLiters(liters).ToFluidOunces();
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    [Serializable]
    public struct Volume : IComparable, IFormattable, IConvertible, IComparable<Volume>, IComparable<double>, IEquatable<Volume>, IEquatable<double>
    {
        #region [ Members ]

        // Constants
        private const double LitersFactor = 0.001D;

        private const double TeaspoonsFactor = 4.928921595e-6D;

        private const double MetricTeaspoonsFactor = 5.0e-6D;

        private const double TablespoonsFactor = 14.7867647825e-6D;

        private const double MetricTablespoonsFactor = 15.0e-6D;

        private const double CupsFactor = 236.5882365e-6D;

        private const double MetricCupsFactor = 250.0e-6D;

        private const double FluidOuncesFactor = 29.5735295625e-6D;

        private const double PintsFactor = 473.176473e-6D;

        private const double QuartsFactor = 946.352946e-6D;

        private const double GallonsFactor = 3.785411784e-3D;

        private const double CubicInchesFactor = 16.387064e-6D;

        private const double CubicFeetFactor = 0.028316846592D;

        // Fields
        private readonly double m_value; // Volume value stored in cubic meters

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="Volume"/>.
        /// </summary>
        /// <param name="value">New volume value in cubic meters.</param>
        public Volume(double value)
        {
            m_value = value;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the <see cref="Volume"/> value in liters.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in liters.</returns>
        public double ToLiters()
        {
            return m_value / LitersFactor;
        }

        /// <summary>
        /// Gets the <see cref="Volume"/> value in US teaspoons.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in US teaspoons.</returns>
        public double ToTeaspoons()
        {
            return m_value / TeaspoonsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Volume"/> value in metric teaspoons.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in metric teaspoons.</returns>
        public double ToMetricTeaspoons()
        {
            return m_value / MetricTeaspoonsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Volume"/> value in US tablespoons.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in US tablespoons.</returns>
        public double ToTablespoons()
        {
            return m_value / TablespoonsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Volume"/> value in metric tablespoons.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in metric tablespoons.</returns>
        public double ToMetricTablespoons()
        {
            return m_value / MetricTablespoonsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Volume"/> value in US cups.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in US cups.</returns>
        public double ToCups()
        {
            return m_value / CupsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Volume"/> value in metric cups.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in metric cups.</returns>
        public double ToMetricCups()
        {
            return m_value / MetricCupsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Volume"/> value in US fluid ounces.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in US fluid ounces.</returns>
        public double ToFluidOunces()
        {
            return m_value / FluidOuncesFactor;
        }

        /// <summary>
        /// Gets the <see cref="Volume"/> value in US fluid pints.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in US fluid pints.</returns>
        public double ToPints()
        {
            return m_value / PintsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Volume"/> value in US fluid quarts.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in US fluid quarts.</returns>
        public double ToQuarts()
        {
            return m_value / QuartsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Volume"/> value in US fluid gallons.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in US fluid gallons.</returns>
        public double ToGallons()
        {
            return m_value / GallonsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Volume"/> value in cubic inches.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in cubic inches.</returns>
        public double ToCubicInches()
        {
            return m_value / CubicInchesFactor;
        }

        /// <summary>
        /// Gets the <see cref="Volume"/> value in cubic feet.
        /// </summary>
        /// <returns>Value of <see cref="Volume"/> in cubic feet.</returns>
        public double ToCubicFeet()
        {
            return m_value / CubicFeetFactor;
        }

        /// <summary>
        /// Converts the <see cref="Volume"/> to the specified <paramref name="targetUnit"/>.
        /// </summary>
        /// <param name="targetUnit">Target units.</param>
        /// <returns><see cref="Volume"/> converted to <paramref name="targetUnit"/>.</returns>
        public double ConvertTo(VolumeUnit targetUnit)
        {
            switch (targetUnit)
            {
                case VolumeUnit.CubicMeters:
                    return m_value;
                case VolumeUnit.Liters:
                    return ToLiters();
                case VolumeUnit.Teaspoons:
                    return ToTeaspoons();
                case VolumeUnit.MetricTeaspoons:
                    return ToMetricTeaspoons();
                case VolumeUnit.Tablespoons:
                    return ToTablespoons();
                case VolumeUnit.MetricTablespoons:
                    return ToMetricTablespoons();
                case VolumeUnit.Cups:
                    return ToCups();
                case VolumeUnit.MetricCups:
                    return ToMetricCups();
                case VolumeUnit.FluidOunces:
                    return ToFluidOunces();
                case VolumeUnit.Pints:
                    return ToPints();
                case VolumeUnit.Quarts:
                    return ToQuarts();
                case VolumeUnit.Gallons:
                    return ToGallons();
                case VolumeUnit.CubicInches:
                    return ToCubicInches();
                case VolumeUnit.CubicFeet:
                    return ToCubicFeet();
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetUnit), targetUnit, null);
            }
        }

        #region [ Numeric Interface Implementations ]

        /// <summary>
        /// Compares this instance to a specified object and returns an indication of their relative values.
        /// </summary>
        /// <param name="value">An object to compare, or null.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        /// <exception cref="ArgumentException">value is not a <see cref="Double"/> or <see cref="Volume"/>.</exception>
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;

            double num;
            if (value is double)
                num = (double)value;

            else if (value is Volume)
                num = (Volume)value;

            else
                throw new ArgumentException("Argument must be a Double or a Volume");

            return m_value < num ? -1 : (m_value > num ? 1 : 0);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Volume"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">A <see cref="Volume"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        public int CompareTo(Volume value)
        {
            return CompareTo((double)value);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Double"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">A <see cref="Double"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        public int CompareTo(double value)
        {
            return m_value < value ? -1 : (m_value > value ? 1 : 0);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare, or null.</param>
        /// <returns>
        /// True if obj is an instance of <see cref="Double"/> or <see cref="Volume"/> and equals the value of this instance;
        /// otherwise, False.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is double)
                return Equals((double)obj);

            if (obj is Volume)
                return Equals((Volume)obj);

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Volume"/> value.
        /// </summary>
        /// <param name="obj">A <see cref="Volume"/> value to compare to this instance.</param>
        /// <returns>
        /// True if obj has the same value as this instance; otherwise, False.
        /// </returns>
        public bool Equals(Volume obj)
        {
            return Equals((double)obj);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Double"/> value.
        /// </summary>
        /// <param name="obj">A <see cref="Double"/> value to compare to this instance.</param>
        /// <returns>
        /// True if obj has the same value as this instance; otherwise, False.
        /// </returns>
        public bool Equals(double obj)
        {
            return m_value == obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the value of this instance, consisting of a minus sign if
        /// the value is negative, and a sequence of digits ranging from 0 to 9 with no leading zeros.
        /// </returns>
        public override string ToString()
        {
            return m_value.ToString();
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation, using
        /// the specified format.
        /// </summary>
        /// <param name="format">A format string.</param>
        /// <returns>
        /// The string representation of the value of this instance as specified by format.
        /// </returns>
        public string ToString(string format)
        {
            return m_value.ToString(format);
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation using the
        /// specified culture-specific format information.
        /// </summary>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// The string representation of the value of this instance as specified by provider.
        /// </returns>
        public string ToString(IFormatProvider provider)
        {
            return m_value.ToString(provider);
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation using the
        /// specified format and culture-specific format information.
        /// </summary>
        /// <param name="format">A format specification.</param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information.
        /// </param>
        /// <returns>
        /// The string representation of the value of this instance as specified by format and provider.
        /// </returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return m_value.ToString(format, provider);
        }

        /// <summary>
        /// Converts the string representation of a number to its <see cref="Volume"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <returns>
        /// A <see cref="Volume"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Volume.MinValue"/> or greater than <see cref="Volume.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        public static Volume Parse(string s)
        {
            return double.Parse(s);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style to its <see cref="Volume"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <returns>
        /// A <see cref="Volume"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Volume.MinValue"/> or greater than <see cref="Volume.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        public static Volume Parse(string s, NumberStyles style)
        {
            return double.Parse(s, style);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified culture-specific format to its <see cref="Volume"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// A <see cref="Volume"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Volume.MinValue"/> or greater than <see cref="Volume.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        public static Volume Parse(string s, IFormatProvider provider)
        {
            return double.Parse(s, provider);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its <see cref="Volume"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// A <see cref="Volume"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Volume.MinValue"/> or greater than <see cref="Volume.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        public static Volume Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            return double.Parse(s, style, provider);
        }

        /// <summary>
        /// Converts the string representation of a number to its <see cref="Volume"/> equivalent. A return value
        /// indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Volume"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null,
        /// is not of the correct format, or represents a number less than <see cref="Volume.MinValue"/> or greater than <see cref="Volume.MaxValue"/>.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string s, out Volume result)
        {
            double parseResult;
            bool parseResponse;

            parseResponse = double.TryParse(s, out parseResult);
            result = parseResult;

            return parseResponse;
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its
        /// <see cref="Volume"/> equivalent. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Volume"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null,
        /// is not in a format compliant with style, or represents a number less than <see cref="Volume.MinValue"/> or
        /// greater than <see cref="Volume.MaxValue"/>. This parameter is passed uninitialized.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> object that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Volume result)
        {
            double parseResult;
            bool parseResponse;

            parseResponse = double.TryParse(s, style, provider, out parseResult);
            result = parseResult;

            return parseResponse;
        }

        /// <summary>
        /// Returns the <see cref="TypeCode"/> for value type <see cref="Double"/>.
        /// </summary>
        /// <returns>The enumerated constant, <see cref="TypeCode.Double"/>.</returns>
        public TypeCode GetTypeCode()
        {
            return TypeCode.Double;
        }

        #region [ Explicit IConvertible Implementation ]

        // These are explicitly implemented on the native System.Double implementations, so we do the same...

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(m_value, provider);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(m_value, provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(m_value, provider);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(m_value, provider);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(m_value, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(m_value, provider);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(m_value, provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(m_value, provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(m_value, provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(m_value, provider);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(m_value, provider);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return m_value;
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(m_value, provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(m_value, provider);
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.ChangeType(m_value, type, provider) ?? Activator.CreateInstance(type);
        }

        #endregion

        #endregion

        #endregion

        #region [ Operators ]

        #region [ Comparison Operators ]

        /// <summary>
        /// Compares the two values for equality.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Volume"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result.</returns>
        public static bool operator ==(Volume value1, Volume value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Compares the two values for inequality.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Volume"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result.</returns>
        public static bool operator !=(Volume value1, Volume value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Returns true if left value is less than right value.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Volume"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result.</returns>
        public static bool operator <(Volume value1, Volume value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        /// <summary>
        /// Returns true if left value is less or equal to than right value.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Volume"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result.</returns>
        public static bool operator <=(Volume value1, Volume value2)
        {
            return value1.CompareTo(value2) <= 0;
        }

        /// <summary>
        /// Returns true if left value is greater than right value.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Volume"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result.</returns>
        public static bool operator >(Volume value1, Volume value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        /// <summary>
        /// Returns true if left value is greater than or equal to right value.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Volume"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result.</returns>
        public static bool operator >=(Volume value1, Volume value2)
        {
            return value1.CompareTo(value2) >= 0;
        }

        #endregion

        #region [ Type Conversion Operators ]

        /// <summary>
        /// Implicitly converts value, represented in cubic meters, to a <see cref="Volume"/>.
        /// </summary>
        /// <param name="value">A <see cref="Double"/> value.</param>
        /// <returns>A <see cref="Volume"/> object.</returns>
        public static implicit operator Volume(double value)
        {
            return new Volume(value);
        }

        /// <summary>
        /// Implicitly converts <see cref="Volume"/>, represented in cubic meters, to a <see cref="Double"/>.
        /// </summary>
        /// <param name="value">A <see cref="Volume"/> object.</param>
        /// <returns>A <see cref="Double"/> value.</returns>
        public static implicit operator double(Volume value)
        {
            return value.m_value;
        }

        #endregion

        #region [ Arithmetic Operators ]

        /// <summary>
        /// Returns computed remainder after dividing first value by the second.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Volume"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Volume"/> object as the result.</returns>
        public static Volume operator %(Volume value1, Volume value2)
        {
            return value1.m_value % value2.m_value;
        }

        /// <summary>
        /// Returns computed sum of values.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Volume"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Volume"/> object as the result.</returns>
        public static Volume operator +(Volume value1, Volume value2)
        {
            return value1.m_value + value2.m_value;
        }

        /// <summary>
        /// Returns computed difference of values.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Volume"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Volume"/> object as the result.</returns>
        public static Volume operator -(Volume value1, Volume value2)
        {
            return value1.m_value - value2.m_value;
        }

        /// <summary>
        /// Returns computed product of values.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Volume"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Volume"/> object as the result.</returns>
        public static Volume operator *(Volume value1, Volume value2)
        {
            return value1.m_value * value2.m_value;
        }

        /// <summary>
        /// Returns computed division of values.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Volume"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Volume"/> object as the result.</returns>
        public static Volume operator /(Volume value1, Volume value2)
        {
            return value1.m_value / value2.m_value;
        }

        // C# doesn't expose an exponent operator but some other .NET languages do,
        // so we expose the operator via its native special IL function name

        /// <summary>
        /// Returns result of first value raised to volume of second value.
        /// </summary>
        /// <param name="value1">A <see cref="Volume"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Volume"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Double"/> value as the result.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced), SpecialName]
        public static double op_Exponent(Volume value1, Volume value2)
        {
            return Math.Pow(value1.m_value, value2.m_value);
        }

        #endregion

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>Represents the largest possible value of an <see cref="Volume"/>. This field is constant.</summary>
        public static readonly Volume MaxValue = double.MaxValue;

        /// <summary>Represents the smallest possible value of an <see cref="Volume"/>. This field is constant.</summary>
        public static readonly Volume MinValue = double.MinValue;

        // Static Methods

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in liters.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in liters.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in liters.</returns>
        public static Volume FromLiters(double value)
        {
            return new Volume(value * LitersFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in US teaspoons.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in US teaspoons.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in US teaspoons.</returns>
        public static Volume FromTeaspoons(double value)
        {
            return new Volume(value * TeaspoonsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in metric teaspoons.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in metric teaspoons.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in metric teaspoons.</returns>
        public static Volume FromMetricTeaspoons(double value)
        {
            return new Volume(value * MetricTeaspoonsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in US tablespoons.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in US tablespoons.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in US tablespoons.</returns>
        public static Volume FromTablespoons(double value)
        {
            return new Volume(value * TablespoonsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in metric tablespoons.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in metric tablespoons.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in metric tablespoons.</returns>
        public static Volume FromMetricTablespoons(double value)
        {
            return new Volume(value * MetricTablespoonsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in US cups.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in US cups.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in US cups.</returns>
        public static Volume FromCups(double value)
        {
            return new Volume(value * CupsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in metric cups.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in metric cups.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in metric cups.</returns>
        public static Volume FromMetricCups(double value)
        {
            return new Volume(value * MetricCupsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in US fluid ounces.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in US fluid ounces.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in US fluid ounces.</returns>
        public static Volume FromFluidOunces(double value)
        {
            return new Volume(value * FluidOuncesFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in US fluid pints.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in US fluid pints.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in US fluid pints.</returns>
        public static Volume FromPints(double value)
        {
            return new Volume(value * PintsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in US fluid quarts.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in US fluid quarts.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in US fluid quarts.</returns>
        public static Volume FromQuarts(double value)
        {
            return new Volume(value * QuartsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in US fluid gallons.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in US fluid gallons.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in US fluid gallons.</returns>
        public static Volume FromGallons(double value)
        {
            return new Volume(value * GallonsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in cubic inches.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in cubic inches.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in cubic inches.</returns>
        public static Volume FromCubicInches(double value)
        {
            return new Volume(value * CubicInchesFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Volume"/> value from the specified <paramref name="value"/> in cubic feet.
        /// </summary>
        /// <param name="value">New <see cref="Volume"/> value in cubic feet.</param>
        /// <returns>New <see cref="Volume"/> object from the specified <paramref name="value"/> in cubic feet.</returns>
        public static Volume FromCubicFeet(double value)
        {
            return new Volume(value * CubicFeetFactor);
        }

        /// <summary>
        /// Converts the <paramref name="value"/> in the specified <paramref name="sourceUnit"/> to a new <see cref="Volume"/> in cubic meters.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <param name="sourceUnit">Source value units.</param>
        /// <returns>New <see cref="Volume"/> from the specified <paramref name="value"/> in <paramref name="sourceUnit"/>.</returns>
        public static Volume ConvertFrom(double value, VolumeUnit sourceUnit)
        {
            switch (sourceUnit)
            {
                case VolumeUnit.CubicMeters:
                    return value;
                case VolumeUnit.Liters:
                    return FromLiters(value);
                case VolumeUnit.Teaspoons:
                    return FromTeaspoons(value);
                case VolumeUnit.MetricTeaspoons:
                    return FromMetricTeaspoons(value);
                case VolumeUnit.Tablespoons:
                    return FromTablespoons(value);
                case VolumeUnit.MetricTablespoons:
                    return FromMetricTablespoons(value);
                case VolumeUnit.Cups:
                    return FromCups(value);
                case VolumeUnit.MetricCups:
                    return FromMetricCups(value);
                case VolumeUnit.FluidOunces:
                    return FromFluidOunces(value);
                case VolumeUnit.Pints:
                    return FromPints(value);
                case VolumeUnit.Quarts:
                    return FromQuarts(value);
                case VolumeUnit.Gallons:
                    return FromGallons(value);
                case VolumeUnit.CubicInches:
                    return FromCubicInches(value);
                case VolumeUnit.CubicFeet:
                    return FromCubicFeet(value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(sourceUnit), sourceUnit, null);
            }
        }

        #endregion
    }
}