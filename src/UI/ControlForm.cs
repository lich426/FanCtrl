using DarkUI.Config;
using DarkUI.Forms;
using FanCtrl.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ZedGraph;

namespace FanCtrl
{
    public partial class ControlForm : ThemeForm
    {
        private Size mLastSize = new Size(1219, 796);
        private Size mNormalLastSize = new Size(1219, 796);

        private bool mIsUpdateGraph = true;
        private bool mIsResize = false;

        private int mSelectedTempIndex = -1;

        private int mSelectedIndex = -1;
        private PointPairList mPointList = null;
        private LineItem mLineItem = null;

        //private PointPairList mNowPoint = null;
        //private LineItem mNowPointLineItem = null;
        private PointObj mNowPoint = null;

        private PolyObj mAutoPolyObj = null;

        private MODE_TYPE mModeType = MODE_TYPE.NORMAL;
        private List<ControlData>[] mControlDataList = new List<ControlData>[4];
        private FanData mSelectedFanData = null;

        private List<BaseControl> mListViewBaseControlList = new List<BaseControl>();

        public event EventHandler onApplyCallback;

        public ControlForm()
        {
            InitializeComponent();
            this.localizeComponent();

            mControlDataList[0] = ControlManager.getInstance().getCloneControlDataList(MODE_TYPE.NORMAL);
            mControlDataList[1] = ControlManager.getInstance().getCloneControlDataList(MODE_TYPE.SILENCE);
            mControlDataList[2] = ControlManager.getInstance().getCloneControlDataList(MODE_TYPE.PERFORMANCE);
            mControlDataList[3] = ControlManager.getInstance().getCloneControlDataList(MODE_TYPE.GAME);

            mModeType = ControlManager.getInstance().ModeType;

            this.initControl();
            this.initGraph();

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

                mAutoLabel.Left = mAutoLabel.Left + widthGap;
                mAutoNumericUpDown.Left = mAutoNumericUpDown.Left + widthGap;

                mPresetLabel.Left = mPresetLabel.Left + widthGap;
                mPresetLoadButton.Left = mPresetLoadButton.Left + widthGap;
                mPresetSaveButton.Left = mPresetSaveButton.Left + widthGap;
                mUnitLabel.Left = mUnitLabel.Left + widthGap;
                mUnitComboBox.Left = mUnitComboBox.Left + widthGap;
                mHysLabel.Left = mHysLabel.Left + widthGap;
                mHysNumericUpDown.Left = mHysNumericUpDown.Left + widthGap;
                mStepCheckBox.Left = mStepCheckBox.Left + widthGap;
                mDelayLabel.Left = mDelayLabel.Left + widthGap;
                mDelayNumericUpDown.Left = mDelayNumericUpDown.Left + widthGap;
                mDelayLabel2.Left = mDelayLabel2.Left + widthGap;

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

            mGraph.GraphPane.Fill.Color = ThemeProvider.Theme.Colors.GreyBackground;
            mGraph.GraphPane.Chart.Fill.Brush = new SolidBrush(ThemeProvider.Theme.Colors.GreyBackground);
            mGraph.GraphPane.Chart.Fill.Color = ThemeProvider.Theme.Colors.GreyBackground;
            mGraph.GraphPane.Chart.Border.Color = ThemeProvider.Theme.Colors.LightText;
            mGraph.GraphPane.Border.Color = ThemeProvider.Theme.Colors.LightText;

            mGraph.GraphPane.YAxis.Color = ThemeProvider.Theme.Colors.LightText;
            mGraph.GraphPane.YAxis.Scale.FontSpec.FontColor = ThemeProvider.Theme.Colors.LightText;
            mGraph.GraphPane.YAxis.MajorTic.Color = ThemeProvider.Theme.Colors.LightText;
            mGraph.GraphPane.YAxis.MinorTic.Color = ThemeProvider.Theme.Colors.LightText;
            mGraph.GraphPane.YAxis.MajorGrid.Color = ThemeProvider.Theme.Colors.LightText;
            mGraph.GraphPane.YAxis.MinorGrid.Color = ThemeProvider.Theme.Colors.LightText;

            // left line is not change color (Zedgraph bug)
            if (OptionManager.getInstance().getNowTheme() == THEME_TYPE.DARK)
            {
                var pointList = new PointPairList();
                pointList.Add(0, 0);
                pointList.Add(0, 100);
                mGraph.GraphPane.AddBar("", pointList, ThemeProvider.Theme.Colors.LightText);
            }

            mGraph.GraphPane.XAxis.Color = ThemeProvider.Theme.Colors.LightText;
            mGraph.GraphPane.XAxis.Scale.FontSpec.FontColor = ThemeProvider.Theme.Colors.LightText;
            mGraph.GraphPane.XAxis.MajorTic.Color = ThemeProvider.Theme.Colors.LightText;
            mGraph.GraphPane.XAxis.MinorTic.Color = ThemeProvider.Theme.Colors.LightText;
            mGraph.GraphPane.XAxis.MajorGrid.Color = ThemeProvider.Theme.Colors.LightText;
            mGraph.GraphPane.XAxis.MinorGrid.Color = ThemeProvider.Theme.Colors.LightText;
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
            mTempGroupBox.Text = StringLib.Target_Temp;
            mFanGroupBox.Text = StringLib.Fan;
            mAddButton.Text = "↓ " + StringLib.Add + " ↓";
            mRemoveButton.Text = StringLib.Remove;
            mGraphGroupBox.Text = StringLib.Graph;
            mPresetLabel.Text = StringLib.Preset;
            mPresetLoadButton.Text = StringLib.Load;
            mPresetSaveButton.Text = StringLib.Save;
            mUnitLabel.Text = StringLib.Unit;
            mHysLabel.Text = StringLib.Hysteresis;
            mStepCheckBox.Text = StringLib.Step;
            mAutoLabel.Text = StringLib.Auto;
            mDelayLabel.Text = StringLib.Delay;
            mOKButton.Text = StringLib.OK;
            mApplyButton.Text = StringLib.Apply;

            FontFamily fontFamily = null;
            try
            {
                fontFamily = new FontFamily("Gulim");
            }
            catch
            {
                fontFamily = FontFamily.GenericSansSerif;
            }

            // Korean
            if (OptionManager.getInstance().Language == 1)
            {
                mDelayLabel.Left = mDelayLabel.Left - 15;
            }

            // Japanese
            else if (OptionManager.getInstance().Language == 2)
            {
                mPresetLabel.Left = mPresetLabel.Left - 20;
                mUnitLabel.Left = mUnitLabel.Left - 5;
                mHysLabel.Left = mHysLabel.Left - 10;
                mStepCheckBox.Left = mStepCheckBox.Left - 2;
                mDelayLabel.Left = mDelayLabel.Left - 15;
            }

            // French
            else if (OptionManager.getInstance().Language == 3)
            {
                mPresetLabel.Left = mPresetLabel.Left - 20;
                mUnitLabel.Left = mUnitLabel.Left + 5;
                mUnitComboBox.Left = mUnitComboBox.Left + 10;
                mHysLabel.Left = mHysLabel.Left + 23;
                mPresetSaveButton.Font = new Font(fontFamily, 6.5f);
            }

            // Spanish
            else if (OptionManager.getInstance().Language == 4)
            {
                mPresetLabel.Left = mPresetLabel.Left - 100;
                mPresetLoadButton.Left = mPresetLoadButton.Left - 55;
                mPresetSaveButton.Left = mPresetSaveButton.Left - 55;
                mUnitLabel.Left = mUnitLabel.Left - 60;
                mUnitComboBox.Left = mUnitComboBox.Left - 45;
                mHysLabel.Left = mHysLabel.Left - 60;
                mHysNumericUpDown.Left = mHysNumericUpDown.Left - 70;
                mStepCheckBox.Left = mStepCheckBox.Left - 80;
                mAutoLabel.Left = mAutoLabel.Left - 40;
                mDelayLabel.Left = mDelayLabel.Left - 10;
            }

            // Russian
            else if (OptionManager.getInstance().Language == 5)
            {
                mEnableCheckBox.Font = new Font(fontFamily, 7.2f);
                mSilenceRadioButton.Left = mSilenceRadioButton.Left + 10;
                mGameRadioButton.Left = mGameRadioButton.Left + 50;

                mPresetLabel.Left = mPresetLabel.Left - 170;
                mPresetLoadButton.Left = mPresetLoadButton.Left - 105;
                mPresetSaveButton.Left = mPresetSaveButton.Left - 105;
                mUnitLabel.Left = mUnitLabel.Left - 100;
                mUnitComboBox.Left = mUnitComboBox.Left - 95;
                mHysLabel.Left = mHysLabel.Left - 100;
                mHysNumericUpDown.Left = mHysNumericUpDown.Left - 80;
                mStepCheckBox.Left = mStepCheckBox.Left - 80;
                mAutoLabel.Left = mAutoLabel.Left - 25;
                mAutoNumericUpDown.Left = mAutoNumericUpDown.Left - 15;
                mDelayLabel.Left = mDelayLabel.Left - 30;

                mPresetLoadButton.Font = new Font(fontFamily, 7.0f);
                mPresetSaveButton.Font = new Font(fontFamily, 7.0f);
            }

            // German
            else if (OptionManager.getInstance().Language == 6)
            {
                mPresetLabel.Left = mPresetLabel.Left - 55;
                mPresetLoadButton.Left = mPresetLoadButton.Left - 15;
                mPresetSaveButton.Left = mPresetSaveButton.Left - 15;
                mUnitLabel.Left = mUnitLabel.Left - 15;
                mUnitComboBox.Left = mUnitComboBox.Left;
                mHysLabel.Left = mHysLabel.Left - 10;
                mHysNumericUpDown.Left = mHysNumericUpDown.Left - 15;
                mStepCheckBox.Left = mStepCheckBox.Left - 25;
                mAutoLabel.Left = mAutoLabel.Left - 25;
                mAutoNumericUpDown.Left = mAutoNumericUpDown.Left - 25;
                mDelayLabel.Left = mDelayLabel.Left - 40;

                mPresetLoadButton.Font = new Font(fontFamily, 8.0f);
                mPresetSaveButton.Font = new Font(fontFamily, 8.0f);
            }

            // Chinese
            else if (OptionManager.getInstance().Language == 7)
            {
                mHysNumericUpDown.Left = mHysNumericUpDown.Left - 30;
                mStepCheckBox.Left = mStepCheckBox.Left - 15;
            }
        }

