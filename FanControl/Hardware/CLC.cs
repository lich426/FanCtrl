using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading;

using System.IO;
using Newtonsoft.Json.Linq;

namespace FanControl
{
    public class CLC : Liquid
    {
        private const byte ENDPOINT_IN = 0x82;
        private const byte ENDPOINT_OUT = 0x02;

        private const string cFileName = "CLC.json";

        private System.Timers.Timer mTimer = new System.Timers.Timer();

        private USBController mUSBController = null;

        private int mLastLiquidTemp = 0;
        private int mLastFanRPM = 0;
        private int mLastPumpRPM = 0;

        private int mPumpSpeed = 50;
        private int mFanPercent = 25;

        private int mLastPumpSpeed = 0;
        private int mLastFanPercent = 0;

        private bool mIsSendCustomData = false;
        private List<byte[]> mCustomDataList = new List<byte[]>();

        public CLC() : base(LiquidCoolerType.CLC) { }

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
            return 50;
        }

        public override int getMaxPumpSpeed()
        {
            return 100;
        }

        public override bool start(USBProductID productID)
        {
            Monitor.Enter(mLock);
            var vendorID = USBVendorID.ASETEK;
            ProductID = productID;

            // SiUSBController
            mUSBController = new SiUSBController(vendorID, productID);
            mUSBController.onRecvHandler += onRecv;
            if (mUSBController.start() == false)
            {
                // WinUSBController
                mUSBController = new WinUSBController(vendorID, productID, ENDPOINT_IN, ENDPOINT_OUT);
                mUSBController.onRecvHandler += onRecv;
                if (mUSBController.start() == false)
                {
                    Monitor.Exit(mLock);
                    this.stop();
                    return false;
                }
            }

            if (this.readFile() == true)
            {
                mIsSendCustomData = (mCustomDataList.Count > 0);
            }

            mTimer.Interval = 500;
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
                if(recvDataSize >= 15)
                {
                    int temp = (int)Math.Round(recvArray[10] + (recvArray[14] * 0.1));
                    int pump = (int)(recvArray[8] << 8 | recvArray[9]);
                    int fan = (int)(recvArray[0] << 8 | recvArray[1]);

                    if (temp > 0 && temp < 100 && pump > 0 && pump < 10000 && fan > 0 && fan < 10000)
                    {
                        mLastLiquidTemp = temp;
                        mLastPumpRPM = pump;
                        mLastFanRPM = fan;
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
                // ping
                var list = new List<byte[]>();
                var command = new byte[] { 0x14, 0x00, 0x00, 0x00 };
                list.Add(command);
                mUSBController.send(list);

                // pump
                if (mPumpSpeed != mLastPumpSpeed)
                {
                    mLastPumpSpeed = mPumpSpeed;

                    double pumpSpeed = Math.Ceiling((mPumpSpeed - 50) / 3.125 + 50);
                    var list2 = new List<byte[]>();
                    var command2 = new byte[] { 0x13, Convert.ToByte(pumpSpeed) };
                    list2.Add(command2);
                    mUSBController.send(list2);
                }

                // fan
                if (mFanPercent != mLastFanPercent)
                {
                    mLastFanPercent = mFanPercent;

                    var list3 = new List<byte[]>();
                    var commandList = new List<byte>();
                    commandList.Add(0x11);
                    commandList.Add(0x00);
                    commandList.Add(0x14);
                    commandList.Add(0x20);
                    commandList.Add(0x2a);
                    commandList.Add(0x33);
                    commandList.Add(0x39);
                    commandList.Add(0x3c);
                    for (int i = 0; i < 6; i++)
                    {
                        commandList.Add(Convert.ToByte(mFanPercent));
                    }
                    list3.Add(commandList.ToArray());
                    mUSBController.send(list3);
                }

                // lighting
                if (mIsSendCustomData == true && mCustomDataList.Count > 0)
                {
                    Console.WriteLine("CLC.onTimer() : send lighting -----------------------");
                    for (int i = 0; i < mCustomDataList.Count; i++)
                    {
                        Util.printHex(mCustomDataList[i].ToArray());
                    }
                    mUSBController.send(mCustomDataList);
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
                for (int i = 0; i < mCustomDataList.Count; i++)
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
            for (int i = 0; i < mCustomDataList.Count; i++)
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
