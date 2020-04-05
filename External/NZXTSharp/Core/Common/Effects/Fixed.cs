/*
Fixed.cs
Copyright (C) 2019  Ari Madian

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

﻿using System;
using System.Collections.Generic;
using System.Text;

using NZXTSharp;
using NZXTSharp.Exceptions;

namespace NZXTSharp
{

    /// <summary>
    /// Represents an RGB fixed effect.
    /// </summary>
    public class Fixed : IEffect {
        private int _EffectByte = 0x00;
        private string _EffectName = "Fixed";

        /// <inheritdoc/>
        public readonly List<NZXTDeviceType> CompatibleWith = new List<NZXTDeviceType>() {
            NZXTDeviceType.HuePlus,
            NZXTDeviceType.KrakenX
        };

        private Color _Color;
        private IChannel _Channel;

        private byte[] CustomBytes;
        private bool IsCustom;

        /// <inheritdoc/>
        public int EffectByte { get; }

        /// <inheritdoc/>
        public IChannel Channel { get; set; }

        /// <inheritdoc/>
        public string EffectName { get; }

        /// <summary>
        /// Constructs an RGB Fixed effect.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to display.</param>
        public Fixed(Color color) {
            this._Color = color;
        }

        /// <summary>
        /// Constructs an RGB Fixed effect.
        /// </summary>
        /// <param name="Channel">The <see cref="Channel"/> the effect will be applied to.</param>
        /// <param name="color">The <see cref="Color"/> to display.</param>
        public Fixed(IChannel Channel, Color color)
        {
            this._Channel = Channel;
            this._Color = color;
        }

        /// <summary>
        /// Constructs an RGB Fixed effect from custom LED colors.
        /// </summary>
        /// <param name="Colors">A byte[] of RGB color values in G, R, B format.
        /// Length must be less than 120 and greater than 0.
        /// RGB values must be 0-255 (inclusive).</param>
        public Fixed(byte[] Colors)
        {
            if (Colors.Length > 120 || Colors.Length < 0)
                throw new InvalidParamException("Invalid RGB color array size. Must have at least one element, and fewer than 120 elements.");

            foreach (byte color in Colors)
            {
                int value = Convert.ToInt32(color);
                if (value > 255 || value < 0)
                {
                    throw new InvalidParamException("Invalid RGB color found in array. All RGB color values must be 0-255 (inclusive).");
                }
            }
            this.CustomBytes = Colors.PadColorArr();
            this.IsCustom = true;
        }

        /// <inheritdoc/>
        public bool IsCompatibleWith(NZXTDeviceType Type)
        {
            return CompatibleWith.Contains(Type) ? true : false;
        }

        /// <inheritdoc/>
        public List<byte[]> BuildBytes(NZXTDeviceType Type, IChannel Channel_) {
            switch (Type)
            {
                case NZXTDeviceType.HuePlus:
                    this._Channel = Channel_;
                    byte[] SettingsBytes = new byte[] { 0x4b, (byte)Channel_.ChannelByte, 0x00, 0x02, 0x03 };
                    byte[] final;
                    if (IsCustom)
                    {
                        final = SettingsBytes.ConcatenateByteArr(Channel_.BuildColorBytes(CustomBytes));
                    }
                    else
                    {
                        final = SettingsBytes.ConcatenateByteArr(Channel_.BuildColorBytes(_Color));
                    }
                    return new List<byte[]>() { final };
                case NZXTDeviceType.KrakenX:
                    this._Channel = Channel_;
                    byte[] KrakenSettingsBytes = new byte[] { 0x02, 0x4c, (byte)Channel_.ChannelByte, 0x00, 0x02 };
                    byte[] KrakenFinal;
                    if (IsCustom)
                    {
                        KrakenFinal = KrakenSettingsBytes.ConcatenateByteArr(Channel_.BuildColorBytes(CustomBytes));
                    } else
                    {
                        KrakenFinal = KrakenSettingsBytes.ConcatenateByteArr(Channel_.BuildColorBytes(_Color));
                    }
                    byte[] TrueFinal = KrakenFinal.ConcatenateByteArr(new byte[] { 0x00 });
                    return new List<byte[]> { TrueFinal };
                default:
                    return null;
            }
        }
    }
}
