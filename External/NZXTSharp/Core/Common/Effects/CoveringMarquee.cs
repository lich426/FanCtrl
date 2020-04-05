/*
CoveringMarquee.cs
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
using NZXTSharp.Exceptions;

namespace NZXTSharp
{

    /// <summary>
    /// Represents an RGB CoveringMarquee effect.
    /// </summary>
    public class CoveringMarquee : IEffect {

        private int _EffectByte = 0x04;
        private string _EffectName = "CoveringMarquee";

        /// <inheritdoc/>
        public readonly List<NZXTDeviceType> CompatibleWith = new List<NZXTDeviceType>() {
            NZXTDeviceType.HuePlus,
            NZXTDeviceType.KrakenX
        };

        private Color[] _Colors;
        private Direction Param1;
        private CISS Param2;
        private IChannel _Channel;
        private int _Speed;

        /// <inheritdoc/>
        public int EffectByte { get; }

        /// <inheritdoc/>
        public IChannel Channel { get; set; }

        /// <inheritdoc/>
        public string EffectName { get; }

        /// <summary>
        /// Constructs a <see cref="CoveringMarquee"/> effect.
        /// </summary>
        /// <param name="Color1">The first <see cref="Color"/> in the effect.</param>
        /// <param name="Color2">The second <see cref="Color"/> in the effect.</param>
        /// <param name="Direction">The <see cref="Direction"/> the effect will go in.</param>
        /// <param name="speed">Speed values must be 0-4 (inclusive). 0 being slowest, 2 being normal, and 4 being fastest. Defaults to 2.</param>
        public CoveringMarquee(Color Color1, Color Color2, Direction Direction, int speed = 2) {
            this._Colors = new Color[] { Color1, Color2 };
            this.Param1 = Direction;
            this._Speed = speed;
            ValidateParams();
        }

        /// <summary>
        /// Constructs a <see cref="CoveringMarquee"/> effect.
        /// </summary>
        /// <param name="Colors"></param>
        /// <param name="Direction">The <see cref="Direction"/> the effect will go in.</param>
        /// <param name="speed">Speed values must be 0-4 (inclusive). 0 being slowest, 2 being normal, and 4 being fastest. Defaults to 2.</param>
        public CoveringMarquee(Color[] Colors, Direction Direction, int speed = 2) {
            this._Colors = Colors;
            this.Param1 = Direction;
            this._Speed = speed;
            ValidateParams();
        }

        private void ValidateParams() {
            if (this._Colors.Length > 15) {
                throw new TooManyColorsProvidedException();
            }

            if (this._Speed > 4 || this._Speed < 0) {
                throw new InvalidEffectSpeedException();
            }
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
                    List<byte[]> outList = new List<byte[]>();
                    for (int colorIndex = 0; colorIndex < _Colors.Length; colorIndex++)
                    {
                        byte[] SettingsBytes = new byte[] { 0x4b, (byte)Channel.ChannelByte, 0x04, Param1, new CISS(colorIndex, this._Speed) };
                        byte[] final = SettingsBytes.ConcatenateByteArr(Channel.BuildColorBytes(_Colors[colorIndex]));
                        outList.Add(final);
                    }

                    return outList;
                case NZXTDeviceType.KrakenX:
                    List<byte[]> KrakenOutList = new List<byte[]>();
                    DCB param = new DCB(Channel.ChannelByte, Param1.IsForward);
                    for (int colorIndex = 0; colorIndex < _Colors.Length; colorIndex++) 
                    {
                        byte[] KrakenSettingsBytes = new byte[] {0x2, 0x4c, (byte)param.GetValue(), 0x04, new CISS(colorIndex, _Speed)};
                        byte[] KrakenFinal = KrakenSettingsBytes.ConcatenateByteArr(Channel.BuildColorBytes(_Colors[colorIndex]));
                        KrakenOutList.Add(KrakenFinal);
                    }
                    return KrakenOutList;
                default:
                    return null;
            }
        }
    }
}
