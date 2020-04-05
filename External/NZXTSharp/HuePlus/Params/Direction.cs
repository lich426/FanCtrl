/*
Direction.cs
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

namespace NZXTSharp.HuePlus {

    /// <summary>
    /// Represents a <see cref="Direction"/> effect param.
    /// </summary>
    public class Direction : IParam {
        private bool _withMovement;
        private bool _isForward;
        private int _Value;
        private List<string> _CompatibleWith = new List<string>() { "HuePlus" };

        /// <summary>
        /// Whether or not the effect will move.
        /// </summary>
        public bool WithMovement { get; }

        /// <summary>
        /// Whether or not the effect will move forward or backward.
        /// </summary>
        public bool IsForward { get; }

        /// <inheritdoc/>
        public int Value { get => GetValue(); }

        /// <summary>
        /// Constructs a <see cref="Direction"/> param.
        /// </summary>
        /// <param name="isForward">Whether or not the param moves forward or backward.</param>
        /// <param name="withMovement">Whether or not the effect will move smoothly.</param>
        public Direction(bool isForward = true, bool withMovement = true) {
            this._withMovement = withMovement;
            this._isForward = isForward;
        }

        /// <inheritdoc/>
        public int GetValue() {
            if (IsForward)
                if (WithMovement)
                    return 0x0b; // Forward W/ movement
                else
                    return 0x03; // Forward W/O movement
            else
                if (WithMovement)
                return 0x1b; // Backward W/ movement
            else
                return 0x13; // Backward W/O movement
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public static implicit operator byte(Direction param) {
            return (byte)param.GetValue();
        }
    }
}
