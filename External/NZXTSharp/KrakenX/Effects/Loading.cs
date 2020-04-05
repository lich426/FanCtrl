/*
Loading.cs
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
    /// Represents a loading RGB effect.
    /// </summary>
    public class Loading : IEffect
    {
        private int _EffectByte = 0x0a;
        private readonly List<NZXTDeviceType> CompatibleWith = new List<NZXTDeviceType> { NZXTDeviceType.KrakenX };
        private Color Color;
        private int Speed;

        /// <inheritdoc/>
        public int EffectByte { get; }

        /// <inheritdoc/>
        public string EffectName => throw new NotImplementedException();

        /// <inheritdoc/>
        public IChannel Channel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Constructs a <see cref="Loading"/> effect.
        /// </summary>
        /// <param name="Color">The <see cref="NZXTSharp.Color"/> of the effect to display.</param>
        /// <param name="Speed">The speed the effect will move at.</param>
        public Loading(Color Color, int Speed = 2)
        {
            if (Speed < 0 || Speed > 4)
            {
                throw new InvalidParamException("Speed values must be between 0-4 (inclusive).");
            }

            this.Color = Color;
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
                throw new IncompatibleParamException("TaiChi channel can only be Ring (ChanneByte: 0x02)");
            }

            List<byte[]> KrakenOutList = new List<byte[]>();
            byte[] KrakenSettingsBytes = new byte[] { 0x02, 0x4c, (byte)Channel.ChannelByte, 0x0a, (byte)Speed };
            byte[] KrakenFinal = KrakenSettingsBytes.ConcatenateByteArr(Channel.BuildColorBytes(Color));
            KrakenOutList.Add(KrakenFinal);

            return KrakenOutList;
        }
    }
}
