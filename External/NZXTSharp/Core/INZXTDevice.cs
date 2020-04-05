/*
INZXTDevice.cs
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
    /// Represents an NZXT device.
    /// </summary>
    public interface INZXTDevice
    {
        /// <summary>
        /// The <see cref="NZXTDeviceType"/> of the <see cref="INZXTDevice"/>.
        /// </summary>
        NZXTDeviceType Type { get; }

        /// <summary>
        /// A unique device ID.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Applies an <see cref="IEffect"/> to the <see cref="INZXTDevice"/>.
        /// </summary>
        /// <param name="Effect">The <see cref="IEffect"/> to apply.</param>
        void ApplyEffect(IEffect Effect);

        /// <summary>
        /// Disposes of the <see cref="INZXTDevice"/> instance.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Attempts to reconnect to the <see cref="INZXTDevice"/>.
        /// </summary>
        void Reconnect();

        Version GetFirmwareVersion();
    }
}
