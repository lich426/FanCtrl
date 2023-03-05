using DarkUI.Forms;
using FanCtrl.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FanCtrl
{
    public partial class PluginForm : ThemeForm
    {
        public PluginForm()
        {
            InitializeComponent();
            this.localizeComponent();

            mClientListView.Columns.Add("", 0);
            mClientListView.Columns.Add(StringLib.IP_address, 130);
            mClientListView.Columns.Add(StringLib.Port_number, 110);

            mTempListView.Columns.Add("", 0);
            mTempListView.Columns.Add("Key", 90);
            mTempListView.Columns.Add("Name", 100);
            mTempListView.MouseDoubleClick += onTempListViewMouseDBClick;

            mFanSpeedListView.Columns.Add("", 0);
            mFanSpeedListView.Columns.Add("Key", 90);
            mFanSpeedListView.Columns.Add("Name", 100);
            mFanSpeedListView.MouseDoubleClick += onFanSpeedListViewMouseDBClick;

            mFanControlListView.Columns.Add("", 0);
            mFanControlListView.Columns.Add("Key", 90);
            mFanControlListView.Columns.Add("Name", 100);
            mFanControlListView.MouseDoubleClick += onFanControlListViewMouseDBClick;

            mPortNumericUpDown.Value = PluginManager.getInstance().Port;

            var tempDeviceList = new List<PluginDevice>();
            var fanDeviceList = new List<PluginDevice>();
            var controlDeviceList = new List<PluginDevice>();
            PluginManager.getInstance().getDeviceList(tempDeviceList, fanDeviceList, controlDeviceList);
            for (int i = 0; i < tempDeviceList.Count; i++)
            {
                var item = new ListViewItem();
                item.SubItems.Add(tempDeviceList[i].Key);
                item.SubItems.Add(tempDeviceList[i].Name);
                mTempListView.Items.Add(item);
            }
            for (int i = 0; i < fanDeviceList.Count; i++)
            {
                var item = new ListViewItem();
                item.SubItems.Add(fanDeviceList[i].Key);
                item.SubItems.Add(fanDeviceList[i].Name);
                mFanSpeedListView.Items.Add(item);
            }
            for (int i = 0; i < controlDeviceList.Count; i++)
            {
                var item = new ListViewItem();
                item.SubItems.Add(controlDeviceList[i].Key);
                item.SubItems.Add(controlDeviceList[i].Name);
                mFanControlListView.Items.Add(item);
            }

            this.FormClosing += (sender, e) =>
            {
                PluginManager.getInstance().onConnectHandler -= onConnectCallback;
                PluginManager.getInstance().onDisconnectHandler -= onDisconnectCallback;
            };

            PluginManager.getInstance().onConnectHandler += onConnectCallback;
            PluginManager.getInstance().onDisconnectHandler += onDisconnectCallback;

            if (PluginManager.getInstance().isStart() == true)
            {
                mServerGroupBox.Text = string.Format("{0}({1})", StringLib.Server, StringLib.Running);
                mServerButton.Text = StringLib.Server_Stop;
                mPortNumericUpDown.Enabled = false;

                var clientList = new List<Tuple<string, int>>();
                PluginManager.getInstance().getClientList(clientList);
                for (int i = 0; i < clientList.Count; i++)
                {
                    var item = new ListViewItem();
                    item.SubItems.Add(clientList[i].Item1);
                    item.SubItems.Add(clientList[i].Item2.ToString());
                }
            }

            this.ActiveControl = mServerButton;
        }

        private void localizeComponent()
        {
            this.Text = StringLib.Plugin;
            mServerGroupBox.Text = string.Format("{0} ({1})", StringLib.Server, StringLib.Stopped);
            mPortLabel.Text = StringLib.Port;
            mServerButton.Text = StringLib.Server_Start;
            mClientGroupBox.Text = StringLib.Clients;
            mTempGroupBox.Text = StringLib.Temperature;
            mFanSpeedGroupBox.Text = StringLib.Fan_speed;
            mFanControlGroupBox.Text = StringLib.Fan_control;
            mTempRemoveButton.Text = StringLib.Remove;
            mTempAddButton.Text = StringLib.Add;
            mFanSpeedRemoveButton.Text = StringLib.Remove;
            mFanSpeedAddButton.Text = StringLib.Add;
            mFanControlRemoveButton.Text = StringLib.Remove;
            mFanControlAddButton.Text = StringLib.Add;
            mOKButton.Text = StringLib.OK;
        }

        private void onConnectCallback(Tuple<string, int> tuple)
        {
            this.BeginInvoke(new Action(delegate ()
            {
                var item = new ListViewItem();
                item.SubItems.Add(tuple.Item1);
                item.SubItems.Add(tuple.Item2.ToString());
                mClientListView.Items.Add(item);
            }));
        }

        private void onDisconnectCallback(Tuple<string, int> tuple)
        {
            this.BeginInvoke(new Action(delegate ()
            {
                for (int i = 0; i < mClientListView.Items.Count; i++)
                {
                    var item = mClientListView.Items[i];
                    string ipString = item.SubItems[1].Text;
                    string port = item.SubItems[2].Text;

                    if (tuple.Item1.CompareTo(ipString) == 0 && tuple.Item2.ToString().CompareTo(port) == 0)
                    {
                        mClientListView.Items.RemoveAt(i);
                        break;
                    }
                }
            }));
        }

        private void onServerButtonClick(object sender, EventArgs e)
        {
            var pluginManager = PluginManager.getInstance();
            if (pluginManager.isStart() == true)
            {
                pluginManager.stop();
                mServerGroupBox.Text = string.Format("{0} ({1})", StringLib.Server, StringLib.Stopped);
                mServerButton.Text = StringLib.Server_Start;
                pluginManager.IsStart = false;
                pluginManager.write();
                mPortNumericUpDown.Enabled = true;
            }
            else
            {
                int port = Decimal.ToInt32(mPortNumericUpDown.Value);
                if (pluginManager.start(port) == true)
                {
                    mServerGroupBox.Text = string.Format("{0} ({1})", StringLib.Server, StringLib.Running);
                    mServerButton.Text = StringLib.Server_Stop;
                    pluginManager.IsStart = true;
                    pluginManager.Port = port;
                    pluginManager.write();
                    mPortNumericUpDown.Enabled = false;
                }
                else
                {
                    DarkMessageBox.ShowError(StringLib.Unable_to_open_socket, "", DarkDialogButton.Ok);
                }
            }
        }

        private void onTempRemoveButtonClick(object sender, EventArgs e)
        {
            int count = mTempListView.SelectedItems.Count;
            while (count > 0)
            {
                mTempListView.SelectedItems[0].Remove();
                count--;
            }
        }

        private void onTempAddButtonClick(object sender, EventArgs e)
        {
            var form = new PluginAddForm("", "");
            if (form.ShowDialog() == DialogResult.OK)
            {
                string key = form.KeyString;
                string name = form.NameString;

                bool isExist = false;
                for (int i = 0; i < mTempListView.Items.Count; i++)
                {
                    string tempKey = mTempListView.Items[i].SubItems[1].Text;
                    if (key.CompareTo(tempKey) == 0)
                    {
                        isExist = true;
                        DarkMessageBox.ShowError(StringLib.Duplicate_Key, "", DarkDialogButton.Ok);
                        break;
                    }
                }
                if (isExist == false)
                {
                    var item = new ListViewItem();
                    item.SubItems.Add(key);
                    item.SubItems.Add(name);
                    mTempListView.Items.Add(item);
                }
            }
        }

        private void onTempListViewMouseDBClick(object sender, MouseEventArgs e)
        {
            if (mTempListView.SelectedItems.Count == 1)
            {
                var item = mTempListView.SelectedItems[0];
                string keyString = item.SubItems[1].Text;
                string nameString = item.SubItems[2].Text;
                int index = item.Index;

                var form = new PluginAddForm(keyString, nameString);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string changeKey = form.KeyString;
                    string changeName = form.NameString;

                    bool isExist = false;
                    for (int i = 0; i < mTempListView.Items.Count; i++)
                    {
                        if (i == index)
                            continue;

                        string tempKey = mTempListView.Items[i].SubItems[1].Text;
                        if (changeKey.CompareTo(tempKey) == 0)
                        {
                            isExist = true;
                            DarkMessageBox.ShowError(StringLib.Duplicate_Key, "", DarkDialogButton.Ok);
                            break;
                        }
                    }
                    if (isExist == false)
                    {
                        item.SubItems[1].Text = changeKey;
                        item.SubItems[2].Text = changeName;
                    }
                }
            }
        }

        private void onFanSpeedRemoveButtonClick(object sender, EventArgs e)
        {
            int count = mFanSpeedListView.SelectedItems.Count;
            while (count > 0)
            {
                mFanSpeedListView.SelectedItems[0].Remove();
                count--;
            }
        }

        private void onFanSpeedAddButtonClick(object sender, EventArgs e)
        {
            var form = new PluginAddForm("", "");
            if (form.ShowDialog() == DialogResult.OK)
            {
                string key = form.KeyString;
                string name = form.NameString;

                bool isExist = false;
                for (int i = 0; i < mFanSpeedListView.Items.Count; i++)
                {
                    string tempKey = mFanSpeedListView.Items[i].SubItems[1].Text;
                    if (key.CompareTo(tempKey) == 0)
                    {
                        isExist = true;
                        DarkMessageBox.ShowError(StringLib.Duplicate_Key, "", DarkDialogButton.Ok);
                        break;
                    }
                }
                if (isExist == false)
                {
                    var item = new ListViewItem();
                    item.SubItems.Add(key);
                    item.SubItems.Add(name);
                    mFanSpeedListView.Items.Add(item);
                }
            }
        }

        private void onFanSpeedListViewMouseDBClick(object sender, MouseEventArgs e)
        {
            if (mFanSpeedListView.SelectedItems.Count == 1)
            {
                var item = mFanSpeedListView.SelectedItems[0];
                string keyString = item.SubItems[1].Text;
                string nameString = item.SubItems[2].Text;
                int index = item.Index;

                var form = new PluginAddForm(keyString, nameString);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string changeKey = form.KeyString;
                    string changeName = form.NameString;

                    bool isExist = false;
                    for (int i = 0; i < mFanSpeedListView.Items.Count; i++)
                    {
                        if (i == index)
                            continue;

                        string tempKey = mFanSpeedListView.Items[i].SubItems[1].Text;
                        if (changeKey.CompareTo(tempKey) == 0)
                        {
                            isExist = true;
                            DarkMessageBox.ShowError(StringLib.Duplicate_Key, "", DarkDialogButton.Ok);
                            break;
                        }
                    }
                    if (isExist == false)
                    {
                        item.SubItems[1].Text = changeKey;
                        item.SubItems[2].Text = changeName;
                    }
                }
            }
        }

        private void onFanControlRemoveButtonClick(object sender, EventArgs e)
        {
            int count = mFanControlListView.SelectedItems.Count;
            while (count > 0)
            {
                mFanControlListView.SelectedItems[0].Remove();
                count--;
            }
        }

        private void onFanControlAddButtonClick(object sender, EventArgs e)
        {
            var form = new PluginAddForm("", "");
            if (form.ShowDialog() == DialogResult.OK)
            {
                string key = form.KeyString;
                string name = form.NameString;

                bool isExist = false;
                for (int i = 0; i < mFanControlListView.Items.Count; i++)
                {
                    string tempKey = mFanControlListView.Items[i].SubItems[1].Text;
                    if (key.CompareTo(tempKey) == 0)
                    {
                        isExist = true;
                        DarkMessageBox.ShowError(StringLib.Duplicate_Key, "", DarkDialogButton.Ok);
                        break;
                    }
                }
                if (isExist == false)
                {
                    var item = new ListViewItem();
                    item.SubItems.Add(key);
                    item.SubItems.Add(name);
                    mFanControlListView.Items.Add(item);
                }
            }
        }

        private void onFanControlListViewMouseDBClick(object sender, MouseEventArgs e)
        {
            if (mFanControlListView.SelectedItems.Count == 1)
            {
                var item = mFanControlListView.SelectedItems[0];
                string keyString = item.SubItems[1].Text;
                string nameString = item.SubItems[2].Text;
                int index = item.Index;

                var form = new PluginAddForm(keyString, nameString);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string changeKey = form.KeyString;
                    string changeName = form.NameString;

                    bool isExist = false;
                    for (int i = 0; i < mFanControlListView.Items.Count; i++)
                    {
                        if (i == index)
                            continue;

                        string tempKey = mFanControlListView.Items[i].SubItems[1].Text;
                        if (changeKey.CompareTo(tempKey) == 0)
                        {
                            isExist = true;
                            DarkMessageBox.ShowError(StringLib.Duplicate_Key, "", DarkDialogButton.Ok);
                            break;
                        }
                    }
                    if (isExist == false)
                    {
                        item.SubItems[1].Text = changeKey;
                        item.SubItems[2].Text = changeName;
                    }
                }
            }
        }

        private void onOKButtonClick(object sender, EventArgs e)
        {
            var tempBaseMap = HardwareManager.getInstance().TempBaseMap;
            var fanBaseMap = HardwareManager.getInstance().FanBaseMap;
            var controlBaseMap = HardwareManager.getInstance().ControlBaseMap;

            var tempDeviceList = new List<PluginDevice>();
            for (int i = 0; i < mTempListView.Items.Count; i++)
            {
                string key = mTempListView.Items[i].SubItems[1].Text;
                string name = mTempListView.Items[i].SubItems[2].Text;
                var device = new PluginDevice(key, name);
                tempDeviceList.Add(device);

                string idString = string.Format("Plugin/{0}/Temp", key);
                if (tempBaseMap.ContainsKey(idString) == true)
                {
                    tempBaseMap[idString].Name = name;
                }
            }

            var fanDeviceList = new List<PluginDevice>();
            for (int i = 0; i < mFanSpeedListView.Items.Count; i++)
            {
                string key = mFanSpeedListView.Items[i].SubItems[1].Text;
                string name = mFanSpeedListView.Items[i].SubItems[2].Text;
                var device = new PluginDevice(key, name);
                fanDeviceList.Add(device);

                string idString = string.Format("Plugin/{0}/Fan", key);
                if (fanBaseMap.ContainsKey(idString) == true)
                {
                    fanBaseMap[idString].Name = name;
                }
            }

            var controlDeviceList = new List<PluginDevice>();
            for (int i = 0; i < mFanControlListView.Items.Count; i++)
            {
                string key = mFanControlListView.Items[i].SubItems[1].Text;
                string name = mFanControlListView.Items[i].SubItems[2].Text;
                var device = new PluginDevice(key, name);
                controlDeviceList.Add(device);

                string idString = string.Format("Plugin/{0}/Control", key);
                if (controlBaseMap.ContainsKey(idString) == true)
                {
                    controlBaseMap[idString].Name = name;
                }
            }

            PluginManager.getInstance().setDeviceList(tempDeviceList, fanDeviceList, controlDeviceList);

            HardwareManager.getInstance().write();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
