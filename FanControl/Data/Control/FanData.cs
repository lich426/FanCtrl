using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{    
    public class FanData
    {
        public const int MAX_FAN_VALUE_SIZE = 21;

        public int Index { get; set; }

        public string Name { get; set; }

        public bool IsStep { get; set; }

        public int Hysteresis { get; set; }

        private int[] mValueList = new int[MAX_FAN_VALUE_SIZE];
        public int[] ValueList
        { 
            get { return mValueList; }
        }

        public int LastChangedValue { get; set; }
        public int LastChangedTemp { get; set; }

        public FanData(int index, string name, bool isStep, int hysteresis)
        {
            Index = index;
            Name = name;
            IsStep = isStep;
            Hysteresis = hysteresis;
            for (int i = 0; i < MAX_FAN_VALUE_SIZE; i++)
                mValueList[i] = 50;
        }

        public FanData clone()
        {
            var fanData = new FanData(Index, Name, IsStep, Hysteresis);
            for (int i = 0; i < MAX_FAN_VALUE_SIZE; i++)
                fanData.ValueList[i] = mValueList[i];
            return fanData;
        }

        public int getValue(int temperature)
        {
            if (temperature >= 100)
            {
                LastChangedTemp = 100;
                LastChangedValue = mValueList[MAX_FAN_VALUE_SIZE - 1];
                return mValueList[MAX_FAN_VALUE_SIZE - 1];
            }

            double divide = temperature / 5.0;
            int prevIndex = (int)Math.Truncate(divide);
            int nextIndex = (int)Math.Ceiling(divide);

            int prevValue = mValueList[prevIndex];
            int nextValue = mValueList[nextIndex];

            // step
            if (IsStep == true)
            {
                if(LastChangedValue == 0 || prevValue >= LastChangedValue ||
                    LastChangedTemp - temperature > Hysteresis)    // check hysteresis
                {
                    int minIndex = this.getSameValueMinIndex(prevIndex);
                    LastChangedValue = mValueList[minIndex];
                    LastChangedTemp = minIndex * 5;
                }
                return LastChangedValue;
            }

            // same temperature
            if (prevValue == nextValue)
            {
                return prevValue;
            }               

            double x1 = prevIndex * 5.0f;
            double y1 = (double)prevValue;

            double x2 = nextIndex * 5.0f;
            double y2 = (double)nextValue;

            double slope = (y2 - y1) / (x2 - x1);
            double result = slope * ((double)temperature - x1) + y1;
            return (int)Math.Round(result);
        }

        private int getSameValueMinIndex(int index)
        {
            int minIndex = index;
            int nowValue = mValueList[index];
            for(int i = index - 1; i >= 0; i--)
            {
                if (nowValue == mValueList[i])
                {
                    minIndex = i;
                    continue;
                }
                break;
            }
            return minIndex;
        }
    }
}
