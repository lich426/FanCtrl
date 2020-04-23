/*
DeviceLoadFilter.cs
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
    /// Defines filters for getting loaded devices.
    /// Ex. DeviceLoadFilter.Coolers will return ONLY coolers.
    /// </summary>
    public enum DeviceLoadFilter
    {
        /// <summary>
        /// All Devices
        /// </summary>
        All = 0,

        /// <summary>
        /// Any Cooler
        /// </summary>
        Coolers = 1,

        /// <summary>
        /// Any Lighting Controller
        /// </summary>
        LightingControllers = 2,

        /// <summary>
        /// Any Fan Controller
        /// </summary>
        FanControllers = 3,

        /// <summary>
        /// Any Kraken Device
        /// </summary>
        Kraken = 4,

        /// <summary>
        /// Any KrakenM Device
        /// </summary>
        KrakenM = 5,

        /// <summary>
        /// Any KrakenX Device
        /// </summary>
        KrakenX = 6,
        KrakenX3 = 13,

        /// <summary>
        /// Any Grid Device
        /// </summary>
        Grid = 7,

        /// <summary>
        /// Any Gridv3 Device
        /// </summary>
        Gridv3 = 8,

        /// <summary>
        /// Any Hue Device
        /// </summary>
        Hue = 9,

        /// <summary>
        /// Any Hue+ Device
        /// </summary>
        HuePlus = 10,

        /// <summary>
        /// Any Hue2 Device
        /// </summary>
        Hue2 = 11,

        /// <summary>
        /// Any Hue Ambient Devices
        /// </summary>
        HueAmbient = 12,
    }
}
