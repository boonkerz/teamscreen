using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Utils
{
    public class NetDataWriter : LiteNetLib.Utils.NetDataWriter
    {

        public void Decrypt()
        {

        }

        public int Position()
        {
            return _position;
        }
    }
}
