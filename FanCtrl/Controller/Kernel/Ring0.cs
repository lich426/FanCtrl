// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;

//SecurityIdentifier

namespace FanCtrl
{
    internal static class Ring0
    {
        private static KernelDriver _driver;
        private static string _fileName;

        private static readonly StringBuilder _report = new StringBuilder();
        
        public static bool IsOpen
        {
            get { return _driver != null; }
        }

        private static Assembly GetAssembly()
        {
            return typeof(Ring0).Assembly;
        }

        private static string GetTempFileName()
        {
            // try to create one in the application folder
            string location = GetAssembly().Location;
            if (!string.IsNullOrEmpty(location))
            {
                try
                {
                    string fileName = Path.ChangeExtension(location, ".sys");

                    using (File.Create(fileName))
                        return fileName;
                }
                catch (Exception)
                { }
            }

            // if this failed, try to get a file in the temporary folder
            try
            {
                return Path.GetTempFileName();
            }
            catch (IOException)
            {
                // some I/O exception
            }
            catch (UnauthorizedAccessException)
            {
                // we do not have the right to create a file in the temp folder
            }
            catch (NotSupportedException)
            {
                // invalid path format of the TMP system environment variable
            }

            return null;
        }

        private static bool ExtractDriver(string fileName)
        {
            string resourceName = "WinRing0x64.sys";

            string[] names = GetAssembly().GetManifestResourceNames();
            byte[] buffer = null;
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].Contains(resourceName) == true)
                {
                    Stream stream = GetAssembly().GetManifestResourceStream(names[i]);

                    if (stream != null)
                    {
                        buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                    }
                }
            }

            if (buffer == null)
                return false;

            try
            {
                FileStream target = new FileStream(fileName, FileMode.Create);

                target.Write(buffer, 0, buffer.Length);
                target.Flush();
                target.Close();
            }
            catch (IOException)
            {
                // for example there is not enough space on the disk
                return false;
            }

            // make sure the file is actually written to the file system
            for (int i = 0; i < 20; i++)
            {
                try
                {
                    if (File.Exists(fileName) &&
                        new FileInfo(fileName).Length == buffer.Length)
                    {
                        return true;
                    }

                    Thread.Sleep(100);
                }
                catch (IOException)
                {
                    Thread.Sleep(10);
                }
            }

            // file still has not the right size, something is wrong
            return false;
        }

        public static void Open()
        {
            if (_driver != null)
                return;


            // clear the current report
            _report.Length = 0;

            _driver = new KernelDriver("WinRing0_1_2_0");
            _driver.Open();

            if (!_driver.IsOpen)
            {
                // driver is not loaded, try to install and open
                _fileName = GetTempFileName();
                if (_fileName != null && ExtractDriver(_fileName))
                {
                    if (_driver.Install(_fileName, out string installError))
                    {
                        _driver.Open();

                        if (!_driver.IsOpen)
                        {
                            _driver.Delete();
                            _report.AppendLine("Status: Opening driver failed after install");
                        }
                    }
                    else
                    {
                        string errorFirstInstall = installError;

                        // install failed, try to delete and reinstall
                        _driver.Delete();

                        // wait a short moment to give the OS a chance to remove the driver
                        Thread.Sleep(2000);

                        if (_driver.Install(_fileName, out string errorSecondInstall))
                        {
                            _driver.Open();

                            if (!_driver.IsOpen)
                            {
                                _driver.Delete();
                                _report.AppendLine("Status: Opening driver failed after reinstall");
                            }
                        }
                        else
                        {
                            _report.AppendLine("Status: Installing driver \"" + _fileName + "\" failed" + (File.Exists(_fileName) ? " and file exists" : string.Empty));
                            _report.AppendLine("First Exception: " + errorFirstInstall);
                            _report.AppendLine("Second Exception: " + errorSecondInstall);
                        }
                    }
                }
                else
                {
                    _report.AppendLine("Status: Extracting driver failed");
                }
                
                try
                {
                    // try to delete the driver file
                    if (File.Exists(_fileName) && _fileName != null)
                        File.Delete(_fileName);

                    _fileName = null;
                }
                catch (IOException)
                { }
                catch (UnauthorizedAccessException)
                { }
            }

            if (!_driver.IsOpen)
                _driver = null;
        }

        public static void Close()
        {
            if (_driver != null)
            {
                uint refCount = 0;
                _driver.DeviceIOControl(Ring0.IOCTL_OLS_GET_REFCOUNT, null, ref refCount);
                _driver.Close();

                if (refCount <= 1)
                    _driver.Delete();

                _driver = null;
            }

            // try to delete temporary driver file again if failed during open
            if (_fileName != null && File.Exists(_fileName))
            {
                try
                {
                    File.Delete(_fileName);
                    _fileName = null;
                }
                catch (IOException)
                { }
                catch (UnauthorizedAccessException)
                { }
            }
        }

        public static string GetReport()
        {
            if (_report.Length > 0)
            {
                StringBuilder r = new StringBuilder();
                r.AppendLine("Ring0");
                r.AppendLine();
                r.Append(_report);
                r.AppendLine();
                return r.ToString();
            }

            return null;
        }

        public static ushort ReadSmbus(ushort port)
        {
            if (_driver == null)
                return 0;

            uint value = 0;
            _driver.DeviceIOControl(Ring0.IOCTL_OLS_READ_IO_PORT_BYTE, port, ref value);
            ushort retVal = (ushort)(value & 0xff);
            return retVal;
        }

        public static void WriteSmbus(ushort port, int value)
        {
            if (_driver == null)
                return;

            WriteIoPortInput input = new WriteIoPortInput { PortNumber = port, Value = (byte)(value & 0xff) };
            _driver.DeviceIOControl(Ring0.IOCTL_OLS_WRITE_IO_PORT_BYTE, input);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct WriteIoPortInput
        {
            public uint PortNumber;
            public byte Value;
        }

        public const uint INVALID_PCI_ADDRESS = 0xFFFFFFFF;

        private const uint OLS_TYPE = 40000;

        public static readonly Kernel32.IOControlCode
            IOCTL_OLS_GET_REFCOUNT = new Kernel32.IOControlCode(OLS_TYPE, 0x801, Kernel32.IOControlCode.Access.Any);

        public static readonly Kernel32.IOControlCode
            IOCTL_OLS_READ_MSR = new Kernel32.IOControlCode(OLS_TYPE, 0x821, Kernel32.IOControlCode.Access.Any);

        public static readonly Kernel32.IOControlCode
            IOCTL_OLS_WRITE_MSR = new Kernel32.IOControlCode(OLS_TYPE, 0x822, Kernel32.IOControlCode.Access.Any);

        public static readonly Kernel32.IOControlCode
            IOCTL_OLS_READ_IO_PORT_BYTE = new Kernel32.IOControlCode(OLS_TYPE, 0x833, Kernel32.IOControlCode.Access.Read);

        public static readonly Kernel32.IOControlCode
            IOCTL_OLS_WRITE_IO_PORT_BYTE = new Kernel32.IOControlCode(OLS_TYPE, 0x836, Kernel32.IOControlCode.Access.Write);

        public static readonly Kernel32.IOControlCode
            IOCTL_OLS_READ_PCI_CONFIG = new Kernel32.IOControlCode(OLS_TYPE, 0x851, Kernel32.IOControlCode.Access.Read);

        public static readonly Kernel32.IOControlCode
            IOCTL_OLS_WRITE_PCI_CONFIG = new Kernel32.IOControlCode(OLS_TYPE, 0x852, Kernel32.IOControlCode.Access.Write);

        public static readonly Kernel32.IOControlCode
            IOCTL_OLS_READ_MEMORY = new Kernel32.IOControlCode(OLS_TYPE, 0x841, Kernel32.IOControlCode.Access.Read);
    }
}
