using Common.EventArgs.Network;
using Network;
using System;namespace Driver.Interfaces{	public interface Display	{		void RequestScreenshot(ScreenshotRequestEventArgs e, HostManager hm, Boolean fullscreen);	}}