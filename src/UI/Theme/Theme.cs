using System;
using System.Runtime.InteropServices;

namespace FanCtrl
{
    public enum THEME_TYPE
    {
        SYSTEM,
        LIGHT,
        DARK,
    }

    public enum DWMWINDOWATTRIBUTE
    {
        DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19,
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
    }

    class Theme
    {
        public static bool isWindows10OrGreater(int build = -1)
        {
            var version = Environment.OSVersion.Version;
            return version.Major >= 10 && version.Build >= build;
        }

        public static void setTheme(IntPtr handle, bool isDarkMode)
        {
            if (isWindows10OrGreater(17763))
            {
                int attribute = (int)DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (isWindows10OrGreater(18985))
                {
                    attribute = (int)DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE;
                }

                int useImmersiveDarkMode = isDarkMode ? 1 : 0;
                DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int));
            }
        }

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
    }
}
