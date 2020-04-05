/*
Strip.cs
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

using NZXTSharp.Exceptions;
using System.Linq;

namespace NZXTSharp
{

    /// <summary>
    /// Represents an RGB Strip subdevice.
    /// </summary>
    public class Strip : ISubDevice {

        private bool _IsActive = true;

        private bool[] _Leds = new bool[]
        {
            true, true, true, true, true,
            true, true, true, true, true
        };

        /// <summary>
        /// Whether or not the current <see cref="Strip"/> instance is active (on).
        /// </summary>
        public bool IsActive { get => _IsActive; }

        /// <summary>
        /// Returns the <see cref="NZXTDeviceType"/> of the fan.
        /// </summary>
        public NZXTDeviceType Type { get => NZXTDeviceType.Fan; }

        /// <summary>
        /// Returns the number of LEDs available on the <see cref="Strip"/>.
        /// </summary>
        public int NumLeds { get => 10; }

        /// <summary>
        /// A list containing the power states of the <see cref="Strip"/>'s LEDs.
        /// </summary>
        public bool[] Leds { get => _Leds; }

        /// <summary>
        /// Constructs a <see cref="Strip"/> instance.
        /// </summary>
        public Strip() {

        }

        /// <summary>
        /// Toggles the <see cref="Strip"/>'s state.
        /// </summary>
        public void ToggleState() {
            this._IsActive = (this._IsActive ? false : true);
        }

        /// <inheritdoc/>
        public void SetState(bool State)
        {
            this._IsActive = State;
        }

        /// <summary>
        /// Toggles a specific LED owned by the <see cref="Strip"/> device.
        /// </summary>
        /// <param name="Index">The index in the <see cref="Strip"/>'s <see cref="Leds"/> list to toggle.</param>
        public void ToggleLed(int Index) {
            if (Index > 7 || Index < 0) { throw new SubDeviceLEDDoesNotExistException(); }
            this._Leds[Index] = (this._Leds[Index] ? false : true);
        }

        /// <summary>
        /// Toggles all LEDs between a given <paramref name="Start"/> and <paramref name="End"/> index.
        /// </summary>
        /// <param name="Start">The index in the <see cref="Strip"/>'s <see cref="Leds"/> list to start at.</param>
        /// <param name="End">The index in the <see cref="Strip"/>'s <see cref="Leds"/> list to end at.</param>
        public void ToggleLedRange(int Start, int End) {
            for (int Index = Start; Index <= End; Index++) {
                _Leds[Index] = (this._Leds[Index] ? false : true);
            }
        }

        /// <summary>
        /// Sets all LEDs between a given <paramref name="Start"/> index and an <paramref name="End"/> index to a given <paramref name="Value"/>.
        /// </summary>
        /// <param name="Start">The index in the <see cref="Strip"/>'s <see cref="Leds"/> list to start at.</param>
        /// <param name="End">The index in the <see cref="Strip"/>'s <see cref="Leds"/> list to end at.</param>
        /// <param name="Value">The value to set each LED to.</param>
        public void SetLedRange(int Start, int End, bool Value) {
            for (int Index = Start; Index <= End; Index++) {
                _Leds[Index] = Value;
            }
        }

        /// <summary>
        /// Sets all LEDs in the <see cref="Strip"/>'s <see cref="Leds"/> list to true.
        /// </summary>
        public void AllLedOn() {
            for (int index = 0; index < 10; index++) {
                _Leds[index] = true;
            }
        }

        /// <summary>
        /// Sets all LEDs in the <see cref="Strip"/>'s <see cref="Leds"/> list to false.
        /// </summary>
        public void AllLedOff() {
            for (int index = 0; index < 10; index++) {
                _Leds[index] = false;
            }
        }

        /// <summary>
        /// Returns a string with all LED states.
        /// </summary>
        /// <returns></returns>
        public string LedsToString() {
            StringBuilder sb = new StringBuilder();
            foreach (bool LED in _Leds)
            {
                sb.Append(LED + " ");
            }
            return sb.ToString();
        }
    }
}
