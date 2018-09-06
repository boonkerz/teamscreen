using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Utils
{
	public class Env
	{
		public enum OS
		{
			Windows,
			Mac,
			X11,
			Other
		}

		public OS OperatingSystem;

		public Env()
		{
			detectOsSystem();
		}

		private void detectOsSystem()
		{
			if (Path.DirectorySeparatorChar == '\\')
				OperatingSystem = OS.Windows;
			else if (IsRunningOnMac())
				OperatingSystem = OS.Mac;
			else if (Environment.OSVersion.Platform == PlatformID.Unix)
				OperatingSystem = OS.X11;
			else
				OperatingSystem = OS.Other;
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
