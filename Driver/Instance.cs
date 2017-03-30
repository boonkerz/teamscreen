using System;
using System.IO;
using System.Runtime.InteropServices;
using Driver;

namespace Driver
{
	public class Manager
	{
		public enum OS
		{
			Windows,
			Mac,
			X11,
			Other
		}

		#region Singleton Design Pattern Implementation

		private static volatile Manager instance;
		private static readonly object syncRoot = new Object();
		private OS operating_system;

		public static Manager Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new Manager();
					}
				}

				return instance;
			}
		}

		#endregion

		public Driver.Interfaces.Mouse Mouse { get; private set; }
		public Driver.Interfaces.Keyboard Keyboard { get; private set; }
		public Interfaces.Display Display { get; private set; }

		private Manager()
		{
            detectOsSystem();
			if (operating_system == OS.Mac)
			{
				Mouse = new Driver.Mac.Mouse();
				Keyboard = new Driver.Mac.Keyboard();
				Display = new Driver.Windows.Display();
			}
			else if (operating_system == OS.Windows)
			{
				Mouse = new Driver.Windows.Mouse();
				Keyboard = new Driver.Windows.Keyboard();
				Display = new Driver.Windows.Display();
			}
			else 
			{
				
			}
		}

		private void detectOsSystem() 
		{
			if (Path.DirectorySeparatorChar == '\\')
				operating_system = OS.Windows;
			else if (IsRunningOnMac())
				operating_system = OS.Mac;
			else if (Environment.OSVersion.Platform == PlatformID.Unix)
				operating_system = OS.X11;
			else
				operating_system = OS.Other;
		}

		[DllImport("libc")]
		static extern int uname(IntPtr buf);

		private bool IsRunningOnMac()
		{
			IntPtr buf = IntPtr.Zero;
			try
			{
				buf = Marshal.AllocHGlobal(8192);
				// This is a hacktastic way of getting sysname from uname ()
				if (uname(buf) == 0)
				{
					string os = Marshal.PtrToStringAnsi(buf);
					if (os == "Darwin")
						return true;
				}
			}
			catch
			{
			}
			finally
			{
				if (buf != IntPtr.Zero)
					Marshal.FreeHGlobal(buf);
			}
			return false;
		}
	}
}
