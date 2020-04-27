using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace FanControl
{
    public partial class FanControlForm : Form
    {
        private int mSelectedIndex = -1;
        private PointPairList mPointList = null;
        private LineItem mLineItem = null;

        private PointPairList mNowPoint = null;
        private LineItem mNowPointLineItem = null;

        private int mModeIndex = 0;
        private List<List<ControlData>> mControlDataList = new List<List<ControlData>>();
        private FanData mSelectedFanData = null;

        public event EventHandler onApplyCallback;
        public event EventHandler onCloseCallback;

        public FanControlForm()
        {
            InitializeComponent();
            this.localizeComponent();

            this.FormClosed += (sender, e) =>
            {
                onCloseCallback(sender, EventArgs.Empty);
            };

            mControlDataList.Add(ControlManager.getInstance().getCloneControlDataList(0));
            mControlDataList.Add(ControlManager.getInstance().getCloneControlDataList(1));
            mControlDataList.Add(ControlManager.getInstance().getCloneControlDataList(2));
            mControlDataList.Add(ControlManager.getInstance().getCloneControlDataList(3));
            mModeIndex = ControlManager.getInstance().ModeIndex;

            this.initControl();
            this.initGraph();
        }

        private void localizeComponent()
        {
            this.Text = StringLib.Auto_Fan_Control;
            mEnableCheckBox.Text = StringLib.Enable_automatic_fan_control;
            mModeGroupBox.Text = StringLib.Mode;
            mNormalRadioButton.Text = StringLib.Normal;
            mSilenceRadioButton.Text = StringLib.Silence;
            mPerformanceRadioButton.Text = StringLib.Performance;
            mGameRadioButton.Text = StringLib.Game;
            mSensorGroupBox.Text = StringLib.Sensor;
            mFanGroupBox.Text = StringLib.Fan;
            mAddButton.Text = StringLib.Add;
            mRemoveButton.Text = StringLib.Remove;
            mGraphGroupBox.Text = StringLib.Graph;
            mHysLabel.Text = StringLib.Hysteresis;
            mStepCheckBox.Text = StringLib.Step;
            mOKButton.Text = StringLib.OK;
            mApplyButton.Text = StringLib.Apply;
        }

        private void initControl()
        {
            ControlManager controlManager = ControlManager.getInstance();

            mEnableCheckBox.Checked = ControlManager.getInstance().IsEnable;

            if (mModeIndex == 0)
            {
                mNormalRadioButton.Checked = true;
                mSilenceRadioButton.Checked = false;
                mPerformanceRadioButton.Checked = false;
                mGameRadioButton.Checked = false;
            }
            else if (mModeIndex == 1)
            {
                mNormalRadioButton.Checked = false;
                mSilenceRadioButton.Checked = true;
                mPerformanceRadioButton.Checked = false;
                mGameRadioButton.Checked = false;
            }
            else if (mModeIndex == 2)
            {
                mNormalRadioButton.Checked = false;
                mSilenceRadioButton.Checked = false;
                mPerformanceRadioButton.Checked = true;
                mGameRadioButton.Checked = false;
            }
            else
            {
                mNormalRadioButton.Checked = false;
                mSilenceRadioButton.Checked = false;
                mPerformanceRadioButton.Checked = false;
                mGameRadioButton.Checked = true;
            }

            mNormalRadioButton.Click += onRadioButtonClick;
            mSilenceRadioButton.Click += onRadioButtonClick;
            mPerformanceRadioButton.Click += onRadioButtonClick;
            mGameRadioButton.Click += onRadioButtonClick;

            mSensorComboBox.SelectedIndexChanged += onSensorComboBoxIndexChanged;

            mFanListView.Columns.Add("MyColumn", -2, HorizontalAlignment.Center);
            mFanListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            mFanListView.GridLines = true;
            mFanListView.SelectedIndexChanged += onFanListViewIndexChanged;

            mHysNumericUpDown.ValueChanged += onHysNumericValueChanged;

            for (int i = 0; i < controlManager.getNameCount(0); i++)
            {
                mSensorComboBox.Items.Add(controlManager.getName(0, i, false));
            }

            for (int i = 0; i < controlManager.getNameCount(2); i++)
            {
                mFanComboBox.Items.Add(controlManager.getName(2, i, false));
            }

            if (mSensorComboBox.Items.Count > 0)
            {
                mSensorComboBox.SelectedIndex = 0;
            }

            if (mFanComboBox.Items.Count > 0)
            {
                mFanComboBox.SelectedIndex = 0;
            }
        }

        private void initGraph()
        {
            mGraph.GraphPane.Title.IsVisible = false;
            mGraph.GraphPane.XAxis.Title.IsVisible = false;
            mGraph.GraphPane.YAxis.Title.IsVisible = false;
            mGraph.IsEnableZoom = false;
            mGraph.MouseDownEvent += onGraphMouseDown;
            mGraph.MouseMoveEvent += onGraphMouseMove;
            mGraph.MouseUpEvent += onGraphMouseUp;

            // X axis
            mGraph.GraphPane.XAxis.Scale.MinorStep = 5;
            mGraph.GraphPane.XAxis.Scale.MajorStep = 10;
            mGraph.GraphPane.XAxis.Scale.Min = 0;
            mGraph.GraphPane.XAxis.Scale.Max = 100;
            mGraph.GraphPane.XAxis.Scale.Format = "0℃";
            mGraph.GraphPane.XAxis.MinorGrid.IsVisible = false;
            mGraph.GraphPane.XAxis.MajorGrid.IsVisible = true;
            mGraph.GraphPane.XAxis.Type = AxisType.Linear;

            // Y axis
            mGraph.GraphPane.YAxis.Scale.MinorStep = 5;
            mGraph.GraphPane.YAxis.Scale.MajorStep = 10;
            mGraph.GraphPane.YAxis.Scale.Min = 0;
            mGraph.GraphPane.YAxis.Scale.Max = 100;
            mGraph.GraphPane.YAxis.Scale.Format = "0％";
            mGraph.GraphPane.YAxis.MajorGrid.IsVisible = true;
            mGraph.GraphPane.YAxis.Type = AxisType.Linear;

            mGraph.GraphPane.CurveList.Clear();

            mNowPoint = new PointPairList();
            mNowPoint.Add(0, 0);
            mNowPointLineItem = mGraph.GraphPane.AddCurve("Now", mNowPoint, Color.Red, SymbolType.Circle);
            mNowPointLineItem.Line.Width = 1.0f;
            mNowPointLineItem.Symbol.Size = 10.0f;
            mNowPointLineItem.Symbol.Fill = new Fill(Color.Red);

            // line
            mPointList = new PointPairList();
            for (int i = 0; i < FanData.MAX_FAN_VALUE_SIZE; i++)
            {
                mPointList.Add(5 * i, 50);
            }
            mLineItem = mGraph.GraphPane.AddCurve("Fan speed", mPointList, Color.Blue, SymbolType.Circle);
            mLineItem.Line.Width = 2.0f;
            mLineItem.Symbol.Size = 10.0f;
            mLineItem.Symbol.Fill = new Fill(Color.White);

            mGraph.Visible = false;
            mStepCheckBox.Visible = false;
            mHysLabel.Visible = false;
            mHysNumericUpDown.Visible = false;
        }

        private void onRadioButtonClick(object sender, EventArgs e)
        {
            int index = -1;
            if(sender == mNormalRadioButton)            index = 0;
            else if (sender == mSilenceRadioButton)     index = 1;
            else if (sender == mPerformanceRadioButton) index = 2;
            else                                         index = 3;
            if (index == mModeIndex)
                return;

            mModeIndex = index;

            if (mModeIndex == 0)
            {
                mNormalRadioButton.Checked = true;
                mSilenceRadioButton.Checked = false;
                mPerformanceRadioButton.Checked = false;
                mGameRadioButton.Checked = false;
            }
            else if (mModeIndex == 1)
            {
                mNormalRadioButton.Checked = false;
                mSilenceRadioButton.Checked = true;
                mPerformanceRadioButton.Checked = false;
                mGameRadioButton.Checked = false;
            }
            else if (mModeIndex == 2)
            {
                mNormalRadioButton.Checked = false;
                mSilenceRadioButton.Checked = false;
                mPerformanceRadioButton.Checked = true;
                mGameRadioButton.Checked = false;
            }
            else
            {
                mNormalRadioButton.Checked = false;
                mSilenceRadioButton.Checked = false;
                mPerformanceRadioButton.Checked = false;
                mGameRadioButton.Checked = true;
            }

            this.onSensorComboBoxIndexChanged(null, EventArgs.Empty);
        }

        private void onSensorComboBoxIndexChanged(object sender, EventArgs e)
        {
            mGraph.Visible = false;
            mStepCheckBox.Visible = false;
            mHysLabel.Visible = false;
            mHysNumericUpDown.Visible = false;
            mSelectedFanData = null;

            mFanListView.BeginUpdate();
            mFanListView.Clear();

            var controlData = this.getControlData(mSensorComboBox.SelectedIndex);
            if(controlData == null)
            {
                mFanListView.EndUpdate();
                return;
            }

            for (int i = 0; i < controlData.FanDataList.Count; i++)
            {
                var fanData = controlData.FanDataList[i];
                mFanListView.Items.Add(fanData.Name);
            }
            mFanListView.EndUpdate();
        }

        private void onStepCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (mLineItem == null)
                return;

            if(mSelectedFanData != null)
            {
                mSelectedFanData.IsStep = mStepCheckBox.Checked;
            }

            mLineItem.Line.StepType = (mStepCheckBox.Checked == true) ? StepType.ForwardStep : StepType.NonStep;
            mHysNumericUpDown.Enabled = mStepCheckBox.Checked;
            mGraph.Refresh();
        }

        private bool onGraphMouseDown(ZedGraphControl sender, MouseEventArgs e)
        {
            mSelectedIndex = -1;
            double x = 0, y = 0;
            mGraph.GraphPane.ReverseTransform(e.Location, out x, out y);
            if (x >= 0 && x <= 100)
            {
                mSelectedIndex = this.getPointIndex(x);
                this.setPoint(y);
            }
            return true;
        }

        private bool onGraphMouseMove(ZedGraphControl sender, MouseEventArgs e)
        {
            if (mSelectedIndex == -1 || mSelectedIndex >= mPointList.Count)
                return true;

            double x = 0, y = 0;
            mGraph.GraphPane.ReverseTransform(e.Location, out x, out y);
            this.setPoint(y);
            return true;
        }

        private bool onGraphMouseUp(ZedGraphControl sender, MouseEventArgs e)
        {
            mSelectedIndex = -1;
            return true;
        }

        private int getPointIndex(double x)
        {
            double divide = x / 5.0;
            int index = (int)Math.Round(divide);
            return index;
        }

        private void setPoint(double y)
        {
            if (mSelectedIndex == -1 || mSelectedIndex >= mPointList.Count)
                return;

            if (y < 0)          y = 0;
            else if (y > 100)   y = 100;

            double divide = y / 5.0;
            int temp = (int)Math.Round(divide);
            y = (double)(temp * 5);

            mPointList[mSelectedIndex].Y = y;
            if (mSelectedFanData != null)
            {
                mSelectedFanData.ValueList[mSelectedIndex] = (int)y;
            }            

            for (int i = 0; i < mSelectedIndex; i++)
            {
                if (mPointList[i].Y > y)
                {
                    mPointList[i].Y = y;
                    if (mSelectedFanData != null)
                    {
                        mSelectedFanData.ValueList[i] = (int)y;
                    }
                }
            }
            for (int i = mSelectedIndex + 1; i < mPointList.Count; i++)
            {
                if (mPointList[i].Y < y)
                {
                    mPointList[i].Y = y;
                    if (mSelectedFanData != null)
                    {
                        mSelectedFanData.ValueList[i] = (int)y;
                    }
                }
            }
            mGraph.Refresh();
        }

        public void onUpdateTimer()
        {
            if (mSensorComboBox.Items.Count == 0 ||
                mFanComboBox.Items.Count == 0 ||
                mSelectedFanData == null ||
                mNowPoint == null)
            {
                return;
            }

            var hardwareManager = HardwareManager.getInstance();
            var sensor = hardwareManager.getSensor(mSensorComboBox.SelectedIndex);
            var control = hardwareManager.getControl(mSelectedFanData.Index);
            if (sensor == null || control == null)
                return;

            mNowPoint[0].X = (double)sensor.Value;
            mNowPoint[0].Y = (double)control.LastValue;
            mGraph.Refresh();
        }

        private ControlData getControlData(int sensorIndex)
        {
            ControlData controlData = null;
            for (int i = 0; i < mControlDataList[mModeIndex].Count; i++)
            {
                var tempControlData = mControlDataList[mModeIndex][i];
                if (tempControlData.Index == sensorIndex)
                {
                    controlData = tempControlData;
                    break;
                }
            }
            return controlData;
        }

        private FanData getFanData(int sensorIndex, int fanIndex)
        {
            var controlData = this.getControlData(sensorIndex);
            if (controlData == null)
                return null;

            FanData fanData = null;
            for (int i = 0; i < controlData.FanDataList.Count; i++)
            {
                var tempFanData = controlData.FanDataList[i];
                if (tempFanData.Index == fanIndex)
                {
                    fanData = tempFanData;
                    break;
                }
            }
            return fanData;
        }

        private FanData getFanData(int sensorIndex, string fanName)
        {
            var controlData = this.getControlData(sensorIndex);
            if (controlData == null)
                return null;

            FanData fanData = null;
            for (int i = 0; i < controlData.FanDataList.Count; i++)
            {
                var tempFanData = controlData.FanDataList[i];
                if (tempFanData.Name.Equals(fanName) == true)
                {
                    fanData = tempFanData;
                    break;
                }
            }
            return fanData;
        }

        private void onFanListViewIndexChanged(object sender, EventArgs e)
        {
            var items = mFanListView.SelectedItems;
            if (items == null || items.Count == 0)
            {
                mGraph.Visible = false;
                mStepCheckBox.Visible = false;
                mHysLabel.Visible = false;
                mHysNumericUpDown.Visible = false;
                mSelectedFanData = null;
                return;
            }

            mGraph.Visible = true;
            mStepCheckBox.Visible = true;
            mHysLabel.Visible = true;
            mHysNumericUpDown.Visible = true;

            var item = items[0];
            mSelectedFanData = this.getFanData(mSensorComboBox.SelectedIndex, item.Text);
            for(int i = 0; i < FanData.MAX_FAN_VALUE_SIZE; i++)
            {
                mPointList[i].Y = (double)mSelectedFanData.ValueList[i];
            }

            mStepCheckBox.Checked = mSelectedFanData.IsStep;
            mLineItem.Line.StepType = (mStepCheckBox.Checked == true) ? StepType.ForwardStep : StepType.NonStep;
            mHysNumericUpDown.Enabled = mStepCheckBox.Checked;
            mHysNumericUpDown.Value = mSelectedFanData.Hysteresis;

            mGraph.Refresh();

            this.onUpdateTimer();
        }

        private void onHysNumericValueChanged(object sender, EventArgs e)
        {
            if (mHysNumericUpDown.Value > 20)       mHysNumericUpDown.Value = 20;
            else if (mHysNumericUpDown.Value < 0)   mHysNumericUpDown.Value = 0;

            if (mSelectedFanData != null)
                mSelectedFanData.Hysteresis = Decimal.ToInt32(mHysNumericUpDown.Value);
        }

        private void onAddButtonClick(object sender, EventArgs e)
        {
            if (mSensorComboBox.Items.Count == 0 || mFanComboBox.Items.Count == 0)
                return;

            int sensorIndex = mSensorComboBox.SelectedIndex;
            int fanIndex = mFanComboBox.SelectedIndex;

            var controlManager = ControlManager.getInstance();
            var sensorName = controlManager.getName(0, sensorIndex, false);
            var fanControlName = controlManager.getName(2, fanIndex, false);

            var controlData = this.getControlData(sensorIndex);
            if(controlData == null)
            {
                controlData = new ControlData(sensorIndex, sensorName);
                mControlDataList[mModeIndex].Add(controlData);
            }

            mFanListView.BeginUpdate();

            var fanData = this.getFanData(sensorIndex, fanIndex);
            if(fanData == null)
            {
                fanData = new FanData(fanIndex, fanControlName, true, 0);
                controlData.FanDataList.Add(fanData);

                mFanListView.Items.Add(fanData.Name);
            }

            mFanListView.EndUpdate();
        }

        private void onRemoveButtonClick(object sender, EventArgs e)
        {
            var items = mFanListView.SelectedItems;
            if (items == null || items.Count == 0)
                return;

            var item = items[0];
            var controlData = this.getControlData(mSensorComboBox.SelectedIndex);
            for(int i = 0; i < controlData.FanDataList.Count; i++)
            {
                var fanData = controlData.FanDataList[i];
                if(fanData.Name.Equals(item.Text) == true)
                {
                    controlData.FanDataList.RemoveAt(i);
                    break;
                }
            }

            mSelectedFanData = null;
            mFanListView.BeginUpdate();
            mFanListView.Items.Remove(item);
            mFanListView.EndUpdate();
        }

        private void onApplyButtonClick(object sender, EventArgs e)
        {
            for (int i = 0; i < mControlDataList.Count; i++)
            {
                for (int j = 0; j < mControlDataList[i].Count; j++)
                {
                    for (int k = 0; k < mControlDataList[i][j].FanDataList.Count; k++)
                    {
                        mControlDataList[i][j].FanDataList[k].LastChangedTemp = 0;
                        mControlDataList[i][j].FanDataList[k].LastChangedValue = 0;
                    }
                }
                ControlManager.getInstance().setControlDataList(i, mControlDataList[i]);
            }
            ControlManager.getInstance().ModeIndex = mModeIndex;
            ControlManager.getInstance().IsEnable = mEnableCheckBox.Checked;
            ControlManager.getInstance().write();

            onApplyCallback(sender, e);
        }

        private void onOKButtonClick(object sender, EventArgs e)
        {
            this.onApplyButtonClick(sender, e);
            this.Close();
        }
        
    }
}
