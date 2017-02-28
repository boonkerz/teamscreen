using System;
using System.Threading;
using Gtk;

namespace GuiHost
{
	class MainClass
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Thread.Sleep(2000);
			Application.Init();
			MainWindow win = new MainWindow();
			win.Show();
			Application.Run();
		}
	}
}
