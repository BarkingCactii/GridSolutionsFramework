﻿//******************************************************************************************************
//  Length.cs - Gbtc
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
    /// Represents the units available for a <see cref="Length"/> value.
    /// </summary>
    public enum LengthUnit
    {
        /// <summary>
        /// Meter length units.
        /// </summary>
        Meters,
        /// <summary>
        /// Foot length units.
        /// </summary>
        Feet,
        /// <summary>
        /// Inch length units.
        /// </summary>
        Inches,
        /// <summary>
        /// Mile length units.
        /// </summary>
        Miles,
        /// <summary>
        /// Light-second length units.
        /// </summary>
        LightSeconds,
        /// <summary>
        /// U.S. survey foot length units.
        /// </summary>
        USSurveyFeet,
        /// <summary>
        /// U.S. survey mile length units.
        /// </summary>
        USSurveyMiles,
        /// <summary>
        /// Nautical mile length units.
        /// </summary>
        NauticalMiles,
        /// <summary>
        /// Yard length units.
        /// </summary>
        Yards
    }

    #endregion

    /// <summary>
    /// Represents a length measurement, in meters, as a double-precision floating-point number.
    /// </summary>
    /// <remarks>
    /// This class behaves just like a <see cref="double"/> representing a length in meters; it is implicitly
    /// castable to and from a <see cref="double"/> and therefore can be generally used "as" a double, but it
    /// has the advantage of handling conversions to and from other length representations, specifically
    /// inches, feet, yards, miles, U.S. survey feet, U.S. survey miles, light-seconds, and nautical miles.
    /// Metric conversions are handled simply by applying the needed <see cref="SI"/> conversion factor, for example:
    /// <example>
    /// Convert length in meters to kilometers:
    /// <code>
    /// public double GetKilometers(Length meters)
    /// {
    ///     return meters / SI.Kilo;
    /// }
    /// </code>
    /// This example converts feet to inches:
    /// <code>
    /// public double GetFeet(double inches)
    /// {
    ///     return Length.FromInches(inches).ToFeet();
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    [Serializable]
    public struct Length : IComparable, IFormattable, IConvertible, IComparable<Length>, IComparable<double>, IEquatable<Length>, IEquatable<double>
    {
        #region [ Members ]

        // Constants
        private const double FeetFactor = 0.3048D;

        private const double InchesFactor = 0.0254D;

        private const double MilesFactor = 1609.344D;

        private const double LightSecondsFactor = 2.99792458E8;

        private const double USSurveyFeetFactor = 0.304800610D;

        private const double USSurveyMilesFactor = 1609.347219D;

        private const double NauticalMilesFactor = 1852.0D;

        private const double YardsFactor = 0.9144D;

        // Fields
        private readonly double m_value; // Length value stored in meters

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="Length"/>.
        /// </summary>
        /// <param name="value">New length value in meters.</param>
        public Length(double value)
        {
            m_value = value;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the <see cref="Length"/> value in feet.
        /// </summary>
        /// <returns>Value of <see cref="Length"/> in feet.</returns>
        public double ToFeet()
        {
            return m_value / FeetFactor;
        }

        /// <summary>
        /// Gets the <see cref="Length"/> value in yards.
        /// </summary>
        /// <returns>Value of <see cref="Length"/> in yards.</returns>
        public double ToYards()
        {
            return m_value / YardsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Length"/> value in inches.
        /// </summary>
        /// <returns>Value of <see cref="Length"/> in inches.</returns>
        public double ToInches()
        {
            return m_value / InchesFactor;
        }

        /// <summary>
        /// Gets the <see cref="Length"/> value in miles.
        /// </summary>
        /// <returns>Value of <see cref="Length"/> in miles.</returns>
        public double ToMiles()
        {
            return m_value / MilesFactor;
        }

        /// <summary>
        /// Gets the <see cref="Length"/> value in light-seconds.
        /// </summary>
        /// <returns>Value of <see cref="Length"/> in light-seconds.</returns>
        public double ToLightSeconds()
        {
            return m_value / LightSecondsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Length"/> value in US survey feet.
        /// </summary>
        /// <returns>Value of <see cref="Length"/> in US survey feet.</returns>
        public double ToUSSurveyFeet()
        {
            return m_value / USSurveyFeetFactor;
        }

        /// <summary>
        /// Gets the <see cref="Length"/> value in US survey miles.
        /// </summary>
        /// <returns>Value of <see cref="Length"/> in US survey miles.</returns>
        public double ToUSSurveyMiles()
        {
            return m_value / USSurveyMilesFactor;
        }

        /// <summary>
        /// Gets the <see cref="Length"/> value in nautical miles.
        /// </summary>
        /// <returns>Value of <see cref="Length"/> in nautical miles.</returns>
        public double ToNauticalMiles()
        {
            return m_value / NauticalMilesFactor;
        }

        /// <summary>
        /// Converts the <see cref="Length"/> to the specified <paramref name="targetUnit"/>.
        /// </summary>
        /// <param name="targetUnit">Target units.</param>
        /// <returns><see cref="Length"/> converted to <paramref name="targetUnit"/>.</returns>
        public double ConvertTo(LengthUnit targetUnit)
        {
            switch (targetUnit)
            {
                case LengthUnit.Meters:
                    return m_value;
                case LengthUnit.Feet:
                    return ToFeet();
                case LengthUnit.Inches:
                    return ToInches();
                case LengthUnit.Miles:
                    return ToMiles();
                case LengthUnit.LightSeconds:
                    return ToLightSeconds();
                case LengthUnit.USSurveyFeet:
                    return ToUSSurveyFeet();
                case LengthUnit.USSurveyMiles:
                    return ToUSSurveyMiles();
                case LengthUnit.NauticalMiles:
                    return ToNauticalMiles();
                case LengthUnit.Yards:
                    return ToYards();
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
        /// <exception cref="ArgumentException">value is not a <see cref="Double"/> or <see cref="Length"/>.</exception>
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;

            double num;

            if (value is double)
                num = (double)value;

            else if (value is Length)
                num = (Length)value;

            else
                throw new ArgumentException("Argument must be a Double or a Length");

            return m_value < num ? -1 : (m_value > num ? 1 : 0);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Length"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">A <see cref="Length"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        public int CompareTo(Length value)
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
        /// True if obj is an instance of <see cref="Double"/> or <see cref="Length"/> and equals the value of this instance;
        /// otherwise, False.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is double)
                return Equals((double)obj);

            if (obj is Length)
                return Equals((Length)obj);

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Length"/> value.
        /// </summary>
        /// <param name="obj">A <see cref="Length"/> value to compare to this instance.</param>
        /// <returns>
        /// True if obj has the same value as this instance; otherwise, False.
        /// </returns>
        public bool Equals(Length obj)
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
        /// Converts the string representation of a number to its <see cref="Length"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <returns>
        /// A <see cref="Length"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Length.MinValue"/> or greater than <see cref="Length.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        public static Length Parse(string s)
        {
            return double.Parse(s);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style to its <see cref="Length"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <returns>
        /// A <see cref="Length"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Length.MinValue"/> or greater than <see cref="Length.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        public static Length Parse(string s, NumberStyles style)
        {
            return double.Parse(s, style);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified culture-specific format to its <see cref="Length"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// A <see cref="Length"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Length.MinValue"/> or greater than <see cref="Length.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        public static Length Parse(string s, IFormatProvider provider)
        {
            return double.Parse(s, provider);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its <see cref="Length"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// A <see cref="Length"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Length.MinValue"/> or greater than <see cref="Length.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        public static Length Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            return double.Parse(s, style, provider);
        }

        /// <summary>
        /// Converts the string representation of a number to its <see cref="Length"/> equivalent. A return value
        /// indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Length"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null,
        /// is not of the correct format, or represents a number less than <see cref="Length.MinValue"/> or greater than <see cref="Length.MaxValue"/>.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string s, out Length result)
        {
            double parseResult;
            bool parseResponse;

            parseResponse = double.TryParse(s, out parseResult);
            result = parseResult;

            return parseResponse;
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its
        /// <see cref="Length"/> equivalent. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Length"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null,
        /// is not in a format compliant with style, or represents a number less than <see cref="Length.MinValue"/> or
        /// greater than <see cref="Length.MaxValue"/>. This parameter is passed uninitialized.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> object that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Length result)
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
        /// <param name="value1">A <see cref="Length"/> object left hand operand.</param>
        /// <param name="value2">A <see cref="Length"/> object right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result of the operation.</returns>
        public static bool operator ==(Length value1, Length value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Compares the two values for inequality.
        /// </summary>
        /// <param name="value1">A <see cref="Length"/> object left hand operand.</param>
        /// <param name="value2">A <see cref="Length"/> object right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result of the operation.</returns>
        public static bool operator !=(Length value1, Length value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Returns true if left value is less than right value.
        /// </summary>
        /// <param name="value1">A <see cref="Length"/> object left hand operand.</param>
        /// <param name="value2">A <see cref="Length"/> object right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result of the operation.</returns>
        public static bool operator <(Length value1, Length value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        /// <summary>
        /// Returns true if left value is less or equal to than right value.
        /// </summary>
        /// <param name="value1">A <see cref="Length"/> object left hand operand.</param>
        /// <param name="value2">A <see cref="Length"/> object right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result of the operation.</returns>
        public static bool operator <=(Length value1, Length value2)
        {
            return value1.CompareTo(value2) <= 0;
        }

        /// <summary>
        /// Returns true if left value is greater than right value.
        /// </summary>
        /// <param name="value1">A <see cref="Length"/> object left hand operand.</param>
        /// <param name="value2">A <see cref="Length"/> object right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result of the operation.</returns>
        public static bool operator >(Length value1, Length value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        /// <summary>
        /// Returns true if left value is greater than or equal to right value.
        /// </summary>
        /// <param name="value1">A <see cref="Length"/> object left hand operand.</param>
        /// <param name="value2">A <see cref="Length"/> object right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result of the operation.</returns>
        public static bool operator >=(Length value1, Length value2)
        {
            return value1.CompareTo(value2) >= 0;
        }

        #endregion

        #region [ Type Conversion Operators ]

        /// <summary>
        /// Implicitly converts value, represented in meters, to a <see cref="Length"/>.
        /// </summary>
        /// <param name="value">A <see cref="Double"/> value.</param>
        /// <returns>A <see cref="Length"/> object.</returns>
        public static implicit operator Length(double value)
        {
            return new Length(value);
        }

        /// <summary>
        /// Implicitly converts <see cref="Length"/>, represented in meters, to a <see cref="Double"/>.
        /// </summary>
        /// <param name="value">A <see cref="Length"/> object.</param>
        /// <returns>A <see cref="Double"/> value.</returns>
        public static implicit operator double(Length value)
        {
            return value.m_value;
        }

        #endregion

        #region [ Arithmetic Operators ]

        /// <summary>
        /// Returns computed remainder after dividing first value by the second.
        /// </summary>
        /// <param name="value1">A <see cref="Length"/> object left hand operand.</param>
        /// <param name="value2">A <see cref="Length"/> object right hand operand.</param>
        /// <returns>A <see cref="Length"/> object as the result.</returns>
        public static Length operator %(Length value1, Length value2)
        {
            return value1.m_value % value2.m_value;
        }

        /// <summary>
        /// Returns computed sum of values.
        /// </summary>
        /// <param name="value1">A <see cref="Length"/> object left hand operand.</param>
        /// <param name="value2">A <see cref="Length"/> object right hand operand.</param>
        /// <returns>A <see cref="Length"/> object as the result.</returns>
        public static Length operator +(Length value1, Length value2)
        {
            return value1.m_value + value2.m_value;
        }

        /// <summary>
        /// Returns computed difference of values.
        /// </summary>
        /// <param name="value1">A <see cref="Length"/> object left hand operand.</param>
        /// <param name="value2">A <see cref="Length"/> object right hand operand.</param>
        /// <returns>A <see cref="Length"/> object as the result.</returns>
        public static Length operator -(Length value1, Length value2)
        {
            return value1.m_value - value2.m_value;
        }

        /// <summary>
        /// Returns computed product of values.
        /// </summary>
        /// <param name="value1">A <see cref="Length"/> object left hand operand.</param>
        /// <param name="value2">A <see cref="Length"/> object right hand operand.</param>
        /// <returns>A <see cref="Length"/> object as the result.</returns>
        public static Length operator *(Length value1, Length value2)
        {
            return value1.m_value * value2.m_value;
        }

        /// <summary>
        /// Returns computed division of values.
        /// </summary>
        /// <param name="value1">A <see cref="Length"/> object left hand operand.</param>
        /// <param name="value2">A <see cref="Length"/> object right hand operand.</param>
        /// <returns>A <see cref="Length"/> object as the result.</returns>
        public static Length operator /(Length value1, Length value2)
        {
            return value1.m_value / value2.m_value;
        }

        // C# doesn't expose an exponent operator but some other .NET languages do,
        // so we expose the operator via its native special IL function name

        /// <summary>
        /// Returns result of first value raised to power of second value.
        /// </summary>
        /// <param name="value1">A <see cref="Length"/> object left hand operand.</param>
        /// <param name="value2">A <see cref="Length"/> object right hand operand.</param>
        /// <returns>A <see cref="Double"/> value as the result.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced), SpecialName]
        public static double op_Exponent(Length value1, Length value2)
        {
            return Math.Pow(value1.m_value, value2.m_value);
        }

        #endregion

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>Represents the largest possible value of an <see cref="Length"/>. This field is constant.</summary>
        public static readonly Length MaxValue = double.MaxValue;

        /// <summary>Represents the smallest possible value of an <see cref="Length"/>. This field is constant.</summary>
        public static readonly Length MinValue = double.MinValue;

        // Static Methods

        /// <summary>
        /// Creates a new <see cref="Length"/> value from the specified <paramref name="value"/> in feet.
        /// </summary>
        /// <param name="value">New <see cref="Length"/> value in feet.</param>
        /// <returns>New <see cref="Length"/> object from the specified <paramref name="value"/> in feet.</returns>
        public static Length FromFeet(double value)
        {
            return new Length(value * FeetFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Length"/> value from the specified <paramref name="value"/> in yards.
        /// </summary>
        /// <param name="value">New <see cref="Length"/> value in yards.</param>
        /// <returns>New <see cref="Length"/> object from the specified <paramref name="value"/> in yards.</returns>
        public static Length FromYards(double value)
        {
            return new Length(value * YardsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Length"/> value from the specified <paramref name="value"/> in inches.
        /// </summary>
        /// <param name="value">New <see cref="Length"/> value in inches.</param>
        /// <returns>New <see cref="Length"/> object from the specified <paramref name="value"/> in inches.</returns>
        public static Length FromInches(double value)
        {
            return new Length(value * InchesFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Length"/> value from the specified <paramref name="value"/> in miles.
        /// </summary>
        /// <param name="value">New <see cref="Length"/> value in miles.</param>
        /// <returns>New <see cref="Length"/> object from the specified <paramref name="value"/> in miles.</returns>
        public static Length FromMiles(double value)
        {
            return new Length(value * MilesFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Length"/> value from the specified <paramref name="value"/> in light-seconds.
        /// </summary>
        /// <param name="value">New <see cref="Length"/> value in light-seconds.</param>
        /// <returns>New <see cref="Length"/> object from the specified <paramref name="value"/> in light-seconds.</returns>
        public static Length FromLightSeconds(double value)
        {
            return new Length(value * LightSecondsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Length"/> value from the specified <paramref name="value"/> in US survey feet.
        /// </summary>
        /// <param name="value">New <see cref="Length"/> value in US survey feet.</param>
        /// <returns>New <see cref="Length"/> object from the specified <paramref name="value"/> in US survey feet.</returns>
        public static Length FromUSSurveyFeet(double value)
        {
            return new Length(value * USSurveyFeetFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Length"/> value from the specified <paramref name="value"/> in US survey miles.
        /// </summary>
        /// <param name="value">New <see cref="Length"/> value in US survey miles.</param>
        /// <returns>New <see cref="Length"/> object from the specified <paramref name="value"/> in US survey miles.</returns>
        public static Length FromUSSurveyMiles(double value)
        {
            return new Length(value * USSurveyMilesFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Length"/> value from the specified <paramref name="value"/> in nautical miles.
        /// </summary>
        /// <param name="value">New <see cref="Length"/> value in nautical miles.</param>
        /// <returns>New <see cref="Length"/> object from the specified <paramref name="value"/> in nautical miles.</returns>
        public static Length FromNauticalMiles(double value)
        {
            return new Length(value * NauticalMilesFactor);
        }

        /// <summary>
        /// Converts the <paramref name="value"/> in the specified <paramref name="sourceUnit"/> to a new <see cref="Length"/> in meters.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <param name="sourceUnit">Source value units.</param>
        /// <returns>New <see cref="Length"/> from the specified <paramref name="value"/> in <paramref name="sourceUnit"/>.</returns>
        public static Length ConvertFrom(double value, LengthUnit sourceUnit)
        {
            switch (sourceUnit)
            {
                case LengthUnit.Meters:
                    return value;
                case LengthUnit.Feet:
                    return FromFeet(value);
                case LengthUnit.Inches:
                    return FromInches(value);
                case LengthUnit.Miles:
                    return FromMiles(value);
                case LengthUnit.LightSeconds:
                    return FromLightSeconds(value);
                case LengthUnit.USSurveyFeet:
                    return FromUSSurveyFeet(value);
                case LengthUnit.USSurveyMiles:
                    return FromUSSurveyMiles(value);
                case LengthUnit.NauticalMiles:
                    return FromNauticalMiles(value);
                case LengthUnit.Yards:
                    return FromYards(value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(sourceUnit), sourceUnit, null);
            }
        }

        #endregion
    }
}