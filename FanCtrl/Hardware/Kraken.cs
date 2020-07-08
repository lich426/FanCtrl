using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading;

using System.IO;
using Newtonsoft.Json.Linq;
using HidSharp;

namespace FanCtrl
{
    public class Kraken : USBDevice
    {
        private const long SEND_DELAY_TIME = 5000;

        private int mLastLiquidTemp = 0;
        private int mLastFanRPM = 0;
        private int mLastPumpRPM = 0;

        private int mPumpSpeed = 50;
        private int mFanPercent = 25;

        private int mLastPumpSpeed = 0;
        private int mLastFanPercent = 0;

        private long mPumpLastSendTime = 0;
        private long mFanLastSendTime = 0;        

        public Kraken() : base(USBDeviceType.Kraken)
        {
            
        }

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
            if (mUSBController.ProductID == USBProductID.KrakenX3)
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

        public bool start(uint index, USBProductID productID)
        {
            Monitor.Enter(mLock);

            if (index == 0)
            {
                mFileName = "Kraken.json";
            }
            else
            {
                mFileName = string.Format("Kraken{0}.json", index + 1);
            }

            mPumpSpeed = 50;
            mFanPercent = 25;
            mLastPumpSpeed = 0;
            mLastFanPercent = 0;

            mPumpLastSendTime = 0;
            mFanLastSendTime = 0;

            mUSBController = new HidUSBController(USBVendorID.NZXT, productID);
            mUSBController.onRecvHandler += onRecv;
            if(mUSBController.start(index) == false)
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

        public void stop()
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
                        int temp = (int)Math.Round(recvArray[15] + (recvArray[16] * 0.1));
                        int pump = (int)(recvArray[18] << 8 | recvArray[17]);

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
                    if (recvDataSize >= 7)
                    {
                        int temp = (int)Math.Round(recvArray[1] + (recvArray[2] * 0.1));
                        int pump = (int)(recvArray[5] << 8 | recvArray[6]);
                        int fan = (int)(recvArray[3] << 8 | recvArray[4]);

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
                if (mPumpSpeed != mLastPumpSpeed || mPumpLastSendTime == 0 || startTime - mPumpLastSendTime >= SEND_DELAY_TIME)
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
                    (mFanPercent != mLastFanPercent || mFanLastSendTime == 0 || startTime - mFanLastSendTime >= SEND_DELAY_TIME))
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
                var jsonString = File.ReadAllText(mFileName);
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
                File.WriteAllText(mFileName, rootObject.ToString());
            }
            catch { }
            Monitor.Exit(mLock);
        }

        public int getPumpSpeed()
        {
            Monitor.Enter(mLock);
            int speed = mLastPumpRPM;
            Monitor.Exit(mLock);
            return speed;
        }

        public void setPumpSpeed(int speed)
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

        public int getFanSpeed()
        {
            Monitor.Enter(mLock);
            int speed = mLastFanRPM;
            Monitor.Exit(mLock);
            return speed;
        }

        public void setFanSpeed(int percent)
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

        public int getLiquidTemp()
        {
            Monitor.Enter(mLock);
            int temp = mLastLiquidTemp;
            Monitor.Exit(mLock);
            return temp;
        }
    }
}
