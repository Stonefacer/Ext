using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ext.Data;
using Ext.System.Collections;

namespace Ext.Tree {
    public class BinaryTreeNode<T> : IBinaryTreeNode<T> {

        public static T EmptyData = default(T);
        public static Converter<T, int> Converter;

        public virtual IBinaryTreeNode<T> Left { get; set; }
        public virtual IBinaryTreeNode<T> Right { get; set; }
        public T Data { get; set; }

        public bool IsLeaf { get { return Left == null && Right == null; } }

        public BinaryTreeNode() {

        }

        public BinaryTreeNode(IBinaryTreeNode<T> Node) {
            Left = Node.Left;
            Right = Node.Right;
        }

        // |(1)| |(2)| |(3)|
        // 11111 11111 ?????
        // (1) - size of offset
        // (2) - size of data
        // (3) - node
        // | (1) | | (2) | |(3)| |  (4)  |
        // ??????? ???????   1   ?????????
        // (1) - offset of the left child
        // (2) - offset of the right child
        // (3) - is data present
        // (4) - data (only if (3) is equal to 1)
        // if (1) and (2) are equal to 0 then current node is a leaf

        public byte[] ToByteArray() {
            int MaxOffset = 0;
            T MaxData = default(T);
            List<BinaryTreeNodeSaver<T>> Result = new List<BinaryTreeNodeSaver<T>>();
            var BinaryTreeNode = TreeFactory.GetTreeSaverNode<T>(this, ref MaxOffset, ref MaxData, Result);
            int MaxLen = ExtBitConverter.GetBitsCount(MaxOffset - 1) - 1;
            int DataLen = ExtBitConverter.GetBitsCount(Converter(MaxData)) - 1;
            ExtBitArray Buffer = new ExtBitArray();
            Buffer.Push(MaxLen, 5);
            Buffer.Push(DataLen, 5);
            foreach(var v in Result) {
                if(!v.IsLeaf) {
                    Buffer.Push(v.Left.Weight - v.Weight - 1, MaxLen);
                    //Buffer.Push(v.Right.Weight - v.Weight - 1, MaxLen);
                    if(!v.Data.Equals(EmptyData)) {
                        Buffer.Push(1, 1);
                        Buffer.Push(Converter(v.Data), DataLen);
                    } else
                        Buffer.Push(0, 1);
                } else {
                    Buffer.Push(0, MaxLen);
                    if(!v.Data.Equals(EmptyData)) {
                        Buffer.Push(1, 1);
                        Buffer.Push(Converter(v.Data), DataLen);
                    } else
                        Buffer.Push(0, 1);
                }
            }
            return Buffer.ToByteArray();
        }

        public static BinaryTreeNode<T> FromByteArray(byte[] Data) {
            BinaryTreeNodeSaver<T> res = new BinaryTreeNodeSaver<T>();
            ExtBitArray Bits = new ExtBitArray(Data);
            Bits.Seek(0);
            int OffsetLen = Bits.Read(5);
            int DataLen = Bits.Read(5);
            res.Weight = 0;
            List<int> Offsets = new List<int>();
            Offsets.Add(Bits.Read(OffsetLen));
            Offsets.Add(Bits.Read(OffsetLen));
            res.Weight = 0;
            int Weight = 0;
            for(int i = 0; i < Offsets.Count; i++) {
                if(Offsets[i] == 0)
                    continue;
                Weight += Offsets[i];
                Bits.Seek((10 + Weight) * 8);
                Offsets.Add(Bits.Read(OffsetLen));
            }
            return res;
        }

    }
}
