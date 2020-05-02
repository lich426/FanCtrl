using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FanControl
{
    static class Program
    {
        private static Mutex sMutex = null;
        private static bool sIsLock = false;

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (IsAdministrator() == false)
            {
                try
                {
                    Program.executeProgram();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            sMutex = new Mutex(true, "FanControl", out sIsLock);
            if (sIsLock == false)
                return;

            Application.Run(new MainForm());

            if(sIsLock == true)
            {
                sIsLock = false;
                sMutex.ReleaseMutex();
            }
        }

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            if (null != identity)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            return false;
        }

        public static void executeProgram()
        {
            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.UseShellExecute = true;
            procInfo.FileName = Application.ExecutablePath;
            procInfo.WorkingDirectory = Environment.CurrentDirectory;
            procInfo.Verb = "runas";
            Process.Start(procInfo);
        }

        public static void restartProgram()
        {
            if (sIsLock == true)
            {
                sIsLock = false;
                sMutex.ReleaseMutex();
            }
            Program.executeProgram();
        }
    }
}
