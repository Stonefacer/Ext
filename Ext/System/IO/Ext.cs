using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ext.System.IO {
    public static class Ext {
        public static void Write(this Stream src, byte[] data) {
            src.Write(data, 0, data.Length);
        }

        public static void Write(this Stream src, int data) {
            src.Write(BitConverter.GetBytes(data));
        }

        public static int ReadInt(this Stream src, out int value) {
            byte[] data = new byte[4];
            int res = src.Read(data, 0, 4);
            if(res == 4)
                value = BitConverter.ToInt32(data, 0);
            else
                value = -1;
            return res;
        }

        public static int ReadShort(this Stream src, out short value) {
            byte[] data = new byte[2];
            int res = src.Read(data, 0, 2);
            if(res == 2)
                value = BitConverter.ToInt16(data, 0);
            else
                value = -1;
            return res;
        }

        public static int Read(this Stream src, byte[] destination) {
            return src.Read(destination, 0, destination.Length);
        }

        /// <summary>
        /// Only ASCII AND UTF-like encodings
        /// </summary>
        public static string ReadLine(this Stream src) {
            var Position = src.Position;
            int Length = 0;
            int Buffer = src.ReadByte();
            while(Buffer != 10 && Buffer != -1) {
                Length++;
                Buffer = src.ReadByte();
            }
            src.Seek(Position, SeekOrigin.Begin);
            byte[] data = new byte[Length];
            src.Read(data);
            src.ReadByte();
            return Encoding.UTF8.GetString(data, 0, data.Length).TrimEnd(new char[]{ '\r', '\n'});
        }

    }
}
