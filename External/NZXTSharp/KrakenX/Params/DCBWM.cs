/*
DCBWM.cs
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
using System.Text.RegularExpressions;
using NZXTSharp.Exceptions;

namespace NZXTSharp.KrakenX
{
    class DCBWM
    {
        public int Value => GetValue();

        private bool IsForward = true;
        private bool WithMovement = true;
        private int ChannelByte;
        private KrakenXChannel Channel;

        public DCBWM(int ChannelByte, bool IsForward, bool WithMovement)
        {
            if (ChannelByte != 0x02)
            {
                throw new InvalidParamException("ChannelBytes for DCBWM param must be 0x02; " +
                    "Alternating effect can only be applied to ring.");
            }
            this.ChannelByte = ChannelByte;
            this.IsForward = IsForward;
            this.WithMovement = WithMovement;
        }

        public DCBWM(string Channel, bool IsForward, bool WithMovement)
        {
            if (!Regex.IsMatch(Channel, @"(0?(x|X)?)\d+"))
            {
                throw new InvalidParamException("ChannelByte input formatted incorrectly. Must be 0x0n, 0n, or n");
            }

            this.ChannelByte = Convert.ToInt32(Channel[Channel.Length - 1]);
            if (this.ChannelByte != 2) 
                throw new InvalidParamException("ChannelBytes for DCBWM param must be 0x02."); 

            this.IsForward = IsForward;
            this.WithMovement = WithMovement;
        }

        public DCBWM(KrakenXChannel Channel, bool IsForward, bool WithMovement)
        {
            this.Channel = Channel;
            if (this.Channel.ChannelByte != 2)
                throw new InvalidParamException("ChannelBytes for DCBWM param must be 0x02." +
                    "Alternating effect can only be applied to ring.");

            this.IsForward = IsForward;
            this.WithMovement = WithMovement;
        }

        public int GetValue()
        {
            string Dir = IsForward ? "1" : "0";
            string WM = WithMovement ? "2" : "A";
            return int.Parse(Dir + WM, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public static implicit operator byte(DCBWM param)
        {
            return (byte)param.GetValue();
        }
    }
}
