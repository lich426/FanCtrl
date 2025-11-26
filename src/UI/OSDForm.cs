using FanCtrl.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FanCtrl
{
    public partial class OSDForm : ThemeForm
    {
        private List<OSDItem> mComboBoxItemList = new List<OSDItem>();

        private List<OSDGroup> mGroupList = new List<OSDGroup>();

        private ListViewItem mGroupEditItem = null;
        private ListViewItem.ListViewSubItem mGroupEditSubItem = null;

        public event EventHandler onApplyCallback;

        public OSDForm()
        {
            InitializeComponent();
            this.localizeComponent();

            mGroupListView.Columns.Add("Color", 50);
            mGroupListView.Columns.Add("Name", 130);
            mGroupListView.SelectedIndexChanged += onGroupListViewIndexChanged;

            mGroupListView.MouseDoubleClick += onGroupListViewMouseDoubleClick;
            mGroupEditTextBox.Leave += onGroupEditTextBoxLeave;
            mGroupEditTextBox.Hide();

            mItemListView.Columns.Add("Color", 50);
            mItemListView.Columns.Add("Item", 280);

            mDigitNumericUpDown.ValueChanged += onDigitNumericUpDownValueChanged;

            mItemComboBox.DropDownHeight = 500;

            this.enableItemConrol(false);

            var hardwareManager = HardwareManager.getInstance();
            var osdSensorList = hardwareManager.OSDSensorList;
            for (int i = 0; i < osdSensorList.Count; i++)
            {
                var sensor = osdSensorList[i];
                var item = new OSDItem();
                item.UnitType = sensor.UnitType;
                item.ID = sensor.ID;
                mComboBoxItemList.Add(item);
                mItemComboBox.Items.Add(sensor.Prefix + sensor.Name);
            }

            mItemComboBox.SelectedIndex = 0;

            mEnableCheckBox.Checked = OSDManager.getInstance().IsEnable;
            mSystemTimeCheckBox.Checked = OSDManager.getInstance().IsTime;

            mGroupList = OSDManager.getInstance().getCloneGroupList();
            for (int i = 0; i < mGroupList.Count; i++)
            {
                var group = mGroupList[i];

                var item = new ListViewItem();
                if(group.IsColor == false)
                {
                    item.Text = "Default";
                }
                else
                {
                    item.Text = "";
                    item.BackColor = group.Color;
                }
                
                item.SubItems.Add(group.Name);
                item.UseItemStyleForSubItems = false;
                mGroupListView.Items.Add(item);
            }
        }

        private void localizeComponent()
        {
            this.mEnableCheckBox.Text = StringLib.Enable_OSD;
            mGroupGroupBox.Text = StringLib.Groups;
            mItemGroupBox.Text = StringLib.Items;
            mDigitLabel.Text = StringLib.Align_digits;
            mGroupAddButton.Text = StringLib.Add;
            mGroupColorButton.Text = StringLib.Color;
            mGroupRemoveButton.Text = StringLib.Remove;
            mSystemTimeCheckBox.Text = StringLib.Show_system_time;
            mItemAddButton.Text = StringLib.Add;
            mItemColorButton.Text = StringLib.Color;
            mItemRemoveButton.Text = StringLib.Remove;
            mApplyButton.Text = StringLib.Apply;
            mOKButton.Text = StringLib.OK;

            FontFamily fontFamily = null;
            try
            {
                fontFamily = new FontFamily("Gulim");
            }
            catch
            {
                fontFamily = FontFamily.GenericSansSerif;
            }

            // japanese
            if (OptionManager.getInstance().Language == 2)
            {
                mDigitLabel.Left = mDigitLabel.Left + 20;
            }
            // french
            else if (OptionManager.getInstance().Language == 3)
            {
                mDigitLabel.Left = mDigitLabel.Left - 40;
            }
            // spanish
            else if (OptionManager.getInstance().Language == 4)
            {
                mDigitLabel.Left = mDigitLabel.Left - 40;
            }
            // Russian
            else if (OptionManager.getInstance().Language == 5)
            {
                mEnableCheckBox.Font = new Font(fontFamily, 7.2f);
                mDigitLabel.Left = mDigitLabel.Left - 60;
            }
            // German
            else if (OptionManager.getInstance().Language == 6)
            {
                mGroupRemoveButton.Font = new Font(fontFamily, 8.5f);
                mItemRemoveButton.Font = new Font(fontFamily, 8.5f);
                mDigitLabel.Left = mDigitLabel.Left - 35;
            }
        }

        private void onGroupListViewIndexChanged(object sender, EventArgs e)
        {
            var items = mGroupListView.SelectedItems;
            if (items == null || items.Count == 0)
            {
                mItemListView.Items.Clear();
                this.enableItemConrol(false);
                return;
            }
            this.enableItemConrol(true);

            try
            {
                int index = mGroupListView.SelectedItems[0].Index;
                if (index < 0)
                    return;

                var hardwareManager = HardwareManager.getInstance();
                var osdSensorMap = hardwareManager.OSDSensorMap;

                var group = mGroupList[index];
                mDigitNumericUpDown.Value = group.Digit;
                for (int i = 0; i < group.ItemList.Count; i++)
                {
                    var item = group.ItemList[i];
                    var listItem = new ListViewItem();

                    if(item.IsColor == false)
                    {
                        listItem.Text = "Default";
                    }
                    else
                    {
                        listItem.Text = "";
                        listItem.BackColor = item.Color;
                    }

                    string id = item.ID;
                    if (osdSensorMap.ContainsKey(id) == false)
                    {
                        continue;
                    }

                    var sensor = osdSensorMap[id];
                    listItem.SubItems.Add(sensor.Prefix + sensor.Name);
                    listItem.UseItemStyleForSubItems = false;
                    mItemListView.Items.Add(listItem);
                }
            }
            catch { }
        }

        private void onGroupListViewMouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                var item = mGroupListView.GetItemAt(e.X, e.Y);
                if (item == null)
                    return;

                var subItem = item.GetSubItemAt(e.X, e.Y);
                int subIndex = item.SubItems.IndexOf(subItem);
                if (subIndex != 1)
                    return;

                mGroupEditItem = item;
                mGroupEditSubItem = subItem;
                int left = mGroupEditSubItem.Bounds.Left + 2;
                int width = mGroupEditSubItem.Bounds.Width;
                mGroupEditTextBox.SetBounds(left + mGroupListView.Left, mGroupEditSubItem.Bounds.Top + mGroupListView.Top, width, mGroupEditSubItem.Bounds.Height);
                mGroupEditTextBox.Text = mGroupEditSubItem.Text;
                mGroupEditTextBox.Show();
                mGroupEditTextBox.Focus();
            }
            catch { }           
        }

        private void onGroupEditTextBoxLeave(object sender, EventArgs e)
        {
            try
            {
                mGroupEditTextBox.Hide();
                mGroupEditSubItem.Text = mGroupEditTextBox.Text;
                mGroupList[mGroupEditItem.Index].Name = mGroupEditTextBox.Text;

                mGroupEditItem = null;
                mGroupEditSubItem = null;
            }
            catch { }
        }

        private void enableItemConrol(bool isEnable)
        {
            //mItemComboBox.Enabled = isEnable;
            mDigitNumericUpDown.Enabled = isEnable;
            //mItemListView.Enabled = isEnable;
            mItemAddButton.Enabled = isEnable;
            mItemUpButton.Enabled = isEnable;
            mItemDownButton.Enabled = isEnable;
            mItemColorButton.Enabled = isEnable;
            mItemRemoveButton.Enabled = isEnable;
        }

        private bool isSelectedGroupListView()
        {
            var items = mGroupListView.SelectedItems;
            if (items == null || items.Count == 0)
                return false;
            return true;
        }

        private bool isSelectedItemListView()
        {
            var items = mItemListView.SelectedItems;
            if (items == null || items.Count == 0)
                return false;
            return true;
        }

        private void onGroupAddButtonClick(object sender, EventArgs e)
        {
            var item = new ListViewItem();
            item.Text = "Default";
            item.SubItems.Add(mGroupAddTextBox.Text);
            item.UseItemStyleForSubItems = false;
            mGroupListView.Items.Add(item);

            var group = new OSDGroup();
            group.Name = mGroupAddTextBox.Text;
            group.IsColor = false;
            mGroupList.Add(group);

            mGroupAddTextBox.Clear();
        }

        private void onGroupUpButtonClick(object sender, EventArgs e)
        {
            if (this.isSelectedGroupListView() == false)
                return;

            try
            {
                int index = mGroupListView.SelectedItems[0].Index;
                if (index == 0)
                    return;

                var item = mGroupListView.SelectedItems[0];
                int nextIndex = index - 1;
                mGroupListView.Items.RemoveAt(index);
                mGroupListView.Items.Insert(nextIndex, item);

                var group = mGroupList[index];
                mGroupList.RemoveAt(index);
                mGroupList.Insert(nextIndex, group);
            }
            catch { }            
        }

        private void onGroupDownButtonClick(object sender, EventArgs e)
        {
            if (this.isSelectedGroupListView() == false)
                return;

            try
            {
                int index = mGroupListView.SelectedItems[0].Index;
                if (index >= mGroupListView.Items.Count - 1)
                    return;

                var item = mGroupListView.SelectedItems[0];
                int nextIndex = index + 1;
                mGroupListView.Items.RemoveAt(index);
                mGroupListView.Items.Insert(nextIndex, item);

                var group = mGroupList[index];
                mGroupList.RemoveAt(index);
                mGroupList.Insert(nextIndex, group);
            }
            catch { }
        }

        private void onGroupColorButtonClick(object sender, EventArgs e)
        {
            if (this.isSelectedGroupListView() == false)
                return;

            try
            {
                int index = mGroupListView.SelectedItems[0].Index;
                var item = mGroupListView.SelectedItems[0];
                var msgBox = MessageBoxEx.Show(StringLib.Change_the_color, StringLib.Color, StringLib.Default_color, StringLib.Set_color, StringLib.Cancel);

                // Default color
                if (msgBox == DialogResult.Yes)
                {
                    item.Text = "Default";
                    item.SubItems[0].BackColor = Color.White;
                    mGroupList[index].IsColor = false;
                }

                // Set color
                else if (msgBox == DialogResult.No)
                {
                    var dialog = new ColorDialog();
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        item.Text = "";
                        item.SubItems[0].BackColor = dialog.Color;
                        mGroupList[index].IsColor = true;
                        mGroupList[index].Color = dialog.Color;
                    }
                }
            }
            catch { }
        }

        private void onGroupRemoveButtonClick(object sender, EventArgs e)
        {
            if (this.isSelectedGroupListView() == false)
                return;

            try
            {
                int index = mGroupListView.SelectedItems[0].Index;
                mGroupListView.Items.RemoveAt(index);
                mGroupList.RemoveAt(index);
            }
            catch { }
        }

        private void onDigitNumericUpDownValueChanged(object sender, EventArgs e)
        {
            if (this.isSelectedGroupListView() == false)
                return;

            try
            {
                int index = mGroupListView.SelectedItems[0].Index;
                mGroupList[index].Digit = Decimal.ToInt32(mDigitNumericUpDown.Value);
            }
            catch { }
        }

        private void onItemAddButtonClick(object sender, EventArgs e)
        {
            if (this.isSelectedGroupListView() == false)
                return;

            try
            {
                int comboBoxIndex = mItemComboBox.SelectedIndex;
                if (comboBoxIndex < 0)
                    return;

                var item = mComboBoxItemList[comboBoxIndex];

                int groupIndex = mGroupListView.SelectedItems[0].Index;
                var group = mGroupList[groupIndex];

                var listItem = new ListViewItem();
                listItem.Text = "Default";
                listItem.SubItems.Add(mItemComboBox.Items[comboBoxIndex].ToString());
                listItem.UseItemStyleForSubItems = false;
                mItemListView.Items.Add(listItem);

                var osdItem = item.clone();
                group.ItemList.Add(osdItem);
            }
            catch { }
        }

        private void onItemUpButtonClick(object sender, EventArgs e)
        {
            if (this.isSelectedGroupListView() == false || this.isSelectedItemListView() == false)
                return;

            try
            {
                int groupIndex = mGroupListView.SelectedItems[0].Index;
                var group = mGroupList[groupIndex];

                int itemIndex = mItemListView.SelectedItems[0].Index;
                if (itemIndex == 0)
                    return;

                var item = mItemListView.SelectedItems[0];
                int nextIndex = itemIndex - 1;
                mItemListView.Items.RemoveAt(itemIndex);
                mItemListView.Items.Insert(nextIndex, item);

                var osdItem = group.ItemList[itemIndex];
                group.ItemList.RemoveAt(itemIndex);
                group.ItemList.Insert(nextIndex, osdItem);
            }
            catch { }
        }

        private void onItemDownButtonClick(object sender, EventArgs e)
        {
            if (this.isSelectedGroupListView() == false || this.isSelectedItemListView() == false)
                return;

            try
            {
                int groupIndex = mGroupListView.SelectedItems[0].Index;
                var group = mGroupList[groupIndex];

                int itemIndex = mItemListView.SelectedItems[0].Index;
                if (itemIndex >= mItemListView.Items.Count - 1)
                    return;

                var item = mItemListView.SelectedItems[0];
                int nextIndex = itemIndex + 1;
                mItemListView.Items.RemoveAt(itemIndex);
                mItemListView.Items.Insert(nextIndex, item);

                var osdItem = group.ItemList[itemIndex];
                group.ItemList.RemoveAt(itemIndex);
                group.ItemList.Insert(nextIndex, osdItem);
            }
            catch { }
        }

        private void onItemColorButtonClick(object sender, EventArgs e)
        {
            if (this.isSelectedGroupListView() == false || this.isSelectedItemListView() == false)
                return;

            try
            {
                int groupIndex = mGroupListView.SelectedItems[0].Index;
                var group = mGroupList[groupIndex];

                int itemIndex = mItemListView.SelectedItems[0].Index;

                var item = mItemListView.Items[itemIndex];
                var msgBox = MessageBoxEx.Show(StringLib.Change_the_color, StringLib.Color, StringLib.Default_color, StringLib.Set_color, StringLib.Cancel);

                // Default color
                if (msgBox == DialogResult.Yes)
                {
                    item.Text = "Default";
                    item.SubItems[0].BackColor = Color.White;
                    group.ItemList[itemIndex].IsColor = false;
                }

                // Set color
                else if (msgBox == DialogResult.No)
                {
                    var dialog = new ColorDialog();
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        item.Text = "";
                        item.SubItems[0].BackColor = dialog.Color;
                        group.ItemList[itemIndex].IsColor = true;
                        group.ItemList[itemIndex].Color = dialog.Color;
                    }
                }
            }
            catch { }
        }

        private void onItemRemoveButtonClick(object sender, EventArgs e)
        {
            if (this.isSelectedGroupListView() == false || this.isSelectedItemListView() == false)
                return;

            try
            {
                int groupIndex = mGroupListView.SelectedItems[0].Index;
                var group = mGroupList[groupIndex];

                int itemIndex = mItemListView.SelectedItems[0].Index;

                mItemListView.Items.RemoveAt(itemIndex);
                group.ItemList.RemoveAt(itemIndex);
            }
            catch { }
        }

        private void onApplyButtonClick(object sender, EventArgs e)
        {
            OSDManager.getInstance().IsEnable = mEnableCheckBox.Checked;
            OSDManager.getInstance().IsTime = mSystemTimeCheckBox.Checked;            
            OSDManager.getInstance().setGroupList(mGroupList);
            OSDManager.getInstance().write();
            onApplyCallback(sender, e);
        }

        private void onOKButtonClick(object sender, EventArgs e)
        {
            this.onApplyButtonClick(sender, e);
            this.Close();
        }
    }
}
