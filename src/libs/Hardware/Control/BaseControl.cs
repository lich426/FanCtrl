using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace FanCtrl
{
    public class BaseControl : BaseDevice
    {
        // Value
        public int Value { get; set; }

        public int NextValue { get; set; }

        public bool IsSetSpeed { get; set; }

        // timer timeout (ms)
        public int Timeout { get; set; }

        private System.Timers.Timer mTimer;
        private object mTimerLock = new object();
        private int mTimerValue;

        public BaseControl(LIBRARY_TYPE type)
        {
            Type = type;
            Value = 0;
            NextValue = 0;
            IsSetSpeed = false;
            mTimerValue = 0;
        }

        ~BaseControl()
        {
            this.stopTimer();
        }

        public virtual int getMinSpeed()
        {
            return 0;
        }

        public virtual int getMaxSpeed()
        {
            return 100;
        }

        public virtual void setSpeed(int value)
        {

        }        

        public virtual void setAuto()
        {
            
        }

        public void setSpeedWithTimer(int value, int time)
        {
            if (time <= 0)
            {
                mTimerValue = value;
                this.setSpeed(value);
                return;
            }

            if (Value == value)
            {
                Monitor.Enter(mTimerLock);
                mTimerValue = value;
                Timeout = 0;
                mTimer?.Stop();
                Monitor.Exit(mTimerLock);
            }
            else if (mTimerValue != value)
            {
                Console.WriteLine("BaseControl.setSpeedWithTimer() : value({0}), mTimerValue({1})", value, mTimerValue);
                mTimerValue = value;
                Timeout = 0;

                Monitor.Enter(mTimerLock);
                mTimer?.Stop();
                Monitor.Exit(mTimerLock);
                
                mTimer = new System.Timers.Timer();
                mTimer.Interval = time;
                mTimer.Elapsed += (sender, e) =>
                {
                    Monitor.Enter(mTimerLock);
                    Console.WriteLine("BaseControl.setSpeedWithTimer() : setSpeed()");
                    this.setSpeed(value);
                    mTimer?.Stop();
                    Monitor.Exit(mTimerLock);
                };
                mTimer.Start();
            }
        }

        public void stopTimer()
        {
            Monitor.Enter(mTimerLock);
            mTimer?.Stop();
            Monitor.Exit(mTimerLock);
        }
    }
}
