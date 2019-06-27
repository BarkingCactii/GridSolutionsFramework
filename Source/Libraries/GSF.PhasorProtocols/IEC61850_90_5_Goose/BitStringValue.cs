//******************************************************************************************************
//  DigitalValue.cs - Gbtc
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
using System.Linq;

namespace GSF.PhasorProtocols.IEC61850_90_5_Goose
{
    /// <summary>
    /// Represents the IEC 61850-90-5 implementation of a <see cref="IDigitalValue"/>.
    /// </summary>
    [Serializable]
    public class BitStringValue : DigitalValueBase
    {
        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="DigitalValue"/>.
        /// </summary>
        /// <param name="parent">The <see cref="IDataCell"/> parent of this <see cref="DigitalValue"/>.</param>
        /// <param name="digitalDefinition">The <see cref="IDigitalDefinition"/> associated with this <see cref="DigitalValue"/>.</param>
        public BitStringValue(IDataCell parent, IDigitalDefinition digitalDefinition)
            : base(parent, digitalDefinition)
        {
        }

        /// <summary>
        /// Creates a new <see cref="DigitalValue"/> from specified parameters.
        /// </summary>
        /// <param name="parent">The <see cref="DataCell"/> parent of this <see cref="DigitalValue"/>.</param>
        /// <param name="digitalDefinition">The <see cref="DigitalDefinition"/> associated with this <see cref="DigitalValue"/>.</param>
        /// <param name="value">The unsigned 16-bit integer value (composed of digital bits) that represents this <see cref="DigitalValue"/>.</param>
        public BitStringValue(DataCell parent, DigitalDefinition digitalDefinition, ushort value)
            : base(parent, digitalDefinition, value)
        {
        }

        /// <summary>
        /// Creates a new <see cref="DigitalValue"/> from serialization parameters.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> with populated with data.</param>
        /// <param name="context">The source <see cref="StreamingContext"/> for this deserialization.</param>
        protected BitStringValue(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the <see cref="DataCell"/> parent of this <see cref="DigitalValue"/>.
        /// </summary>
        public new virtual DataCell Parent
        {
            get
            {
                return base.Parent as DataCell;
            }
            set
            {
                base.Parent = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DigitalDefinition"/> associated with this <see cref="DigitalValue"/>.
        /// </summary>
        public new virtual DigitalDefinition Definition
        {
            get
            {
                return base.Definition as DigitalDefinition;
            }
            set
            {
                base.Definition = value;
            }
        }

        #endregion

        #region [ Static ]

        // Static Methods

        // Delegate handler to create a new IEC 61850-90-5 digital value
        internal static IDigitalValue CreateNewValue(IDataCell parent, IDigitalDefinition definition, byte[] buffer, int startIndex, out int parsedLength)
        {
            IDigitalValue digital = new DigitalValue(parent, definition);

            parsedLength = digital.ParseBinaryImage(buffer, startIndex, 0);

            return digital;
        }

        // Delegate handler to create a new IEC 61850-90-5 digital value
        internal static IDigitalValue CreateNewVariableValue(IDataCell parent, IDigitalDefinition definition, byte[] buffer, int startIndex, int length)
        {
            IDigitalValue digital = new BitStringValue(parent, definition);

            int parsedLength = digital.ParseBinaryImage(buffer, startIndex, length);

            return digital;
        }

        #endregion

        /// <summary>
        /// Parses the binary body image.
        /// </summary>
        /// <param name="buffer">Binary image to parse.</param>
        /// <param name="startIndex">Start index into <paramref name="buffer"/> to begin parsing.</param>
        /// <param name="length">Length of valid data within <paramref name="buffer"/>.</param>
        /// <returns>The length of the data that was parsed.</returns>
        /// <remarks>
        /// The base implementation assumes fixed integer values are represented as 16-bit signed
        /// integers and floating point values are represented as 32-bit single-precision floating-point
        /// values (i.e., short and float data types respectively).
        /// </remarks>
        protected override int ParseBodyImage(byte[] buffer, int startIndex, int length)
        {
            // Length is validated at a frame level well in advance so that low level parsing routines do not have
            // to re-validate that enough length is available to parse needed information as an optimization...

            /*
             * According to bitstring spec, the first byte is the padding with the remainder the value
             * So in an example where the buffer contains 
             * 0x84 - Type BitString
             * 0x02 - Length - 2 bytes
             * 0x06 - Passing - 6 bits
             * 0x80 - the data. 
             * 
             * This means that this bitstring value is actually 2 bits long (it has 6 bits of padding).
             *
             * To get the actual value you need to right shift the bitstring value by the padding bits, e.g:
             * Value = ([bitstring value] >> [padding bits])
             * Value = (80 >> 6) = 2
             */
            if (DataFormat == DataFormat.FixedInteger)
            {
                if (length > 1)
                {
                    ushort padding = buffer[startIndex];

                    byte[] bytes = new byte[length - 1];
                    Array.Copy(buffer, startIndex + 1, bytes, 0, length - 1);
                    if (BitConverter.IsLittleEndian)
                    {
                        bytes = bytes.Reverse().ToArray();
                    }

                    for (int i = 0; i < padding; i++)
                        ShiftRight(bytes);

                    switch ( bytes.Length )
                    {
                        case 1:
                            Value = (ushort)bytes[0];
                            break;
                        case 2:
                            Value = (ushort)BitConverter.ToInt16(bytes, 0);
                            break;
                        case 4:
                            Value = (ushort)BitConverter.ToInt32(bytes, 0);
                            break;
                        default:
                            Value = 0xff;
                            break;

                    }
                }
                else
                {
                    Value = buffer[startIndex];
                }
                return length;
            }
            else
            {
                // not supported, set to -1
                Value = 0;
                return length;
            }
        }

        /// <summary>
        /// Shifts the bits in an array of bytes to the right.
        /// </summary>
        /// <param name="bytes">The byte array to shift.</param>
        private static bool ShiftRight(byte[] bytes)
        {
            bool rightMostCarryFlag = false;
            int rightEnd = bytes.Length - 1;

            // Iterate through the elements of the array right to left.
            for (int index = rightEnd; index >= 0; index--)
            {
                // If the rightmost bit of the current byte is 1 then we have a carry.
                bool carryFlag = (bytes[index] & 0x01) > 0;

                if (index < rightEnd)
                {
                    if (carryFlag == true)
                    {
                        // Apply the carry to the leftmost bit of the current bytes neighbor to the right.
                        bytes[index + 1] = (byte)(bytes[index + 1] | 0x80);
                    }
                }
                else
                {
                    rightMostCarryFlag = carryFlag;
                }

                bytes[index] = (byte)(bytes[index] >> 1);
            }

            return rightMostCarryFlag;
        }
    }
}