using System;
using System.Security.Cryptography;
using System.Text;
using LiteNetLib;
using Network.Utils;

namespace Network
{
	public class BaseManager
	{
		System.Collections.Hashtable _myKeys;
		System.Collections.Hashtable _remoteKeys;

		NetManager _netManager;
		MessageHandler _messageHandler;
		public String SystemId { get; set; }
		public String Password { get; set; }

		protected UnicodeEncoding _encoder = new UnicodeEncoding();

		public BaseManager(INetEventListener Listener, string ConnectKey)
		{
			_myKeys = new System.Collections.Hashtable();
			_remoteKeys = new System.Collections.Hashtable();
			_netManager = new NetManager(Listener, 12000, ConnectKey);
			_netManager.UnsyncedEvents = true;
			_netManager.PingInterval = 10000;
			_netManager.DisconnectTimeout = 20000;
			_messageHandler = new MessageHandler();
		}

		public void SaveHostPublicKey(string RemoteSystemId, string RemotePublicKey)
		{
			if (_remoteKeys.Contains(RemoteSystemId))
			{
				_remoteKeys.Remove(RemoteSystemId);
			}
			this._remoteKeys.Add(RemoteSystemId, RemotePublicKey);
		}

		public string Decode(string MySystemId, string Text)
		{
			var pair = _myKeys[MySystemId] as PublicPrivateKey;
			return this.Decrypt(pair.PrivateKey, Text);
		}

		public string Encode(string RemoteSystemId, string Text)
		{
			return this.Encrypt(_remoteKeys[RemoteSystemId] as String, Text);
		}

		public PublicPrivateKey CreateNewKeyPairKey(String HostSystemId)
		{
			var rsa = new RSACryptoServiceProvider();
			PublicPrivateKey pair = new PublicPrivateKey();
			pair.PrivateKey = rsa.ToXmlString(true);
			pair.PublicKey = rsa.ToXmlString(false);

            if(_myKeys.ContainsKey(HostSystemId))
            {
                _myKeys.Remove(HostSystemId);
            }
			this._myKeys.Add(HostSystemId, pair);
			return pair;
		}

		protected string Decrypt(string privateKey, string data)
		{
			var rsa = new RSACryptoServiceProvider();
			var dataByte = Convert.FromBase64String(data);

			rsa.FromXmlString(privateKey);
			var decryptedByte = rsa.Decrypt(dataByte, false);
			return _encoder.GetString(decryptedByte);
		}

		protected string Encrypt(string publicKey, string data)
		{
			var rsa = new RSACryptoServiceProvider();
			rsa.FromXmlString(publicKey);
			var dataToEncrypt = _encoder.GetBytes(data);
			var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false);
			return Convert.ToBase64String(encryptedByteArray);
		}

		public void sendMessage(Message message)
		{
			_netManager.SendToAll(_messageHandler.encodeMessage(message), SendOptions.ReliableOrdered);
		}

		public bool Start()
		{
			return _netManager.Start();
		}

		public void Stop()
		{
			_netManager.Stop();
		}

		public void Connect(String host, int port)
		{
			_netManager.Connect(host, port);
		}
	}
}
