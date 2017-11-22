﻿//******************************************************************************************************
//  Energy.cs - Gbtc
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
    /// Represents the units available for an <see cref="Energy"/> value.
    /// </summary>
    public enum EnergyUnit
    {
        /// <summary>
        /// Joule energy units, i.e., watt-seconds.
        /// </summary>
        Joules,
        /// <summary>
        /// WattHour energy units.
        /// </summary>
        WattHours,
        /// <summary>
        /// BTU energy units.
        /// </summary>
        BTU,
        /// <summary>
        /// Celsius heat energy units.
        /// </summary>
        CelsiusHeatUnits,
        /// <summary>
        /// Liters atmosphere energy units.
        /// </summary>
        LitersAtmosphere,
        /// <summary>
        /// Calorie energy units.
        /// </summary>
        Calories,
        /// <summary>
        /// Horsepower-hour energy units.
        /// </summary>
        HorsepowerHours,
        /// <summary>
        /// Barrels of oil energy units.
        /// </summary>
        BarrelsOfOil,
        /// <summary>
        /// Tons of coal energy units.
        /// </summary>
        TonsOfCoal
    }

    #endregion

    /// <summary>
    /// Represents an energy measurement, in joules (i.e., watt-seconds), as a double-precision floating-point number.
    /// </summary>
    /// <remarks>
    /// This class behaves just like a <see cref="double"/> representing an energy in joules; it is implicitly
    /// castable to and from a <see cref="double"/> and therefore can be generally used "as" a double, but it
    /// has the advantage of handling conversions to and from other energy representations, specifically
    /// watt-hours, BTU, Celsius heat unit, liter-atmosphere, calorie, horsepower-hour, barrel of oil and ton of coal.
    /// Metric conversions are handled simply by applying the needed <see cref="SI"/> conversion factor, for example:
    /// <example>
    /// Convert energy in joules to megajoules:
    /// <code>
    /// public double GetMegajoules(Energy joules)
    /// {
    ///     return joules / SI.Mega;
    /// }
    /// </code>
    /// This example converts megajoules to kilowatt-hours:
    /// <code>
    /// public double GetKilowattHours(double megajoules)
    /// {
    ///     return (new Energy(megajoules * SI.Mega)).ToWattHours() / SI.Kilo;
    /// }
    /// </code>
    /// This example converts kilowatt-hours to megawatt-hours:
    /// <code>
    /// public double GetMegawattHours(double kilowattHours)
    /// {
    ///     return (kilowattHours * SI.Kilo) / SI.Mega;
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    [Serializable]
    public struct Energy : IComparable, IFormattable, IConvertible, IComparable<Energy>, IComparable<double>, IEquatable<Energy>, IEquatable<double>
    {
        #region [ Members ]

        // Constants
        private const double WattHoursFactor = Time.SecondsPerHour; // 1 joule = 1 watt-second

        private const double BTUFactor = 1.05505585262E+3D;

        private const double CelsiusHeatUnitsFactor = 1.899100534716E+3D;

        private const double LitersAtmosphereFactor = 101.325D;

        private const double CaloriesFactor = 4.1868D;

        private const double HorsepowerHoursFactor = 2.684519537696172792E+6D;

        private const double BarrelsOfOilFactor = 6.12E+9D;

        private const double TonsOfCoalFactor = 29.3076E+9D;

        // Fields
        private readonly double m_value; // Energy value stored in joules

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="Energy"/>.
        /// </summary>
        /// <param name="value">New energy value in joules.</param>
        public Energy(double value)
        {
            m_value = value;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the <see cref="Charge"/> value in coulombs given the specified <paramref name="volts"/>.
        /// </summary>
        /// <param name="volts">Source <see cref="Voltage"/> used to calculate <see cref="Charge"/> value.</param>
        /// <returns><see cref="Charge"/> value in coulombs given the specified <paramref name="volts"/>.</returns>
        public Charge ToCoulombs(Voltage volts)
        {
            return m_value / (double)volts;
        }

        /// <summary>
        /// Gets the <see cref="Energy"/> value in watt-hours.
        /// </summary>
        /// <returns>Value of <see cref="Energy"/> in watt-hours.</returns>
        public double ToWattHours()
        {
            return m_value / WattHoursFactor;
        }

        /// <summary>
        /// Gets the <see cref="Energy"/> value in BTU (International Table).
        /// </summary>
        /// <returns>Value of <see cref="Energy"/> in BTU.</returns>
        public double ToBTU()
        {
            return m_value / BTUFactor;
        }

        /// <summary>
        /// Gets the <see cref="Energy"/> value in Celsius heat units (International Table).
        /// </summary>
        /// <returns>Value of <see cref="Energy"/> in Celsius heat units.</returns>
        public double ToCelsiusHeatUnits()
        {
            return m_value / CelsiusHeatUnitsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Energy"/> value in liters-atmosphere.
        /// </summary>
        /// <returns>Value of <see cref="Energy"/> in liters-atmosphere.</returns>
        public double ToLitersAtmosphere()
        {
            return m_value / LitersAtmosphereFactor;
        }

        /// <summary>
        /// Gets the <see cref="Energy"/> value in calories (International Table).
        /// </summary>
        /// <returns>Value of <see cref="Energy"/> in calories.</returns>
        public double ToCalories()
        {
            return m_value / CaloriesFactor;
        }

        /// <summary>
        /// Gets the <see cref="Energy"/> value in horsepower-hours.
        /// </summary>
        /// <returns>Value of <see cref="Energy"/> in horsepower-hours.</returns>
        public double ToHorsepowerHours()
        {
            return m_value / HorsepowerHoursFactor;
        }

        /// <summary>
        /// Gets the <see cref="Energy"/> value in equivalent barrels of oil.
        /// </summary>
        /// <returns>Value of <see cref="Energy"/> in equivalent barrels of oil.</returns>
        public double ToBarrelsOfOil()
        {
            return m_value / BarrelsOfOilFactor;
        }

        /// <summary>
        /// Gets the <see cref="Energy"/> value in equivalent tons of coal.
        /// </summary>
        /// <returns>Value of <see cref="Energy"/> in equivalent tons of coal.</returns>
        public double ToTonsOfCoal()
        {
            return m_value / TonsOfCoalFactor;
        }

        /// <summary>
        /// Converts the <see cref="Energy"/> to the specified <paramref name="targetUnit"/>.
        /// </summary>
        /// <param name="targetUnit">Target units.</param>
        /// <returns><see cref="Energy"/> converted to <paramref name="targetUnit"/>.</returns>
        public double ConvertTo(EnergyUnit targetUnit)
        {
            switch (targetUnit)
            {
                case EnergyUnit.Joules:
                    return m_value;
                case EnergyUnit.WattHours:
                    return ToWattHours();
                case EnergyUnit.BTU:
                    return ToBTU();
                case EnergyUnit.CelsiusHeatUnits:
                    return ToCelsiusHeatUnits();
                case EnergyUnit.LitersAtmosphere:
                    return ToLitersAtmosphere();
                case EnergyUnit.Calories:
                    return ToCalories();
                case EnergyUnit.HorsepowerHours:
                    return ToHorsepowerHours();
                case EnergyUnit.BarrelsOfOil:
                    return ToBarrelsOfOil();
                case EnergyUnit.TonsOfCoal:
                    return ToTonsOfCoal();
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
        /// <exception cref="ArgumentException">value is not a <see cref="Double"/> or <see cref="Energy"/>.</exception>
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;

            double num;

            if (value is double)
                num = (double)value;

            else if (value is Energy)
                num = (Energy)value;

            else
                throw new ArgumentException("Argument must be a Double or an Energy");

            return m_value < num ? -1 : (m_value > num ? 1 : 0);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Energy"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">An <see cref="Energy"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        public int CompareTo(Energy value)
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
        /// True if obj is an instance of <see cref="Double"/> or <see cref="Energy"/> and equals the value of this instance;
        /// otherwise, False.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is double)
                return Equals((double)obj);

            if (obj is Energy)
                return Equals((Energy)obj);

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Energy"/> value.
        /// </summary>
        /// <param name="obj">An <see cref="Energy"/> value to compare to this instance.</param>
        /// <returns>
        /// True if obj has the same value as this instance; otherwise, False.
        /// </returns>
        public bool Equals(Energy obj)
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
        /// Converts the string representation of a number to its <see cref="Energy"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <returns>
        /// An <see cref="Energy"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Energy.MinValue"/> or greater than <see cref="Energy.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        public static Energy Parse(string s)
        {
            return double.Parse(s);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style to its <see cref="Energy"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <returns>
        /// An <see cref="Energy"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Energy.MinValue"/> or greater than <see cref="Energy.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        public static Energy Parse(string s, NumberStyles style)
        {
            return double.Parse(s, style);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified culture-specific format to its <see cref="Energy"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// An <see cref="Energy"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Energy.MinValue"/> or greater than <see cref="Energy.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        public static Energy Parse(string s, IFormatProvider provider)
        {
            return double.Parse(s, provider);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its <see cref="Energy"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// An <see cref="Energy"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Energy.MinValue"/> or greater than <see cref="Energy.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        public static Energy Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            return double.Parse(s, style, provider);
        }

        /// <summary>
        /// Converts the string representation of a number to its <see cref="Energy"/> equivalent. A return value
        /// indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Energy"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null,
        /// is not of the correct format, or represents a number less than <see cref="Energy.MinValue"/> or greater than <see cref="Energy.MaxValue"/>.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string s, out Energy result)
        {
            double parseResult;
            bool parseResponse;

            parseResponse = double.TryParse(s, out parseResult);
            result = parseResult;

            return parseResponse;
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its
        /// <see cref="Energy"/> equivalent. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Energy"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null,
        /// is not in a format compliant with style, or represents a number less than <see cref="Energy.MinValue"/> or
        /// greater than <see cref="Energy.MaxValue"/>. This parameter is passed uninitialized.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> object that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Energy result)
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
        /// <param name="value1">An <see cref="Energy"/> object as the left hand operand.</param>
        /// <param name="value2">An <see cref="Energy"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the result of the operation.</returns>
        public static bool operator ==(Energy value1, Energy value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Compares the two values for inequality.
        /// </summary>
        /// <param name="value1">An <see cref="Energy"/> object as the left hand operand.</param>
        /// <param name="value2">An <see cref="Energy"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the result of the operation.</returns>
        public static bool operator !=(Energy value1, Energy value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Returns true if left value is less than right value.
        /// </summary>
        /// <param name="value1">An <see cref="Energy"/> object as the left hand operand.</param>
        /// <param name="value2">An <see cref="Energy"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the result of the operation.</returns>
        public static bool operator <(Energy value1, Energy value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        /// <summary>
        /// Returns true if left value is less or equal to than right value.
        /// </summary>
        /// <param name="value1">An <see cref="Energy"/> object as the left hand operand.</param>
        /// <param name="value2">An <see cref="Energy"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the result of the operation.</returns>
        public static bool operator <=(Energy value1, Energy value2)
        {
            return value1.CompareTo(value2) <= 0;
        }

        /// <summary>
        /// Returns true if left value is greater than right value.
        /// </summary>
        /// <param name="value1">An <see cref="Energy"/> object as the left hand operand.</param>
        /// <param name="value2">An <see cref="Energy"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the result of the operation.</returns>
        public static bool operator >(Energy value1, Energy value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        /// <summary>
        /// Returns true if left value is greater than or equal to right value.
        /// </summary>
        /// <param name="value1">An <see cref="Energy"/> object as the left hand operand.</param>
        /// <param name="value2">An <see cref="Energy"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the result of the operation.</returns>
        public static bool operator >=(Energy value1, Energy value2)
        {
            return value1.CompareTo(value2) >= 0;
        }

        #endregion

        #region [ Type Conversion Operators ]

        /// <summary>
        /// Implicitly converts value, represented in joules, to an <see cref="Energy"/>.
        /// </summary>
        /// <param name="value">A <see cref="Double"/> value.</param>
        /// <returns>An <see cref="Energy"/> object.</returns>
        public static implicit operator Energy(double value)
        {
            return new Energy(value);
        }

        /// <summary>
        /// Implicitly converts <see cref="Energy"/>, represented in joules, to a <see cref="Double"/>.
        /// </summary>
        /// <param name="value">An <see cref="Energy"/> object.</param>
        /// <returns>A <see cref="Double"/> value.</returns>
        public static implicit operator double(Energy value)
        {
            return value.m_value;
        }

        #endregion

        #region [ Arithmetic Operators ]

        /// <summary>
        /// Returns computed remainder after dividing first value by the second.
        /// </summary>
        /// <param name="value1">An <see cref="Energy"/> object as the left hand operand.</param>
        /// <param name="value2">An <see cref="Energy"/> object as the right hand operand.</param>
        /// <returns>An <see cref="Energy"/> object as the result of the operation.</returns>
        public static Energy operator %(Energy value1, Energy value2)
        {
            return value1.m_value % value2.m_value;
        }

        /// <summary>
        /// Returns computed sum of values.
        /// </summary>
        /// <param name="value1">An <see cref="Energy"/> object as the left hand operand.</param>
        /// <param name="value2">An <see cref="Energy"/> object as the right hand operand.</param>
        /// <returns>An <see cref="Energy"/> object as the result of the operation.</returns>
        public static Energy operator +(Energy value1, Energy value2)
        {
            return value1.m_value + value2.m_value;
        }

        /// <summary>
        /// Returns computed difference of values.
        /// </summary>
        /// <param name="value1">An <see cref="Energy"/> object as the left hand operand.</param>
        /// <param name="value2">An <see cref="Energy"/> object as the right hand operand.</param>
        /// <returns>An <see cref="Energy"/> object as the result of the operation.</returns>
        public static Energy operator -(Energy value1, Energy value2)
        {
            return value1.m_value - value2.m_value;
        }

        /// <summary>
        /// Returns computed product of values.
        /// </summary>
        /// <param name="value1">An <see cref="Energy"/> object as the left hand operand.</param>
        /// <param name="value2">An <see cref="Energy"/> object as the right hand operand.</param>
        /// <returns>An <see cref="Energy"/> object as the result of the operation.</returns>
        public static Energy operator *(Energy value1, Energy value2)
        {
            return value1.m_value * value2.m_value;
        }

        /// <summary>
        /// Returns computed division of values.
        /// </summary>
        /// <param name="value1">An <see cref="Energy"/> object as the left hand operand.</param>
        /// <param name="value2">An <see cref="Energy"/> object as the right hand operand.</param>
        /// <returns>An <see cref="Energy"/> object as the result of the operation.</returns>
        public static Energy operator /(Energy value1, Energy value2)
        {
            return value1.m_value / value2.m_value;
        }

        // C# doesn't expose an exponent operator but some other .NET languages do,
        // so we expose the operator via its native special IL function name

        /// <summary>
        /// Returns result of first value raised to power of second value.
        /// </summary>
        /// <param name="value1">An <see cref="Energy"/> object as the left hand operand.</param>
        /// <param name="value2">An <see cref="Energy"/> object as the right hand operand.</param>
        /// <returns>A <see cref="Double"/> value as the result of the operation.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced), SpecialName]
        public static double op_Exponent(Energy value1, Energy value2)
        {
            return Math.Pow(value1.m_value, value2.m_value);
        }

        #endregion

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>Represents the largest possible value of an <see cref="Energy"/>. This field is constant.</summary>
        public static readonly Energy MaxValue = double.MaxValue;

        /// <summary>Represents the smallest possible value of an <see cref="Energy"/>. This field is constant.</summary>
        public static readonly Energy MinValue = double.MinValue;

        // Static Methods

        /// <summary>
        /// Creates a new <see cref="Energy"/> value from the specified <paramref name="value"/> in watt-hours.
        /// </summary>
        /// <param name="value">New <see cref="Energy"/> value in watt-hours.</param>
        /// <returns>New <see cref="Energy"/> object from the specified <paramref name="value"/> in watt-hours.</returns>
        public static Energy FromWattHours(double value)
        {
            return new Energy(value * WattHoursFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Energy"/> value from the specified <paramref name="value"/> in BTU (International Table).
        /// </summary>
        /// <param name="value">New <see cref="Energy"/> value in BTU.</param>
        /// <returns>New <see cref="Energy"/> object from the specified <paramref name="value"/> in BTU.</returns>
        public static Energy FromBTU(double value)
        {
            return new Energy(value * BTUFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Energy"/> value from the specified <paramref name="value"/> in Celsius heat units (International Table).
        /// </summary>
        /// <param name="value">New <see cref="Energy"/> value in Celsius heat units.</param>
        /// <returns>New <see cref="Energy"/> object from the specified <paramref name="value"/> in Celsius heat units.</returns>
        public static Energy FromCelsiusHeatUnits(double value)
        {
            return new Energy(value * CelsiusHeatUnitsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Energy"/> value from the specified <paramref name="value"/> in liters-atmosphere.
        /// </summary>
        /// <param name="value">New <see cref="Energy"/> value in liters-atmosphere.</param>
        /// <returns>New <see cref="Energy"/> object from the specified <paramref name="value"/> in liters-atmosphere.</returns>
        public static Energy FromLitersAtmosphere(double value)
        {
            return new Energy(value * LitersAtmosphereFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Energy"/> value from the specified <paramref name="value"/> in calories (International Table).
        /// </summary>
        /// <param name="value">New <see cref="Energy"/> value in calories.</param>
        /// <returns>New <see cref="Energy"/> object from the specified <paramref name="value"/> in calories.</returns>
        public static Energy FromCalories(double value)
        {
            return new Energy(value * CaloriesFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Energy"/> value from the specified <paramref name="value"/> in horsepower-hours.
        /// </summary>
        /// <param name="value">New <see cref="Energy"/> value in horsepower-hours.</param>
        /// <returns>New <see cref="Energy"/> object from the specified <paramref name="value"/> in horsepower-hours.</returns>
        public static Energy FromHorsepowerHours(double value)
        {
            return new Energy(value * HorsepowerHoursFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Energy"/> value from the specified <paramref name="value"/> in equivalent barrels of oil.
        /// </summary>
        /// <param name="value">New <see cref="Energy"/> value in equivalent barrels of oil.</param>
        /// <returns>New <see cref="Energy"/> object from the specified <paramref name="value"/> in equivalent barrels of oil.</returns>
        public static Energy FromBarrelsOfOil(double value)
        {
            return new Energy(value * BarrelsOfOilFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Energy"/> value from the specified <paramref name="value"/> in equivalent tons of coal.
        /// </summary>
        /// <param name="value">New <see cref="Energy"/> value in equivalent tons of coal.</param>
        /// <returns>New <see cref="Energy"/> object from the specified <paramref name="value"/> in equivalent tons of coal.</returns>
        public static Energy FromTonOfCoal(double value)
        {
            return new Energy(value * TonsOfCoalFactor);
        }

        /// <summary>
        /// Converts the <paramref name="value"/> in the specified <paramref name="sourceUnit"/> to a new <see cref="Energy"/> in joules.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <param name="sourceUnit">Source value units.</param>
        /// <returns>New <see cref="Energy"/> from the specified <paramref name="value"/> in <paramref name="sourceUnit"/>.</returns>
        public static Energy ConvertFrom(double value, EnergyUnit sourceUnit)
        {
            switch (sourceUnit)
            {
                case EnergyUnit.Joules:
                    return value;
                case EnergyUnit.WattHours:
                    return FromWattHours(value);
                case EnergyUnit.BTU:
                    return FromBTU(value);
                case EnergyUnit.CelsiusHeatUnits:
                    return FromCelsiusHeatUnits(value);
                case EnergyUnit.LitersAtmosphere:
                    return FromLitersAtmosphere(value);
                case EnergyUnit.Calories:
                    return FromCalories(value);
                case EnergyUnit.HorsepowerHours:
                    return FromHorsepowerHours(value);
                case EnergyUnit.BarrelsOfOil:
                    return FromBarrelsOfOil(value);
                case EnergyUnit.TonsOfCoal:
                    return FromTonOfCoal(value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(sourceUnit), sourceUnit, null);
            }
        }

        #endregion
    }
}