using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Listing
    {
        public String Path { get; set; }
        public String Name { get; set; }
        public Boolean Directory { get; set; }
        public long Size { get; set; }
        public String Modified { get; set; }

        public void WritePayload(NetDataWriter writer)
        {
            writer.Put(this.Path);
            writer.Put(this.Name);
            writer.Put(this.Directory);
            writer.Put(this.Size);
            writer.Put(this.Modified.ToString());
        }

        public void ReadPayload(NetDataReader reader)
        {
            this.Path = reader.GetString(100);
            this.Name = reader.GetString(100);
            this.Directory = reader.GetBool();
            this.Size = reader.GetLong();
            this.Modified = reader.GetString(100);
        }
    }
}
