using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading;

using System.IO;
using Newtonsoft.Json.Linq;

namespace FanCtrl
{
    public class Kraken : Liquid
    {
        private const string cFileName = "Kraken.json";

        private System.Timers.Timer mTimer = new System.Timers.Timer();

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

        private USBController mUSBController = null;

        public Kraken() : base(LiquidCoolerType.Kraken) { }

        public override int getMinFanSpeed()
        {
            return 25;
        }

        public override int getMaxFanSpeed()
        {
            return 100;
        }

        public override int getMinPumpSpeed()
        {
            // X3
            if (mUSBController.ProductID == USBProductID.KrakenX3)
            {
                return 20;
            }

            // X2
            return 50;
        }

        public override int getMaxPumpSpeed()
        {
            return 100;
        }

        public override bool start(USBProductID productID)
        {
            Monitor.Enter(mLock);

            ProductID = productID;
            mPumpSpeed = 50;
            mFanPercent = 25;
            mLastPumpSpeed = 0;
            mLastFanPercent = 0;

            mPumpLastSendTime = 0;
            mFanLastSendTime = 0;

            mUSBController = new HidUSBController(USBVendorID.NZXT, ProductID);

            mUSBController.onRecvHandler += onRecv;
            if(mUSBController.start() == false)
            {
                Monitor.Exit(mLock);
                this.stop();
                return false;
            }

            if (this.readFile() == true)
            {
                mIsSendCustomData = (mCustomDataList.Count > 0);
            }

            mTimer.Interval = 1000;
            mTimer.Elapsed += onTimer;
            mTimer.Start();
            Monitor.Exit(mLock);
            return true;
        }

        public override void stop()
        {
            Monitor.Enter(mLock);
            mTimer.Stop();

            if (mUSBController != null)
            {
                mUSBController.stop();
            }
            Monitor.Exit(mLock);
        }

