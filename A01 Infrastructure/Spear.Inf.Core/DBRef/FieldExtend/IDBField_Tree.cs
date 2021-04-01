using System.Collections.Generic;
using System.Linq;

using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.DBRef
{
    public interface IDBField_Tree<T>
    {
        T CurNode { get; set; }

        T ParentNode { get; set; }

        string FullNode { get; set; }
    }

    public static class IDBField_Tree_Ext
    {
        public static ODTO_Tree<TEntity> ToTree<TEntity, TKey>(this List<TEntity> sourceDataList, TKey pathTag = default) where TEntity : class, IDBField_Tree<TKey>
        {
            ODTO_Tree<TEntity> tree = new ODTO_Tree<TEntity>();

            var query = sourceDataList.AsQueryable();

            if (pathTag == null)
                query = query.Where(o => o.ParentNode == null);
            else
                query = query.Where(o => o.ParentNode != null && o.ParentNode.ToString() == pathTag.ToString());

            List<TEntity> dataList = query.ToList();
            foreach (TEntity data in dataList)
            {
                ODTO_TreeNode<TEntity> treeNode = new ODTO_TreeNode<TEntity>();
                treeNode.Info = data;
                treeNode.Childs = sourceDataList.ToTree(data.CurNode);

                tree.Add(treeNode);
            }

            return tree;
        }

        public static void SetFullNode<TEntity, TKey>(this TEntity obj, string full = "") where TEntity : class, IDBField_Tree<TKey>
        {
            if (obj == null)
                return;

            string cur_str = obj.CurNode.ToString();
            string full_str = full == null ? "" : full.ToString();

            if (full_str.IsEmptyString() && cur_str.IsEmptyString())
            {
                obj.FullNode = "";
                return;
            }

            if (full_str.IsEmptyString())
            {
                obj.FullNode = "," + cur_str + ",";
                return;
            }

            full_str = full_str.StartsWith(",") ? full_str : "," + full_str;
            full_str = full_str.EndsWith(",") ? full_str : full_str + ",";

            obj.FullNode = full_str + cur_str + ",";
        }

        public static void SetFullNode<TEntity, TKey>(this TEntity obj, string[] fulls) where TEntity : class, IDBField_Tree<TKey>
        {
            if (obj == null)
                return;

            string cur_str = obj.CurNode.ToString();

            if ((fulls == null || fulls.Length == 0) && cur_str.IsEmptyString())
            {
                obj.FullNode = "";
                return;
            }

            if (fulls == null || fulls.Length == 0)
            {
                obj.FullNode = "," + cur_str + ",";
                return;
            }

            string fulls_str = string.Join(",", fulls);
            fulls_str = fulls_str.StartsWith(",") ? fulls_str : "," + fulls_str;
            fulls_str = fulls_str.EndsWith(",") ? fulls_str : fulls_str + ",";

            obj.FullNode = fulls_str + cur_str + ",";
        }
    }
}
