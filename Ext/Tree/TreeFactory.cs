using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.Tree {
    public static class TreeFactory {
        public static BinaryTreeNodeSaver<T> GetTreeSaverNode<T>(IBinaryTreeNode<T> Root, ref int MaxOffset, ref T MaxData, List<BinaryTreeNodeSaver<T>> Result = null) {
            var Res = new BinaryTreeNodeSaver<T>(Root);
            Result?.Add(Res);
            List<BinaryTreeNodeSaver<T>> Saver = new List<BinaryTreeNodeSaver<T>>();
            Saver.Add(Res);
            List<BinaryTreeNodeSaver<T>> Buf = new List<BinaryTreeNodeSaver<T>>();
            //Res.Update(Buf, ref MaxOffset, ref MaxData);
            do {
                foreach(var v in Saver) {
                    v.Update(Buf, ref MaxOffset, ref MaxData);
                }
                Result.AddRange(Buf);
                var SwapBuf = Saver;
                Saver = Buf;
                Buf = SwapBuf;
                Buf.Clear();
            } while(Saver.Count != 0);
            var id = Result.IndexOf(Result.Last(x => !x.IsLeaf)) + 1;
            Result.RemoveRange(id, Result.Count - id);
            return Res;
        }

    }
}
