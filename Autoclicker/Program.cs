using System;
using System.Windows.Forms;
using Autoclicker.Utils;

namespace Autoclicker;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (WinApi.FindWindow("LWJGL", null) == IntPtr.Zero)
		{
			MessageBox.Show("Failed to detect Minecraft");
			return;
		}
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run((Form)(object)new Main());
	}
}
