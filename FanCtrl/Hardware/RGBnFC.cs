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
    public class RGBnFC
    {
        private const string cFileName = "RGBnFC.json";
        public const int cMaxFanCount = 3;
        private const long cSendDelayTime = 5000;

        private object mLock = new object();
        private System.Timers.Timer mTimer = new System.Timers.Timer();

        private USBController mUSBController = null;

        private int[] mLastFanRPM = new int[cMaxFanCount];

        private int[] mFanPercent = new int[cMaxFanCount];
        private int[] mLastFanPercent = new int[cMaxFanCount];

        private long mFanLastSendTime = Util.getNowMS();

        private bool mIsSendCustomData = false;
        private List<byte[]> mCustomDataList = new List<byte[]>();

        public RGBnFC()
        {
            for (int i = 0; i < cMaxFanCount; i++)
            {
                mLastFanRPM[i] = 0;
                mFanPercent[i] = 20;
                mLastFanPercent[i] = 20;
            }
        }

        public bool start()
        {
            Monitor.Enter(mLock);

            mUSBController = new HidUSBController(USBVendorID.NZXT, USBProductID.RGBAndFanController);

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
                    mFanLastSendTime == 0 || startTime - mFanLastSendTime >= cSendDelayTime)
                {
                    mFanLastSendTime = startTime;
                    mLastFanPercent[0] = mFanPercent[0];
                    mLastFanPercent[1] = mFanPercent[1];
                    mLastFanPercent[2] = mFanPercent[2];

                    var commandList = new List<byte>();
                    commandList.Add(0x62);
                    commandList.Add(0x01);
                    commandList.Add(0x07);
                    for (int i = 0; i < cMaxFanCount; i++)
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

        private bool readFile()
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

        public void writeFile()
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
        
        public void setCustomDataList(List<string> hexStringList)
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

        public List<string> getCustomDataList()
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
