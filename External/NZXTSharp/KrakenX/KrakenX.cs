/*
KrakenX.cs
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

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading;

using NZXTSharp;
using NZXTSharp.COM;
using NZXTSharp.Exceptions;

using HidLibrary;

namespace NZXTSharp.KrakenX
{
    /// <summary>
    /// Which thread to stop.
    /// </summary>
    public enum OverrideThread
    {
        /// <summary>
        /// Stop the fan override thread.
        /// </summary>
        Fan = 0,

        /// <summary>
        /// Stop the pump override thread.
        /// </summary>
        Pump = 1,
    }

    /// <summary>
    /// Thread stop types.
    /// </summary>
    public enum ThreadStopType
    {
        /// <summary>
        /// If you need the override thread to stop immediately.
        /// Best/ safest method is to use <see cref="ThreadStopType.Flag"/>.
        /// </summary>
        Abort = 0,

        /// <summary>
        /// Stops the override thread with a flag. This is the best/ safest method, 
        /// but may take up to 20 seconds to take effect.
        /// </summary>
        Flag = 1,
    }


    /// <summary>
    /// Represents an NZXT KrakenX device.
    /// </summary>
    public class KrakenX : INZXTDevice
    {
        #region Fields and Properties
        internal DeviceLoadFilter[] LoadFilters = new DeviceLoadFilter[]
        {
            DeviceLoadFilter.All,
            DeviceLoadFilter.Coolers,
            DeviceLoadFilter.Kraken,
            DeviceLoadFilter.KrakenX
        };

        private KrakenXChannel _Both;
        private KrakenXChannel _Logo;
        private KrakenXChannel _Ring;
        private Version _FirmwareVersion;

        private object Lock = new object();

        private Thread PumpOverrideThread;
        private bool StopPumpOverrideLoop = false;

        private Thread FanOverrideThread;
        private bool StopFanOverrideLoop = false;

        private USBController _COMController;

        /// <summary>
        /// The <see cref="HIDDeviceID"/> of the <see cref="KrakenX"/> device. Will always be <see cref="HIDDeviceID.KrakenX"/>.
        /// </summary>
        public HIDDeviceID DeviceID { get => HIDDeviceID.KrakenX; }

        /// <summary>
        /// The <see cref="NZXTDeviceType"/> of the <see cref="KrakenX"/> device. Will always be <see cref="NZXTDeviceType.KrakenX"/>.
        /// </summary>
        public NZXTDeviceType Type { get => NZXTDeviceType.KrakenX; }

        /// <inheritdoc/>
        public int ID { get => 0x170e; }

        /// <summary>
        /// Represents both the <see cref="Logo"/>, and <see cref="Ring"/> channels.
        /// </summary>
        public KrakenXChannel Both { get => _Both; }

        /// <summary>
        /// Represents the <see cref="KrakenX"/>'s logo RGB channel.
        /// </summary>
        public KrakenXChannel Logo { get => _Logo; }
        
        /// <summary>
        /// Represents the <see cref="KrakenX"/>'s ring RGB channel.
        /// </summary>
        public KrakenXChannel Ring { get => _Ring; }

        /// <summary>
        /// The <see cref="KrakenX"/> device's firmware version.
        /// </summary>
        public Version FirmwareVersion { get => _FirmwareVersion; }
        
        #endregion

        /// <summary>
        /// Constructs an instance of a <see cref="KrakenX"/> device.
        /// </summary>
        public KrakenX()
        {
            Initialize();
        }

        #region Methods
        private void Initialize()
        {
            InitializeChannels();
            _COMController = new USBController(Type);
            InitializeDeviceInfo();
        }

        private void InitializeChannels()
        {
            _Both = new KrakenXChannel(0x00, this);
            _Logo = new KrakenXChannel(0x01, this);
            _Ring = new KrakenXChannel(0x02, this);
        }

        private void InitializeDeviceInfo()
        {
            this._FirmwareVersion = GetFirmwareVersion();
        }

        /// <summary>
        /// Writes a custom <paramref name="Buffer"/> to the <see cref="KrakenX"/> device.
        /// </summary>
        /// <param name="Buffer"></param>
        public void WriteCustom(byte[] Buffer)
        {
            _COMController.Write(Buffer);
        }

        /// <summary>
        /// Stops the thread overriding a given <paramref name="Thread"/>, using a given <paramref name="StopType"/>.
        /// </summary>
        /// <param name="Thread">Which <see cref="OverrideThread"/> to stop.</param>
        /// <param name="StopType">How to stop the given <paramref name="Thread"/>.</param>
        public void StopOverrideThread(OverrideThread Thread, ThreadStopType StopType = ThreadStopType.Flag)
        {
            switch (Thread) // Which thread to stop
            {
                case OverrideThread.Fan: 
                    if (StopType == ThreadStopType.Abort) {
                        FanOverrideThread?.Abort();
                    }
                    else if (StopType == ThreadStopType.Flag) {
                        StopFanOverrideLoop = true;
                    }
                    break;
                case OverrideThread.Pump:
                    if (StopType == ThreadStopType.Abort){
                        PumpOverrideThread?.Abort();
                    }
                    else if (StopType == ThreadStopType.Flag){
                        StopPumpOverrideLoop = true;
                    }
                    break;
            }
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
        /// Applies a given <see cref="IEffect"/> <paramref name="Effect"/> to a given 
        /// <see cref="KrakenXChannel"/> <paramref name="Channel"/>.
        /// </summary>
        /// <param name="Channel"></param>
        /// <param name="Effect"></param>
        /// <param name="ApplyToChannel">Whether or not to apply <paramref name="Effect"/>
        /// to the <paramref name="Channel"/> as its last applied effect.</param>
        public void ApplyEffect(KrakenXChannel Channel, IEffect Effect, bool ApplyToChannel = true)
        {
            if (!Effect.IsCompatibleWith(Type)) // If the effect is not compatible with a KrakenX
                throw new IncompatibleEffectException("KrakenX", Effect.EffectName);

            if (ApplyToChannel)
            {
                if (Channel.ChannelByte == 0x00)
                {
                    _Both.UpdateEffect(Effect);
                    _Logo.UpdateEffect(Effect);
                    _Ring.UpdateEffect(Effect);
                }
                else if (Channel.ChannelByte == 0x01) Logo.UpdateEffect(Effect); 
                else if (Channel.ChannelByte == 0x02) Ring.UpdateEffect(Effect);
            }
            
            List<byte[]> CommandQueue = Effect.BuildBytes(Type, Channel);
            if (CommandQueue == null)
                throw new NullReferenceException("CommandQueue for ApplyEffect returned null.");
            
            foreach (byte[] Command in CommandQueue) 
            {
                if (Command.Length <= 0x41)
                {
                    _COMController.Write(Command);
                } else
                {
                    byte[] truncCommand = new byte[0x41];
                    Array.Copy(Command, truncCommand, truncCommand.Length);
                    _COMController.Write(truncCommand);
                }
            }
        }

        /// <summary>
        /// Gets the pump speed last reported by the <see cref="KrakenX"/> device.
        /// </summary>
        /// <returns>The last reported pump speed in RPM.</returns>
        public int GetPumpSpeed()
        {
            if (_COMController.LastReport != null)
            {
                HidReport report = _COMController.LastReport;
                return report.Data[4] << 8 | report.Data[5];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Sets the <see cref="KrakenX"/>'s pump speed to a given percent or RPM.
        /// </summary>
        /// <param name="Speed">
        /// The speed value to set. Must be 50-100 (inclusive).
        /// </param>
        public void SetPumpSpeed(int Speed)
        {
            Monitor.Enter(Lock);
            if (PumpOverrideThread != null)
            {
                StopPumpOverrideLoop = true;
                PumpOverrideThread.Join();
                PumpOverrideThread = null;
            }

            if (Speed > 100)        Speed = 100;
            else if (Speed < 50)    Speed = 50;

            byte[] command = new byte[] { 0x02, 0x4d, 0x40, 0x00, Convert.ToByte(Speed) };
            this.StopPumpOverrideLoop = false;
            PumpOverrideThread = new Thread(new ParameterizedThreadStart(PumpSpeedOverrideLoop));
            PumpOverrideThread.Start(command);
            Monitor.Exit(Lock);
        }

        /// <summary>
        /// Gets the last fan speed reported by the <see cref="KrakenX"/> device.
        /// </summary>
        /// <returns>The last reported fan speed in RPM.</returns>
        public int GetFanSpeed()
        {
            if (_COMController.LastReport != null)
            {
                HidReport report = _COMController.LastReport;
                return report.Data[2] << 8 | report.Data[3];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Sets all fans connected to the <see cref="KrakenX"/> device to a given <paramref name="Percent"/>.
        /// </summary>
        /// <param name="Percent">The percentage to set the fans to. Must be 25-100 (inclusive).</param>
        public void SetFanSpeed(int Percent)
        {
            Monitor.Enter(Lock);
            if (FanOverrideThread != null)
            {
                StopFanOverrideLoop = true;
                FanOverrideThread.Join();
                FanOverrideThread = null;
            }

            if (Percent > 100)      Percent = 100;
            else if (Percent < 25)  Percent = 25;

            byte[] command = new byte[] { 0x02, 0x4d, 0x00, 0x00, Convert.ToByte(Percent) };
            this.StopFanOverrideLoop = false;
            FanOverrideThread = new Thread(new ParameterizedThreadStart(FanSpeedOverrideLoop));
            FanOverrideThread.Start(command);
            Monitor.Exit(Lock);
        }

        /// <summary>
        /// Gets the last reported liquid temp.
        /// </summary>
        /// <param name="AsFarenheit">Whether or not to return the temp value in degrees F.</param>
        /// <returns>The last reported liquid temp as a rounded integer, in degrees C.</returns>
        public int? GetLiquidTemp(bool AsFarenheit = false)
        {
            if (_COMController.LastReport != null)
            {
                HidReport report = _COMController.LastReport;
                double temp = (report.Data[0] + (report.Data[1] * 0.1));
                return AsFarenheit ? temp.Round().DegreesCtoF() : temp.Round();
            } else
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the last HID report received from the KrakenX device.
        /// </summary>
        /// <returns>An <see cref="HidReport"/>.</returns>
        public HidReport GetLastReport()
        {
            return _COMController.LastReport;
        }
        
        /// <summary>
        /// Gets the <see cref="KrakenX"/>'s firmware version.
        /// </summary>
        /// <returns>A <see cref="System.Version"/> object.</returns>
        public Version GetFirmwareVersion()
        {
            while (_COMController.LastReport == null)
            {
                Thread.Sleep(25);
            }

            HidReport report = _COMController.LastReport;
            int Major = report.Data[10];
            int Minor = report.Data[12].ConcatenateInt(report.Data[13]);
            return new Version(Major, Minor);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Monitor.Enter(Lock);
            StopPumpOverrideLoop = true;
            StopFanOverrideLoop = true;

            if (PumpOverrideThread != null)
            {
                PumpOverrideThread.Join();
                PumpOverrideThread = null;
            }

            if (FanOverrideThread != null)
            {
                FanOverrideThread.Join();
                FanOverrideThread = null;
            }
            Monitor.Exit(Lock);

            _COMController?.Dispose();

            _Both = null;
            _Logo = null;
            _Ring = null;
            _FirmwareVersion = null;
        }

        /// <inheritdoc/>
        public void Reconnect()
        {
            Dispose();
            InitializeChannels();
            Initialize();
        }

        /// <summary>
        /// Starts a thread that sends pump speed override packets.
        /// </summary>
        /// <param name="Buffer"></param>
        internal void PumpSpeedOverrideLoop(object Buffer)
        {
            while (!this.StopPumpOverrideLoop)
            {
                _COMController.Write((byte[])Buffer);

                long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                while (!this.StopPumpOverrideLoop)
                {
                    Thread.Sleep(100);
                    long nowTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    long delayTime = nowTime - startTime;
                    if (delayTime >= 5000)
                        break;
                }
            }
        }

        /// <summary>
        /// Starts a thread that starts fan speed override packets.
        /// </summary>
        /// <param name="Buffer"></param>
        internal void FanSpeedOverrideLoop(object Buffer)
        {
            while (!this.StopFanOverrideLoop)
            {
                _COMController.Write((byte[])Buffer);

                long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                while (!this.StopFanOverrideLoop)
                {
                    Thread.Sleep(100);
                    long nowTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    long delayTime = nowTime - startTime;
                    if (delayTime >= 5000)
                        break;
                }
            }
        }
        #endregion
    }
}
