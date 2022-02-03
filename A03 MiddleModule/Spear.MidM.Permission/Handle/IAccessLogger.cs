using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.Interface;

namespace Spear.MidM.Permission
{
    public interface IAccessLoggerRepository : IDBRepository
    {
        bool Create(AccessRecord accessRecord);

        DBEntity_Base GetDataObj(string tbName, string primeryKeyName, object primeryKeyValue);
    }
}
