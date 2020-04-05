/*
IEffect.cs
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
    /// Represents a generic RGB effect.
    /// </summary>
    public interface IEffect {

        /// <summary>
        /// The <see cref="IEffect"/>'s EffectByte.
        /// </summary>
        int EffectByte { get; }

        /// <summary>
        /// The name of the <see cref="IEffect"/>.
        /// </summary>
        string EffectName { get; }

        /// <summary>
        /// The <see cref="Channel"/> to set the <see cref="IEffect"/> on.
        /// </summary>
        IChannel Channel { get; set; }

        /// <summary>
        /// Checks to see if the <see cref="IEffect"/> is compatible with a given <see cref="NZXTDeviceType"/> <paramref name="Type"/>.
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        bool IsCompatibleWith(NZXTDeviceType Type);

        /// <summary>
        /// Builds and returns the buffer queue needed to set the <see cref="IEffect"/>.
        /// </summary>
        /// <param name="Type">The <see cref="NZXTDeviceType"/> to build effect bytes for.</param>
        /// <param name="Channel">The <see cref="IChannel"/> to build effect bytes for.</param>
        /// <returns></returns>
        List<byte[]> BuildBytes(NZXTDeviceType Type, IChannel Channel);
    }
}
