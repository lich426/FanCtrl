// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) FanControl and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;

namespace FanCtrl
{
    public class StartupControl
    {
        private const string RegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private bool _startup;
        private const string RegistryName = "FanControl";

        public int DelayTime { get; set; }

        public StartupControl()
        {
            if (Environment.OSVersion.Platform >= PlatformID.Unix)
            {
                IsAvailable = false;
                return;
            }

            bool isTaskConnected = false;
            try
            {
                isTaskConnected = TaskService.Instance.Connected;
            }
            catch(Exception e)
            {
                Console.WriteLine("StartupControl.StartupControl() : {0}", e.Message);
            }

            if (IsAdministrator() && isTaskConnected)
            {
                IsAvailable = true;

                var task = GetTask();
                if (task != null)
                {
                    foreach (var action in task.Definition.Actions)
                    {
                        if (action.ActionType == TaskActionType.Execute && action is ExecAction execAction)
                        {
                            if (execAction.Path.Equals(Application.ExecutablePath, StringComparison.OrdinalIgnoreCase))
                            {
                                _startup = true;
                            }
                        }
                    }
                }
            }
            else
            {
                try
                {
                    using (var registryKey = Registry.CurrentUser.OpenSubKey(RegistryPath))
                    {
                        string value = (string)registryKey?.GetValue(RegistryName);

                        if (value != null)
                        {
                            _startup = (value == Application.ExecutablePath);
                        }
                    }

                    IsAvailable = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("StartupControl.Startup() : {0}", e.Message);
                    IsAvailable = false;
                }
            }
        }

        public bool IsAvailable { get; }

        public bool Startup
        {
            get { return _startup; }
            set
            {
                if (_startup != value)
                {
                    if (IsAvailable)
                    {
                        bool isTaskConnected = false;
                        try
                        {
                            isTaskConnected = TaskService.Instance.Connected;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("StartupControl.Startup() : {0}", e.Message);
                        }

                        if (isTaskConnected)
                        {                            
                            if (value)
                            {
                                if (CreateTask() == false)
                                {
                                    _startup = false;
                                    return;
                                }
                            }
                            else
                            {
                                DeleteTask();
                            }                            
                        }
                        else
                        {
                            if (value)
                            {
                                if (CreateRegistryKey() == false)
                                {
                                    _startup = false;
                                    return;
                                }
                            }
                            else
                            {
                                DeleteRegistryKey();
                            }
                        }
                        _startup = value;
                    }
                    else
                    {
                        _startup = false;
                    }
                }
            }
        }

        private bool IsAdministrator()
        {
            try
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);

                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception e)
            {
                Console.WriteLine("StartupControl.IsAdministrator() : {0}", e.Message);
            }
            return false;
        }

        private Task GetTask()
        {
            try
            {
                return TaskService.Instance.AllTasks.FirstOrDefault(x => x.Name.Equals(RegistryName, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception e)
            {
                Console.WriteLine("StartupControl.GetTask() : {0}", e.Message);
            }
            return null;
        }

        private bool CreateTask()
        {
            try
            {
                var taskDefinition = TaskService.Instance.NewTask();
                taskDefinition.RegistrationInfo.Description = "Starts FanCtrl on Windows startup.";

                var trigger = new LogonTrigger();
                trigger.Delay = new TimeSpan(0, 0, DelayTime);
                taskDefinition.Triggers.Add(trigger);

                taskDefinition.Settings.StartWhenAvailable = true;
                taskDefinition.Settings.DisallowStartIfOnBatteries = false;
                taskDefinition.Settings.StopIfGoingOnBatteries = false;
                taskDefinition.Settings.ExecutionTimeLimit = TimeSpan.Zero;

                taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;
                taskDefinition.Principal.LogonType = TaskLogonType.InteractiveToken;

                taskDefinition.Actions.Add(new ExecAction(Application.ExecutablePath, "", Path.GetDirectoryName(Application.ExecutablePath)));

                TaskService.Instance.RootFolder.RegisterTaskDefinition(RegistryName, taskDefinition);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("StartupControl.CreateTask() : {0}", e.Message);
            }
            return false;
        }

        private void DeleteTask()
        {
            var task = GetTask();
            task?.Folder.DeleteTask(task.Name, false);
        }

        private bool CreateRegistryKey()
        {
            var registryKey = Registry.CurrentUser.CreateSubKey(RegistryPath);
            if (registryKey == null)
            {
                return false;
            }
            registryKey?.SetValue(RegistryName, Application.ExecutablePath);
            return true;
        }

        private void DeleteRegistryKey()
        {
            var registryKey = Registry.CurrentUser.CreateSubKey(RegistryPath);
            registryKey?.DeleteValue(RegistryName);
        }
    }
}
