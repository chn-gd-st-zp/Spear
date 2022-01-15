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

        public static string FormatSequence(this string field)
        {
            return field.PadLeft(MaxLength, '0');
        }

        public static string DefaultSequence(this string value)
        {
            return "0".FormatSequence();
        }

        public static void SetSequence(this IDBField_Sequence obj, string value)
        {
            obj.CurSequence = value.FormatSequence();
        }

        public static string GetSequence(this IDBField_Sequence obj, int plusNum = 0)
        {
            int value = 0;
            if (!int.TryParse(obj.CurSequence, out value))
                value = 0;

            value += plusNum;

            return value.ToString().FormatSequence();
        }
    }
}
