﻿//******************************************************************************************************
//  Temperature.cs - Gbtc
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
    /// Represents the units available for a <see cref="Temperature"/> value.
    /// </summary>
    public enum TemperatureUnit
    {
        /// <summary>
        /// Kelvin temperature units.
        /// </summary>
        Kelvin,
        /// <summary>
        /// Celsius temperature units.
        /// </summary>
        Celsius,
        /// <summary>
        /// Fahrenheit temperature units.
        /// </summary>
        Fahrenheit,
        /// <summary>
        /// Newton temperature units.
        /// </summary>
        Newton,
        /// <summary>
        /// Rankine temperature units.
        /// </summary>
        Rankine,
        /// <summary>
        /// Delisle temperature units.
        /// </summary>
        Delisle,
        /// <summary>
        /// Réaumur temperature units.
        /// </summary>
        Réaumur,
        /// <summary>
        /// Rømer temperature units.
        /// </summary>
        Rømer
    }

    #endregion

    /// <summary>
    /// Represents a temperature, in Kelvin, as a double-precision floating-point number.
    /// </summary>
    /// <remarks>
    /// This class behaves just like a <see cref="double"/> representing a temperature in Kelvin; it is implicitly
    /// castable to and from a <see cref="double"/> and therefore can be generally used "as" a double, but it
    /// has the advantage of handling conversions to and from other temperature representations, specifically
    /// Celsius, Fahrenheit, Newton, Rankine, Delisle, Réaumur and Rømer.
    /// <example>
    /// This example converts Celsius to Fahrenheit:
    /// <code>
    /// public double GetFahrenheit(double celsius)
    /// {
    ///     return Temperature.FromCelsius(celsius).ToFahrenheit();
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    [Serializable]
    public struct Temperature : IComparable, IFormattable, IConvertible, IComparable<Temperature>, IComparable<double>, IEquatable<Temperature>, IEquatable<double>
    {
        #region [ Members ]

        // Constants
        private const double CelsiusFactor = 1.0D;
        private const double CelsiusOffset = 273.15D;
        private const double CelsiusStep = 0.0D;

        private const double FahrenheitFactor = 5.0D / 9.0D;
        private const double FahrenheitOffset = 0.0D;
        private const double FahrenheitStep = 459.67D;

        private const double NewtonFactor = 100.0D / 33.0D;
        private const double NewtonOffset = CelsiusOffset;
        private const double NewtonStep = 0.0D;

        private const double RankineFactor = FahrenheitFactor;
        private const double RankineOffset = 0.0D;
        private const double RankineStep = 0.0D;

        private const double DelisleFactor = -2.0D / 3.0D;
        private const double DelisleOffset = 373.15D;
        private const double DelisleStep = 0.0D;

        private const double RéaumurFactor = 5.0D / 4.0D;
        private const double RéaumurOffset = CelsiusOffset;
        private const double RéaumurStep = 0.0D;

        private const double RømerFactor = 40.0D / 21.0D;
        private const double RømerOffset = CelsiusOffset;
        private const double RømerStep = -7.5D;

        // Fields
        private readonly double m_value; // Temperature value stored in Kelvin

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="Temperature"/>.
        /// </summary>
        /// <param name="value">New temperature value in Kelvin.</param>
        public Temperature(double value)
        {
            m_value = value;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the <see cref="Temperature"/> value in Celsius.
        /// </summary>
        /// <returns>Value of <see cref="Temperature"/> in Celsius.</returns>
        public double ToCelsius()
        {
            return ToTemperature(CelsiusFactor, CelsiusOffset, CelsiusStep);
        }

        /// <summary>
        /// Gets the <see cref="Temperature"/> value in Fahrenheit.
        /// </summary>
        /// <returns>Value of <see cref="Temperature"/> in Fahrenheit.</returns>
        public double ToFahrenheit()
        {
            return ToTemperature(FahrenheitFactor, FahrenheitOffset, FahrenheitStep);
        }

        /// <summary>
        /// Gets the <see cref="Temperature"/> value in Newton.
        /// </summary>
        /// <returns>Value of <see cref="Temperature"/> in Newton.</returns>
        public double ToNewton()
        {
            return ToTemperature(NewtonFactor, NewtonOffset, NewtonStep);
        }

        /// <summary>
        /// Gets the <see cref="Temperature"/> value in Rankine.
        /// </summary>
        /// <returns>Value of <see cref="Temperature"/> in Rankine.</returns>
        public double ToRankine()
        {
            return ToTemperature(RankineFactor, RankineOffset, RankineStep);
        }

        /// <summary>
        /// Gets the <see cref="Temperature"/> value in Delisle.
        /// </summary>
        /// <returns>Value of <see cref="Temperature"/> in Delisle.</returns>
        public double ToDelisle()
        {
            return ToTemperature(DelisleFactor, DelisleOffset, DelisleStep);
        }

        /// <summary>
        /// Gets the <see cref="Temperature"/> value in Réaumur.
        /// </summary>
        /// <returns>Value of <see cref="Temperature"/> in Réaumur.</returns>
        public double ToRéaumur()
        {
            return ToTemperature(RéaumurFactor, RéaumurOffset, RéaumurStep);
        }

        /// <summary>
        /// Gets the <see cref="Temperature"/> value in Rømer.
        /// </summary>
        /// <returns>Value of <see cref="Temperature"/> in Rømer.</returns>
        public double ToRømer()
        {
            return ToTemperature(RømerFactor, RømerOffset, RømerStep);
        }

        /// <summary>
        /// Converts the <see cref="Temperature"/> to the specified <paramref name="targetUnit"/>.
        /// </summary>
        /// <param name="targetUnit">Target units.</param>
        /// <returns><see cref="Temperature"/> converted to <paramref name="targetUnit"/>.</returns>
        public double ConvertTo(TemperatureUnit targetUnit)
        {
            switch (targetUnit)
            {
                case TemperatureUnit.Kelvin:
                    return m_value;
                case TemperatureUnit.Celsius:
                    return ToCelsius();
                case TemperatureUnit.Fahrenheit:
                    return ToFahrenheit();
                case TemperatureUnit.Newton:
                    return ToNewton();
                case TemperatureUnit.Rankine:
                    return ToRankine();
                case TemperatureUnit.Delisle:
                    return ToDelisle();
                case TemperatureUnit.Réaumur:
                    return ToRéaumur();
                case TemperatureUnit.Rømer:
                    return ToRømer();
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetUnit), targetUnit, null);
            }
        }

        // Calculate temperature based on value = (K - offset) / factor - step
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Temperature ToTemperature(double factor, double offset, double step)
        {
            return (m_value - offset) / factor - step;
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
        /// <exception cref="ArgumentException">value is not a <see cref="Double"/> or <see cref="Temperature"/>.</exception>
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;

            double num;

            if (value is double)
                num = (double)value;

            else if (value is Temperature)
                num = (Temperature)value;

            else
                throw new ArgumentException("Argument must be a Double or a Temperature");

            return m_value < num ? -1 : (m_value > num ? 1 : 0);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Temperature"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">A <see cref="Temperature"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        public int CompareTo(Temperature value)
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
        /// True if obj is an instance of <see cref="Double"/> or <see cref="Temperature"/> and equals the value of this instance;
        /// otherwise, False.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is double)
                return Equals((double)obj);

            if (obj is Temperature)
                return Equals((Temperature)obj);

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Temperature"/> value.
        /// </summary>
        /// <param name="obj">A <see cref="Temperature"/> value to compare to this instance.</param>
        /// <returns>
        /// True if obj has the same value as this instance; otherwise, False.
        /// </returns>
        public bool Equals(Temperature obj)
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
        /// Converts the string representation of a number to its <see cref="Temperature"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <returns>
        /// A <see cref="Temperature"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Temperature.MinValue"/> or greater than <see cref="Temperature.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        public static Temperature Parse(string s)
        {
            return double.Parse(s);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style to its <see cref="Temperature"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <returns>
        /// A <see cref="Temperature"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Temperature.MinValue"/> or greater than <see cref="Temperature.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        public static Temperature Parse(string s, NumberStyles style)
        {
            return double.Parse(s, style);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified culture-specific format to its <see cref="Temperature"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// A <see cref="Temperature"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Temperature.MinValue"/> or greater than <see cref="Temperature.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        public static Temperature Parse(string s, IFormatProvider provider)
        {
            return double.Parse(s, provider);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its <see cref="Temperature"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// A <see cref="Temperature"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Temperature.MinValue"/> or greater than <see cref="Temperature.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        public static Temperature Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            return double.Parse(s, style, provider);
        }

        /// <summary>
        /// Converts the string representation of a number to its <see cref="Temperature"/> equivalent. A return value
        /// indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Temperature"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null,
        /// is not of the correct format, or represents a number less than <see cref="Temperature.MinValue"/> or greater than <see cref="Temperature.MaxValue"/>.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string s, out Temperature result)
        {
            double parseResult;
            bool parseResponse;

            parseResponse = double.TryParse(s, out parseResult);
            result = parseResult;

            return parseResponse;
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its
        /// <see cref="Temperature"/> equivalent. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Temperature"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null,
        /// is not in a format compliant with style, or represents a number less than <see cref="Temperature.MinValue"/> or
        /// greater than <see cref="Temperature.MaxValue"/>. This parameter is passed uninitialized.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> object that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Temperature result)
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
        /// <param name="value1">A <see cref="Temperature"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Temperature"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the result of the operation.</returns>
        public static bool operator ==(Temperature value1, Temperature value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Compares the two values for inequality.
        /// </summary>
        /// <param name="value1">A <see cref="Temperature"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Temperature"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the result of the operation.</returns>
        public static bool operator !=(Temperature value1, Temperature value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Returns true if left value is less than right value.
        /// </summary>
        /// <param name="value1">A <see cref="Temperature"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Temperature"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the result of the operation.</returns>
        public static bool operator <(Temperature value1, Temperature value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        /// <summary>
        /// Returns true if left value is less or equal to than right value.
        /// </summary>
        /// <param name="value1">A <see cref="Temperature"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Temperature"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the result of the operation.</returns>
        public static bool operator <=(Temperature value1, Temperature value2)
        {
            return value1.CompareTo(value2) <= 0;
        }

        /// <summary>
        /// Returns true if left value is greater than right value.
        /// </summary>
        /// <param name="value1">A <see cref="Temperature"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Temperature"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the result of the operation.</returns>
        public static bool operator >(Temperature value1, Temperature value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        /// <summary>
        /// Returns true if left value is greater than or equal to right value.
        /// </summary>
        /// <param name="value1">A <see cref="Temperature"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Temperature"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the result of the operation.</returns>
        public static bool operator >=(Temperature value1, Temperature value2)
        {
            return value1.CompareTo(value2) >= 0;
        }

        #endregion

        #region [ Type Conversion Operators ]

        /// <summary>
        /// Implicitly converts value, represented in Kelvin, to a <see cref="Temperature"/>.
        /// </summary>
        /// <param name="value">A <see cref="Double"/> value.</param>
        /// <returns>A <see cref="Temperature"/> object.</returns>
        public static implicit operator Temperature(double value)
        {
            return new Temperature(value);
        }

        /// <summary>
        /// Implicitly converts <see cref="Temperature"/>, represented in Kelvin, to a <see cref="Double"/>.
        /// </summary>
        /// <param name="value">A <see cref="Temperature"/> object.</param>
        /// <returns>A <see cref="Double"/> value.</returns>
        public static implicit operator double(Temperature value)
        {
            return value.m_value;
        }

        #endregion

        #region [ Arithmetic Operators ]

        /// <summary>
        /// Returns computed remainder after dividing first value by the second.
        /// </summary>
        /// <param name="value1">A <see cref="Temperature"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Temperature"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Temperature"/> object as the result of the operation.</returns>
        public static Temperature operator %(Temperature value1, Temperature value2)
        {
            return value1.m_value % value2.m_value;
        }

        /// <summary>
        /// Returns computed sum of values.
        /// </summary>
        /// <param name="value1">A <see cref="Temperature"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Temperature"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Temperature"/> object as the result of the operation.</returns>
        public static Temperature operator +(Temperature value1, Temperature value2)
        {
            return value1.m_value + value2.m_value;
        }

        /// <summary>
        /// Returns computed difference of values.
        /// </summary>
        /// <param name="value1">A <see cref="Temperature"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Temperature"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Temperature"/> object as the result of the operation.</returns>
        public static Temperature operator -(Temperature value1, Temperature value2)
        {
            return value1.m_value - value2.m_value;
        }

        /// <summary>
        /// Returns computed product of values.
        /// </summary>
        /// <param name="value1">A <see cref="Temperature"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Temperature"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Temperature"/> object as the result of the operation.</returns>
        public static Temperature operator *(Temperature value1, Temperature value2)
        {
            return value1.m_value * value2.m_value;
        }

        /// <summary>
        /// Returns computed division of values.
        /// </summary>
        /// <param name="value1">A <see cref="Temperature"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Temperature"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Temperature"/> object as the result of the operation.</returns>
        public static Temperature operator /(Temperature value1, Temperature value2)
        {
            return value1.m_value / value2.m_value;
        }

        // C# doesn't expose an exponent operator but some other .NET languages do,
        // so we expose the operator via its native special IL function name

        /// <summary>
        /// Returns result of first value raised to power of second value.
        /// </summary>
        /// <param name="value1">A <see cref="Temperature"/> object as the left hand operand.</param>
        /// <param name="value2">A <see cref="Temperature"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Double"/> value as the result of the operation.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced), SpecialName]
        public static double op_Exponent(Temperature value1, Temperature value2)
        {
            return Math.Pow(value1.m_value, value2.m_value);
        }

        #endregion

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>Represents the largest possible value of a <see cref="Temperature"/>. This field is constant.</summary>
        public static readonly Temperature MaxValue = double.MaxValue;

        /// <summary>Represents the smallest possible value of a <see cref="Temperature"/>. This field is constant.</summary>
        public static readonly Temperature MinValue = double.MinValue;

        // Static Methods

        /// <summary>
        /// Creates a new <see cref="Temperature"/> value from the specified <paramref name="value"/> in Celsius.
        /// </summary>
        /// <param name="value">New <see cref="Temperature"/> value in Celsius.</param>
        /// <returns>New <see cref="Temperature"/> object from the specified <paramref name="value"/> in Celsius.</returns>
        public static Temperature FromCelsius(double value)
        {
            return FromTemperature(value, CelsiusFactor, CelsiusOffset, CelsiusStep);
        }

        /// <summary>
        /// Creates a new <see cref="Temperature"/> value from the specified <paramref name="value"/> in Fahrenheit.
        /// </summary>
        /// <param name="value">New <see cref="Temperature"/> value in Fahrenheit.</param>
        /// <returns>New <see cref="Temperature"/> object from the specified <paramref name="value"/> in Fahrenheit.</returns>
        public static Temperature FromFahrenheit(double value)
        {
            return FromTemperature(value, FahrenheitFactor, FahrenheitOffset, FahrenheitStep);
        }

        /// <summary>
        /// Creates a new <see cref="Temperature"/> value from the specified <paramref name="value"/> in Newton.
        /// </summary>
        /// <param name="value">New <see cref="Temperature"/> value in Newton.</param>
        /// <returns>New <see cref="Temperature"/> object from the specified <paramref name="value"/> in Newton.</returns>
        public static Temperature FromNewton(double value)
        {
            return FromTemperature(value, NewtonFactor, NewtonOffset, NewtonStep);
        }

        /// <summary>
        /// Creates a new <see cref="Temperature"/> value from the specified <paramref name="value"/> in Rankine.
        /// </summary>
        /// <param name="value">New <see cref="Temperature"/> value in Rankine.</param>
        /// <returns>New <see cref="Temperature"/> object from the specified <paramref name="value"/> in Rankine.</returns>
        public static Temperature FromRankine(double value)
        {
            return FromTemperature(value, RankineFactor, RankineOffset, RankineStep);
        }

        /// <summary>
        /// Creates a new <see cref="Temperature"/> value from the specified <paramref name="value"/> in Delisle.
        /// </summary>
        /// <param name="value">New <see cref="Temperature"/> value in Delisle.</param>
        /// <returns>New <see cref="Temperature"/> object from the specified <paramref name="value"/> in Delisle.</returns>
        public static Temperature FromDelisle(double value)
        {
            return FromTemperature(value, DelisleFactor, DelisleOffset, DelisleStep);
        }

        /// <summary>
        /// Creates a new <see cref="Temperature"/> value from the specified <paramref name="value"/> in Réaumur.
        /// </summary>
        /// <param name="value">New <see cref="Temperature"/> value in Réaumur.</param>
        /// <returns>New <see cref="Temperature"/> object from the specified <paramref name="value"/> in Réaumur.</returns>
        public static Temperature FromRéaumur(double value)
        {
            return FromTemperature(value, RéaumurFactor, RéaumurOffset, RéaumurStep);
        }

        /// <summary>
        /// Creates a new <see cref="Temperature"/> value from the specified <paramref name="value"/> in Rømer.
        /// </summary>
        /// <param name="value">New <see cref="Temperature"/> value in Rømer.</param>
        /// <returns>New <see cref="Temperature"/> object from the specified <paramref name="value"/> in Rømer.</returns>
        public static Temperature FromRømer(double value)
        {
            return FromTemperature(value, RømerFactor, RømerOffset, RømerStep);
        }

        /// <summary>
        /// Converts the <paramref name="value"/> in the specified <paramref name="sourceUnit"/> to a new <see cref="Temperature"/> in Kelvin.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <param name="sourceUnit">Source value units.</param>
        /// <returns>New <see cref="Temperature"/> from the specified <paramref name="value"/> in <paramref name="sourceUnit"/>.</returns>
        public static Temperature ConvertFrom(double value, TemperatureUnit sourceUnit)
        {
            switch (sourceUnit)
            {
                case TemperatureUnit.Kelvin:
                    return value;
                case TemperatureUnit.Celsius:
                    return FromCelsius(value);
                case TemperatureUnit.Fahrenheit:
                    return FromFahrenheit(value);
                case TemperatureUnit.Newton:
                    return FromNewton(value);
                case TemperatureUnit.Rankine:
                    return FromRankine(value);
                case TemperatureUnit.Delisle:
                    return FromDelisle(value);
                case TemperatureUnit.Réaumur:
                    return FromRéaumur(value);
                case TemperatureUnit.Rømer:
                    return FromRømer(value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(sourceUnit), sourceUnit, null);
            }
        }

        // Calculate temperature based on K = (value + step) * factor + offset
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Temperature FromTemperature(double value, double factor, double offset, double step)
        {
            return new Temperature((value + step) * factor + offset);
        }

        #endregion
    }
}
