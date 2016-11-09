using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ext.System.IO {
    public class BitStream : Stream {

        public static int ReverseBits(int data, int bitsCount) {
            int i = 0, j = bitsCount - 1;
            while(i < j) {
                if((data & (1 << i))!=0 && (data & (1 << j)) == 0){ // 1 0
                    data |= 1 << j;
                    data ^= 1 << i;
                } else if((data & (1 << i)) == 0 && (data & (1 << j)) != 0) { // 0 1
                    data ^= 1 << j;
                    data |= 1 << i;
                }
                // 00 11 do nothing
                i++;
                j--;
            }
            return data;
        }

        private Stream _BaseStream = null;
        private int _Position = 0;
        private int _CurrentData = 0;
        private int _WriteData = 0;
        private int _WritePosition = 0;

        public override bool CanRead { get { return _BaseStream?.CanRead ?? false; } }
        public override bool CanSeek { get { return _BaseStream?.CanSeek ?? false; } }
        public override bool CanWrite { get { return _BaseStream?.CanWrite ?? false; } }
        public override long Length { get { return _BaseStream?.Length ?? 0; } }
        public override long Position
        {
            get { return _BaseStream?.Position ?? -1; }
            set
            {
                if(_BaseStream == null)
                    throw new InvalidOperationException();
                _BaseStream.Seek(value, SeekOrigin.Begin);
            }
        }

        public BitStream(Stream DataStream) {
            _BaseStream = DataStream;
            var Position = _BaseStream.Position;
            _CurrentData = _BaseStream.ReadByte();
            //_BaseStream.Seek(Position, SeekOrigin.Begin);
            _Position = 0;
        }

        public override void Flush() {
            if(_BaseStream == null)
                throw new InvalidOperationException();
            _BaseStream.Flush();
        }

        public override int Read(Byte[] buffer, int offset, int count) {
            if(_BaseStream == null)
                throw new InvalidOperationException();
            var res = _BaseStream.Read(buffer, offset, count);
            var Position = _BaseStream.Position;
            _CurrentData = _BaseStream.ReadByte();
            _BaseStream.Seek(Position, SeekOrigin.Begin);
            Position = 0;
            return res;
        }

        public int NextBit() {
            if(_Position == 8) {
                _CurrentData = _BaseStream.ReadByte();
                if(_CurrentData == -1)
                    return -1;
                _Position = 0;
            }
            if(_Position == -1)
                throw new InvalidOperationException();
            return (_CurrentData & (1 << (_Position++))) != 0 ? 1 : 0;
        }

        public int ReadInt(int BitCount) {
            if(BitCount > 31 || BitCount < 0)
                throw new ArgumentOutOfRangeException();
            int Res = 0;
            int Total = BitCount - 1;
            while(BitCount-- > 0) {
                var Next = NextBit();
                if(Next == -1)
                    break;
                Res |= (Next << Total - BitCount);
            }
            return Res;
        }

        public uint ReadUInt(int BitCount) {
            if(BitCount > 32 || BitCount < 0)
                throw new ArgumentOutOfRangeException();
            uint Res = 0;
            while(BitCount-- > 0) {
                uint Next = (uint)NextBit();
                if(Next == uint.MaxValue)
                    break;
                Res |= Next;
                Res <<= 1;
            }
            return Res;
        }

        public long ReadLong(int BitCount) {
            if(BitCount > 63 || BitCount < 0)
                throw new ArgumentOutOfRangeException();
            long Res = 0;
            while(BitCount-- > 0) {
                long Next = NextBit();
                if(Next == -1)
                    break;
                Res |= Next;
                Res <<= 1;
            }
            return Res;
        }

        public override long Seek(long offset, SeekOrigin origin) {
            if(_BaseStream == null)
                throw new InvalidOperationException();
            var res = _BaseStream.Seek(offset, origin);
            _CurrentData = ReadByte();
            Position = 0;
            return res;
        }

        public override void SetLength(long value) {
            if(_BaseStream == null)
                throw new InvalidOperationException();
            _BaseStream.SetLength(value);
        }

        public override void Write(Byte[] buffer, int offset, int count) {
            if(_BaseStream == null)
                throw new InvalidOperationException();
            _BaseStream.Write(buffer, offset, count);
            _Position = -1;
        }

        public void Write(bool BitValue) {
            if(_WritePosition == 24)
                FlushBinaryData();
            if(BitValue)
                _WriteData |= 1 << _WritePosition;
            _WritePosition++;
        }

        public void WriteData(int Data, int BitCount) {
            while(BitCount-- > 0) {
                Write((Data & 1) != 0);
                Data >>= 1;
            }
        }

        public void FlushBinaryData() {
            while(_WritePosition > 0) {
                _BaseStream.WriteByte((byte)(_WriteData & 0xff));
                _WritePosition -= 8;
                _WriteData >>= 8;
            }
            _WriteData = 0;
            _WritePosition = 0;
        }

        public void Reset() {
            _CurrentData = _BaseStream.ReadByte();
            _Position = 0;
            _WritePosition = 0;
            _WriteData = 0;
        }

    }
}
