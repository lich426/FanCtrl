/*
Wings.cs
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
    /// Represents an RGB wings effect.
    /// </summary>
    public class Wings : IEffect {
        private int _EffectByte = 0x0c;
        private string _EffectName = "Wings";

        /// <inheritdoc/>
        public readonly List<NZXTDeviceType> CompatibleWith = new List<NZXTDeviceType>() {
            NZXTDeviceType.HuePlus,
            NZXTDeviceType.KrakenX
        };

        /// <summary>
        /// The array of colors used by the effect.
        /// </summary>
        public Color Color;
        private IChannel _Channel;
        private CISS _Param2;
        private int speed = 2;

        /// <inheritdoc/>
        public int EffectByte { get; }

        /// <inheritdoc/>
        public IChannel Channel { get; set; }

        /// <inheritdoc/>
        public string EffectName { get; }

        /// <summary>
        /// Constructs a <see cref="Wings"/> effect with the given <see cref="Color"/> array and speed.
        /// </summary>
        /// <param name="Color"></param>
        /// <param name="Speed">Speed values must be 0-4 (inclusive). 0 being slowest, 2 being normal, and 4 being fastest. Defaults to 2.</param>
        public Wings(Color Color, int Speed = 2) {
            this.Color = Color;
            this.speed = Speed;
            ValidateParams();
        }

        private void ValidateParams() 
        {
            if (speed > 4 || speed < 0) 
            {
                throw new InvalidEffectSpeedException();
            }
        }

        /// <inheritdoc/>
        public bool IsCompatibleWith(NZXTDeviceType Type) 
        {
            return CompatibleWith.Contains(Type) ? true : false;
        }

        /// <inheritdoc/>
        public List<byte[]> BuildBytes(NZXTDeviceType Type, IChannel Channel) 
        {
            switch (Type) {
                case NZXTDeviceType.HuePlus:
                    List<byte[]> outList = new List<byte[]>();
                    byte[] SettingsBytes = new byte[] { 0x4b, (byte)Channel.ChannelByte, 0x0c, 0x03, new CISS(0, this.speed) };
                    byte[] final = SettingsBytes.ConcatenateByteArr(Channel.BuildColorBytes(Color));
                    outList.Add(final);

                    return outList;
                case NZXTDeviceType.KrakenX:
                    List<byte[]> KrakenXOutBytes = new List<byte[]>();
                    byte[] KrakenXSettingsBytes = new byte[] { 0x02, 0x4c, (byte)Channel.ChannelByte, 0x0c, (byte)speed };
                    byte[] KrakenXFinal = KrakenXSettingsBytes.ConcatenateByteArr(Channel.BuildColorBytes(Color));
                    KrakenXOutBytes.Add(KrakenXFinal);

                    return KrakenXOutBytes;
                default:
                    return null;
            }
        }
    }
}
