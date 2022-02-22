using System.Collections.Generic;

using MessagePack;

namespace Spear.Inf.Core.DTO
{
    [MessagePackObject(true)]
    public class ODTO_Tree<T> : List<ODTO_TreeNode<T>>, IDTO where T : class
    {
        //
    }

    [MessagePackObject(true)]
    public class ODTO_TreeNode<T> : IDTO where T : class
    {
        public T Info { get; set; }
        public List<ODTO_TreeNode<T>> Childs { get; set; }

        public ODTO_TreeNode()
        {
            Info = default(T);
            Childs = new ODTO_Tree<T>();
        }
    }
}
