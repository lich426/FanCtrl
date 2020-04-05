/*
NZXTDeviceType.cs
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

namespace NZXTSharp
{

    /// <summary>
    /// Definitions of unique IDs for all NZXT devices.
    /// </summary>
    public enum NZXTDeviceType 
    {
        /// <summary>
        /// If device type is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 
        /// </summary>
        NotActive = 1,


        // SubDevices
        /// <summary>
        /// Any fan, RGB or otherwise.
        /// </summary>
        Fan = 2,

        /// <summary>
        /// An RGB strip.
        /// </summary>
        Strip = 3,

        // Krakens
        /// <summary>
        /// A generic Kraken device.
        /// </summary>
        Kraken = 4,

        /// <summary>
        /// A generic KrakenX device.
        /// </summary>
        KrakenX = 5,

        /// <summary>
        /// A generic KrakenM device.
        /// </summary>
        KrakenM = 6,

        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        KrakenM22 = 7,
        KrakenX42 = 8,
        KrakenX52 = 9,
        KrakenX62 = 10,
        KrakenX72 = 11,
        #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


        // Hues
        /// <summary>
        /// A generic Hue device.
        /// </summary>
        Hue = 12,

        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        HuePlus = 13,
        Hue2 = 14,
        HueAmbient = 15,
        #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


        // N7s
        /// <summary>
        /// A generic motherboard device.
        /// </summary>
        Motherboard = 16,

        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        N7 = 17,
        N7_Z390 = 18,
        #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


        // Grids
        /// <summary>
        /// A generic Grid device.
        /// </summary>
        Grid = 19,

        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        GridPlus = 20,
        GridV2 = 21,
        GridV3 = 22,
        #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        // Misc
        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        H7Lumi = 23,
        SmartDevice = 24,
        #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
