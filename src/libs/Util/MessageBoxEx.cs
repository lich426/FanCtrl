using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FanCtrl
{
    public class MessageBoxEx
    {
        delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int hook, HookProc callback, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr GetDlgItem(IntPtr hDlg, DialogResult nIDDlgItem);

        [DllImport("user32.dll")]
        static extern bool SetDlgItemText(IntPtr hDlg, DialogResult nIDDlgItem, string lpString);

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        static IntPtr g_hHook;

        static string yes;
        static string cancel;
        static string no;

        public static DialogResult Show(string text, string caption, string yes, string no, string cancel)
        {
            MessageBoxEx.yes = yes;
            MessageBoxEx.cancel = cancel;
            MessageBoxEx.no = no;
            g_hHook = SetWindowsHookEx(5, new HookProc(HookWndProc), IntPtr.Zero, GetCurrentThreadId());
            return MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel);
        }

        static int HookWndProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            IntPtr hChildWnd;

            if (nCode == 5)
            {
                hChildWnd = wParam;

                if (GetDlgItem(hChildWnd, DialogResult.Yes) != null)
                    SetDlgItemText(hChildWnd, DialogResult.Yes, MessageBoxEx.yes);

                if (GetDlgItem(hChildWnd, DialogResult.No) != null)
                    SetDlgItemText(hChildWnd, DialogResult.No, MessageBoxEx.no);

                if (GetDlgItem(hChildWnd, DialogResult.Cancel) != null)
                    SetDlgItemText(hChildWnd, DialogResult.Cancel, MessageBoxEx.cancel);

                UnhookWindowsHookEx(g_hHook);
            }
            else
                CallNextHookEx(g_hHook, nCode, wParam, lParam);

            return 0;
        }
    }
}
