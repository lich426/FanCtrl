using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FanCtrl
{
    static class Program
    {
        private static Mutex sMutex = null;
        private static bool sIsLock = false;

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Program.createMutex() == false)
                return;

            Application.Run(new MainForm());

            Program.releaseMutex();
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

        public static bool createMutex()
        {
            if (sIsLock == true)
                return true;
            sMutex = new Mutex(true, "FanCtrl", out sIsLock);
            return sIsLock;
        }

        public static void releaseMutex()
        {
            if (sIsLock == true)
            {
                sIsLock = false;
                sMutex.ReleaseMutex();
            }
        }
    }
}
