using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.Interface;

namespace Spear.MidM.Permission
{
    public interface IAccessLoggerRepository : IDBRepository
    {
        DBEntity_Base GetDataObj(string tbName, object primeryKey);

        bool Create(AccessRecord accessRecord);
    }
}
