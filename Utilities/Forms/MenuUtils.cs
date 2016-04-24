using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LiveSplit.Spelunky.Utilities.Forms
{
    public static class MenuUtils
    {
        private const int MF_BYPOSITION = 0x400;

        [DllImport("User32.dll")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("User32.dll")]
        private static extern int GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("User32.dll")]
        private static extern int GetMenuItemCount(IntPtr hWnd);

        public static void DisableCloseButton(Form form)
        {
            var hMenu = (IntPtr)GetSystemMenu(form.Handle, false);
            var menuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }
    }
}
