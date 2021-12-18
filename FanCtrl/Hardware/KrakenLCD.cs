using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class KrakenLCD : USBDevice
    {
        public KrakenLCD() : base(USBDeviceType.Kraken)
        {
        }

        public bool start(uint index, USBProductID productID)
        {
            Monitor.Enter(mLock);

            if (index == 0)
            {
                mFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "KrakenZ3.json";
            }
            else
            {
                mFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + string.Format("KrakenZ3-{0}.json", index + 1);
            }

            mUSBController = new LibUSBController(USBVendorID.NZXT, productID);
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
            Monitor.Exit(mLock);
            return true;
        }

        public void stop()
        {
            Monitor.Enter(mLock);
            if (mUSBController != null)
            {
                mUSBController.stop();
            }
            Monitor.Exit(mLock);
        }

        protected override bool readFile()
        {
            try
            {
                var jsonString = File.ReadAllText(mFileName);
                var rootObject = JObject.Parse(jsonString);

                var listObject = rootObject.Value<JArray>("list");
                for (int i = 0; i < listObject.Count; i++)
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
                File.WriteAllText(mFileName, rootObject.ToString());
            }
            catch { }
            finally 
            {
                Monitor.Exit(mLock);
            }
        }

        public override void send()
        {
            Monitor.Enter(mLock);
            try
            {
                if (mUSBController != null)
                {
                    if (mCustomDataList.Count == 0)
                    {
                        Monitor.Exit(mLock);
                        return;
                    }
                    mUSBController.send(mCustomDataList);
                }
            }
            catch { }
            Monitor.Exit(mLock);
        }
    }
}
