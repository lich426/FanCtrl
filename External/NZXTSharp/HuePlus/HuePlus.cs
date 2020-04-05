/*
    HuePlus.cs facilitates interaction with NZXT's Hue+ RGB Controller
    Copyright (C) 2018  Ari Madian

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.IO.Ports;
using System.Linq;

using NZXTSharp.Exceptions;

using NZXTSharp.COM;

// ReSharper disable InconsistentNaming
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace NZXTSharp.HuePlus
{

    /// <summary>
    /// Triggers when a generic log message is sent.
    /// </summary>
    public delegate void LogHandler(string message);

    /// <summary>
    /// Triggers when data is received from the COM port.
    /// </summary>
    public delegate void DataRecieved(string message);

    /// <summary>
    /// Represents an NZXT Hue+ lighting controller.
    /// </summary>
    public class HuePlus : INZXTDevice
    {
        #region Fields
        internal DeviceLoadFilter[] LoadFilters = new DeviceLoadFilter[]
        {
            DeviceLoadFilter.All,
            DeviceLoadFilter.LightingControllers,
            DeviceLoadFilter.Hue,
            DeviceLoadFilter.HuePlus
        };

        private readonly string _CustomName = null;
        private readonly int _MaxHandshakeRetry = 5;

        private SerialController _COMController;

        private HuePlusChannel _Both;
        private HuePlusChannel _Channel1;
        private HuePlusChannel _Channel2;
        private List<HuePlusChannel> _Channels;

        private Version _FirmwareVersion;
        #endregion

        #region Properties
        /// <summary>
        /// The device's product name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// A <see cref="HuePlusChannel"/> object representing both channels on the <see cref="HuePlus"/>.
        /// </summary>
        public HuePlusChannel Both { get => _Both; }

        /// <summary>
        /// A <see cref="HuePlusChannel"/> object representing the Channel 1 of the <see cref="HuePlus"/> device.
        /// </summary>
        public HuePlusChannel Channel1 { get => _Channel1; }

        /// <summary>
        /// A <see cref="HuePlusChannel"/> object representing the Channel 2 of the <see cref="HuePlus"/> device.
        /// </summary>
        public HuePlusChannel Channel2 { get => _Channel2; }

        /// <summary>
        /// A <see cref="List{T}"/> containing all <see cref="HuePlusChannel"/> objects owned by the <see cref="HuePlus"/> device.
        /// </summary>
        public List<HuePlusChannel> Channels { get => _Channels; }

        /// <summary>
        /// A custom name for the <see cref="HuePlus"/> instance.
        /// </summary>
        public string CustomName { get; set; }

        /// <summary>
        /// The <see cref="NZXTDeviceType"/> of the <see cref="HuePlus"/> object.
        /// </summary>
        public NZXTDeviceType Type { get => NZXTDeviceType.HuePlus; }

        /// <inheritdoc/>
        public int ID { get => 0x11111111; }

        /// <summary>
        /// The firmware version of the <see cref="HuePlus"/> device.
        /// </summary>
        public Version FirmwareVersion { get => _FirmwareVersion; }
        
        #endregion


        /// <summary>
        /// Triggers when a generic log message is sent.
        /// </summary>
        public event LogHandler OnLogMessage;

        /// <summary>
        /// Constructs a <see cref="HuePlus"/> instance.
        /// </summary>
        public HuePlus()
        {
            Initialize();
        }

        /// <summary>
        /// Constructs a <see cref="HuePlus"/> instance with a custom <paramref name="MaxHandshakeRetry"/> count,
        /// and a custom name <paramref name="CustomName"/>.
        /// </summary>
        /// <param name="MaxHandshakeRetry"></param>
        /// <param name="CustomName">A custom name for the <see cref="HuePlus"/> instance.</param>
        public HuePlus(int MaxHandshakeRetry = 5, string CustomName = null)
        {
            if (MaxHandshakeRetry <= 0)
                throw new InvalidParamException("Invalid MaxHandshakeRetry may not be less than or equal to 0.");

            this._MaxHandshakeRetry = MaxHandshakeRetry;
            this._CustomName = CustomName;
            Initialize();
        }

        private bool Initialize()
        {
            SerialCOMData data = new SerialCOMData(
                Parity.None,
                StopBits.One,
                1000,
                1000,
                256000,
                8,
                "HuePlus"
            );

            _COMController = new SerialController
            (
                SerialPort.GetPortNames(),
                data
            );

            if (_COMController.IsOpen)
            {
                int Retries = 0;

                while (true)
                {

                    if (_COMController.Write(new byte[1] { 0xc0 }, 1).FirstOrDefault() == 1)
                    {
                        SendLogEvent("Handshake Response Good");
                        break;
                    } else
                    {
                        Retries++;

                        Thread.Sleep(50);

                        if (Retries >= _MaxHandshakeRetry)
                            throw new MaxHandshakeRetryExceededException(_MaxHandshakeRetry);
                    }
                }

                InitializeChannels();
                InitializeChannelInfo();

                Channel1.BuildSubDevices();
                Channel2.BuildSubDevices();

                InitializeDeviceInfo();

                return true;
            }
            else { /*Logger.Error("Could not connect to serial port");*/ return false; }
        }

        private void InitializeChannels()
        {
            SendLogEvent("Initializing Channels");
            this._Both = new HuePlusChannel(0x00, Parent: this);
            this._Channel1 = new HuePlusChannel(0x01, Parent: this);
            this._Channel2 = new HuePlusChannel(0x02, Parent: this);
            this._Channels = new List<HuePlusChannel>() { _Both, _Channel1, _Channel2 };
        }

        private void InitializeChannelInfo()
        {
            UpdateChannelInfo(this._Both);
        }

        private void InitializeDeviceInfo()
        {
            this._FirmwareVersion = GetFirmwareVersion();
        }

        /// <summary>
        /// Disposes of and reconnects to the device's <see cref="SerialController"/>.
        /// </summary>
        public void Reconnect()
        {
            _COMController.Dispose();

            Initialize();
            InitializeChannels();
        }

        /// <summary>
        /// Disposes of the device's <see cref="SerialController"/>.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public void Dispose()
        {
            _COMController.Dispose();
        }

        /// <summary>
        /// Applies an <paramref name="Effect"/> to both channels.
        /// </summary>
        /// <param name="Effect">An <see cref="IEffect"/>.</param>
        public void ApplyEffect(IEffect Effect)
        {
            ApplyEffect(this.Both, Effect);
        }

        /// <summary>
        /// Applies the given <paramref name="effect"/> to the given <paramref name="channel"/>.
        /// </summary>
        /// <param name="channel">The <see cref="HuePlusChannel"/> to apply the effect to.</param>
        /// <param name="effect">The <see cref="IEffect"/> to apply.</param>
        /// <param name="SaveToChannel">Whether or not to save the given effect to the given channel.</param>
        public void ApplyEffect(HuePlusChannel channel, IEffect effect, bool SaveToChannel = true)
        {
            if (!effect.IsCompatibleWith(Type))
                throw new IncompatibleEffectException(Type.ToString(), effect.EffectName);




            List<byte[]> commandQueue = new List<byte[]>();

            // TODO : Improve this, not elegant.
            if (channel == this._Both) // If both channels, build and send bytes for both individually
            {
                foreach (byte[] arr in effect.BuildBytes(Type, this._Channel1))
                {
                    commandQueue.Add(arr);
                }

                foreach (byte[] arr in effect.BuildBytes(Type, this._Channel2))
                {
                    commandQueue.Add(arr);
                }

                if (SaveToChannel)
                {
                    _Channel1.UpdateEffect(effect);
                    _Channel2.UpdateEffect(effect);
                }
            }
            else // Otherwise, just build for the selected channel
            {
                commandQueue = effect.BuildBytes(Type, channel);
            }


            if (SaveToChannel) { channel.UpdateEffect(effect); }
            effect.Channel = channel;

            foreach (byte[] command in commandQueue) // Send command buffer
            {
                _COMController.WriteNoReponse(command);
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Writes a custom <paramref name="Buffer"/> to the device's <see cref="SerialController"/>.
        /// </summary>
        /// <param name="Buffer">The buffer to write to the device.</param>
        public void ApplyCustom(byte[] Buffer)
        {
            _COMController.WriteNoReponse(Buffer);
        }

        /// <summary>
        /// Turns the device's unit led on.
        /// </summary>
        public void UnitLedOn()
        {
            byte[] commandBytes = new byte[] { 0x46, 0x00, 0xc0, 0x00, 0x00, 0x00, 0xff };
            ApplyCustom(commandBytes);
        }

        /// <summary>
        /// Turns the device's unit led off.
        /// </summary>
        public void UnitLedOff()
        {
            byte[] commandBytes = new byte[] { 0x46, 0x00, 0xc0, 0x00, 0x00, 0xff, 0x00 };
            ApplyCustom(commandBytes);
        }

        /// <summary>
        /// Sets the device's unit led state. true: on, false: off.
        /// </summary>
        /// <param name="State">Which state to set the LED to; true: on, false: off.</param>
        public void SetUnitLed(bool State)
        {
            if (State)
            {
                UnitLedOn();
            } else
            {
                UnitLedOff();
            }
        }

        /// <summary>
        /// Gets the <see cref="HuePlus"/> device's firmware version.
        /// Based on the method found in a decompile of CAM.
        /// </summary>
        /// <returns>A <see cref="System.Version"/> object.</returns>
        public Version GetFirmwareVersion()
        {
            byte[] reply = _COMController.Write(new byte[] { 0x8c, 0 }, 5);
            int Major = reply[0] - 0xc0;
            int Minor = Convert.ToInt32(reply[2].ToString() + reply[3].ToString());
            return new Version(Major, Minor);
        }

        private void SendLogEvent(string Message)
        {
            var baseString = "NZXTSharp HuePlus " + (this.CustomName ?? "") + " - ";
            OnLogMessage?.Invoke(baseString + Message);
        }

        private void SendDataRecvd(string Message)
        {
            var baseString = "NZXTSharp HuePlus " + (this.CustomName ?? "") + " - ";
            //OnDataReceived?.Invoke(baseString + Message);
        }

        /// <summary>
        /// Updates the given <see cref="HuePlusChannel"/>'s <see cref="HuePlusChannelInfo"/>.
        /// </summary>
        /// <param name="Channel"></param>
        public void UpdateChannelInfo(HuePlusChannel Channel)
        {
            UpdateChannelInfoOp(this._Channel1);
            UpdateChannelInfoOp(this._Channel2);
        }

        private void UpdateChannelInfoOp(HuePlusChannel channel)
        {
            _COMController.Port.DiscardInBuffer(); //This will have to be removed later
            _COMController.Port.DiscardOutBuffer(); //This will have to be removed later
            channel.SetChannelInfo(new HuePlusChannelInfo(channel, _COMController.Write(new byte[] { 0x8d, (byte)channel }, 5)));
        }
    }
}
