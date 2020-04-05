/*
SerialCOMData.cs
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

using System.IO.Ports;

namespace NZXTSharp.COM {

    /// <summary>
    /// Contains information needed to open a COM port.
    /// </summary>
    public class SerialCOMData {

        #region Properties and Fields
        private readonly Parity _Parity;
        private readonly StopBits _StopBits;
        private readonly int _WriteTimeout;
        private readonly int _ReadTimeout;
        private readonly int _Baud;
        private readonly int _DataBits;
        private readonly string _Name;

        /// <summary>
        /// The <see cref="System.IO.Ports.Parity"/> setting of the <see cref="SerialCOMData"/> instance.
        /// </summary>
        public Parity Parity { get => _Parity; }

        /// <summary>
        /// The <see cref="System.IO.Ports.StopBits"/> setting of the <see cref="SerialCOMData"/> instance.
        /// </summary>
        public StopBits StopBits { get => _StopBits; }

        /// <summary>
        /// The write timeout setting of the <see cref="SerialCOMData"/> instance (ms).
        /// </summary>
        public int WriteTimeout { get => _WriteTimeout; }

        /// <summary>
        /// The read timeout setting of the <see cref="SerialCOMData"/> instance (ms).
        /// </summary>
        public int ReadTimeout { get => _ReadTimeout; }

        /// <summary>
        /// The baud setting of the <see cref="SerialCOMData"/> instance.
        /// </summary>
        public int Baud { get => _Baud; }

        /// <summary>
        /// The databits setting of the <see cref="SerialCOMData"/> instance.
        /// </summary>
        public int DataBits { get => _DataBits; }

        /// <summary>
        /// The custom name of the <see cref="SerialCOMData"/> instance.
        /// </summary>
        public string Name { get => _Name; }
        #endregion


        #region Methods
        /// <summary>
        /// Constructs a <see cref="SerialCOMData"/> object.
        /// </summary>
        /// <param name="Parity"> The <see cref="Parity"/> type to use.</param>
        /// <param name="StopBits"> The number of <see cref="StopBits"/> to use. </param>
        /// <param name="WriteTimeout"> The WriteTimeout in ms.</param>
        /// <param name="ReadTimeout"> The ReadTimeout in ms.</param>
        /// <param name="Baud"> The baud to use.</param>
        /// <param name="DataBits"> The number of DataBits to use.</param>
        /// <param name="Name">A custom name for the <see cref="SerialCOMData"/>.</param>
        public SerialCOMData(Parity Parity, StopBits StopBits, int WriteTimeout, int ReadTimeout, int Baud, int DataBits, string Name = "") {
            this._Parity = Parity;
            this._StopBits = StopBits;
            this._WriteTimeout = WriteTimeout;
            this._ReadTimeout = ReadTimeout;
            this._Baud = Baud;
            this._DataBits = DataBits;
            this._Name = Name;
        }

        /// <summary>
        /// Generates a string with information about the <see cref="SerialCOMData"/> instance.
        /// </summary>
        /// <returns>A string with information about the <see cref="SerialCOMData"/> instance.</returns>
        public override string ToString()
        {
            return String.Format("Parity: {0}, StopBits: {1}, WriteTimeout: {2}, ReadTimeout: {3}, Baud: {4}, DataBits: {5}, Name: {6}",
                this.Parity,
                this.StopBits,
                this.WriteTimeout,
                this.ReadTimeout,
                this.Baud,
                this.DataBits,
                this.Name
            );
        }
        #endregion
    }
}
