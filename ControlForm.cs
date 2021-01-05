using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace FanCtrl
{
    public partial class ControlForm : Form
    {
        private Size mLastSize = new Size(940, 543);
        private Size mNormalLastSize = new Size(940, 543);

        private bool mIsUpdateGraph = true;
        private bool mIsResize = false;

        private int mSelectedSensorIndex = -1;

        private int mSelectedIndex = -1;
        private PointPairList mPointList = null;
        private LineItem mLineItem = null;

        private PointPairList mNowPoint = null;
        private LineItem mNowPointLineItem = null;

        private int mModeIndex = 0;
        private List<List<ControlData>> mControlDataList = new List<List<ControlData>>();
        private FanData mSelectedFanData = null;

        public event EventHandler onApplyCallback;

        public ControlForm()
        {
            InitializeComponent();
            this.localizeComponent();

            mControlDataList.Add(ControlManager.getInstance().getCloneControlDataList(0));
            mControlDataList.Add(ControlManager.getInstance().getCloneControlDataList(1));
            mControlDataList.Add(ControlManager.getInstance().getCloneControlDataList(2));
            mControlDataList.Add(ControlManager.getInstance().getCloneControlDataList(3));
            mModeIndex = ControlManager.getInstance().ModeIndex;

            this.initControl();
            this.initGraph();

            // Can only resize windows with Windows 10
            if (System.Environment.OSVersion.Version.Major >= 10)
            {
                this.SetStyle(ControlStyles.UserPaint |
                            ControlStyles.OptimizedDoubleBuffer |
                            ControlStyles.AllPaintingInWmPaint |
                            ControlStyles.SupportsTransparentBackColor, true);
                this.Resize += (sender, e) =>
                {
                    if (mIsResize == true)
                        return;
                    mIsResize = true;

                    //Console.WriteLine("Size : {0}, {1}", this.Width, this.Height);

                    int widthGap = this.Width - mLastSize.Width;
                    int heightGap = this.Height - mLastSize.Height;

                    //Console.WriteLine("Gap : {0}, {1}", widthGap, heightGap);

                    mFanGroupBox.Height = mFanGroupBox.Height + heightGap;
                    mFanListView.Height = mFanListView.Height + heightGap;
                    mRemoveButton.Top = mRemoveButton.Top + heightGap;

                    mModeGroupBox.Width = mModeGroupBox.Width + widthGap;

                    mGraphGroupBox.Width = mGraphGroupBox.Width + widthGap;
                    mGraphGroupBox.Height = mGraphGroupBox.Height + heightGap;

                    mGraph.Width = mGraph.Width + widthGap;
                    mGraph.Height = mGraph.Height + heightGap;

                    mPresetLabel.Left = mPresetLabel.Left + widthGap;
                    mPresetLoadButton.Left = mPresetLoadButton.Left + widthGap;
                    mPresetSaveButton.Left = mPresetSaveButton.Left + widthGap;
                    mUnitLabel.Left = mUnitLabel.Left + widthGap;
                    mUnitComboBox.Left = mUnitComboBox.Left + widthGap;
                    mHysLabel.Left = mHysLabel.Left + widthGap;
                    mHysNumericUpDown.Left = mHysNumericUpDown.Left + widthGap;
                    mStepCheckBox.Left = mStepCheckBox.Left + widthGap;

                    mApplyButton.Left = mApplyButton.Left + widthGap;
                    mApplyButton.Top = mApplyButton.Top + heightGap;

                    mOKButton.Left = mOKButton.Left + widthGap;
                    mOKButton.Top = mOKButton.Top + heightGap;

                    mLastSize.Width = this.Width;
                    mLastSize.Height = this.Height;

                    if (this.WindowState != FormWindowState.Maximized)
                    {
                        mNormalLastSize.Width = this.Width;
                        mNormalLastSize.Height = this.Height;
                    }

                    mIsResize = false;
                };

                this.ResizeBegin += (s, e) =>
                {
                    mIsUpdateGraph = false;
                    this.SuspendLayout();
                };
                this.ResizeEnd += (s, e) =>
                {
                    this.ResumeLayout();
                    mIsUpdateGraph = true;
                };

                if (ControlManager.getInstance().IsMaximize == true)
                {
                    this.WindowState = FormWindowState.Maximized;
                }

                this.Width = ControlManager.getInstance().Width;
                this.Height = ControlManager.getInstance().Height;
                mLastSize.Width = this.Width;
                mLastSize.Height = this.Height;
                mNormalLastSize.Width = this.Width;
                mNormalLastSize.Height = this.Height;
            }
            else
            {
                this.MaximizeBox = false;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
            }
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
            mPresetLabel.Text = StringLib.Preset;
            mPresetLoadButton.Text = StringLib.Load;
            mPresetSaveButton.Text = StringLib.Save;
            mUnitLabel.Text = StringLib.Unit;
            mHysLabel.Text = StringLib.Hysteresis;
            mStepCheckBox.Text = StringLib.Step;
            mOKButton.Text = StringLib.OK;
            mApplyButton.Text = StringLib.Apply;
        }

        private void initControl()
        {
            ControlManager controlManager = ControlManager.getInstance();

            mEnableCheckBox.Checked = ControlManager.getInstance().IsEnable;

            mNormalRadioButton.Checked = (mModeIndex == 0);
            mSilenceRadioButton.Checked = (mModeIndex == 1);
            mPerformanceRadioButton.Checked = (mModeIndex == 2);
            mGameRadioButton.Checked = (mModeIndex == 3);

            mNormalRadioButton.Click += onRadioButtonClick;
            mSilenceRadioButton.Click += onRadioButtonClick;
            mPerformanceRadioButton.Click += onRadioButtonClick;
            mGameRadioButton.Click += onRadioButtonClick;

            mSensorComboBox.SelectedIndexChanged += onSensorComboBoxIndexChanged;

            mFanListView.Columns.Add("MyColumn", -2, HorizontalAlignment.Center);
            mFanListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            mFanListView.GridLines = true;
            mFanListView.SelectedIndexChanged += onFanListViewIndexChanged;

            mUnitComboBox.Items.Add("1");
            mUnitComboBox.Items.Add("5");
            mUnitComboBox.Items.Add("10");
            mUnitComboBox.SelectedIndex = 1;
            mUnitComboBox.SelectedIndexChanged += onUnitComboBoxIndexChanged;

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
                mSelectedSensorIndex = 0;
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
            mGraph.GraphPane.XAxis.MinorGrid.IsVisible = false;
            mGraph.GraphPane.XAxis.MajorGrid.IsVisible = true;
            mGraph.GraphPane.XAxis.Type = AxisType.Linear;

            mGraph.GraphPane.XAxis.ScaleFormatEvent += (pane, axis, val, index) =>
            {
                var min = OptionManager.getInstance().IsFahrenheit == false ? 0 : 32;
                var majorStep = OptionManager.getInstance().IsFahrenheit == false ? 10 : 18;
                var temp = min + majorStep * index;
                return temp + (OptionManager.getInstance().IsFahrenheit == false ? "°C" : "°F");
            };

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
            for (int i = 0; i < FanData.MAX_FAN_VALUE_SIZE_5; i++)
            {
                mPointList.Add(5 * i, 50);
            }
            mLineItem = mGraph.GraphPane.AddCurve("Graph", mPointList, Color.Blue, SymbolType.Circle);
            mLineItem.Line.Width = 2.0f;
            mLineItem.Symbol.Size = 10.0f;
            mLineItem.Symbol.Fill = new Fill(Color.White);

            mPresetLabel.Visible = false;
            mPresetLoadButton.Visible = false;
            mPresetSaveButton.Visible = false;
            mUnitLabel.Visible = false;
            mUnitComboBox.Visible = false;
            mGraph.Visible = false;
            mStepCheckBox.Visible = false;
            mHysLabel.Visible = false;
            mHysNumericUpDown.Visible = false;
        }

        private void setGraphFromSelectedFanData()
        {
            if (mSelectedFanData == null)
                return;

            var unit = mSelectedFanData.Unit;
            if (unit == FanValueUnit.Size_1)
            {
                mGraph.GraphPane.XAxis.Scale.MinorStep = 1;
                mGraph.GraphPane.YAxis.Scale.MinorStep = 1;
                mLineItem.Symbol.Size = 2.0f;
            }
            else if (unit == FanValueUnit.Size_5)
            {
                mGraph.GraphPane.XAxis.Scale.MinorStep = 5;
                mGraph.GraphPane.YAxis.Scale.MinorStep = 5;
                mLineItem.Symbol.Size = 10.0f;
            }
            else
            {
                mGraph.GraphPane.XAxis.Scale.MinorStep = 10;
                mGraph.GraphPane.YAxis.Scale.MinorStep = 10;
                mLineItem.Symbol.Size = 10.0f;
            }

            //mGraph.GraphPane.CurveList.Clear();
            mPointList.Clear();
            for (int i = 0; i < mSelectedFanData.getMaxFanValue(); i++)
            {
                mPointList.Add(mSelectedFanData.getDivideValue() * i, mSelectedFanData.ValueList[i]);
            }
            mGraph.Refresh();
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

            mNormalRadioButton.Checked = (mModeIndex == 0);
            mSilenceRadioButton.Checked = (mModeIndex == 1);
            mPerformanceRadioButton.Checked = (mModeIndex == 2);
            mGameRadioButton.Checked = (mModeIndex == 3);

            this.onSensorComboBoxIndexChanged(null, EventArgs.Empty);
        }

        private void onSensorComboBoxIndexChanged(object sender, EventArgs e)
        {
            mPresetLabel.Visible = false;
            mPresetLoadButton.Visible = false;
            mPresetSaveButton.Visible = false;
            mUnitLabel.Visible = false;
            mUnitComboBox.Visible = false;
            mGraph.Visible = false;
            mStepCheckBox.Visible = false;
            mHysLabel.Visible = false;
            mHysNumericUpDown.Visible = false;
            mSelectedFanData = null;

            mFanListView.BeginUpdate();
            mFanListView.Clear();

            mSelectedSensorIndex = mSensorComboBox.SelectedIndex;
            var controlData = this.getControlData(mSelectedSensorIndex);
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
            double divide = x / mSelectedFanData.getDivideValue();
            int index = (int)Math.Round(divide);
            return index;
        }

        private void setPoint(double y)
        {
            if (mSelectedIndex == -1 || mSelectedIndex >= mPointList.Count)
                return;

            if (y < 0)          y = 0;
            else if (y > 100)   y = 100;

            double divide = y / mSelectedFanData.getDivideValue();
            int temp = (int)Math.Round(divide);
            y = (double)(temp * mSelectedFanData.getDivideValue());

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
                mSelectedSensorIndex == -1 ||
                mFanComboBox.Items.Count == 0 ||
                mSelectedFanData == null ||
                mNowPoint == null ||
                mIsUpdateGraph == false)
            {
                return;
            }

            var hardwareManager = HardwareManager.getInstance();
            var sensor = hardwareManager.getSensor(mSelectedSensorIndex);
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
                mPresetLabel.Visible = false;
                mPresetLoadButton.Visible = false;
                mPresetSaveButton.Visible = false;
                mUnitLabel.Visible = false;
                mUnitComboBox.Visible = false;
                mGraph.Visible = false;
                mStepCheckBox.Visible = false;
                mHysLabel.Visible = false;
                mHysNumericUpDown.Visible = false;
                mSelectedFanData = null;
                return;
            }

            mPresetLabel.Visible = true;
            mPresetLoadButton.Visible = true;
            mPresetSaveButton.Visible = true;
            mUnitLabel.Visible = true;
            mUnitComboBox.Visible = true;
            mGraph.Visible = true;
            mStepCheckBox.Visible = true;
            mHysLabel.Visible = true;
            mHysNumericUpDown.Visible = true;

            var item = items[0];
            mSelectedFanData = this.getFanData(mSelectedSensorIndex, item.Text);

            // setGraphFromSelectedFanData
            this.setGraphFromSelectedFanData();

            mUnitComboBox.SelectedIndex = (int)mSelectedFanData.Unit;
            mStepCheckBox.Checked = mSelectedFanData.IsStep;
            mLineItem.Line.StepType = (mStepCheckBox.Checked == true) ? StepType.ForwardStep : StepType.NonStep;
            mHysNumericUpDown.Enabled = mStepCheckBox.Checked;
            mHysNumericUpDown.Value = mSelectedFanData.Hysteresis;

            this.onUpdateTimer();
        }

        private void onUnitComboBoxIndexChanged(object sender, EventArgs e)
        {
            if (mSelectedFanData == null)
                return;

            mSelectedFanData.setChangeUnitAndFanValue((FanValueUnit)mUnitComboBox.SelectedIndex);

            // setGraphFromSelectedFanData
            this.setGraphFromSelectedFanData();
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

            int sensorIndex = mSelectedSensorIndex;
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
                fanData = new FanData(fanIndex, fanControlName, FanValueUnit.Size_5, true, 0);
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
            var controlData = this.getControlData(mSelectedSensorIndex);
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

        private void onPresetLoadButtonClick(object sender, EventArgs e)
        {
            if (mSelectedFanData == null)
                return;

            string nowDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string presetDirectory = nowDirectory + "\\preset";
            if (Directory.Exists(presetDirectory) == false)
            {
                presetDirectory = nowDirectory;
            }

            var ofd = new OpenFileDialog();
            ofd.Title = StringLib.Load;
            ofd.InitialDirectory = presetDirectory;
            ofd.Filter = "Preset file (*.preset) | *.preset;";
            var dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                try
                {
                    string fileName = ofd.FileName;
                    string jsonString = File.ReadAllText(fileName);

                    // check data
                    var rootObject = JObject.Parse(jsonString);
                    bool isStep = rootObject.Value<bool>("step");
                    int hysteresis = rootObject.Value<int>("hysteresis");
                    var unit = (FanValueUnit)rootObject.Value<int>("unit");

                    var valueList = rootObject.Value<JArray>("value");
                    if (unit == FanValueUnit.Size_1)
                    {
                        if (valueList.Count != FanData.MAX_FAN_VALUE_SIZE_1)
                            throw new Exception();
                    }
                    else if (unit == FanValueUnit.Size_5)
                    {
                        if (valueList.Count != FanData.MAX_FAN_VALUE_SIZE_5)
                            throw new Exception();
                    }
                    else if (unit == FanValueUnit.Size_10)
                    {
                        if (valueList.Count != FanData.MAX_FAN_VALUE_SIZE_10)
                            throw new Exception();
                    }
                    else
                    {
                        throw new Exception();
                    }

                    mSelectedFanData.IsStep = isStep;
                    mSelectedFanData.Hysteresis = hysteresis;
                    mSelectedFanData.setChangeUnitAndFanValue(unit);
                    for (int i = 0; i < valueList.Count; i++)
                    {
                        int value = valueList[i].Value<int>();
                        mSelectedFanData.ValueList[i] = value;
                    }

                    // setGraphFromSelectedFanData
                    this.setGraphFromSelectedFanData();

                    mUnitComboBox.SelectedIndex = (int)mSelectedFanData.Unit;
                    mStepCheckBox.Checked = mSelectedFanData.IsStep;
                    mLineItem.Line.StepType = (mStepCheckBox.Checked == true) ? StepType.ForwardStep : StepType.NonStep;
                    mHysNumericUpDown.Enabled = mStepCheckBox.Checked;
                    mHysNumericUpDown.Value = mSelectedFanData.Hysteresis;

                    this.onUpdateTimer();
                }
                catch
                {
                    MessageBox.Show(StringLib.Failed_to_read_preset_file);
                }
            }
        }

        private void onPresetSaveButtonClick(object sender, EventArgs e)
        {
            if (mSelectedFanData == null)
                return;

            string nowDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string presetDirectory = nowDirectory + "\\preset";
            if (Directory.Exists(presetDirectory) == false)
            {
                presetDirectory = nowDirectory;
            }

            var sfd = new SaveFileDialog();
            sfd.Title = StringLib.Save;
            sfd.InitialDirectory = presetDirectory;
            sfd.Filter = "Preset file (*.preset) | *.preset;";
            var dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                var rootObject = new JObject();
                rootObject["step"] = mSelectedFanData.IsStep;
                rootObject["hysteresis"] = mSelectedFanData.Hysteresis;
                rootObject["unit"] = (int)mSelectedFanData.Unit;

                var valueList = new JArray();
                for (int i = 0; i < mSelectedFanData.getMaxFanValue(); i++)
                {
                    int value = mSelectedFanData.ValueList[i];
                    valueList.Add(value);
                }
                rootObject["value"] = valueList;

                string fileName = sfd.FileName;
                File.WriteAllText(fileName, rootObject.ToString());
            }
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

            ControlManager.getInstance().Width = mNormalLastSize.Width;
            ControlManager.getInstance().Height = mNormalLastSize.Height;
            ControlManager.getInstance().IsMaximize = (this.WindowState == FormWindowState.Maximized);

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
