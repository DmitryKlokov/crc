using System;

namespace CRC
{
    public class file
    {
        public string name;
        public uint crc;
        public file(string name,uint crc)
        {
            this.name = name;
            this.crc = crc;
        }
        public file() { }
    }
}
