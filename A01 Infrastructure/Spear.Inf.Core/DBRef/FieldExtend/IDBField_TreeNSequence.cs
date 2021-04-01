using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.DBRef
{
    public interface IDBField_TreeNSequence<T> : IDBField_Tree<T>, IDBField_Sequence
    {
        string FullSequence { get; set; }
    }

    public static class IDBField_TreeNSequence_Ext
    {
        public static void SetFullSequence<TEntity, TKey>(this TEntity obj, string full = "") where TEntity : class, IDBField_TreeNSequence<TKey>
        {
            if (obj == null)
                return;

            string cur_str = obj.CurSequence.ToString();
            string full_str = full == null ? "" : full.ToString();

            if (full_str.IsEmptyString() && cur_str.IsEmptyString())
            {
                obj.FullSequence = "";
                return;
            }

            if (full_str.IsEmptyString())
            {
                obj.FullSequence = "," + cur_str + ",";
                return;
            }

            full_str = full_str.StartsWith(",") ? full_str : "," + full_str;
            full_str = full_str.EndsWith(",") ? full_str : full_str + ",";

            obj.FullSequence = full_str + cur_str + ",";
        }

        public static void SetFullSequenc<TEntity, TKey>(this TEntity obj, string[] fulls) where TEntity : class, IDBField_TreeNSequence<TKey>
        {
            if (obj == null)
                return;

            string cur_str = obj.CurSequence;

            if ((fulls == null || fulls.Length == 0) && cur_str.IsEmptyString())
            {
                obj.FullSequence = "";
                return;
            }

            if (fulls == null || fulls.Length == 0)
            {
                obj.FullSequence = "," + cur_str + ",";
                return;
            }

            string fulls_str = string.Join(",", fulls);
            fulls_str = fulls_str.StartsWith(",") ? fulls_str : "," + fulls_str;
            fulls_str = fulls_str.EndsWith(",") ? fulls_str : fulls_str + ",";

            obj.FullSequence = fulls_str + cur_str + ",";
        }
    }
}
