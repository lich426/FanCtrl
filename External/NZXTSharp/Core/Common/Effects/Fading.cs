/*
Fading.cs
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
    /// Represents an RGB Fading effect.
    /// </summary>
    public class Fading : IEffect {
        private int _EffectByte = 0x01;
        private string _EffectName = "Fading";

        /// <inheritdoc/>
        public readonly List<NZXTDeviceType> CompatibleWith = new List<NZXTDeviceType>() {
            NZXTDeviceType.HuePlus,
            NZXTDeviceType.KrakenX
        };

        /// <summary>
        /// The array of colors used by the effect.
        /// </summary>
        private Color[] DefaultColors = new Color[] { new Color(255, 0, 0), new Color(0, 255, 0), new Color(0, 0, 255) };

        /// <summary>
        /// The colors that the effect will apply.
        /// </summary>
        public Color[] Colors;

        private CISS Param2;
        private IChannel _Channel;
        private int _Speed = 2;

        /// <inheritdoc/>
        public int EffectByte { get; }

        /// <inheritdoc/>
        public IChannel Channel { get; set; }

        /// <inheritdoc/>
        public string EffectName { get; }

        /// <summary>
        /// Constructs a <see cref="Fading"/> effect.
        /// </summary>
        /// <param name="Colors">The <see cref="Color"/>s to display.</param>
        /// /// <param name="speed">Speed values must be 0-4 (inclusive). 0 being slowest, 2 being normal, and 4 being fastest. Defaults to 2.</param>
        public Fading(Color[] Colors = null, int speed = 2) {
            this.Colors = Colors ?? DefaultColors;
            this._Speed = speed;
            ValidateParams();
        }

        private void ValidateParams() {
            if (this.Colors.Length > 15) {
                throw new TooManyColorsProvidedException();
            }
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
                    List<byte[]> outList = new List<byte[]>();
                    for (int colorIndex = 0; colorIndex < Colors.Length; colorIndex++)
                    {
                        byte[] SettingsBytes = new byte[] { 0x4b, (byte)Channel_.ChannelByte, 0x01, 0x03, new CISS(colorIndex, this._Speed) };
                        byte[] final = SettingsBytes.ConcatenateByteArr(Channel_.BuildColorBytes(Colors[colorIndex]));
                        outList.Add(final);
                    }
                    return outList;
                case NZXTDeviceType.KrakenX:
                    List<byte[]> KrakenXOutList = new List<byte[]>();
                    for (int colorIndex = 0; colorIndex < Colors.Length; colorIndex++)
                    {
                        byte[] SettingsBytes = new byte[] { 0x02, 0x4c, (byte)Channel_.ChannelByte, 0x01, new CISS(colorIndex, this._Speed) };
                        byte[] KrakenXfinal = SettingsBytes.ConcatenateByteArr(Channel_.BuildColorBytes(Colors[colorIndex]));
                        KrakenXOutList.Add(KrakenXfinal);
                    }
                    return KrakenXOutList;
                default:
                    return null;
            }
        }
    }
}
