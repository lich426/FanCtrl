/*
ISubDevice.cs
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
    /// Represents a sub device.
    /// </summary>
    public interface ISubDevice {

        /// <summary>
        /// The <see cref="ISubDevice"/>'s <see cref="NZXTDeviceType"/>.
        /// </summary>
        NZXTDeviceType Type { get; }

        /// <summary>
        /// Whether or not the current <see cref="ISubDevice"/> instance is active (on).
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Returns the number of LEDs available on the <see cref="ISubDevice"/>.
        /// </summary>
        int NumLeds { get; }

        /// <summary>
        /// A list containing the power states of the <see cref="ISubDevice"/>'s LEDs.
        /// </summary>
        bool[] Leds { get; }

        /// <summary>
        /// Toggles the <see cref="ISubDevice"/>'s state.
        /// </summary>
        void ToggleState();

        /// <summary>
        /// Sets the <see cref="ISubDevice"/>'s state.
        /// </summary>
        /// <param name="State">The state to set the <see cref="ISubDevice"/> to. true: on, false: off.</param>
        void SetState(bool State);

        /// <summary>
        /// Toggles a specific LED owned by the <see cref="ISubDevice"/>.
        /// </summary>
        /// <param name="Index">The index in the <see cref="ISubDevice"/>'s <see cref="Leds"/> list to toggle.</param>
        void ToggleLed(int Index);

        /// <summary>
        /// Toggles all LEDs between a given <paramref name="Start"/> and <paramref name="End"/> index.
        /// </summary>
        /// <param name="Start">The index in the <see cref="ISubDevice"/>'s <see cref="Leds"/> list to start at.</param>
        /// <param name="End">The index in the <see cref="ISubDevice"/>'s <see cref="Leds"/> list to end at.</param>
        void ToggleLedRange(int Start, int End);

        /// <summary>
        /// Sets all LEDs between a given <paramref name="Start"/> index and an <paramref name="End"/> index to a given <paramref name="Value"/>.
        /// </summary>
        /// <param name="Start">The index in the <see cref="ISubDevice"/>'s <see cref="Leds"/> list to start at.</param>
        /// <param name="End">The index in the <see cref="ISubDevice"/>'s <see cref="Leds"/> list to end at.</param>
        /// <param name="Value">The value to set each LED to.</param>
        void SetLedRange(int Start, int End, bool Value);

        /// <summary>
        /// Sets all LEDs in the <see cref="ISubDevice"/>'s <see cref="Leds"/> list to true.
        /// </summary>
        void AllLedOn();

        /// <summary>
        /// Sets all LEDs in the <see cref="ISubDevice"/>'s <see cref="Leds"/> list to false.
        /// </summary>
        void AllLedOff();

        /// <summary>
        /// Returns a string with all LED states.
        /// </summary>
        /// <returns></returns>
        string LedsToString();
    }
}
