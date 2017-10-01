using System;
using System.IO;
using System.Runtime.InteropServices;
using Driver;

namespace Driver
{
	public class Manager
	{
		
		#region Singleton Design Pattern Implementation

		private static volatile Manager instance;
		private static readonly object syncRoot = new Object();

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
        public Interfaces.FileManager FileManager { get; private set; }
		public Common.Utils.Env Env { get; set; }

        private Manager()
		{
            FileManager = new Driver.Windows.FileManager();
			Env = new Common.Utils.Env();
			if (Env.OperatingSystem == Common.Utils.Env.OS.Mac)
			{
				Mouse = new Driver.Mac.Mouse();
				Keyboard = new Driver.Mac.Keyboard();
				Display = new Driver.Mac.Display();
			}
			else if (Env.OperatingSystem == Common.Utils.Env.OS.Windows)
			{
				Mouse = new Driver.Windows.Mouse();
				Keyboard = new Driver.Windows.Keyboard();
				Display = new Driver.Windows.Display();
			}
			else 
			{
				
			}
		}


	}
}
