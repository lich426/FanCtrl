/*
CISS.cs
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

namespace NZXTSharp
{

    /// <summary>
    /// Represents a CISS effect param.
    /// </summary>
    public class CISS : IParam {
        private readonly int colorIndex;
        private int speed;
        private int evaluatedIndex;
        private readonly int _Value;

        /// <inheritdoc/>
        public int Value { get => GetValue(); }

        /// <summary>
        /// Constructs a <see cref="CISS"/> instance.
        /// </summary>
        /// <param name="speed">Speed values must be 0-4 (inclusive). 0 being slowest, 2 being normal, and 4 being fastest. Defaults to 2.</param>
        public CISS(int speed = 2) {
            this.speed = speed;
            ValidateInput();
        }

        /// <summary>
        /// Constructs a <see cref="CISS"/> instance.
        /// </summary>
        /// <param name="colorIndex">The index of the color in the list.</param>
        /// <param name="speed">Speed values must be 0-4 (inclusive). 0 being slowest, 2 being normal, and 4 being fastest. Defaults to 2.</param>
        public CISS(int colorIndex, int speed = 2) {
            this.colorIndex = colorIndex;
            this.speed = speed;

            ValidateInput();

            this.evaluatedIndex = colorIndex * 2;
        }

        private void ValidateInput() {
            if (speed > 4 || speed < 0)
                throw new InvalidParamException("Invalid Param; Speed Must Be Between 0 and 4 (inclusive).");

            if (colorIndex > 7 || colorIndex < 0)
                throw new InvalidParamException("Invalid Param; ColorIndex Value Must Be Between 7 and 0 (inclusive). (Zero-Indexed)");
        }

        /// <inheritdoc/>
        public int GetValue() {
            string concatenated = evaluatedIndex.ToString("X") + speed.ToString();
            return int.Parse(concatenated, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="param"></param>
        public static implicit operator byte(CISS param) {
            return (byte)param.GetValue();
        }
    }
}
