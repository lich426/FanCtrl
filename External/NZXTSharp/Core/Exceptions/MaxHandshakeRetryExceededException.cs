/*
MaxHandshakeRetryExceededException.cs
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

namespace NZXTSharp.Exceptions
{
    /// <summary>
    /// Thrown when the maximum number of handshake attempts 
    /// has been exceeded during device intitialization.
    /// 
    /// Max Retry Count is 5 by default.
    /// </summary>
    public class MaxHandshakeRetryExceededException : Exception
    {
        /// <inheritdoc/>
        public MaxHandshakeRetryExceededException()
        {

        }

        /// <inheritdoc/>
        public MaxHandshakeRetryExceededException(string message)
            : base(message)
        {

        }


        /// <summary>
        /// Constructs a <see cref="MaxHandshakeRetryExceededException"/>,
        /// with more information about the max handshake retry.
        /// </summary>
        /// <param name="MaxCount">The max retry count.</param>
        public MaxHandshakeRetryExceededException(int MaxCount)
            : base(String.Format("Max handshake retry count ({0}) exceeded.", MaxCount))
        {

        }

        /// <inheritdoc/>
        public MaxHandshakeRetryExceededException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
