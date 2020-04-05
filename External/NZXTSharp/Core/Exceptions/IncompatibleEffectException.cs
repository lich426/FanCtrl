/*
IncompatibleEffectException.cs
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

namespace NZXTSharp.Exceptions {

    /// <summary>
    /// Thrown when an effect passed to <see cref="INZXTDevice.ApplyEffect(IEffect)"/> 
    /// is not compatible with that <see cref="INZXTDevice"/>.
    /// </summary>
    public class IncompatibleEffectException : Exception {

        private static string baseString = "NXZTSharp.Exceptions.IncompatibleEffectException; ";

        /// <inheritdoc/>
        public IncompatibleEffectException() 
            : base(baseString + "Invalid Effect Supplied To ApplyEffect()") {

        }

        /// <inheritdoc/>
        public IncompatibleEffectException(string message)
            : base(message) {

        }

        /// <summary>
        /// Constructs an <see cref="IncompatibleEffectException"/> with more information
        /// about the <see cref="INZXTDevice"/> and <see cref="IEffect"/>.
        /// </summary>
        /// <param name="DeviceName"></param>
        /// <param name="EffectName"></param>
        public IncompatibleEffectException(string DeviceName, string EffectName) 
            : base(string.Format(baseString + "Invalid Effect \"{0}\" Supplied To ApplyEffect() Of Device \"{1}\"",
                EffectName, DeviceName)){

        }

        /// <inheritdoc/>
        public IncompatibleEffectException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
