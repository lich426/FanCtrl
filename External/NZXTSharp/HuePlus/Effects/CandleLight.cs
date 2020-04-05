/*
CandleLight.cs
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

namespace NZXTSharp.HuePlus
{

    /// <summary>
    /// Represents an RGB Candle Light effect.
    /// </summary>
    public class CandleLight : IEffect {
        private int _EffectByte = 0x09;
        private string _EffectName = "CandleLight";

        /// <inheritdoc/>
        public readonly List<NZXTDeviceType> CompatibleWith = new List<NZXTDeviceType>() { NZXTDeviceType.HuePlus };

        /// <inheritdoc/>
        public Color Color;
        private HuePlusChannel _Channel;

        /// <inheritdoc/>
        public int EffectByte { get; }

        /// <inheritdoc/>
        public HuePlusChannel Channel { get; set; }

        /// <inheritdoc/>
        public string EffectName { get; }
        IChannel IEffect.Channel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Constructs a <see cref="CandleLight"/> effect with a given <paramref name="Color"/>.
        /// </summary>
        /// <param name="Color">The <see cref="Color"/> to display.</param>
        public CandleLight(Color Color) {
            this.Color = Color;
        }

        /// <inheritdoc/>
        public bool IsCompatibleWith(NZXTDeviceType Type)
        {
            return CompatibleWith.Contains(Type) ? true : false;
        }

        /// <inheritdoc/>
        public List<byte[]> BuildBytes(NZXTDeviceType Type, IChannel Channel) {
            switch (Type)
            {
                case NZXTDeviceType.HuePlus:
                    byte[] SettingsBytes = new byte[] { 0x4b, (byte)Channel.ChannelByte, 0x09, 0x03, 0x02 };
                    byte[] final = SettingsBytes.ConcatenateByteArr(Channel.BuildColorBytes(Color));
                    return new List<byte[]>() { final };
                case NZXTDeviceType.KrakenX:
                // TODO
                default:
                    return null;
            }
        }
    }
}
