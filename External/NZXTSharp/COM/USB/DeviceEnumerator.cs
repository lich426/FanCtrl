/*
DeviceEnumerator.cs
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

using HidLibrary;
using NZXTSharp;

namespace NZXTSharp.COM
{
    /// <summary>
    /// Copied from https://github.com/DarkMio/Octopode with modifications.
    /// </summary>
    public class DeviceEnumerator
    {
        /// <summary>
        /// Enumerates all HID devices connected to the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> containing all found <see cref="HidDevice"/>s.</returns>
        public static IEnumerable<HidDevice> EnumAllDevices()
        {
            return HidDevices.Enumerate();
        }

        /// <summary>
        /// Enumerates all NZXT devices connected to the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> containing all found NZXT <see cref="HidDevice"/>s.</returns>
        public static IEnumerable<HidDevice> EnumNZXTDevices()
        {
            return HidDevices.Enumerate(0x1E71);
        }

        /// <summary>
        /// Enumerates all <see cref="KrakenX.KrakenX"/> devices connected to the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> containing all found
        /// <see cref="KrakenX.KrakenX"/> <see cref="HidDevice"/>s.</returns>
        public static IEnumerable<HidDevice> EnumKrakenXDevices()
        {
            foreach (var device in EnumNZXTDevices())
            {
                if (device.Attributes.ProductId == (int)HIDDeviceID.KrakenX)
                {
                    yield return device;
                }
            }
        }
    }
}
