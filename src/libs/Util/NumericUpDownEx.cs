using DarkUI.Controls;
using System;

namespace FanCtrl
{
    class NumericUpDownEx : DarkNumericUpDown
    {
        public event EventHandler ValueChangedInput;
        private bool mIsSetValue = false;

        public decimal ExValue
        {
            get
            {
                return Value;
            }
            set
            {
                mIsSetValue = true;
                Value = value;
                mIsSetValue = false;
            }
        }

        protected override void OnValueChanged(EventArgs e)
        {
            if (ValueChangedInput != null && mIsSetValue == false)
                ValueChangedInput(this, e);
            base.OnValueChanged(e);
        }
    }
}
