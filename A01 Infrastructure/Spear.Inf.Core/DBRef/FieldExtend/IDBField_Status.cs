using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.DBRef
{
    public interface IDBField_Status
    {
        string Status { get; set; }

        Enum_Status EStatus { get; set; }
    }
}