        private void onRecv(byte[] recvArray, int recvDataSize)
        {
            if (Monitor.TryEnter(mLock) == false)
                return;

            try
            {
                // X3
                if (mUSBController.ProductID == USBProductID.KrakenX3)
                {
                    if(recvDataSize >= 18)
                    {
                        int temp = (int)Math.Round(recvArray[14] + (recvArray[15] * 0.1));
                        int pump = (int)(recvArray[17] << 8 | recvArray[16]);

                        if (temp > 0 && temp < 100 && pump > 0 && pump < 10000)
                        {
                            mLastLiquidTemp = temp;
                            mLastPumpRPM = pump;
                        }
                    }                    
                }
                // X2
                else
                {
                    if (recvDataSize >= 6)
                    {
                        int temp = (int)Math.Round(recvArray[0] + (recvArray[1] * 0.1));
                        int pump = (int)(recvArray[4] << 8 | recvArray[5]);
                        int fan = (int)(recvArray[2] << 8 | recvArray[3]);

                        if (temp > 0 && temp < 100 && pump > 0 && pump < 10000 && fan > 0 && fan < 10000)
                        {
                            mLastLiquidTemp = temp;
                            mLastPumpRPM = pump;
                            mLastFanRPM = fan;
                        }
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
                long startTime = Util.getNowMS();

                // pump
                if (mPumpSpeed != mLastPumpSpeed || mPumpLastSendTime == 0 || startTime - mPumpLastSendTime >= mSendDelayTime)
                {
                    mLastPumpSpeed = mPumpSpeed;
                    mPumpLastSendTime = startTime;

                    // X3
                    if (mUSBController.ProductID == USBProductID.KrakenX3)
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
                        mUSBController.send(commandList.ToArray());
                    }

                    // X2
                    else
                    {
                        var command = new byte[] { 0x02, 0x4d, 0x40, 0x00, Convert.ToByte(mPumpSpeed) };
                        mUSBController.send(command);
                    }
                }

                // fan
                if ((mUSBController.ProductID == USBProductID.KrakenX2) &&
                    (mFanPercent != mLastFanPercent || mFanLastSendTime == 0 || startTime - mFanLastSendTime >= mSendDelayTime))
                {
                    mLastFanPercent = mFanPercent;
                    mFanLastSendTime = startTime;

                    var command = new byte[] { 0x02, 0x4d, 0x00, 0x00, Convert.ToByte(mFanPercent) };
                    mUSBController.send(command);
                }

                // lighting
                if (mIsSendCustomData == true && mCustomDataList.Count > 0)
                {
                    for (int i = 0; i < mCustomDataList.Count; i++)
                    {
                        mUSBController.send(mCustomDataList[i]);
                    }
                    mIsSendCustomData = false;
                }
            }
            catch { }
            Monitor.Exit(mLock);
        }

        protected override bool readFile()
        {
            try
            {
                var jsonString = File.ReadAllText(cFileName);
                var rootObject = JObject.Parse(jsonString);

                var listObject = rootObject.Value<JArray>("list");
                for(int i = 0; i < listObject.Count; i++)
                {
                    var hexString = listObject[i].Value<string>();
                    mCustomDataList.Add(Util.getHexBytes(hexString));
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override void writeFile()
        {
            Monitor.Enter(mLock);
            try
            {
                var rootObject = new JObject();
                var listObject = new JArray();
                for(int i = 0; i <mCustomDataList.Count; i++)
                {
                    string hexString = Util.getHexString(mCustomDataList[i]);
                    listObject.Add(hexString);
                }
                rootObject["list"] = listObject;
                File.WriteAllText(cFileName, rootObject.ToString());
            }
            catch { }
            Monitor.Exit(mLock);
        }

        public override void setCustomDataList(List<string> hexStringList)
        {
            Monitor.Enter(mLock);
            mCustomDataList.Clear();
            if (hexStringList.Count == 0)
            {
                Monitor.Exit(mLock);
                return;
            }            
            for (int i = 0; i < hexStringList.Count; i++)
            {
                mCustomDataList.Add(Util.getHexBytes(hexStringList[i]));
            }
            mIsSendCustomData = (mCustomDataList.Count > 0);
            Monitor.Exit(mLock);
        }

        public override List<string> getCustomDataList()
        {
            Monitor.Enter(mLock);
            var hexStringList = new List<string>();
            for(int i = 0; i < mCustomDataList.Count; i++)
            {
                hexStringList.Add(Util.getHexString(mCustomDataList[i]));
            }
            Monitor.Exit(mLock);
            return hexStringList;
        }

        public override int getPumpSpeed()
        {
            Monitor.Enter(mLock);
            int speed = mLastPumpRPM;
            Monitor.Exit(mLock);
            return speed;
        }

        public override void setPumpSpeed(int speed)
        {
            Monitor.Enter(mLock);
            if (speed > this.getMaxPumpSpeed())
            {
                speed = this.getMaxPumpSpeed();
            }
            else if (speed < this.getMinPumpSpeed())
            {
                speed = this.getMinPumpSpeed();
            }
            mPumpSpeed = speed;
            Monitor.Exit(mLock);
        }

        public override int getFanSpeed()
        {
            Monitor.Enter(mLock);
            int speed = mLastFanRPM;
            Monitor.Exit(mLock);
            return speed;
        }

        public override void setFanSpeed(int percent)
        {
            Monitor.Enter(mLock);
            if (percent > this.getMaxFanSpeed())
            {
                percent = this.getMaxFanSpeed();
            }
            else if (percent < this.getMinFanSpeed())
            {
                percent = this.getMinFanSpeed();
            }
            mFanPercent = percent;
            Monitor.Exit(mLock);
        }

        public override int getLiquidTemp()
        {
            Monitor.Enter(mLock);
            int temp = mLastLiquidTemp;
            Monitor.Exit(mLock);
            return temp;
        }
    }
}
