using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.DBRef
{
    public interface IDBField_Json
    {
        string JsonData { get; set; }
    }

    public static class DBField_Json_Ext
    {
        public static string ToJson<TSource>(this TSource sourceObj) where TSource : IDBField_Json
        {
            return sourceObj.JsonData.ToJson();
        }

        public static TTarget ToObject<TSource, TTarget>(this TSource sourceObj) where TSource : IDBField_Json
        {
            return sourceObj.JsonData.ToObject<TTarget>();
        }
    }
}
