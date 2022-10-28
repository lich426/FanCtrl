using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FanCtrl
{
    public partial class LiquidctlForm : Form
    {
        private List<LiquidctlControl> mLiquidctlControlList = new List<LiquidctlControl>();
        private List<string> mLiquidctlCommandList = new List<string>();

        public LiquidctlForm()
        {
            InitializeComponent();
            this.localizeComponent();

            mLocationTextBox.Text = LiquidctlManager.getInstance().LiquidctlPath;
            mControlListView.MouseDoubleClick += onControlListViewDoubleClick;
            mCommandListView.MouseDoubleClick += onCommandListViewDoubleClick;

            int count = LiquidctlManager.getInstance().getLiquidctlControlCount();
            for (int i = 0; i < count; i++)
            {
                var control = LiquidctlManager.getInstance().getLiquidctlControl(i);
                mLiquidctlControlList.Add(control);

                var item = new ListViewItem(control.DeviceName);
                item.SubItems.Add(control.ChannelName);
                item.SubItems.Add(control.Address);
                mControlListView.Items.Add(item);
            }

            count = LiquidctlManager.getInstance().getLiquidctlCommandCount();
            for (int i = 0; i < count; i++)
            {
                var command = LiquidctlManager.getInstance().getLiquidctlCommand(i);
                mLiquidctlCommandList.Add(command);

                var item = new ListViewItem(command);
                mCommandListView.Items.Add(item);
            }

            this.ActiveControl = mLocationButton;
        }

        private void localizeComponent()
        {
            this.Text = StringLib.liquidctl_Setting;
            mLocationGroupBox.Text = StringLib.Location;
            mControlGroupBox.Text = StringLib.Fan_control;
            mCommandGroupBox.Text = StringLib.Command;
            mLocationButton.Text = StringLib.Open;
            mControlAddButton.Text = StringLib.Add;
            mControlRemoveButton.Text = StringLib.Remove;
            mCommandAddButton.Text = StringLib.Add;
            mCommandRemoveButton.Text = StringLib.Remove;
            mOKButton.Text = StringLib.OK;
        }

        private void onLocationButtonClick(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "EXE file|*.exe|All files(*.*)|*.*";
            ofd.InitialDirectory = Path.GetDirectoryName(mLocationTextBox.Text);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                mLocationTextBox.Text = ofd.FileName;
            }
        }

        private void onControlAddButtonClick(object sender, EventArgs e)
        {
            var form = new LiquidctlFanControlForm("", "");
            if (form.ShowDialog() == DialogResult.OK)
            {
                var item = new ListViewItem(form.DeviceName);
                item.SubItems.Add(form.ChannelName);
                item.SubItems.Add(form.Address);
                mControlListView.Items.Add(item);

                string name = string.Format("Control #{0}", mLiquidctlControlList.Count + 1);
                var control = new LiquidctlControl(mLiquidctlControlList.Count, form.DeviceName, name, form.Address, form.ChannelName);
                mLiquidctlControlList.Add(control);
            }
        }

        private void onControlRemoveButtonClick(object sender, EventArgs e)
        {
            if (mControlListView.SelectedItems.Count > 0)
            {
                int count = mControlListView.SelectedItems.Count;
                for (int i = 0; i < count; i++)
                {
                    int index = mControlListView.SelectedItems[0].Index;
                    mLiquidctlControlList.RemoveAt(index);
                    mControlListView.Items.RemoveAt(index);
                }
            }
        }

        private void onControlListViewDoubleClick(object sender, MouseEventArgs e)
        {
            if (mControlListView.SelectedItems.Count == 1)
            {
                var item = mControlListView.SelectedItems[0];

                string channel = item.SubItems[1].Text;
                string address = item.SubItems[2].Text;                
                var form = new LiquidctlFanControlForm(address, channel);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    item.Text = form.DeviceName;
                    item.SubItems[1].Text = form.ChannelName;
                    item.SubItems[2].Text = form.Address;
                }
            }
        }

        private void onCommandAddButtonClick(object sender, EventArgs e)
        {
            var form = new LiquidctlCommandForm("");
            if (form.ShowDialog() == DialogResult.OK)
            {
                var item = new ListViewItem(form.Command);
                mCommandListView.Items.Add(item);
                mLiquidctlCommandList.Add(form.Command);
            }
        }

        private void onCommandRemoveButtonClick(object sender, EventArgs e)
        {
            if (mCommandListView.SelectedItems.Count > 0)
            {
                int count = mCommandListView.SelectedItems.Count;
                for (int i = 0; i < count; i++)
                {
                    int index = mCommandListView.SelectedItems[0].Index;
                    mLiquidctlCommandList.RemoveAt(index);
                    mCommandListView.Items.RemoveAt(index);
                }
            }
        }

        private void onCommandListViewDoubleClick(object sender, MouseEventArgs e)
        {
            if (mCommandListView.SelectedItems.Count == 1)
            {
                var item = mCommandListView.SelectedItems[0];
                var form = new LiquidctlCommandForm(item.Text);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    item.Text = form.Command;
                }
            }
        }

        private void onOKButtonClick(object sender, EventArgs e)
        {
            var controlList = new List<LiquidctlControl>();
            for (int i = 0; i < mControlListView.Items.Count; i++)
            {
                var item = mControlListView.Items[i];

                string deviceName = item.Text;
                string channel = item.SubItems[1].Text;
                string address = item.SubItems[2].Text;

                var control = new LiquidctlControl(i, deviceName, mLiquidctlControlList[i].Name, address, channel);
                controlList.Add(control);
            }

            var commandList = new List<string>();
            for (int i = 0; i < mCommandListView.Items.Count; i++)
            {
                var item = mCommandListView.Items[i];
                string command = item.Text;
                commandList.Add(command);
            }

            LiquidctlManager.getInstance().write(controlList, commandList);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
