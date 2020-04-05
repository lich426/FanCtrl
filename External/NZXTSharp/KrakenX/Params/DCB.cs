/*
DCB.cs
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
using System.Text.RegularExpressions;

using NZXTSharp;
using NZXTSharp.Exceptions;

namespace NZXTSharp.KrakenX
{
    /// <summary>
    /// Represents a DCB param.
    /// </summary>
    public class DCB : IParam
    {
        /// <summary>
        /// The <see cref="DCB"/> param's value.
        /// </summary>
        public int Value => GetValue();

        private bool _IsForward = true;
        private int ChannelByte;
        private KrakenXChannel Channel;


        /// <summary>
        /// Constructs a <see cref="DCB"/> instance.
        /// </summary>
        /// <param name="ChannelByte">The ChannelByte to construct with.</param>
        /// <param name="IsForward">Whether or not the effect is moving forward.</param>
        public DCB(int ChannelByte, bool IsForward)
        {
            if (ChannelByte != 0x00 || ChannelByte != 0x02) {
                //throw new InvalidParamException("ChannelBytes for DCB param must be 0x00 or 0x02.");
            }
            this.ChannelByte = ChannelByte;
            this._IsForward = IsForward;
        }

        /// <summary>
        /// Constructs a <see cref="DCB"/> instance.
        /// </summary>
        /// <param name="Channel">A string representation of the ChannelByte to construct with.</param>
        /// <param name="IsForward">Whether or not the effect os moving forward.</param>
        public DCB(string Channel, bool IsForward)
        {
            if (!Regex.IsMatch(Channel, @"(0?(x|X)?)\d+"))
            {
                throw new InvalidParamException("ChannelByte input formatted incorrectly. Must be 0x0n, 0n, or n");
            }

            this.ChannelByte = Convert.ToInt32(Channel[Channel.Length - 1]);
            this._IsForward = IsForward;
        }

        /// <summary>
        /// Constructs a <see cref="DCB"/> instance.
        /// </summary>
        /// <param name="Channel">The <see cref="KrakenXChannel"/> to construct the param for.</param>
        /// <param name="IsForward">Whether or not the effect is moving forward.</param>
        public DCB(KrakenXChannel Channel, bool IsForward)
        {
            this.Channel = Channel;
            this._IsForward = IsForward;
        }

        /// <summary>
        /// Gets the <see cref="DCB"/> param's value.
        /// </summary>
        /// <returns></returns>
        public int GetValue()
        {
            string Dir = _IsForward ? "0" : "1";
            string CB = Channel != null ? Channel.ChannelByte.ToString() : ChannelByte.ToString();
            return int.Parse(Dir + CB, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public static implicit operator byte(DCB param)
        {
            return (byte)param.GetValue();
        }
    }
}
