﻿//******************************************************************************************************
//  Angle.cs - Gbtc
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using GSF.NumericalAnalysis;

namespace GSF.Units
{
    #region [ Enumerations ]

    /// <summary>
    /// Represents the units available for an <see cref="Angle"/> value.
    /// </summary>
    public enum AngleUnit
    {
        /// <summary>
        /// Radian angle units.
        /// </summary>
        Radians,
        /// <summary>
        /// Degree angle units.
        /// </summary>
        Degrees,
        /// <summary>
        /// Grad angle units, a.k.a., grade, gradian and gon.
        /// </summary>
        Grads,
        /// <summary>
        /// ArcMinute angle units, a.k.a., minute of arc or MOA.
        /// </summary>
        ArcMinutes,
        /// <summary>
        /// ArcSecond angle units, a.k.a., second of arc.
        /// </summary>
        ArcSeconds,
        /// <summary>
        /// AngularMil angle units, a.k.a., mil.
        /// </summary>
        AngularMil
    }

    #endregion

    /// <summary>
    /// Represents an angle, in radians, as a double-precision floating-point number.
    /// </summary>
    /// <remarks>
    /// This class behaves just like a <see cref="double"/> representing an angle in radians; it is implicitly
    /// castable to and from a <see cref="double"/> and therefore can be generally used "as" a double, but it
    /// has the advantage of handling conversions to and from other angle representations, specifically
    /// degrees, grads (a.k.a., grade, gradian and gon), arcminutes (a.k.a., minute of arc or MOA),
    /// arcseconds (a.k.a., second of arc) and angular mil (a.k.a., mil).
    /// <example>
    /// This example converts degrees to grads:
    /// <code>
    /// public double GetGrads(double degrees)
    /// {
    ///     return Angle.FromDegrees(degrees).ToGrads();
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    [Serializable]
    public struct Angle : IComparable, IFormattable, IConvertible, IComparable<Angle>, IComparable<double>, IEquatable<Angle>, IEquatable<double>
    {
        #region [ Members ]

        // Constants
        private const double DegreesFactor = Math.PI / 180.0D;

        private const double GradsFactor = Math.PI / 200.0D;

        private const double ArcMinutesFactor = Math.PI / 180.0D / 60.0D;

        private const double ArcSecondsFactor = Math.PI / 180.0D / 3600.0D;

        private const double AngularMilFactor = 2.0D * Math.PI / 6400.0D;

        private const double TwoPI = 2.0D * Math.PI;

        // Fields
        private readonly double m_value; // Angle value stored in radians

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="Angle"/>.
        /// </summary>
        /// <param name="value">New angle value in radians.</param>
        public Angle(double value)
        {
            m_value = value;
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Gets the <see cref="Angle"/> value in degrees.
        /// </summary>
        /// <returns>Value of <see cref="Angle"/> in degrees.</returns>
        public double ToDegrees()
        {
            return m_value / DegreesFactor;
        }

        /// <summary>
        /// Gets the <see cref="Angle"/> value in grads.
        /// </summary>
        /// <returns>Value of <see cref="Angle"/> in grads.</returns>
        public double ToGrads()
        {
            return m_value / GradsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Angle"/> value in arcminutes.
        /// </summary>
        /// <returns>Value of <see cref="Angle"/> in arcminutes.</returns>
        public double ToArcMinutes()
        {
            return m_value / ArcMinutesFactor;
        }

        /// <summary>
        /// Gets the <see cref="Angle"/> value in arcseconds.
        /// </summary>
        /// <returns>Value of <see cref="Angle"/> in arcseconds.</returns>
        public double ToArcSeconds()
        {
            return m_value / ArcSecondsFactor;
        }

        /// <summary>
        /// Gets the <see cref="Angle"/> value in angular mil.
        /// </summary>
        /// <returns>Value of <see cref="Angle"/> in angular mil.</returns>
        public double ToAngularMil()
        {
            return m_value / AngularMilFactor;
        }

        /// <summary>
        /// Converts the <see cref="Angle"/> to the specified <paramref name="targetUnit"/>.
        /// </summary>
        /// <param name="targetUnit">Target units.</param>
        /// <returns><see cref="Angle"/> converted to <paramref name="targetUnit"/>.</returns>
        public double ConvertTo(AngleUnit targetUnit)
        {
            switch (targetUnit)
            {
                case AngleUnit.Radians:
                    return m_value;
                case AngleUnit.Degrees:
                    return ToDegrees();
                case AngleUnit.Grads:
                    return ToGrads();
                case AngleUnit.ArcMinutes:
                    return ToArcMinutes();
                case AngleUnit.ArcSeconds:
                    return ToArcSeconds();
                case AngleUnit.AngularMil:
                    return ToAngularMil();
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetUnit), targetUnit, null);
            }
        }

