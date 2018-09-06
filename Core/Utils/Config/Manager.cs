using System;
using System.IO;
using Newtonsoft.Json;

namespace Utils.Config
{
	public class Manager
	{

		public String ConfigPath;

		public Host HostConfig { get; set; }
		public Client ClientConfig { get; set; }
		public Utils.Env Env { get; set; }

		public Manager()
		{
			Env = new Utils.Env();
			HostConfig = new Host();
			ClientConfig = new Client();
			if(Env.OperatingSystem == Utils.Env.OS.Mac) {
				ConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			}else{
				ConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			}

			ConfigPath += Path.DirectorySeparatorChar + "teamscreen" + Path.DirectorySeparatorChar;

			loadConfig();
		}

		public void Reload()
		{
			this.loadConfig();
		}

		protected void loadConfig()
		{

			if (!Directory.Exists(this.ConfigPath))
			{
				Directory.CreateDirectory(ConfigPath);
			}
			if (!File.Exists(ConfigPath + "host.json"))
			{
				saveHostConfig();
			}
			if (!File.Exists(ConfigPath + "client.json"))
			{
				saveClientConfig();
			}

            HostConfig = JsonConvert.DeserializeObject<Host>(File.ReadAllText(@"" + ConfigPath + "host.json"));
			ClientConfig = JsonConvert.DeserializeObject<Client>(File.ReadAllText(@"" + ConfigPath + "client.json"));
		}

		public void saveHostConfig()
		{
			File.WriteAllText(@"" + ConfigPath + "host.json", JsonConvert.SerializeObject(HostConfig));
		}

		public void saveClientConfig()
		{
			File.WriteAllText(@"" + ConfigPath + "client.json", JsonConvert.SerializeObject(ClientConfig));
		}
	}
}
