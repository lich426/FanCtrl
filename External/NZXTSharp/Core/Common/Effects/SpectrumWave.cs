/*
SpectrumWave.cs
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
using NZXTSharp.HuePlus;
using NZXTSharp.KrakenX;

namespace NZXTSharp
{

    /// <summary>
    /// Represents an RGB Spectrum Wave effect.
    /// </summary>
    public class SpectrumWave : IEffect {
        private int _EffectByte = 0x02;
        private string _EffectName = "SpectrumWave";

        /// <inheritdoc/>
        public readonly List<NZXTDeviceType> CompatibleWith = new List<NZXTDeviceType>() {
            NZXTDeviceType.HuePlus,
            NZXTDeviceType.KrakenX
        };

        private int speed;
        private Direction Param1;
        private CISS Param2;
        private IChannel _Channel;

        /// <inheritdoc/>
        public int EffectByte { get; }

        /// <inheritdoc/>
        public IChannel Channel { get; set; }

        /// <inheritdoc/>
        public string EffectName { get; }

        /// <summary>
        /// Constructs a <see cref="SpectrumWave"/> effect with the given <see cref="Direction"/>.
        /// </summary>
        /// <param name="Direction"></param>
        /// <param name="Speed">Speed values must be 0-4 (inclusive). 0 being slowest, 2 being normal, and 4 being fastest. Defaults to 2.</param>
        public SpectrumWave(Direction Direction = null, int Speed = 2) {
            this.speed = Speed;
            this.Param1 = Direction ?? new Direction();
            this.Param2 = new CISS(0, Speed);
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
                    byte[] SettingsBytes = new byte[] { 0x4b, (byte)Channel.ChannelByte, 0x02, Param1, Param2 };
                    byte[] final = SettingsBytes.ConcatenateByteArr(Channel.BuildColorBytes(new Color(0, 0, 255)));
                    return new List<byte[]>() { final };
                case NZXTDeviceType.KrakenX:
                    DCB param = new DCB(Channel.ChannelByte, Param1.IsForward);
                    byte[] KrakenSettingsBytes = new byte[] { 0x2, 0x4c, (byte)param.GetValue(), 0x02, (byte)speed };
                    byte[] KrakenFinal = KrakenSettingsBytes.ConcatenateByteArr(Channel.BuildColorBytes(new Color(0, 0, 255)));
                    return new List<byte[]>() { KrakenFinal };
                default:
                    return null;
            }
        }
    }
}
