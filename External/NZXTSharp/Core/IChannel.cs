/*
IChannel.cs
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
    /// Represents an RGB or Fan channel on an <see cref="INZXTDevice"/>.
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// The ChannelByte of the <see cref="IChannel"/>.
        /// </summary>
        int ChannelByte { get; }

        /// <summary>
        /// Whether or not the <see cref="IChannel"/> is on.
        /// </summary>
        bool State { get; }

        /// <summary>
        /// Builds color bytes for an RGB effect for a given <see cref="Color"/>.
        /// </summary>
        /// <param name="Color"></param>
        /// <returns></returns>
        byte[] BuildColorBytes(Color Color);

        /// <summary>
        /// Builds color bytes for an RGB effect for a given custom color set.
        /// </summary>
        /// <param name="Custom"></param>
        /// <returns></returns>
        byte[] BuildColorBytes(byte[] Custom);
    }
}
