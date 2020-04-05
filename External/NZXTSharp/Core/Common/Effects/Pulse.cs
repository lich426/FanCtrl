/*
Pulse.cs
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
    /// Represents an RGB pulse effect.
    /// </summary>
    public class Pulse : IEffect {
        #pragma warning disable IDE0044 // Add readonly modifier
        private int _EffectByte = 0x06;
        private string _EffectName = "Pulse";

        /// <inheritdoc/>
        public readonly List<NZXTDeviceType> CompatibleWith = new List<NZXTDeviceType>() {
            NZXTDeviceType.HuePlus,
            NZXTDeviceType.KrakenX
        };

        /// <summary>
        /// The array of colors used by the effect.
        /// </summary>
        public Color[] Colors;
        private IChannel _Channel;
        private CISS _Param2;
        private int speed = 2;
        #pragma warning restore IDE0044 // Add readonly modifier

        /// <inheritdoc/>
        public int EffectByte { get; }

        /// <inheritdoc/>
        public IChannel Channel { get; set; }

        /// <inheritdoc/>
        public string EffectName { get; }

        /// <summary>
        /// Constructs a <see cref="Pulse"/> effect with the given <see cref="Color"/> array and speed.
        /// </summary>
        /// <param name="Colors"></param>
        /// <param name="Speed">Speed values must be 0-4 (inclusive). 0 being slowest, 2 being normal, and 4 being fastest. Defaults to 2.</param>
        public Pulse(Color[] Colors, int Speed = 2) {
            this.Colors = Colors;
            this.speed = Speed;
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
        public List<byte[]> BuildBytes(NZXTDeviceType Type, IChannel Channel) {
            switch (Type)
            {
                case NZXTDeviceType.HuePlus:
                    List<byte[]> outList = new List<byte[]>();
                    for (int colorIndex = 0; colorIndex < Colors.Length; colorIndex++)
                    {
                        byte[] SettingsBytes = new byte[] { 0x4b, (byte)Channel.ChannelByte, 0x06, 0x03, new CISS(colorIndex, this.speed) };
                        byte[] final = SettingsBytes.ConcatenateByteArr(Channel.BuildColorBytes(Colors[colorIndex]));
                        outList.Add(final);
                    }
                    return outList;
                case NZXTDeviceType.KrakenX:
                    List<byte[]> KrakenOutList = new List<byte[]>();
                    Console.WriteLine("ColorsLength - " + Colors.Length);
                    for (int colorIndex = 0; colorIndex < Colors.Length; colorIndex++)
                    {
                        byte[] KrakenSettingsBytes = new byte[] { 0x02, 0x4c, (byte)Channel.ChannelByte, 0x07, new CISS(colorIndex, this.speed) };
                        byte[] KrakenFinal = KrakenSettingsBytes.ConcatenateByteArr(Channel.BuildColorBytes(Colors[colorIndex]));
                        KrakenOutList.Add(KrakenFinal);
                    }
                    Console.WriteLine("OutList Length - " + KrakenOutList.Count);
                    return KrakenOutList;
                default:
                    return null;
            }
        }
    }
}
