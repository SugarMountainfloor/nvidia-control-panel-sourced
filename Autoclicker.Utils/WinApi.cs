using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Autoclicker.Utils;

internal static class WinApi
{
	[DllImport("user32.dll")]
	public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

	[DllImport("User32.dll")]
	public static extern bool GetAsyncKeyState(Keys key);

	[DllImport("user32.dll")]
	public static extern IntPtr GetForegroundWindow();
}
