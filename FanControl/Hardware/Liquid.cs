using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading;

namespace FanControl
{
    public enum LiquidCoolerType
    {
        Kraken = 0,
        CLC,
    };

    public class Liquid
    {
        protected object mLock = new object();
        
        public Liquid(LiquidCoolerType type)
        {
            CoolerType = type;
        }

        public LiquidCoolerType CoolerType { get; }

        public USBProductID ProductID { get; set;}

        public virtual int getMinFanSpeed()
        {
            return 0;
        }

        public virtual int getMaxFanSpeed()
        {
            return 100;
        }

        public virtual int getMinPumpSpeed()
        {
            return 0;
        }

        public virtual int getMaxPumpSpeed()
        {
            return 100;
        }   

        public virtual bool start(USBProductID productID)
        {
            return false;
        }

        public virtual void stop()
        {

        }

        protected virtual bool readFile()
        {
            return false;
        }

        public virtual void writeFile()
        {
            
        }

        public virtual void setCustomDataList(List<string> hexStringList)
        {
            
        }

        public virtual List<string> getCustomDataList()
        {
            return null;
        }

        public virtual int getPumpSpeed()
        {
            return 0;
        }

        public virtual void setPumpSpeed(int speed)
        {

        }

        public virtual int getFanSpeed()
        {
            return 0;
        }

        public virtual void setFanSpeed(int percent)
        {

        }

        public virtual int getLiquidTemp()
        {
            return 0;
        }
    }
}
