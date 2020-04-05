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

        private int[] mValueList = new int[MAX_FAN_VALUE_SIZE];
        public int[] ValueList
        { 
            get { return mValueList; }
        }

        public FanData(int index, string name, bool isStep)
        {
            Index = index;
            Name = name;
            IsStep = isStep;
            for (int i = 0; i < MAX_FAN_VALUE_SIZE; i++)
                mValueList[i] = 50;
        }

        public FanData clone()
        {
            var fanData = new FanData(Index, Name, IsStep);
            for (int i = 0; i < MAX_FAN_VALUE_SIZE; i++)
                fanData.ValueList[i] = mValueList[i];
            return fanData;
        }

        public int getValue(int temperature)
        {
            if (temperature >= 100)
                return mValueList[MAX_FAN_VALUE_SIZE - 1];

            double divide = temperature / 5.0;
            int prevIndex = (int)Math.Truncate(divide);
            int nextIndex = (int)Math.Ceiling(divide);

            int prevValue = mValueList[prevIndex];
            int nextValue = mValueList[nextIndex];

            // same temperature or step
            if (prevValue == nextValue || IsStep == true)
                return prevValue;

            double x1 = prevIndex * 5.0f;
            double y1 = (double)prevValue;

            double x2 = nextIndex * 5.0f;
            double y2 = (double)nextValue;

            double slope = (y2 - y1) / (x2 - x1);
            double result = slope * ((double)temperature - x1) + y1;
            return (int)Math.Round(result);
        }
    }
}
