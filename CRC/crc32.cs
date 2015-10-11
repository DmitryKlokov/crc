using System;
using System.IO;

namespace CRC
{
    class crc32
    {
        private uint[] crc_table = new uint[256]; //результат операции исключающее ИЛИ, проведённое над двумя байтами, будет занимать 1 байт. т.е. максимум 11111111 что равно 255

        private void BuildTable()
        {
            uint crc;

            for (uint i = 0; i < 256; i++)
            {
                crc = i;
                for (int j = 0; j < 8; j++)
                    //сравниваются биты црц с 1 если биты равны то 1 если нет то 0
                    //если да то сдвиг вправо на 1 и выполняется побитовое исключающее или если биты разные то 1
                    // если нет то сдвиг на 1
                    crc = ((crc & 1) == 1) ? (crc >> 1) ^ 0xEDB88320 : crc >> 1;

                crc_table[i] = crc;
            }
        }

        private uint Crc32(byte[] array)
        {
            uint result = 0xFFFFFFFF; //все 1

            for (int i = 0; i < array.Length; i++)
            {
                byte last_byte = (byte)(result & 0xFF);
                result >>= 8;
                result = result ^ crc_table[last_byte ^ array[i]];
            }
            return result;
        }

        public uint GetFileCrc(string filename)
        {
            var fileInfo = new FileInfo(filename);
            var reader = fileInfo.OpenRead();
            var buffer = new byte[reader.Length];

            reader.Read(buffer, 0, (int)reader.Length);

            return Crc32(buffer);
        }

        public crc32()
        {
            BuildTable();
        }
    }
}
