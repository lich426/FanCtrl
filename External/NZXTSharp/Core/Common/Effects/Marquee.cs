/*
Marquee.cs
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

namespace NZXTSharp
{

    /// <summary>
    /// Represents an RGB marquee effect.
    /// </summary>
    public class Marquee : IEffect {

        private int _EffectByte = 0x03;
        private string _EffectName = "Marquee";

        /// <inheritdoc/>
        public readonly List<NZXTDeviceType> CompatibleWith = new List<NZXTDeviceType>() {
            NZXTDeviceType.HuePlus,
            NZXTDeviceType.KrakenX
        };

        private Color _Color;
        private Direction Param1;
        private LSS Param2;
        private IChannel _Channel;

        #region Properties
        /// <inheritdoc/>
        public int EffectByte { get; }

        /// <inheritdoc/>
        public IChannel Channel { get; set; }

        /// <inheritdoc/>
        public string EffectName { get; }
        #endregion

        /// <summary>
        /// Constructs a <see cref="Marquee"/> effect.
        /// </summary>
        /// <param name="Color">The <see cref="Color"/> of the effect.</param>
        /// <param name="LSS">The <see cref="LSS"/> param to apply.</param>
        public Marquee(Color Color, LSS LSS)
        {
            this._Color = Color;
            this.Param2 = LSS;
        }

        /// <summary>
        /// Constructs a <see cref="Marquee"/> effect.
        /// </summary>
        /// <param name="Color">The <see cref="Color"/> of the effect.</param>
        /// <param name="Direction">The <see cref="Direction"/> of the effect.</param>
        /// <param name="LSS">The <see cref="LSS"/> param to apply.</param>
        public Marquee(Color Color, Direction Direction, LSS LSS) {
            this._Color = Color;
            this.Param1 = Direction;
            this.Param2 = LSS;
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
                    byte[] SettingsBytes = new byte[] { 0x4b, (byte)Channel.ChannelByte, 0x03, Param1, Param2 };
                    byte[] final = SettingsBytes.ConcatenateByteArr(Channel.State == false ? Color.AllOff() : Channel.BuildColorBytes(_Color));
                    outList.Add(final);

                    return outList;
                case NZXTDeviceType.KrakenX:
                    List<byte[]> KrakenXOutList = new List<byte[]>();
                    byte[] KrakenXSettingsBytes = new byte[] { 0x02, 0x4c, 0x02, 0x03, Param2 };
                    byte[] KrakenXfinal = KrakenXSettingsBytes.ConcatenateByteArr(Channel.State == false ? Color.AllOff() : Channel.BuildColorBytes(_Color));
                    KrakenXOutList.Add(KrakenXfinal);
                    return KrakenXOutList;
                default:
                    return null;
            }
        }
    }
}
