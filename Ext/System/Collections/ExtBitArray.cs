using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.System.Collections {
    public class ExtBitArray {

        private int _ElementId = 0;
        private int _BitId = 0;
        private ulong[] _Data = null;

        /// <summary>
        /// Max capasity in bits
        /// </summary>
        public int Capasity { get { return _Data.Length * sizeof(ulong) * 8; } }

        /// <summary>
        /// Currently in use (bits)
        /// </summary>
        public int CountBit { get { return _ElementId * sizeof(ulong) * 8 + _BitId; } }

        public ExtBitArray() {
            _Data = new ulong[256];
        }

        public ExtBitArray(int Lenght) {
            _Data = new ulong[Lenght];
        }

        public ExtBitArray(byte[] Data) {
            while(_ElementId * sizeof(ulong) < Data.Length - sizeof(ulong)) {
                _Data[_ElementId++] = BitConverter.ToUInt64(Data, _ElementId * sizeof(ulong));
                if(_ElementId >= Data.Length)
                    AutoReszie();
            }
        }

        public void Push(int Num, int Len) {
            while(Len-- > 0) {
                if((Num & 1) != 0) {
                    _Data[_ElementId] |= (1UL << _BitId);
                }
                Num >>= 1;
                Incerement();
            }
        }

        public int Read(int Len) {
            int Res = 0;
            for(int i=0;i< Len; i++) {
                if((_Data[_ElementId] & (1UL << _BitId)) != 0)
                    Res |= 1;
                Incerement();
            }
            return Res;
        }

        public void Seek(int BitId) {
            _ElementId = BitId / sizeof(ulong);
            _BitId = BitId % sizeof(ulong);
        }

        public void Clear() {
            _ElementId = 0;
            _BitId = 0;
        }

        public byte[] ToByteArray() {
            int Len = (_Data.Length * 64 + _BitId) >> 3;
            if(_BitId % 8 != 0)
                Len++;
            int Offset = 0;
            byte[] Result = new byte[Len];
            while(Len - Offset > sizeof(ulong)) {
                Array.Copy(BitConverter.GetBytes(_Data[Offset >> 3]), 0, Result, Offset, sizeof(ulong));
                Offset += sizeof(ulong);
            }
            Array.Copy(BitConverter.GetBytes(_Data[Offset >> 3]), 0, Result, Offset, Len);
            return Result;
        }

        private void AutoReszie() {
            ulong[] buf = new ulong[_Data.Length * 2];
            Array.Copy(_Data, _Data, _Data.Length);
            _Data = buf;
        }

        private void Incerement() {
            _BitId++;
            if(_BitId > 63) {
                _BitId = 0;
                _ElementId++;
                if(_ElementId >= _Data.Length)
                    AutoReszie();
            }
        }

        private void Decrement() {
            if(_BitId == 0) {
                if(_ElementId == 0)
                    throw new InvalidOperationException();
                _ElementId--;
            } else {
                _BitId--;
            }
        }


    }
}
