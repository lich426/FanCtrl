using System;
using System.Threading;

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

        private object mTimerLock = new object();
        private System.Timers.Timer mTimer;
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

        public void setSpeedWithTimer(int value)
        {
            Monitor.Enter(mTimerLock);
            int time = Timeout;
            Timeout = 0;

            if (time <= 0)
            {
                mTimerValue = value;
                mTimer?.Stop();
                this.setSpeed(value);
                Monitor.Exit(mTimerLock);
                return;
            }

            if (Value == value)
            {
                mTimer?.Stop();
                mTimerValue = value;
                Console.WriteLine("BaseControl.setSpeedWithTimer() : equal value({0})", value);
            }
            else if (mTimerValue != value)
            {
                Console.WriteLine("BaseControl.setSpeedWithTimer() : value({0}), mTimerValue({1})", value, mTimerValue);
                
                mTimer?.Stop();
                mTimerValue = value;

                mTimer = new System.Timers.Timer();
                mTimer.Interval = time;
                var timer = mTimer;
                mTimer.Elapsed += (sender, e) =>
                {
                    Monitor.Enter(mTimerLock);
                    if (timer.Enabled == false)
                    {
                        Monitor.Exit(mTimerLock);
                        return;
                    }

                    Console.WriteLine("BaseControl.setSpeedWithTimer() : setSpeed()");
                    this.setSpeed(value);
                    timer.Stop();
                    Monitor.Exit(mTimerLock);
                };
                mTimer.Start();
            }
            Monitor.Exit(mTimerLock);
        }

        public void checkTimer()
        {
            Monitor.Enter(mTimerLock);
            if (Value == NextValue && Value != mTimerValue)
            {
                mTimerValue = Value;
                mTimer?.Stop();                
                Console.WriteLine("BaseControl.checkTimer() : stop timer");
            }
            Monitor.Exit(mTimerLock);
        }

        public void stopTimer()
        {
            Monitor.Enter(mTimerLock);
            mTimer?.Stop();
            Monitor.Exit(mTimerLock);
        }
    }
}