        private void initControl()
        {
            ControlManager controlManager = ControlManager.getInstance();

            mEnableCheckBox.Checked = ControlManager.getInstance().IsEnable;

            mNormalRadioButton.Checked = (mModeType == MODE_TYPE.NORMAL);
            mSilenceRadioButton.Checked = (mModeType == MODE_TYPE.SILENCE);
            mPerformanceRadioButton.Checked = (mModeType == MODE_TYPE.PERFORMANCE);
            mGameRadioButton.Checked = (mModeType == MODE_TYPE.GAME);

            mNormalRadioButton.Click += onRadioButtonClick;
            mSilenceRadioButton.Click += onRadioButtonClick;
            mPerformanceRadioButton.Click += onRadioButtonClick;
            mGameRadioButton.Click += onRadioButtonClick;

            //this.mAddTempListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            mAddTempListView.Columns.Add("1", 20, HorizontalAlignment.Center);
            mAddTempListView.Columns.Add("2", 240, HorizontalAlignment.Left);
            mAddTempListView.SelectedIndexChanged += onAddTempListViewIndexChanged;

            //this.mAddFanListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            mAddFanListView.Columns.Add("1", 20, HorizontalAlignment.Center);
            mAddFanListView.Columns.Add("2", 240, HorizontalAlignment.Left);

            //this.mFanListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
            mFanListView.Columns.Add("1", 20, HorizontalAlignment.Center);
            mFanListView.Columns.Add("2", 240, HorizontalAlignment.Left);
            mFanListView.SelectedIndexChanged += onFanListViewIndexChanged;

            mUnitComboBox.Items.Add("1");
            mUnitComboBox.Items.Add("5");
            mUnitComboBox.Items.Add("10");
            mUnitComboBox.SelectedIndex = 1;
            mUnitComboBox.SelectedIndexChanged += onUnitComboBoxIndexChanged;
            
            mHysNumericUpDown.ValueChanged += onHysNumericValueChanged;

            mAutoNumericUpDown.ValueChanged += onAutoNumericUpDownValueChanged;

            mDelayNumericUpDown.ValueChanged += onDelayTimeUpDownValueChanged;

            var tempBaseList = HardwareManager.getInstance().TempBaseList;
            var controlBaseList = HardwareManager.getInstance().ControlBaseList;

            for (int i = 0; i < tempBaseList.Count; i++)
            {
                var item = mAddTempListView.Items.Add("");
                item.SubItems.Add(tempBaseList[i].Name);

                var controlData = this.getControlData(i);
                if (controlData != null && controlData.FanDataList.Count > 0)
                {
                    item.Text = "●";
                }
            }

            for (int i = 0; i < controlBaseList.Count; i++)
            {
                var item = mAddFanListView.Items.Add("");
                item.SubItems.Add(controlBaseList[i].Name);
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

            // line
            mPointList = new PointPairList();
            for (int i = 0; i < FanData.MAX_FAN_VALUE_SIZE_5; i++)
            {
                mPointList.Add(5 * i, 50);
            }

            var type = OptionManager.getInstance().getNowTheme();
            var lineColor = (type == THEME_TYPE.DARK) ? Color.FromArgb(255, 82, 162, 242) : Color.Blue;
            mLineItem = mGraph.GraphPane.AddCurve(StringLib.Graph, mPointList, lineColor, SymbolType.Circle);
            mLineItem.Line.Width = 2.0f;
            mLineItem.Symbol.Size = 10.0f;
            mLineItem.Symbol.Fill = new Fill(Color.White);

            mAutoPolyObj = new ZedGraph.PolyObj
            {
                Points = new[]
                {
                    new ZedGraph.PointD(0, 0),
                    new ZedGraph.PointD(0, 100),
                    new ZedGraph.PointD(0, 0),
                },
                Fill = new ZedGraph.Fill(Color.White),
                ZOrder = ZedGraph.ZOrder.B_BehindLegend,
            };
            mGraph.GraphPane.GraphObjList.Add(mAutoPolyObj);

            var pointColor = (type == THEME_TYPE.DARK) ? Color.FromArgb(255, 244, 75, 86) : Color.Red;
            mNowPoint = new PointObj(50, 50, 10.0, 10.0, ZedGraph.SymbolType.Circle, pointColor);
            mNowPoint.Fill = new ZedGraph.Fill(pointColor);
            mNowPoint.ZOrder = ZedGraph.ZOrder.A_InFront;
            mGraph.GraphPane.GraphObjList.Add(mNowPoint);

            mPresetLabel.Visible = false;
            mPresetLoadButton.Visible = false;
            mPresetSaveButton.Visible = false;
            mUnitLabel.Visible = false;
            mUnitComboBox.Visible = false;
            mGraph.Visible = false;
            mAutoLabel.Visible = false;
            mAutoNumericUpDown.Visible = false;
            mStepCheckBox.Visible = false;
            mHysLabel.Visible = false;
            mHysNumericUpDown.Visible = false;
            mDelayLabel.Visible = false;
            mDelayNumericUpDown.Visible = false;
            mDelayLabel2.Visible = false;
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

            if (mUnitComboBox.SelectedIndex == 0)
            {
                mAutoNumericUpDown.Increment = 1;
                mAutoNumericUpDown.Value = mSelectedFanData.Auto;
            }
            else if (mUnitComboBox.SelectedIndex == 1)
            {
                mAutoNumericUpDown.Increment = 5;
                mAutoNumericUpDown.Value = mSelectedFanData.Auto / 5 * 5;
            }
            else
            {
                mAutoNumericUpDown.Increment = 10;
                mAutoNumericUpDown.Value = mSelectedFanData.Auto / 10 * 10;
            }

            //mGraph.GraphPane.CurveList.Clear();
            mPointList.Clear();
            for (int i = 0; i < mSelectedFanData.getMaxFanValue(); i++)
            {
                mPointList.Add(mSelectedFanData.getDivideValue() * i, mSelectedFanData.ValueList[i]);
            }

            int value = mSelectedFanData.Auto;
            mAutoPolyObj.Points = new[]
                {
                    new ZedGraph.PointD(0, 0),
                    new ZedGraph.PointD(value, 0),
                    new ZedGraph.PointD(value, 100),
                    new ZedGraph.PointD(0, 100),
                    new ZedGraph.PointD(0, 0),
                };

            mGraph.Refresh();
        }

        private void onRadioButtonClick(object sender, EventArgs e)
        {
            MODE_TYPE modeType = MODE_TYPE.NORMAL;
            if(sender == mNormalRadioButton)
                modeType = MODE_TYPE.NORMAL;
            else if (sender == mSilenceRadioButton)
                modeType = MODE_TYPE.SILENCE;
            else if (sender == mPerformanceRadioButton)
                modeType = MODE_TYPE.PERFORMANCE;
            else
                modeType = MODE_TYPE.GAME;
            if (modeType == mModeType)
                return;

            mModeType = modeType;

            mNormalRadioButton.Checked = (mModeType == MODE_TYPE.NORMAL);
            mSilenceRadioButton.Checked = (mModeType == MODE_TYPE.SILENCE);
            mPerformanceRadioButton.Checked = (mModeType == MODE_TYPE.PERFORMANCE);
            mGameRadioButton.Checked = (mModeType == MODE_TYPE.GAME);

            this.onAddTempListViewIndexChanged(null, EventArgs.Empty);

            for (int i = 0; i < mAddTempListView.Items.Count; i++)
            {
                var item = mAddTempListView.Items[i];
                var controlData = this.getControlData(i);
                if (controlData != null && controlData.FanDataList.Count > 0)
                {
                    item.Text = "●";
                }
                else
                {
                    item.Text = "";
                }
            }
        }

        private void onAddTempListViewIndexChanged(object sender, EventArgs e)
        {
            if (mAddTempListView.SelectedItems.Count == 0)
            {
                Console.WriteLine("mAddTempListView.SelectedItems.Count : {0}", mAddTempListView.SelectedItems.Count);
                setUseFanTextToAddFanListView();
                return;
            }

            mPresetLabel.Visible = false;
            mPresetLoadButton.Visible = false;
            mPresetSaveButton.Visible = false;
            mUnitLabel.Visible = false;
            mUnitComboBox.Visible = false;
            mGraph.Visible = false;
            mAutoLabel.Visible = false;
            mAutoNumericUpDown.Visible = false;
            mStepCheckBox.Visible = false;
            mHysLabel.Visible = false;
            mHysNumericUpDown.Visible = false;
            mDelayLabel.Visible = false;
            mDelayNumericUpDown.Visible = false;
            mDelayLabel2.Visible = false;
            mSelectedFanData = null;

            mListViewBaseControlList.Clear();
            mFanListView.Items.Clear();

            mSelectedTempIndex = mAddTempListView.SelectedItems[0].Index;
            Console.WriteLine("mAddTempListView.SelectedItems[0].Index : {0}", mSelectedTempIndex);

            var controlData = this.getControlData(mSelectedTempIndex);
            if(controlData == null)
            {
                setUseFanTextToAddFanListView();
                return;
            }

            var controlBaseMap = HardwareManager.getInstance().ControlBaseMap;
            for (int i = 0; i < controlData.FanDataList.Count; i++)
            {
                var fanData = controlData.FanDataList[i];

                string fanID = fanData.ID;
                if (controlBaseMap.ContainsKey(fanID) == false)
                {
                    continue;
                }

                var device = controlBaseMap[fanID];
                mListViewBaseControlList.Add(device);

                var item = mFanListView.Items.Add("");
                item.SubItems.Add(device.Name);
            }
            setUseFanTextToAddFanListView();
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
            if (mAddTempListView.Items.Count == 0 ||
                mSelectedTempIndex == -1 ||
                mAddFanListView.Items.Count == 0 ||
                mSelectedFanData == null ||
                mNowPoint == null ||
                mIsUpdateGraph == false)
            {
                return;
            }

            var items = mFanListView.SelectedItems;
            if (items == null || items.Count == 0)
                return;

            int itemIndex = items[0].Index;
            var hardwareManager = HardwareManager.getInstance();
            var tempBaseList = hardwareManager.TempBaseList;
            var controlBaseList = hardwareManager.ControlBaseList;

            if (mSelectedTempIndex >= tempBaseList.Count || itemIndex >= mListViewBaseControlList.Count)
                return;

            var tempDevice = tempBaseList[mSelectedTempIndex];
            var controlDevice = mListViewBaseControlList[itemIndex];
            if (tempDevice == null || controlDevice == null)
                return;
            
            mNowPoint.Location.X = (double)tempDevice.Value;
            mNowPoint.Location.Y = (double)controlDevice.Value;
            
            mGraph.Refresh();
        }

        private ControlData getControlData(int tempIndex)
        {
            var tempBaseList = HardwareManager.getInstance().TempBaseList;
            if (tempIndex >= tempBaseList.Count || tempIndex < 0)
                return null;

            string id = tempBaseList[tempIndex].ID;

            ControlData controlData = null;
            int modeIndex = (int)mModeType;
            for (int i = 0; i < mControlDataList[modeIndex].Count; i++)
            {
                var tempControlData = mControlDataList[modeIndex][i];
                if (tempControlData.ID.Equals(id) == true)
                {
                    controlData = tempControlData;
                    break;
                }
            }
            return controlData;
        }

        private FanData getFanData(int tempIndex, int fanIndex)
        {
            var controlData = this.getControlData(tempIndex);
            if (controlData == null)
                return null;

            if (fanIndex >= mListViewBaseControlList.Count)
                return null;

            var device = mListViewBaseControlList[fanIndex];
            for (int i = 0; i < controlData.FanDataList.Count; i++)
            {
                var fanData = controlData.FanDataList[i];
                if (fanData.ID.Equals(device.ID) == true)
                {
                    return fanData;
                }
            }
            return null;
        }

        private FanData getFanData(int tempIndex, string id)
        {
            var controlData = this.getControlData(tempIndex);
            if (controlData == null)
                return null;

            for (int i = 0; i < controlData.FanDataList.Count; i++)
            {
                var fanData = controlData.FanDataList[i];
                if (fanData.ID.Equals(id) == true)
                {
                    return fanData;
                }
            }
            return null;
        }

        private void setUseFanTextToAddFanListView()
        {
            int tempIndex = mSelectedTempIndex;
            var controlData = this.getControlData(tempIndex);
            if (mAddTempListView.SelectedItems.Count == 0 || controlData == null || controlData.FanDataList.Count == 0)
            {
                for (int i = 0; i < mAddFanListView.Items.Count; i++)
                {
                    var item = mAddFanListView.Items[i];
                    item.Text = "";
                }
                return;
            }

            var controlBaseList = HardwareManager.getInstance().ControlBaseList;
            for (int i = 0; i < mAddFanListView.Items.Count; i++)
            {
                var item = mAddFanListView.Items[i];
                var controlDevice = controlBaseList[i];
                var fanData = this.getFanData(tempIndex, controlDevice.ID);
                item.Text = (fanData != null) ? "●" : "";
            }
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
                mAutoLabel.Visible = false;
                mAutoNumericUpDown.Visible = false;
                mStepCheckBox.Visible = false;
                mHysLabel.Visible = false;
                mHysNumericUpDown.Visible = false;
                mDelayLabel.Visible = false;
                mDelayNumericUpDown.Visible = false;
                mDelayLabel2.Visible = false;
                mSelectedFanData = null;
                return;
            }

            mPresetLabel.Visible = true;
            mPresetLoadButton.Visible = true;
            mPresetSaveButton.Visible = true;
            mUnitLabel.Visible = true;
            mUnitComboBox.Visible = true;
            mGraph.Visible = true;
            mAutoLabel.Visible = true;
            mAutoNumericUpDown.Visible = true;
            mStepCheckBox.Visible = true;
            mHysLabel.Visible = true;
            mHysNumericUpDown.Visible = true;
            mDelayLabel.Visible = true;
            mDelayNumericUpDown.Visible = true;
            mDelayLabel2.Visible = true;

            var item = items[0];
            int itemIndex = item.Index;
            mSelectedFanData = this.getFanData(mSelectedTempIndex, itemIndex);
            if (mSelectedFanData == null)
                return;

            // setGraphFromSelectedFanData
            this.setGraphFromSelectedFanData();

            mUnitComboBox.SelectedIndex = (int)mSelectedFanData.Unit;
            mStepCheckBox.Checked = mSelectedFanData.IsStep;
            mLineItem.Line.StepType = (mStepCheckBox.Checked == true) ? StepType.ForwardStep : StepType.NonStep;
            mHysNumericUpDown.Enabled = mStepCheckBox.Checked;
            mHysNumericUpDown.Value = mSelectedFanData.Hysteresis;
            mDelayNumericUpDown.Value = mSelectedFanData.DelayTime;

            if (mUnitComboBox.SelectedIndex == 0)
            {
                mAutoNumericUpDown.Increment = 1;
                mAutoNumericUpDown.Value = mSelectedFanData.Auto;
            }
            else if (mUnitComboBox.SelectedIndex == 1)
            {
                mAutoNumericUpDown.Increment = 5;
                mAutoNumericUpDown.Value = mSelectedFanData.Auto / 5 * 5;
            }
            else
            {
                mAutoNumericUpDown.Increment = 10;
                mAutoNumericUpDown.Value = mSelectedFanData.Auto / 10 * 10;
            }

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

        private void onAutoNumericUpDownValueChanged(object sender, EventArgs e)
        {
            int value = Decimal.ToInt32(mAutoNumericUpDown.Value);

            mAutoPolyObj.Points = new[]
                {
                    new ZedGraph.PointD(0, 0),
                    new ZedGraph.PointD(value, 0),
                    new ZedGraph.PointD(value, 100),
                    new ZedGraph.PointD(0, 100),
                    new ZedGraph.PointD(0, 0),
                };

            if (mSelectedFanData != null)
            {
                mSelectedFanData.Auto = value;
            }

            mGraph.Refresh();
        }

        private void onDelayTimeUpDownValueChanged(object sender, EventArgs e)
        {
            if (mSelectedFanData != null)
                mSelectedFanData.DelayTime = Decimal.ToInt32(mDelayNumericUpDown.Value);
        }

        private void onAddButtonClick(object sender, EventArgs e)
        {
            if (mAddTempListView.SelectedItems.Count == 0 || mAddFanListView.SelectedItems.Count == 0)
                return;

            int modeIndex = (int)mModeType;
            int tempIndex = mSelectedTempIndex;

            var tempBaseList = HardwareManager.getInstance().TempBaseList;
            var controlBaseList = HardwareManager.getInstance().ControlBaseList;

            if (tempIndex >= tempBaseList.Count || tempIndex < 0)
                return;

            var tempDevice = tempBaseList[tempIndex];
            for (int i = 0; i < mAddFanListView.SelectedItems.Count; i++)
            {
                int fanIndex = mAddFanListView.SelectedItems[i].Index;
                if (fanIndex >= controlBaseList.Count || fanIndex < 0)
                    continue;

                var controlDevice = controlBaseList[fanIndex];

                var controlData = this.getControlData(tempIndex);
                if (controlData == null)
                {
                    controlData = new ControlData(tempDevice.ID);
                    mControlDataList[modeIndex].Add(controlData);
                }

                var fanData = this.getFanData(tempIndex, controlDevice.ID);
                if (fanData == null)
                {
                    fanData = new FanData(controlDevice.ID, FanValueUnit.Size_5, true, 0, 0, 0);
                    controlData.FanDataList.Add(fanData);

                    mListViewBaseControlList.Add(controlDevice);
                    var item = mFanListView.Items.Add("");
                    item.SubItems.Add(controlDevice.Name);
                }
            }

            var tempItem = mAddTempListView.Items[tempIndex];
            tempItem.Text = "●";
            setUseFanTextToAddFanListView();
        }

        private void onRemoveButtonClick(object sender, EventArgs e)
        {
            var items = mFanListView.SelectedItems;
            if (items == null || items.Count == 0)
                return;

            var item = items[0];
            int itemIndex = item.Index;
            var controlData = this.getControlData(mSelectedTempIndex);
            if (controlData == null)
                return;

            mListViewBaseControlList.RemoveAt(itemIndex);
            controlData.FanDataList.RemoveAt(itemIndex);

            mSelectedFanData = null;
            mFanListView.Items.Remove(item);

            if (controlData.FanDataList.Count == 0)
            {
                var tempItem = mAddTempListView.Items[mSelectedTempIndex];
                tempItem.Text = "";
            }

            setUseFanTextToAddFanListView();
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
                    bool isStep = (rootObject.ContainsKey("step") == true) ? rootObject.Value<bool>("step") : true;
                    int hysteresis = (rootObject.ContainsKey("hysteresis") == true) ? rootObject.Value<int>("hysteresis") : 0;
                    int auto = (rootObject.ContainsKey("auto") == true) ? rootObject.Value<int>("auto") : 0;
                    int delay = (rootObject.ContainsKey("delay") == true) ? rootObject.Value<int>("delay") : 0;

                    var unit = (rootObject.ContainsKey("unit") == true) ? (FanValueUnit)rootObject.Value<int>("unit") : FanValueUnit.Size_5;
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
                    mSelectedFanData.Auto = auto;
                    mSelectedFanData.DelayTime = delay;
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
                    mDelayNumericUpDown.Value = mSelectedFanData.DelayTime;

                    this.onUpdateTimer();
                }
                catch
                {
                    DarkMessageBox.ShowError(StringLib.Failed_to_read_preset_file, "", DarkDialogButton.Ok);
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
                rootObject["auto"] = mSelectedFanData.Auto;
                rootObject["delay"] = mSelectedFanData.DelayTime;

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
            for (int i = 0; i < mControlDataList.Length; i++)
            {
                for (int j = 0; j < mControlDataList[i].Count; j++)
                {
                    for (int k = 0; k < mControlDataList[i][j].FanDataList.Count; k++)
                    {
                        mControlDataList[i][j].FanDataList[k].LastChangedTemp = 0;
                        mControlDataList[i][j].FanDataList[k].LastChangedValue = 0;
                    }
                }
                ControlManager.getInstance().setControlDataList((MODE_TYPE)i, mControlDataList[i]);
            }

            ControlManager.getInstance().Width = mNormalLastSize.Width;
            ControlManager.getInstance().Height = mNormalLastSize.Height;
            ControlManager.getInstance().IsMaximize = (this.WindowState == FormWindowState.Maximized);

            ControlManager.getInstance().ModeType = mModeType;
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
