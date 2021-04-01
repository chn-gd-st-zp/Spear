using MessagePack;
using System.Collections.Generic;

namespace Spear.Inf.Core.DTO
{
    [MessagePackObject(true)]
    public class ODTO_Tree<T> : List<ODTO_TreeNode<T>> where T : class
    {
        //
    }

    [MessagePackObject(true)]
    public class ODTO_TreeNode<T> where T : class
    {
        public T Info { get; set; }
        public ODTO_Tree<T> Childs { get; set; }

        public ODTO_TreeNode()
        {
            Info = default(T);
            Childs = new ODTO_Tree<T>();
        }
    }
}
