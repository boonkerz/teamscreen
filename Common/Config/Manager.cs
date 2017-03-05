using System;
using System.IO;
using Newtonsoft.Json;

namespace Common.Config
{
	public class Manager
	{

		protected String ConfigPath;

		public Host HostConfig { get; set; }
		public Client ClientConfig { get; set; }

		public Manager()
		{
			HostConfig = new Host();
			ClientConfig = new Client();
			ConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			ConfigPath += Path.DirectorySeparatorChar + "teamscreen" + Path.DirectorySeparatorChar;

			loadConfig();
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
