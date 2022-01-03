using System.Linq;

namespace Spear.Inf.Core.DBRef
{
    public interface IDBField_Sequence
    {
        string CurSequence { get; set; }
    }

    public static class IDBField_Sequence_Extend
    {
        public static readonly int MaxLength = 10;
        public static readonly int MaxLength_Full = 255;

        public static string FormatField_Sort(this int field)
        {
            return field.ToString().PadLeft(MaxLength, '0');
        }

        public static string DefaultField_Sort(this int value)
        {
            value = 0;
            return value.FormatField_Sort();
        }

        public static void SetSort(this IDBField_Sequence obj, int value)
        {
            obj.CurSequence = value.FormatField_Sort();
        }

        public static string GetSort(this IDBField_Sequence obj, int plusNum = 0)
        {
            int value = 0;
            if (!int.TryParse(obj.CurSequence, out value))
                value = 0;

            value += plusNum;

            return value.FormatField_Sort();
        }

        public static string GenericNewSort<T>(this IQueryable<T> query) where T : IDBField_Sequence, new()
        {
            string result = null;

            T obj = query.OrderByDescending(o => o.CurSequence).FirstOrDefault();
            if (obj == null)
                obj = new T();

            result = obj.GetSort(1);

            return result;
        }
    }
}
