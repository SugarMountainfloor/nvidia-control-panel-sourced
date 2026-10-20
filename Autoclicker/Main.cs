using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autoclicker.Utils;

namespace Autoclicker;

public class Main : Form
{
	private Random _random = new Random();

	private Thread _clickThread;

	private Thread _clickToggleThread;

	private int _minCps;

	private int _maxCps;

	private bool _clickIsPressed;

	private bool _clickToggle;

	private Keys _clickToggleKey = (Keys)115;

	private Thread _hideToggleThread;

	private bool _hideIsPressed;

	private Keys _hideToggleKey = (Keys)118;

	private IContainer components;

	private TrackBar TrackBarMinCps;

	private TrackBar TrackBarMaxCps;

	private Label LabelMinCps;

	private Label LabelMaxCps;

	private Label LabelToggleKey;

	private Label label1;

	public Main()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Control.CheckForIllegalCrossThreadCalls = false;
		_clickThread = new Thread((ThreadStart)delegate
		{
			while (true)
			{
				if (_clickToggle && WinApi.GetAsyncKeyState((Keys)1) && _minCps > 0 && _maxCps > 0)
				{
					IntPtr hwnd = WinApi.FindWindow("LWJGL", null);
					if (!(hwnd != WinApi.GetForegroundWindow()))
					{
						Task.Factory.StartNew(delegate
						{
							WinApi.PostMessage(hwnd, 513u, 0, 0);
							Thread.Sleep(GetRandomInt(5, 10));
							WinApi.PostMessage(hwnd, 514u, 0, 0);
						});
						Thread.Sleep(1000 / GetRandomInt(_minCps, _maxCps));
					}
				}
			}
		});
		_clickToggleThread = new Thread((ThreadStart)delegate
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			while (true)
			{
				if (!_clickIsPressed)
				{
					if (WinApi.GetAsyncKeyState(_clickToggleKey))
					{
						_clickToggle = !_clickToggle;
						_clickIsPressed = true;
					}
				}
				else if (!WinApi.GetAsyncKeyState(_clickToggleKey))
				{
					_clickIsPressed = false;
				}
				Thread.Sleep(10);
			}
		});
		_hideToggleThread = new Thread((ThreadStart)delegate
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			while (true)
			{
				if (!_hideIsPressed)
				{
					if (WinApi.GetAsyncKeyState(_hideToggleKey))
					{
						((Control)this).Visible = !((Control)this).Visible;
						_hideIsPressed = true;
					}
				}
				else if (!WinApi.GetAsyncKeyState(_hideToggleKey))
				{
					_hideIsPressed = false;
				}
				Thread.Sleep(10);
			}
		});
		InitializeComponent();
		_clickThread.Start();
		_clickToggleThread.Start();
		_hideToggleThread.Start();
	}

	private void TrackBarMinCps_ValueChanged(object sender, EventArgs e)
	{
		if (TrackBarMinCps.Value > TrackBarMaxCps.Value)
		{
			if (TrackBarMaxCps.Value < TrackBarMaxCps.Maximum)
			{
				TrackBar trackBarMaxCps = TrackBarMaxCps;
				int value = trackBarMaxCps.Value;
				trackBarMaxCps.Value = value + 1;
			}
			else
			{
				TrackBar trackBarMinCps = TrackBarMinCps;
				int value = trackBarMinCps.Value;
				trackBarMinCps.Value = value - 1;
			}
		}
		((Control)LabelMinCps).Text = TrackBarMinCps.Value.ToString();
		_minCps = TrackBarMinCps.Value;
	}

	private void TrackBarMaxCps_ValueChanged(object sender, EventArgs e)
	{
		if (TrackBarMaxCps.Value < TrackBarMinCps.Value)
		{
			if (TrackBarMinCps.Value > 0)
			{
				TrackBar trackBarMinCps = TrackBarMinCps;
				int value = trackBarMinCps.Value;
				trackBarMinCps.Value = value - 1;
			}
			else
			{
				TrackBar trackBarMaxCps = TrackBarMaxCps;
				int value = trackBarMaxCps.Value;
				trackBarMaxCps.Value = value + 1;
			}
		}
		((Control)LabelMaxCps).Text = TrackBarMaxCps.Value.ToString();
		_maxCps = TrackBarMaxCps.Value;
	}

	private int GetRandomInt(int min, int max)
	{
		if (max - min < 2)
		{
			return GetRandomInt(min - 1, max + 1);
		}
		byte[] array = new byte[4];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)((byte)_random.Next(0, 255) ^ (byte)_random.Next(0, 255));
		}
		int num = Math.Abs(BitConverter.ToInt32(array, 0));
		num = min + num % (max - min) + ((_random.Next(0, 2) == 1) ? (-1) : 2);
		return num + ((_random.Next(0, 2) == 1) ? (-3) : 2);
	}

	private void Main_FormClosing(object sender, FormClosingEventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		if ((int)MessageBox.Show("Do you want to self destruct ?", "", (MessageBoxButtons)4) == 6)
		{
			string path = Environment.ExpandEnvironmentVariables("%TEMP%");
			string path2 = Environment.ExpandEnvironmentVariables("%USERPROFILE%") + "\\Recent";
			string path3 = Environment.ExpandEnvironmentVariables("%SYSTEMROOT%") + "\\Prefetch";
			ClearFolder(path);
			ClearFolder(path2);
			ClearFolder(path3);
		}
		Environment.Exit(0);
	}

	private void ClearFolder(string path)
	{
		try
		{
			DateTime lastWriteTime = Directory.GetLastWriteTime(path);
			string[] files = Directory.GetFiles(path);
			foreach (string path2 in files)
			{
				try
				{
					File.Delete(path2);
				}
				catch
				{
				}
			}
			try
			{
				Directory.SetLastWriteTime(path, lastWriteTime);
			}
			catch
			{
			}
		}
		catch
		{
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Expected O, but got Unknown
		TrackBarMinCps = new TrackBar();
		TrackBarMaxCps = new TrackBar();
		LabelMinCps = new Label();
		LabelMaxCps = new Label();
		LabelToggleKey = new Label();
		label1 = new Label();
		((ISupportInitialize)TrackBarMinCps).BeginInit();
		((ISupportInitialize)TrackBarMaxCps).BeginInit();
		((Control)this).SuspendLayout();
		((Control)TrackBarMinCps).Location = new Point(10, 40);
		TrackBarMinCps.Maximum = 30;
		TrackBarMinCps.Minimum = 1;
		((Control)TrackBarMinCps).Name = "TrackBarMinCps";
		((Control)TrackBarMinCps).Size = new Size(400, 45);
		((Control)TrackBarMinCps).TabIndex = 0;
		TrackBarMinCps.TickFrequency = 1000;
		TrackBarMinCps.TickStyle = (TickStyle)3;
		TrackBarMinCps.Value = 1;
		TrackBarMinCps.ValueChanged += TrackBarMinCps_ValueChanged;
		((Control)TrackBarMaxCps).Location = new Point(10, 90);
		TrackBarMaxCps.Maximum = 30;
		TrackBarMaxCps.Minimum = 1;
		((Control)TrackBarMaxCps).Name = "TrackBarMaxCps";
		((Control)TrackBarMaxCps).Size = new Size(400, 45);
		((Control)TrackBarMaxCps).TabIndex = 1;
		TrackBarMaxCps.TickFrequency = 1000;
		TrackBarMaxCps.TickStyle = (TickStyle)3;
		TrackBarMaxCps.Value = 1;
		TrackBarMaxCps.ValueChanged += TrackBarMaxCps_ValueChanged;
		((Control)LabelMinCps).AutoSize = true;
		((Control)LabelMinCps).Location = new Point(425, 55);
		((Control)LabelMinCps).Name = "LabelMinCps";
		((Control)LabelMinCps).Size = new Size(13, 13);
		((Control)LabelMinCps).TabIndex = 2;
		((Control)LabelMinCps).Text = "1";
		((Control)LabelMaxCps).AutoSize = true;
		((Control)LabelMaxCps).Location = new Point(425, 105);
		((Control)LabelMaxCps).Name = "LabelMaxCps";
		((Control)LabelMaxCps).Size = new Size(13, 13);
		((Control)LabelMaxCps).TabIndex = 3;
		((Control)LabelMaxCps).Text = "1";
		((Control)LabelToggleKey).AutoSize = true;
		((Control)LabelToggleKey).Location = new Point(12, 9);
		((Control)LabelToggleKey).Name = "LabelToggleKey";
		((Control)LabelToggleKey).Size = new Size(61, 13);
		((Control)LabelToggleKey).TabIndex = 5;
		((Control)LabelToggleKey).Text = "Toggle [F4]";
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(10, 236);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(50, 13);
		((Control)label1).TabIndex = 7;
		((Control)label1).Text = "Hide [F7]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(484, 261);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)LabelToggleKey);
		((Control)this).Controls.Add((Control)(object)LabelMaxCps);
		((Control)this).Controls.Add((Control)(object)LabelMinCps);
		((Control)this).Controls.Add((Control)(object)TrackBarMaxCps);
		((Control)this).Controls.Add((Control)(object)TrackBarMinCps);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Control)this).Name = "Main";
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).FormClosing += new FormClosingEventHandler(Main_FormClosing);
		((ISupportInitialize)TrackBarMinCps).EndInit();
		((ISupportInitialize)TrackBarMaxCps).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
