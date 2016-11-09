using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.Tree {
    public interface IBinaryTreeNode<T>:ITreeNode {
        IBinaryTreeNode<T> Left { get; set; }
        IBinaryTreeNode<T> Right { get; set; }
        T Data { get; set; }
    }
}
