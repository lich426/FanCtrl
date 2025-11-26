using DarkUI.Config;
using System;
using System.Threading;
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

            var type = OptionManager.getInstance().getNowTheme();
            if (type == THEME_TYPE.DARK)
            {
                ThemeProvider.Theme = new DarkTheme();
            }
            else
            {
                ThemeProvider.Theme = new LightTheme();
            }

            Application.Run(new MainForm());

            Program.releaseMutex();
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

