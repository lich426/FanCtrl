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
using System.IO;
using Newtonsoft.Json.Linq;

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
        private const string cKrakenFileName = "Kraken.json";

        #region Fields and Properties
        internal DeviceLoadFilter[] LoadFilters = new DeviceLoadFilter[]
        {
            DeviceLoadFilter.All,
            DeviceLoadFilter.Coolers,
            DeviceLoadFilter.Kraken,
            DeviceLoadFilter.KrakenX,
            DeviceLoadFilter.KrakenX3
        };

        private KrakenXChannel _Both;
        private KrakenXChannel _Logo;
        private KrakenXChannel _Ring;
        private Version _FirmwareVersion;

        private object mLock = new object();
        private System.Timers.Timer mTimer = null;

        private long mSendDelayTime = 5000;

        private int mLastLiquidTemp = 0;
        private int mLastFanRPM = 0;
        private int mLastPumpRPM = 0;

        private int mPumpSpeed = 50;
        private int mFanPercent = 25;
        private int mLastPumpSpeed = 0;
        private int mLastFanPercent = 0;

        private long mPumpLastSendTime = 0;
        private long mFanLastSendTime = 0;

        private bool mIsSendCustomData = false;
        private List<byte[]> mCustomDataList = new List<byte[]>();

        private USBController _COMController;

        private HidReport mLastReport = null;

        /// <summary>
        /// The <see cref="HIDDeviceID"/> of the <see cref="KrakenX"/> device. Will always be <see cref="HIDDeviceID.KrakenX"/>.
        /// </summary>
        //public HIDDeviceID DeviceID { get => HIDDeviceID.KrakenX; }

        /// <summary>
        /// The <see cref="NZXTDeviceType"/> of the <see cref="KrakenX"/> device. Will always be <see cref="NZXTDeviceType.KrakenX"/>.
        /// </summary>
        private NZXTDeviceType mType = NZXTDeviceType.KrakenX;
        public NZXTDeviceType Type { get => mType; }

        /// <inheritdoc/>
        private int mID = 0x170e;
        public int ID { get => mID; }

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
        
        public int getMinFanSpeed()
        {
            return 25;
        }

        public int getMaxFanSpeed()
        {
            return 100;
        }

        public int getMinPumpSpeed()
        {
            // X3
            if (mType == NZXTDeviceType.KrakenX3)
            {
                return 20;
            }

            // X2
            return 50;
        }

        public int getMaxPumpSpeed()
        {
            return 100;
        }

        #endregion

        /// <summary>
        /// Constructs an instance of a <see cref="KrakenX"/> device.
        /// </summary>
        public KrakenX(NZXTDeviceType type)
        {
            mType = type;
            switch (type)
            {
                case NZXTDeviceType.KrakenX:
                    mID = 0x170e;
                    mSendDelayTime = 5000;
                    break;

                case NZXTDeviceType.KrakenX3:
                    mID = 0x2007;
                    mSendDelayTime = 10000;
                    break;
                default:
                    throw new Exception();
            }
            Initialize();

            if(this.read() == true)
            {
                mIsSendCustomData = (mCustomDataList.Count > 0);
            }
        }

        #region Methods
        private void Initialize()
        {
            InitializeChannels();
            mLastReport = null;
            _COMController = new USBController(Type);
            _COMController.ReportCallback += onReport;
            _COMController.Initialize();
            InitializeDeviceInfo();

            mPumpSpeed = 50;
            mFanPercent = 25;
            mLastPumpSpeed = 0;
            mLastFanPercent = 0;

            mPumpLastSendTime = 0;
            mFanLastSendTime = 0;

            mTimer = new System.Timers.Timer();
            mTimer.Interval = 1000;
            mTimer.Elapsed += onTimer;
            mTimer.Start();
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

        public bool read()
        {
            Monitor.Enter(mLock);
            try
            {
                var jsonString = File.ReadAllText(cKrakenFileName);
                var rootObject = JObject.Parse(jsonString);

                var listObject = rootObject.Value<JArray>("list");
                for(int i = 0; i < listObject.Count; i++)
                {
                    var hexString = listObject[i].Value<string>();
                    mCustomDataList.Add(this.getHexBytes(hexString));
                }
            }
            catch
            {
                Monitor.Exit(mLock);
                return false;
            }
            Monitor.Exit(mLock);
            return true;
        }

        public void write()
        {
            Monitor.Enter(mLock);
            try
            {
                var rootObject = new JObject();
                var listObject = new JArray();
                for(int i = 0; i <mCustomDataList.Count; i++)
                {
                    string hexString = this.getHexString(mCustomDataList[i]);
                    listObject.Add(hexString);
                }
                rootObject["list"] = listObject;
                File.WriteAllText(cKrakenFileName, rootObject.ToString());
            }
            catch { }
            Monitor.Exit(mLock);
        }

        public void setCustomDataList(List<string> hexStringList)
        {
            Monitor.Enter(mLock);
            if (hexStringList.Count == 0)
            {
                Monitor.Exit(mLock);
                return;
            }
            mCustomDataList.Clear();
            for (int i = 0; i < hexStringList.Count; i++)
            {
                mCustomDataList.Add(this.getHexBytes(hexStringList[i]));
            }
            mIsSendCustomData = (mCustomDataList.Count > 0);
            Monitor.Exit(mLock);
        }

        public List<string> getCustomDataList()
        {
            Monitor.Enter(mLock);
            var hexStringList = new List<string>();
            for(int i = 0; i < mCustomDataList.Count; i++)
            {
                hexStringList.Add(this.getHexString(mCustomDataList[i]));
            }
            Monitor.Exit(mLock);
            return hexStringList;
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
            Monitor.Enter(mLock);
            int speed = mLastPumpRPM;
            Monitor.Exit(mLock);
            return speed;
        }

        /// <summary>
        /// Sets the <see cref="KrakenX"/>'s pump speed to a given percent or RPM.
        /// </summary>
        /// <param name="Speed">
        /// The speed value to set. Must be 50-100 (inclusive).
        /// </param>
        public void SetPumpSpeed(int speed)
        {
            Monitor.Enter(mLock);
            if (speed > this.getMaxPumpSpeed())         speed = this.getMaxPumpSpeed();
            else if (speed < this.getMinPumpSpeed())    speed = this.getMinPumpSpeed();
            mPumpSpeed = speed;
            Monitor.Exit(mLock);
        }

        /// <summary>
        /// Gets the last fan speed reported by the <see cref="KrakenX"/> device.
        /// </summary>
        /// <returns>The last reported fan speed in RPM.</returns>
        public int GetFanSpeed()
        {
            Monitor.Enter(mLock);
            int speed = mLastFanRPM;
            Monitor.Exit(mLock);
            return speed;
        }

        /// <summary>
        /// Sets all fans connected to the <see cref="KrakenX"/> device to a given <paramref name="Percent"/>.
        /// </summary>
        /// <param name="Percent">The percentage to set the fans to. Must be 25-100 (inclusive).</param>
        public void SetFanSpeed(int percent)
        {
            Monitor.Enter(mLock);
            if (percent > this.getMaxFanSpeed())        percent = this.getMaxFanSpeed();
            else if (percent < this.getMinFanSpeed())   percent = this.getMinFanSpeed();
            mFanPercent = percent;
            Monitor.Exit(mLock);
        }

        /// <summary>
        /// Gets the last reported liquid temp.
        /// </summary>
        /// <param name="AsFarenheit">Whether or not to return the temp value in degrees F.</param>
        /// <returns>The last reported liquid temp as a rounded integer, in degrees C.</returns>
        public int GetLiquidTemp()
        {
            Monitor.Enter(mLock);
            int temp = mLastLiquidTemp;
            Monitor.Exit(mLock);
            return temp;
        }
        
        /// <summary>
        /// Gets the <see cref="KrakenX"/>'s firmware version.
        /// </summary>
        /// <returns>A <see cref="System.Version"/> object.</returns>
        public Version GetFirmwareVersion()
        {
            int major = 0;
            int minor = 0;
            while (true)
            {
                Monitor.Enter(mLock);
                if(mLastReport == null)
                {
                    Monitor.Exit(mLock);
                    Thread.Sleep(25);
                    continue;
                }

                major = mLastReport.Data[10];
                minor = mLastReport.Data[12].ConcatenateInt(mLastReport.Data[13]);
                Monitor.Exit(mLock);
                break;
            }
            return new Version(major, minor);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Monitor.Enter(mLock);
            if(mTimer != null)
            {
                mTimer.Enabled = false;
                mTimer.Stop();
                mTimer = null;
            }            
            Monitor.Exit(mLock);

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

        private void onReport(object sender, EventArgs e)
        {
            Monitor.Enter(mLock);
            try
            {
                mLastReport = (HidReport)sender;

                if(this.Type == NZXTDeviceType.KrakenX)
                {
                    int temp = (int)Math.Round(mLastReport.Data[0] + (mLastReport.Data[1] * 0.1));
                    int pump = (int)(mLastReport.Data[4] << 8 | mLastReport.Data[5]);
                    int fan = (int)(mLastReport.Data[2] << 8 | mLastReport.Data[3]);

                    if(temp > 0 && temp < 100 && pump > 0 && pump < 10000 && fan > 0 && fan < 10000)
                    {
                        mLastLiquidTemp = temp;
                        mLastPumpRPM = pump;
                        mLastFanRPM = fan;
                    }
                }
                else
                {
                    int temp = (int)Math.Round(mLastReport.Data[14] + (mLastReport.Data[15] * 0.1));
                    int pump = (int)(mLastReport.Data[17] << 8 | mLastReport.Data[16]);

                    if (temp > 0 && temp < 100 && pump > 0 && pump < 10000)
                    {
                        mLastLiquidTemp = temp;
                        mLastPumpRPM = pump;
                    }
                }                
            }
            catch { }
            Monitor.Exit(mLock);
        }

        private void onTimer(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(mLock) == false)
                return;

            try
            {
                long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                // pump
                if (mPumpSpeed != mLastPumpSpeed || mPumpLastSendTime == 0 || startTime - mPumpLastSendTime >= mSendDelayTime)
                {
                    mLastPumpSpeed = mPumpSpeed;
                    mPumpLastSendTime = startTime;

                    if (this.Type == NZXTDeviceType.KrakenX)
                    {
                        var command = new byte[] { 0x02, 0x4d, 0x40, 0x00, Convert.ToByte(mPumpSpeed) };
                        _COMController.Write(command);
                    }
                    else
                    {
                        var commandList = new List<byte>();
                        commandList.Add(0x72);
                        commandList.Add(0x01);
                        commandList.Add(0x00);
                        commandList.Add(0x00);
                        for (int i = 0; i < 40; i++)
                        {
                            commandList.Add(Convert.ToByte(mPumpSpeed));
                        }
                        _COMController.Write(commandList.ToArray());
                    }                        
                }

                // fan
                if ((this.Type == NZXTDeviceType.KrakenX) &&
                    (mFanPercent != mLastFanPercent || mFanLastSendTime == 0 || startTime - mFanLastSendTime >= mSendDelayTime))
                {
                    mLastFanPercent = mFanPercent;
                    mFanLastSendTime = startTime;

                    var command = new byte[] { 0x02, 0x4d, 0x00, 0x00, Convert.ToByte(mFanPercent) };
                    _COMController.Write(command);
                }

                if(mIsSendCustomData == true && mCustomDataList.Count > 0)
                {
                    for(int i = 0; i < mCustomDataList.Count; i++)
                    {
                        _COMController.Write(mCustomDataList[i]);
                    }
                    mIsSendCustomData = false;
                }
            }
            catch { }
            Monitor.Exit(mLock);
        }

        private byte[] getHexBytes(string hexString)
        {
            try
            {
                int length = hexString.Length;
                var bytes = new byte[length / 2];
                for (int i = 0; i < length; i += 2)
                {
                    bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
                }
                return bytes;
            }
            catch {}
            return null;
        }

        private string getHexString(byte[] datas)
        {
            try
            {
                string hexString = string.Empty;
                hexString = string.Concat(Array.ConvertAll(datas, byt => byt.ToString("X2")));
                return hexString;
            }
            catch { }
            return "";           
        }

        #endregion
    }
}
