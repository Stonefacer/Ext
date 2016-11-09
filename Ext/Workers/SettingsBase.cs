using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
using System.Security.Cryptography;
#if NET40 || NET45
#if EXT_BIGINT
using System.Numerics;
#endif
using System.Threading.Tasks;
#endif

using Ext.System.Core;
using Ext.System.Numerics;
using Ext.Exceptions;
using Ext.System.Security;

namespace Ext.Workers {
#if (NET40 || NET45) && EXT_BIGINT
    public abstract class SignedFileBase {

        //public static readonly string PaddingString = "JyUZs30QmcOnKowc2nuuysK6rXcgCR79zayhGRiJRmRuAQF2bPrQ95lRdNhLRJDKVFokqFLb4bTCRPwRHZ5nagztpX3skNs7Y7PucVBM3GMXcY0DRG3rEQjoFuvIBSqg";

        protected virtual string FileName { get; } = "settings";
        protected abstract void LoadData(MemoryStream MemStream);
        protected abstract byte[] GetSaveData();

        //                      //
        //    File Srtucture    //
        //                      //
        // EncType    Reserved
        // |          |
        // 0000 0000  0000 0000 0000 0000 0000 0000
        //  |   N  |   |   E  | |    Sign LEN     |
        // E MAX Length = 128 B INC
        // N MAX Length = 128 B INC
        // E(real) = (E+32) * 4
        // N(real) = (N+32) * 4
        // E(real) =   32 -  160
        // E(real) =  128 -  640 B
        // E(real) = 1024 - 5120 bit
        // Comments
        // |----- Next 4 Bytes -----|
        // EncType
        //  -- if 0 = Plain Text --
        // RSA N Key Length = 0
        // RSA E Key Length = 0
        // Sign LEN = 64
        // SHA512 64 bytes
        //  -- if 1 = RSA --
        // RSA N Key Length
        // RSA E Key Length
        // Sign LEN
        // RSA N Key
        // RSA E Key
        // Sign
        // |-------- DATA --------|
        // Delay
        // Repeats
        // Proxy
        // OutputFilePath
        // OutputFormat
        // Filter
        // URLCount
        // http://dawda.da
        // http://dawada.da

        public virtual void Load() {
            using(MemoryStream MemStream = new MemoryStream()) {
                using(FileStream fs = new FileStream(FileName, FileMode.Open))
                using(GZipStream ZipStream = new GZipStream(fs, CompressionMode.Decompress))
                    ZipStream.CopyTo(MemStream);
                MemStream.Seek(0, SeekOrigin.Begin);
                byte[] buf = new byte[MemStream.Length];
                MemStream.Read(buf, 0, 4);
                int Header = BitConverter.ToInt16(buf, 0);
                int SignLen = BitConverter.ToInt16(buf, 2);
                if(SignLen > buf.Length)
                    throw new FileCorruptedException();
                if(Header == 0) { // plaint text
                    if(SignLen != 64)
                        throw new FileCorruptedException();
                    var Sign = new byte[64];
                    MemStream.Read(Sign, 0, Sign.Length);
                    var FileOffset = MemStream.Position;
                    var hash = SHA512.Create().ComputeHash(MemStream);
                    if(hash.CompareTo(Sign) != 0)
                        throw new FileCorruptedException(string.Format("Signature is incorrect. Signature: {0}. Must be: {1}", Sign.ToHexString(), hash.ToHexString()));
                    MemStream.Seek(FileOffset, SeekOrigin.Begin);
                } else if((Header & 0x80) != 0) {
                    var len = ((Header & 0x7f)+32)*4;
                    byte[] N = new byte[len];
                    MemStream.Read(N, 0, len);
                    len = (Header>>8) & 0x7f;
                    byte[] E = new byte[len];
                    MemStream.Read(E, 0, len);
                    var rsa = Ext.System.Security.advRSA.InstanceEnc(N, E);
                    var Sign = new byte[SignLen];
                    MemStream.Read(Sign, 0, SignLen);
                    var Offset = MemStream.Position;
                    var hash = SHA512.Create().ComputeHash(MemStream);
                    if(!rsa.CheckSignature(Sign, hash))
                        throw new FileCorruptedException(string.Format("Signature is incorrect. Signature: {0}. Must be: {1}", Sign.ToHexString(), hash.ToHexString()));
                    MemStream.Seek(Offset, SeekOrigin.Begin);
                } else {
                    throw new FileCorruptedException();
                }
                LoadData(MemStream);
            }
        }

        public virtual void Save() {
            using(FileStream fs = new FileStream(FileName, FileMode.Create)) {
                using(GZipStream ZipStream = new GZipStream(fs, CompressionMode.Compress)) {
                    using(MemoryStream MemStream = new MemoryStream()) {
                        MemStream.Write(BitConverter.GetBytes((short)0), 0, 2);
                        MemStream.Write(BitConverter.GetBytes((short)64), 0, 2);
                        byte[] Data = GetSaveData();
                        var hash = SHA512.Create().ComputeHash(Data);
                        MemStream.Write(hash, 0, hash.Length);
                        MemStream.Write(Data, 0, Data.Length);
                        MemStream.Seek(0, SeekOrigin.Begin);
                        MemStream.CopyTo(ZipStream);
                    }
                }
            }
        }

        public virtual void SaveRSA(advRSA rsa) {
            using(MemoryStream MemStream = new MemoryStream()) {
                short head = (short)(rsa.N.GetByteCount() / 4 - 32);
                if(head > 127)
                    throw new Exception("N is too big.");
                head |= (short)(rsa.E.GetByteCount() << 8);
                head |= 0x80; // RSA flag
                MemStream.Write(BitConverter.GetBytes(head), 0, 2);
                byte[] data = GetSaveData();
                var hash = SHA512.Create().ComputeHash(data);
                hash = rsa.GetSignature(hash);
                MemStream.Write(BitConverter.GetBytes((short)(hash.Length)), 0, 2);
                var key = rsa.N.ToByteArray();
                MemStream.Write(key, 0, key.Length);
                key = rsa.E.ToByteArray();
                MemStream.Write(key, 0, key.Length);
                MemStream.Write(hash, 0, hash.Length);
                MemStream.Write(data, 0, data.Length);
                MemStream.Seek(0, SeekOrigin.Begin);
                using(FileStream fs = new FileStream(FileName, FileMode.Create)) {
                    using(GZipStream ZipStream = new GZipStream(fs, CompressionMode.Compress)) {
                        MemStream.CopyTo(ZipStream);
                    }
                }
            }
        }
    }
#endif
}

