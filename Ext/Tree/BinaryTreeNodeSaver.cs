using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Ext.System.Core;
using Ext.Data;
using Ext.System.Collections;

namespace Ext.Tree {
    public class BinaryTreeNodeSaver<T> : BinaryTreeNode<T> {

        public static int LastWeight = 0;

        public new BinaryTreeNodeSaver<T> Left { get; set; }
        public new BinaryTreeNodeSaver<T> Right { get; set; }
        public int Weight { get; set; }
        public new bool IsLeaf { get { return base.IsLeaf; } }

        public BinaryTreeNodeSaver() {
            Weight = LastWeight++;
        }

        public BinaryTreeNodeSaver(IBinaryTreeNode<T> Node):base(Node) {
            Weight = LastWeight++;
            Data = Node.Data;
        }

        public void Update(List<BinaryTreeNodeSaver<T>> Result, ref int MaxOffset, ref T MaxData) {
            if(!base.IsLeaf) {
                Left = new BinaryTreeNodeSaver<T>(base.Left);
                Right = new BinaryTreeNodeSaver<T>(base.Right);
                Result.Add(Left);
                Result.Add(Right);
                var Offset = Right.Weight - Weight;
                if(Offset > MaxOffset)
                    MaxOffset = Offset;
            }
            if(!Data.Equals(EmptyData) && Converter(Data) > Converter(MaxData))
                MaxData = Data;
        }

        //public int GetMaxOffset() {
        //    if(IsLeaf)
        //        return 0;
        //    return Math.Max(Weight - Right.Weight, Math.Max(Left.GetMaxOffset(), Right.GetMaxOffset()));
        //}

    }
}
