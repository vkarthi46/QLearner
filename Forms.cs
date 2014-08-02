using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public static class Extensions
    {
        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, uint Msg);
        private const uint SW_RESTORE = 0x09;
        [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
        public static void Restore(this Form form)
        {
            if (form.WindowState == FormWindowState.Minimized)
            {
                ShowWindow(form.Handle, SW_RESTORE);
            }
        }
        [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
        public static void BringUp(this Form form)
        {
            form.Restore();
            form.Show();
            form.TopMost = true;
            form.BringToFront();
            form.TopMost = false;
        }
    }
}