/*
TaiChi.cs
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

namespace NZXTSharp.KrakenX
{
    /// <summary>
    /// Represents a TaiChi RGB effect.
    /// </summary>
    public class TaiChi : IEffect
    {
        private readonly List<NZXTDeviceType> CompatibleWith = new List<NZXTDeviceType> { NZXTDeviceType.KrakenX };

        private int _EffectByte = 0x08;
        private Color[] Colors;
        private int Speed;

        private IChannel _Channel;
        
        /// <inheritdoc/>
        public int EffectByte { get; }

        /// <inheritdoc/>
        public string EffectName => "TaiChi";

        /// <inheritdoc/>
        public IChannel Channel { get; set; }

        /// <summary>
        /// Constructs a <see cref="TaiChi"/> effect.
        /// </summary>
        /// <param name="Colors">The <see cref="Color"/>s to display.</param>
        /// <param name="Speed">The speed for the effect to move at.</param>
        public TaiChi(Color[] Colors, int Speed = 2)
        {
            if (Colors.Length < 2)
            {
                throw new InvalidParamException("TaiChi colors must be of length 2 or more.");
            }

            if (Speed < 0 || Speed > 4)
            {
                throw new InvalidParamException("Speed values must be between 0-4 (inclusive).");
            }

            this.Colors = Colors;
            this.Speed = Speed;
        }

        /// <inheritdoc/>
        public bool IsCompatibleWith(NZXTDeviceType Type)
        {
            return CompatibleWith.Contains(Type) ? true : false;
        }

        /// <inheritdoc/>
        public List<byte[]> BuildBytes(NZXTDeviceType Type, IChannel Channel)
        {
            if (Channel.ChannelByte == 0x00 || Channel.ChannelByte == 0x01)
            {
                throw new IncompatibleParamException("TaiChi channel can only be Ring or 0x02");
            }

            List<byte[]> KrakenOutList = new List<byte[]>();
            for (int ColorIndex = 0; ColorIndex < Colors.Length; ColorIndex++)
            {
                byte[] KrakenSettingsBytes = new byte[] { 0x02, 0x4c, (byte)Channel.ChannelByte, 0x08, new CISS(ColorIndex, Speed) };
                byte[] KrakenFinal = KrakenSettingsBytes.ConcatenateByteArr(Channel.BuildColorBytes(Colors[ColorIndex]));
                KrakenOutList.Add(KrakenFinal);
            }
            return KrakenOutList;
        }
    }
}
