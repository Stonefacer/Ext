using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ext.Tree;

namespace VerySlowCompress {
    public class HuffmanNode<T> : BinaryTreeNode<T>, IComparable<HuffmanNode<T>> {

        public HuffmanNode<T> Parent = null;
        public HuffmanNode<T> LeftChild = null;
        public HuffmanNode<T> RightChild = null;

        public long Weight { get; protected set; }
        public new bool IsLeaf { get { return LeftChild == null && RightChild == null; } }

        public override IBinaryTreeNode<T> Left { get { return LeftChild; } }
        public override IBinaryTreeNode<T> Right { get { return RightChild; } }

        public HuffmanNode(T Data, long Weight) {
            this.Weight = Weight;
            this.Data = Data;
        }

        public HuffmanNode(HuffmanNode<T> LeftChild, HuffmanNode<T> RightChild) {
            Weight = LeftChild.Weight + RightChild.Weight;
            LeftChild.Parent = this;
            RightChild.Parent = this;
            this.LeftChild = LeftChild;
            this.RightChild = RightChild;
            Data = EmptyData;
        }

        public int GetMaxDeep() {
            int res = 0;
            var Current = this;
            while(Current != null) {
                Current = Current.LeftChild;
                res++;
            }
            return res;
        }

        public int CompareTo(HuffmanNode<T> other) {
            var res = Weight - other.Weight;
            if(res > int.MaxValue)
                return 1;
            else if(res < int.MinValue)
                return -1;
            return (int)res;
        }
    }
}