        /// <summary>
        /// Gets the equivalent angle moved within the range of <paramref name="minValue"/>
        /// and <paramref name="minValue"/> + 2.0 * <see cref="Math.PI"/>.
        /// </summary>
        /// <param name="minValue">The minimum value of the range.</param>
        /// <param name="inclusive">Indicates whether the range is inclusive of the minimum value.</param>
        /// <returns>The equivalent angle within the specified range.</returns>
        public Angle ToRange(Angle minValue, bool inclusive = true)
        {
            return inclusive
                ? Euclidean.Wrap(this, minValue, TwoPI)
                : Euclidean.Wrap(this, minValue + TwoPI, -TwoPI);
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
        /// <exception cref="ArgumentException">value is not a <see cref="Double"/> or <see cref="Angle"/>.</exception>
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;

            if (!(value is double) && !(value is Angle))
                throw new ArgumentException("Argument must be a Double or an Angle");

            double num = (double)value;
            return m_value < num ? -1 : (m_value > num ? 1 : 0);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Angle"/> and returns an indication of their
        /// relative values.
        /// </summary>
        /// <param name="value">An <see cref="Angle"/> to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value. Returns less than zero
        /// if this instance is less than value, zero if this instance is equal to value, or greater than zero
        /// if this instance is greater than value.
        /// </returns>
        public int CompareTo(Angle value)
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
        /// True if obj is an instance of <see cref="Double"/> or <see cref="Angle"/> and equals the value of this instance;
        /// otherwise, False.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is double)
                return Equals((double)obj);

            if (obj is Angle)
                return Equals((Angle)obj);

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Angle"/> value.
        /// </summary>
        /// <param name="obj">An <see cref="Angle"/> value to compare to this instance.</param>
        /// <returns>
        /// True if obj has the same value as this instance; otherwise, False.
        /// </returns>
        public bool Equals(Angle obj)
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
        /// Converts the string representation of a number to its <see cref="Angle"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <returns>
        /// An <see cref="Angle"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Angle.MinValue"/> or greater than <see cref="Angle.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        public static Angle Parse(string s)
        {
            return double.Parse(s);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style to its <see cref="Angle"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <returns>
        /// An <see cref="Angle"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Angle.MinValue"/> or greater than <see cref="Angle.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        public static Angle Parse(string s, NumberStyles style)
        {
            return double.Parse(s, style);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified culture-specific format to its <see cref="Angle"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// An <see cref="Angle"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Angle.MinValue"/> or greater than <see cref="Angle.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in the correct format.</exception>
        public static Angle Parse(string s, IFormatProvider provider)
        {
            return double.Parse(s, provider);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its <see cref="Angle"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>
        /// An <see cref="Angle"/> equivalent to the number contained in s.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        /// <exception cref="ArgumentNullException">s is null.</exception>
        /// <exception cref="OverflowException">
        /// s represents a number less than <see cref="Angle.MinValue"/> or greater than <see cref="Angle.MaxValue"/>.
        /// </exception>
        /// <exception cref="FormatException">s is not in a format compliant with style.</exception>
        public static Angle Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            return double.Parse(s, style, provider);
        }

        /// <summary>
        /// Converts the string representation of a number to its <see cref="Angle"/> equivalent. A return value
        /// indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Angle"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null,
        /// is not of the correct format, or represents a number less than <see cref="Angle.MinValue"/> or greater than <see cref="Angle.MaxValue"/>.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string s, out Angle result)
        {
            double parseResult;
            bool parseResponse;

            parseResponse = double.TryParse(s, out parseResult);
            result = parseResult;

            return parseResponse;
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its
        /// <see cref="Angle"/> equivalent. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing a number to convert.</param>
        /// <param name="style">
        /// A bitwise combination of System.Globalization.NumberStyles values that indicates the permitted format of s.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Angle"/> value equivalent to the number contained in s,
        /// if the conversion succeeded, or zero if the conversion failed. The conversion fails if the s parameter is null,
        /// is not in a format compliant with style, or represents a number less than <see cref="Angle.MinValue"/> or
        /// greater than <see cref="Angle.MaxValue"/>. This parameter is passed uninitialized.
        /// </param>
        /// <param name="provider">
        /// A <see cref="System.IFormatProvider"/> object that supplies culture-specific formatting information about s.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentException">
        /// style is not a System.Globalization.NumberStyles value. -or- style is not a combination of
        /// System.Globalization.NumberStyles.AllowHexSpecifier and System.Globalization.NumberStyles.HexNumber values.
        /// </exception>
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Angle result)
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
        /// <param name="value1">An <see cref="Angle"/> as the left hand operand.</param>
        /// <param name="value2">An <see cref="Angle"/> as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the operation result.</returns>
        public static bool operator ==(Angle value1, Angle value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Compares the two values for inequality.
        /// </summary>
        /// <param name="value1">An <see cref="Angle"/> as the left hand operand.</param>
        /// <param name="value2">An <see cref="Angle"/> as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the operation result.</returns>
        public static bool operator !=(Angle value1, Angle value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Returns true if left value is less than right value.
        /// </summary>
        /// <param name="value1">An <see cref="Angle"/> as the left hand operand.</param>
        /// <param name="value2">An <see cref="Angle"/> as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the operation result.</returns>
        public static bool operator <(Angle value1, Angle value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        /// <summary>
        /// Returns true if left value is less or equal to than right value.
        /// </summary>
        /// <param name="value1">An <see cref="Angle"/> as the left hand operand.</param>
        /// <param name="value2">An <see cref="Angle"/> as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the operation result.</returns>
        public static bool operator <=(Angle value1, Angle value2)
        {
            return value1.CompareTo(value2) <= 0;
        }

        /// <summary>
        /// Returns true if left value is greater than right value.
        /// </summary>
        /// <param name="value1">An <see cref="Angle"/> as the left hand operand.</param>
        /// <param name="value2">An <see cref="Angle"/> as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the operation result.</returns>
        public static bool operator >(Angle value1, Angle value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        /// <summary>
        /// Returns true if left value is greater than or equal to right value.
        /// </summary>
        /// <param name="value1">An <see cref="Angle"/> as the left hand operand.</param>
        /// <param name="value2">An <see cref="Angle"/> as the right hand operand.</param>
        /// <returns>A <see cref="Boolean"/> as the operation result.</returns>
        public static bool operator >=(Angle value1, Angle value2)
        {
            return value1.CompareTo(value2) >= 0;
        }

        #endregion

        #region [ Type Conversion Operators ]

        /// <summary>
        /// Implicitly converts value, represented in radians, to an <see cref="Angle"/>.
        /// </summary>
        /// <param name="value">A <see cref="Double"/> value.</param>
        /// <returns>An <see cref="Angle"/> object.</returns>
        public static implicit operator Angle(double value)
        {
            return new Angle(value);
        }

        /// <summary>
        /// Implicitly converts <see cref="Angle"/>, represented in radians, to a <see cref="Double"/>.
        /// </summary>
        /// <param name="value">An <see cref="Angle"/> object.</param>
        /// <returns>A <see cref="Double"/> value.</returns>
        public static implicit operator double(Angle value)
        {
            return value.m_value;
        }

        #endregion

        #region [ Arithmetic Operators ]

        /// <summary>
        /// Returns computed remainder after dividing first value by the second.
        /// </summary>
        /// <param name="value1">An <see cref="Angle"/> as the left hand operand.</param>
        /// <param name="value2">An <see cref="Angle"/> as the right hand operand.</param>
        /// <returns>An <see cref="Angle"/> as the result.</returns>
        public static Angle operator %(Angle value1, Angle value2)
        {
            return value1.m_value % value2.m_value;
        }

        /// <summary>
        /// Returns computed sum of values.
        /// </summary>
        /// <param name="value1">An <see cref="Angle"/> as the left hand operand.</param>
        /// <param name="value2">An <see cref="Angle"/> as the right hand operand.</param>
        /// <returns>An <see cref="Angle"/> as the result.</returns>
        public static Angle operator +(Angle value1, Angle value2)
        {
            return value1.m_value + value2.m_value;
        }

        /// <summary>
        /// Returns computed difference of values.
        /// </summary>
        /// <param name="value1">An <see cref="Angle"/> as the left hand operand.</param>
        /// <param name="value2">An <see cref="Angle"/> as the right hand operand.</param>
        /// <returns>An <see cref="Angle"/> as the result.</returns>
        public static Angle operator -(Angle value1, Angle value2)
        {
            return value1.m_value - value2.m_value;
        }

        /// <summary>
        /// Returns computed product of values.
        /// </summary>
        /// <param name="value1">An <see cref="Angle"/> as the left hand operand.</param>
        /// <param name="value2">An <see cref="Angle"/> as the right hand operand.</param>
        /// <returns>An <see cref="Angle"/> as the result.</returns>
        public static Angle operator *(Angle value1, Angle value2)
        {
            return value1.m_value * value2.m_value;
        }

        /// <summary>
        /// Returns computed division of values.
        /// </summary>
        /// <param name="value1">An <see cref="Angle"/> as the left hand operand.</param>
        /// <param name="value2">An <see cref="Angle"/> as the right hand operand.</param>
        /// <returns>An <see cref="Angle"/> as the result.</returns>
        public static Angle operator /(Angle value1, Angle value2)
        {
            return value1.m_value / value2.m_value;
        }

        // C# doesn't expose an exponent operator but some other .NET languages do,
        // so we expose the operator via its native special IL function name

        /// <summary>
        /// Returns result of first value raised to power of second value.
        /// </summary>
        /// <param name="value1">An <see cref="Angle"/> as the left hand operand.</param>
        /// <param name="value2">An <see cref="Angle"/> as the right hand operand.</param>
        /// <returns>A <see cref="Double"/> as the result.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced), SpecialName]
        public static double op_Exponent(Angle value1, Angle value2)
        {
            return Math.Pow(value1.m_value, value2.m_value);
        }

        #endregion

        #endregion

        #region [ Static ]

        // Static Fields

        /// <summary>Represents the largest possible value of an <see cref="Angle"/>. This field is constant.</summary>
        public static readonly Angle MaxValue = double.MaxValue;

        /// <summary>Represents the smallest possible value of an <see cref="Angle"/>. This field is constant.</summary>
        public static readonly Angle MinValue = double.MinValue;

        // Static Methods

        /// <summary>
        /// Creates a new <see cref="Angle"/> value from the specified <paramref name="value"/> in degrees.
        /// </summary>
        /// <param name="value">New <see cref="Angle"/> value in degrees.</param>
        /// <returns>New <see cref="Angle"/> object from the specified <paramref name="value"/> in degrees.</returns>
        public static Angle FromDegrees(double value)
        {
            return new Angle(value * DegreesFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Angle"/> value from the specified <paramref name="value"/> in grads.
        /// </summary>
        /// <param name="value">New <see cref="Angle"/> value in grads.</param>
        /// <returns>New <see cref="Angle"/> object from the specified <paramref name="value"/> in grads.</returns>
        public static Angle FromGrads(double value)
        {
            return new Angle(value * GradsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Angle"/> value from the specified <paramref name="value"/> in arcminutes.
        /// </summary>
        /// <param name="value">New <see cref="Angle"/> value in arcminutes.</param>
        /// <returns>New <see cref="Angle"/> object from the specified <paramref name="value"/> in arcminutes.</returns>
        public static Angle FromArcMinutes(double value)
        {
            return new Angle(value * ArcMinutesFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Angle"/> value from the specified <paramref name="value"/> in arcseconds.
        /// </summary>
        /// <param name="value">New <see cref="Angle"/> value in arcseconds.</param>
        /// <returns>New <see cref="Angle"/> object from the specified <paramref name="value"/> in arcseconds.</returns>
        public static Angle FromArcSeconds(double value)
        {
            return new Angle(value * ArcSecondsFactor);
        }

        /// <summary>
        /// Creates a new <see cref="Angle"/> value from the specified <paramref name="value"/> in angular mil.
        /// </summary>
        /// <param name="value">New <see cref="Angle"/> value in angular mil.</param>
        /// <returns>New <see cref="Angle"/> object from the specified <paramref name="value"/> in angular mil.</returns>
        public static Angle FromAngularMil(double value)
        {
            return new Angle(value * AngularMilFactor);
        }

        /// <summary>
        /// Converts the <paramref name="value"/> in the specified <paramref name="sourceUnit"/> to a new <see cref="Angle"/> in radians.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <param name="sourceUnit">Source value units.</param>
        /// <returns>New <see cref="Angle"/> from the specified <paramref name="value"/> in <paramref name="sourceUnit"/>.</returns>
        public static Angle ConvertFrom(double value, AngleUnit sourceUnit)
        {
            switch (sourceUnit)
            {
                case AngleUnit.Radians:
                    return value;
                case AngleUnit.Degrees:
                    return FromDegrees(value);
                case AngleUnit.Grads:
                    return FromGrads(value);
                case AngleUnit.ArcMinutes:
                    return FromArcMinutes(value);
                case AngleUnit.ArcSeconds:
                    return FromArcSeconds(value);
                case AngleUnit.AngularMil:
                    return FromAngularMil(value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(sourceUnit), sourceUnit, null);
            }
        }

        /// <summary>
        /// Unwraps a set of <see cref="Angle"/> values so a comparable mathematical operation can be applied.
        /// </summary>
        /// <param name="source">Sequence of <see cref="Angle"/> values to unwrap.</param>
        /// <returns>Unwrapped set of <see cref="Angle"/> values.</returns>
        /// <remarks>
        /// For Angles that wrap, e.g., between -180 and +180, this algorithm unwraps the values to make the values mathematically comparable.
        /// </remarks>
        public static IEnumerable<Angle> Unwrap(IEnumerable<Angle> source)
        {
            double[] sourceAngles = source.Select(angle => angle.m_value).ToArray();
            return Unwrap(sourceAngles).Select(angle => new Angle(angle));
        }

        /// <summary>
        /// Calculates an average of the specified sequence of <see cref="Angle"/> values.
        /// </summary>
        /// <param name="source">Sequence of <see cref="Angle"/> values over which to calculate average.</param>
        /// <returns>Average of the specified sequence of <see cref="Angle"/> values.</returns>
        /// <remarks>
        /// For Angles that wrap, e.g., between -180 and +180, this algorithm takes the wrapping into account when calculating the average.
        /// </remarks>
        public static Angle Average(IEnumerable<Angle> source)
        {
            double[] sourceAngles = source.Select(angle => angle.m_value).ToArray();
            return new Angle(Unwrap(sourceAngles).Average()).ToRange(-Math.PI, false);
        }

        private static double[] Unwrap(double[] sourceAngles)
        {
            double[] unwrappedAngles = new double[sourceAngles.Length];

            if (sourceAngles.Length > 0)
            {
                double offset = 0.0D, dis0, dis1, dis2;

                unwrappedAngles[0] = sourceAngles[0];

                // Unwrap all source angles
                for (int i = 1; i < sourceAngles.Length; i++)
                {
                    dis0 = Math.Abs(sourceAngles[i] + offset - unwrappedAngles[i - 1]);
                    dis1 = Math.Abs(sourceAngles[i] + offset - unwrappedAngles[i - 1] + TwoPI);
                    dis2 = Math.Abs(sourceAngles[i] + offset - unwrappedAngles[i - 1] - TwoPI);

                    if (dis1 < dis0 && dis1 < dis2)
                        offset = offset + TwoPI;
                    else if (dis2 < dis0 && dis2 < dis1)
                        offset = offset - TwoPI;

                    unwrappedAngles[i] = sourceAngles[i] + offset;
                }
            }

            return unwrappedAngles;
        }

        #endregion
    }
}