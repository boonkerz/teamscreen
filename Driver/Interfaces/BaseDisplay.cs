using Common.EventArgs.Network;
using Network;
using Network.Manager;
using Network.Messages.Connection;
using System;
namespace Driver.Interfaces
{
	public interface BaseDisplay
	{

		void RequestScreenshot(ScreenshotRequestEventArgs e, Boolean fullscreen);

        void SetManager(HostManager HostManager);
        void Clear();
        void SendScreenshot(Boolean fullscreen);
        void RemoveClient(string clientSystemId);
        void RemoveAllClients();
    }
}
