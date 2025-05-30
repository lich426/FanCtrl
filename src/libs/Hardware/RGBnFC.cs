using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading;

using System.IO;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace FanCtrl
{
    public class RGBnFC : USBDevice
    {
        public const int MAX_FAN_COUNT = 3;
        private const long SEND_DELAY_TIME = 5000;

        private int[] mLastFanRPM = new int[MAX_FAN_COUNT];

        private int[] mFanPercent = new int[MAX_FAN_COUNT];
        private int[] mLastFanPercent = new int[MAX_FAN_COUNT];

        private long mFanLastSendTime = Util.getNowMS();
        
        public RGBnFC() : base(USBDeviceType.RGBnFC)
        {
            for (int i = 0; i < MAX_FAN_COUNT; i++)
            {
                mLastFanRPM[i] = 0;
                mFanPercent[i] = 20;
                mLastFanPercent[i] = 20;
            }
        }

        public bool start(uint index, USBProductID productID)
        {
            Monitor.Enter(mLock);

            if (index == 0)
            {
                mFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "RGBnFC.json";
            }
            else
            {
                mFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + string.Format("RGBnFC{0}.json", index + 1);
            }

            // Support both V1 (0x2009) and V2 (0x2019) devices
            mUSBController = new HidUSBController(USBVendorID.NZXT, productID);
            mUSBController.onRecvHandler += onRecv;
            if (mUSBController.start(index) == false)
            {
                Monitor.Exit(mLock);
                this.stop();
                return false;
            }

            if (this.readFile() == true)
            {
                mIsSendCustomData = (mCustomDataList.Count > 0);
            }

            // initialize
            this.initialize();

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

        private void initialize()
        {
            var command = new byte[] { 0x10, 0x01 };
            mUSBController.send(command);

            command = new byte[] { 0x20, 0x03 };
            mUSBController.send(command);

            command = new byte[] { 0x60, 0x03 };
            mUSBController.send(command);

            command = new byte[] { 0x60, 0x02, 0x01, 0xe8, 0x03, 0x01, 0xe8, 0x03 };
            mUSBController.send(command);

            if(mIsSendCustomData == false)
            {
                command = new byte[] { 0x28, 0x03, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                mUSBController.send(command);

                command = new byte[] { 0x28, 0x03, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
                mUSBController.send(command);
            }
        }

        private void onTimer(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(mLock) == false)
                return;

            try
            {
                long startTime = Util.getNowMS();

                // fan
                if (mLastFanPercent[0] != mFanPercent[0] ||
                    mLastFanPercent[1] != mFanPercent[1] ||
                    mLastFanPercent[2] != mFanPercent[2] ||
                    mFanLastSendTime == 0 || startTime - mFanLastSendTime >= SEND_DELAY_TIME)
                {
                    mFanLastSendTime = startTime;
                    mLastFanPercent[0] = mFanPercent[0];
                    mLastFanPercent[1] = mFanPercent[1];
                    mLastFanPercent[2] = mFanPercent[2];

                    var commandList = new List<byte>();
                    commandList.Add(0x62);
                    commandList.Add(0x01);
                    commandList.Add(0x07);
                    for (int i = 0; i < MAX_FAN_COUNT; i++)
                    {
                        commandList.Add(Convert.ToByte(mFanPercent[i]));
                    }
                    mUSBController.send(commandList.ToArray());
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

        private void onRecv(byte[] recvArray, int recvDataSize)
        {
            if (Monitor.TryEnter(mLock) == false)
                return;

            try
            {
                if (recvArray[0] == 0x67 && recvArray[1] == 0x02)
                {
                    int rpm = (int)(recvArray[25] << 8 | recvArray[24]);
                    if (rpm > 0 && rpm < 10000)
                    {
                        mLastFanRPM[0] = rpm;
                    }

                    rpm = (int)(recvArray[27] << 8 | recvArray[26]);
                    if (rpm > 0 && rpm < 10000)
                    {
                        mLastFanRPM[1] = rpm;
                    }

                    rpm = (int)(recvArray[29] << 8 | recvArray[28]);
                    if (rpm > 0 && rpm < 10000)
                    {
                        mLastFanRPM[2] = rpm;
                    }
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
        
        public int getMinFanSpeed(int index)
        {
            return 20;
        }

        public int getMaxFanSpeed(int index)
        {
            return 100;
        }

        public int getFanSpeed(int index)
        {
            Monitor.Enter(mLock);
            int speed = mLastFanRPM[index];
            Monitor.Exit(mLock);
            return speed;
        }

        public void setFanSpeed(int index, int percent)
        {
            Monitor.Enter(mLock);
            mFanPercent[index] = percent;
            Monitor.Exit(mLock);
        }
    }
}
