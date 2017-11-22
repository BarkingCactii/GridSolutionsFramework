﻿//******************************************************************************************************
//  Speed.cs - Gbtc
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
    /// Represents the units available for a <see cref="Speed"/> value.
    /// </summary>
    public enum SpeedUnit
    {
        /// <summary>
        /// Meters per second speed units.
        /// </summary>
        MetersPerSecond,
        /// <summary>
        /// Miles per hour speed units.
        /// </summary>
        MilesPerHour,
        /// <summary>
        /// Kilometers per hour speed units.
        /// </summary>
        KilometersPerHour,
        /// <summary>
        /// Feet per minute speed units.
        /// </summary>
        FeetPerMinute,
        /// <summary>
        /// Inches per second speed units.
        /// </summary>
        InchesPerSecond,
        /// <summary>
        /// Knot speed units, international.
        /// </summary>
        Knots,
        /// <summary>
        /// Mach speed units.
        /// </summary>
        /// <remarks>
        /// As modeled by International Standard Atmosphere, dry air at mean sea level, standard temperature of 15 °C.
        /// </remarks>
        Mach
    }

    #endregion

    /// <summary>
    /// Represents a speed measurement, in meters per second, as a double-precision floating-point number.
    /// </summary>
    /// <remarks>
    /// This class behaves just like a <see cref="double"/> representing a speed in meters per second; it is implicitly
    /// castable to and from a <see cref="double"/> and therefore can be generally used "as" a double, but it
    /// has the advantage of handling conversions to and from other speed representations, specifically
    /// miles per hour, kilometers per hour, feet per minute, inches per second, knots and Mach.
    /// <example>
    /// This example converts mph to km/h:
    /// <code>
    /// public double GetKilometersPerHour(double milesPerHour)
    /// {
    ///     return Speed.FromMilesPerHour(milesPerHour).ToKilometersPerHour();
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    [Serializable]
    public struct Speed : IComparable, IFormattable, IConvertible, IComparable<Speed>, IComparable<double>, IEquatable<Speed>, IEquatable<double>
    {
        #region [ Members ]

        // Constants
        private const double MilesPerHourFactor = 0.44704D;

        private const double KilometersPerHourFactor = 2.777778e-1D;

        private const double FeetPerMinuteFactor = 5.08e-3D;

        private const double InchesPerSecondFactor = 2.54e-2D;

        private const double KnotsFactor = 0.514444D;

        private const double MachFactor = 340.3D; // As modeled by ISA

        // Fields
        private readonly double m_value; // Speed value stored in meters per second

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="Speed"/>.
        /// </summary>
        /// <param name="value">New speed value in meters per second.</param>
        public Speed(double value)
        {
            m_value = value;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the <see cref="Speed"/> value in miles per hour.
        /// </summary>
        /// <returns>Value of <see cref="Speed"/> in miles per hour.</returns>
        public double ToMilesPerHour()
        {
            return m_value / MilesPerHourFactor;
        }

        /// <summary>
        /// Gets the <see cref="Speed"/> value in kilometers per hour.
        /// </summary>
        /// <returns>Value of <see cref="Speed"/> in kilometers per hour.</returns>
        public double ToKilometersPerHour()
        {
            return m_value / KilometersPerHourFactor;
        }

        /// <summary>
        /// Gets the <see cref="Speed"/> value in feet per minute.
        /// </summary>
        /// <returns>Value of <see cref="Speed"/> in feet per minute.</returns>
        public double ToFeetPerMinute()
        {
            return m_value / FeetPerMinuteFactor;
        }

        /// <summary>
        /// Gets the <see cref="Speed"/> value in inches per second.
        /// </summary>
        /// <returns>Value of <see cref="Speed"/> in inches per second.</returns>
        public double ToInchesPerSecond()
        {
            return m_value / InchesPerSecondFactor;
        }

        /// <summary>
        /// Gets the <see cref="Speed"/> value in knots (International).
        /// </summary>
        /// <returns>Value of <see cref="Speed"/> in knots.</returns>
        public double ToKnots()
        {
            return m_value / KnotsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Speed"/> value in Mach.
        /// </summary>
        /// <returns>Value of <see cref="Speed"/> in Mach.</returns>
        public double ToMach()
        {
            return m_value / MachFactor;
        }

        /// <summary>
        /// Converts the <see cref="Speed"/> to the specified <paramref name="targetUnit"/>.
        /// </summary>
        /// <param name="targetUnit">Target units.</param>
        /// <returns><see cref="Speed"/> converted to <paramref name="targetUnit"/>.</returns>
        public double ConvertTo(SpeedUnit targetUnit)
        {
            switch (targetUnit)
            {
                case SpeedUnit.MetersPerSecond:
                    return m_value;
                case SpeedUnit.MilesPerHour:
                    return ToMilesPerHour();
                case SpeedUnit.KilometersPerHour:
                    return ToKilometersPerHour();
                case SpeedUnit.FeetPerMinute:
                    return ToFeetPerMinute();
                case SpeedUnit.InchesPerSecond:
                    return ToInchesPerSecond();
                case SpeedUnit.Knots:
                    return ToKnots();
                case SpeedUnit.Mach:
                    return ToMach();
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
        /// <exception cref="ArgumentException">value is not a <see cref="Double"/> or <see cref="Speed"/>.</exception>
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;

            double num;

            if (value is double)
                num = (double)value;

            else if (value is Speed)
                num = (Speed)value;

            else
                throw new ArgumentException("Argument must be a Double or a Speed");

            return m_value < num ? -1 : (m_value > num ? 1 : 0);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Speed"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">A <see cref="Speed"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        public int CompareTo(Speed value)
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
        /// True if obj is an instance of <see cref="Double"/> or <see cref="Speed"/> and equals the value of this instance;
        /// otherwise, False.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is double)
                return Equals((double)obj);

            if (obj is Speed)
                return Equals((Speed)obj);

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Speed"/> value.
        /// </summary>
        /// <param name="obj">A <see cref="Speed"/> value to compare to this instance.</param>
        /// <returns>
        /// True if obj has the same value as this instance; otherwise, False.
        /// </returns>
        public bool Equals(Speed obj)
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
        /// Converts the string representation of a number to its <see cref="Speed"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <returns>
        /// A <see cref="Speed"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Speed.MinValue"/> or greater than <see cref="Speed.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        public static Speed Parse(string s)
        {
            return double.Parse(s);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style to its <see cref="Speed"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <returns>
        /// A <see cref="Speed"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Speed.MinValue"/> or greater than <see cref="Speed.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        public static Speed Parse(string s, NumberStyles style)
        {
            return double.Parse(s, style);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified culture-specific format to its <see cref="Speed"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// A <see cref="Speed"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Speed.MinValue"/> or greater than <see cref="Speed.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        public static Speed Parse(string s, IFormatProvider provider)
        {
            return double.Parse(s, provider);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its <see cref="Speed"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// A <see cref="Speed"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Speed.MinValue"/> or greater than <see cref="Speed.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        public static Speed Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            return double.Parse(s, style, provider);
        }

        /// <summary>
        /// Converts the string representation of a number to its <see cref="Speed"/> equivalent. A return value
        /// indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Speed"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter per second is null,
        /// is not of the correct format, or represents a number less than <see cref="Speed.MinValue"/> or greater than <see cref="Speed.MaxValue"/>.
        /// This parameter per second is passed uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string s, out Speed result)
        {
            double parseResult;
            bool parseResponse;

            parseResponse = double.TryParse(s, out parseResult);
            result = parseResult;

            return parseResponse;
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its
        /// <see cref="Speed"/> equivalent. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Speed"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter per second is null,
        /// is not in a format compliant with style, or represents a number less than <see cref="Speed.MinValue"/> or
        /// greater than <see cref="Speed.MaxValue"/>. This parameter per second is passed uninitialized.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> object that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Speed result)
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
        /// <param name="value1">A <see cref="Speed"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Speed"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result of the operation.</returns>
        public static bool operator ==(Speed value1, Speed value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Compares the two values for inequality.
        /// </summary>
        /// <param name="value1">A <see cref="Speed"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Speed"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result of the operation.</returns>
        public static bool operator !=(Speed value1, Speed value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Returns true if left value is less than right value.
        /// </summary>
        /// <param name="value1">A <see cref="Speed"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Speed"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result of the operation.</returns>
        public static bool operator <(Speed value1, Speed value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        /// <summary>
        /// Returns true if left value is less or equal to than right value.
        /// </summary>
        /// <param name="value1">A <see cref="Speed"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Speed"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result of the operation.</returns>
        public static bool operator <=(Speed value1, Speed value2)
        {
            return value1.CompareTo(value2) <= 0;
        }

        /// <summary>
        /// Returns true if left value is greater than right value.
        /// </summary>
        /// <param name="value1">A <see cref="Speed"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Speed"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result of the operation.</returns>
        public static bool operator >(Speed value1, Speed value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        /// <summary>
        /// Returns true if left value is greater than or equal to right value.
        /// </summary>
        /// <param name="value1">A <see cref="Speed"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Speed"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> value as the result of the operation.</returns>
        public static bool operator >=(Speed value1, Speed value2)
        {
            return value1.CompareTo(value2) >= 0;
        }

        #endregion

        #region [ Type Conversion Operators ]

        /// <summary>
        /// Implicitly converts value, represented in meters per second, to a <see cref="Speed"/>.
        /// </summary>
        /// <param name="value">A <see cref="Double"/> value.</param>
        /// <returns>A <see cref="Speed"/> object.</returns>
        public static implicit operator Speed(double value)
        {
            return new Speed(value);
        }

        /// <summary>
        /// Implicitly converts <see cref="Speed"/>, represented in meters per second, to a <see cref="Double"/>.
        /// </summary>
        /// <param name="value">A <see cref="Speed"/> object.</param>
        /// <returns>A <see cref="Double"/> value.</returns>
        public static implicit operator double(Speed value)
        {
            return value.m_value;
        }

        #endregion

        #region [ Arithmetic Operators ]

        /// <summary>
        /// Returns computed remainder after dividing first value by the second.
        /// </summary>
        /// <param name="value1">A <see cref="Speed"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Speed"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Speed"/> object as the result.</returns>
        public static Speed operator %(Speed value1, Speed value2)
        {
            return value1.m_value % value2.m_value;
        }

        /// <summary>
        /// Returns computed sum of values.
        /// </summary>
        /// <param name="value1">A <see cref="Speed"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Speed"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Speed"/> object as the result.</returns>
        public static Speed operator +(Speed value1, Speed value2)
        {
            return value1.m_value + value2.m_value;
        }

        /// <summary>
        /// Returns computed difference of values.
        /// </summary>
        /// <param name="value1">A <see cref="Speed"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Speed"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Speed"/> object as the result.</returns>
        public static Speed operator -(Speed value1, Speed value2)
        {
            return value1.m_value - value2.m_value;
        }

        /// <summary>
        /// Returns computed product of values.
        /// </summary>
        /// <param name="value1">A <see cref="Speed"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Speed"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Speed"/> object as the result.</returns>
        public static Speed operator *(Speed value1, Speed value2)
        {
            return value1.m_value * value2.m_value;
        }

        /// <summary>
        /// Returns computed division of values.
        /// </summary>
        /// <param name="value1">A <see cref="Speed"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Speed"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Speed"/> object as the result.</returns>
        public static Speed operator /(Speed value1, Speed value2)
        {
            return value1.m_value / value2.m_value;
        }

        // C# doesn't expose an exponent operator but some other .NET languages do,
        // so we expose the operator via its native special IL function name

        /// <summary>
        /// Returns result of first value raised to speed of second value.
        /// </summary>
        /// <param name="value1">A <see cref="Speed"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Speed"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Double"/> value as the result.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced), SpecialName]
        public static double op_Exponent(Speed value1, Speed value2)
        {
            return Math.Pow(value1.m_value, value2.m_value);
        }

        #endregion

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>Represents the largest possible value of an <see cref="Speed"/>. This field is constant.</summary>
        public static readonly Speed MaxValue = double.MaxValue;

        /// <summary>Represents the smallest possible value of an <see cref="Speed"/>. This field is constant.</summary>
        public static readonly Speed MinValue = double.MinValue;

        // Static Methods

        /// <summary>
        /// Creates a new <see cref="Speed"/> value from the specified <paramref name="value"/> in miles per hour.
        /// </summary>
        /// <param name="value">New <see cref="Speed"/> value in miles per hour.</param>
        /// <returns>New <see cref="Speed"/> object from the specified <paramref name="value"/> in miles per hour.</returns>
        public static Speed FromMilesPerHour(double value)
        {
            return new Speed(value * MilesPerHourFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Speed"/> value from the specified <paramref name="value"/> in kilometers per hour.
        /// </summary>
        /// <param name="value">New <see cref="Speed"/> value in kilometers per hour.</param>
        /// <returns>New <see cref="Speed"/> object from the specified <paramref name="value"/> in kilometers per hour.</returns>
        public static Speed FromKilometersPerHour(double value)
        {
            return new Speed(value * KilometersPerHourFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Speed"/> value from the specified <paramref name="value"/> in feet per minute.
        /// </summary>
        /// <param name="value">New <see cref="Speed"/> value in feet per minute.</param>
        /// <returns>New <see cref="Speed"/> object from the specified <paramref name="value"/> in feet per minute.</returns>
        public static Speed FromFeetPerMinute(double value)
        {
            return new Speed(value * FeetPerMinuteFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Speed"/> value from the specified <paramref name="value"/> in inches per second.
        /// </summary>
        /// <param name="value">New <see cref="Speed"/> value in inches per second.</param>
        /// <returns>New <see cref="Speed"/> object from the specified <paramref name="value"/> in inches per second.</returns>
        public static Speed FromInchesPerSecond(double value)
        {
            return new Speed(value * InchesPerSecondFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Speed"/> value from the specified <paramref name="value"/> in knots (International).
        /// </summary>
        /// <param name="value">New <see cref="Speed"/> value in knots.</param>
        /// <returns>New <see cref="Speed"/> object from the specified <paramref name="value"/> in knots.</returns>
        public static Speed FromKnots(double value)
        {
            return new Speed(value * KnotsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Speed"/> value from the specified <paramref name="value"/> in Mach.
        /// </summary>
        /// <param name="value">New <see cref="Speed"/> value in Mach.</param>
        /// <returns>New <see cref="Speed"/> object from the specified <paramref name="value"/> in Mach.</returns>
        public static Speed FromMach(double value)
        {
            return new Speed(value * MachFactor);
        }

        /// <summary>
        /// Converts the <paramref name="value"/> in the specified <paramref name="sourceUnit"/> to a new <see cref="Speed"/> in meters per second.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <param name="sourceUnit">Source value units.</param>
        /// <returns>New <see cref="Speed"/> from the specified <paramref name="value"/> in <paramref name="sourceUnit"/>.</returns>
        public static Speed ConvertFrom(double value, SpeedUnit sourceUnit)
        {
            switch (sourceUnit)
            {
                case SpeedUnit.MetersPerSecond:
                    return value;
                case SpeedUnit.MilesPerHour:
                    return FromMilesPerHour(value);
                case SpeedUnit.KilometersPerHour:
                    return FromKilometersPerHour(value);
                case SpeedUnit.FeetPerMinute:
                    return FromFeetPerMinute(value);
                case SpeedUnit.InchesPerSecond:
                    return FromInchesPerSecond(value);
                case SpeedUnit.Knots:
                    return FromKnots(value);
                case SpeedUnit.Mach:
                    return FromMach(value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(sourceUnit), sourceUnit, null);
            }
        }

        #endregion
    }
}