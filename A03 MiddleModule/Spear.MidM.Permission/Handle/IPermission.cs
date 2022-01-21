using System.Collections.Generic;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Injection;

namespace Spear.MidM.Permission
{
    public interface IPermission
    {
        //
    }

    public interface IPermissionRepository : IDBRepository
    {
        bool Create(IEnumerable<IPermission> permissions);

        bool Delete(IEnumerable<IPermission> permissions);

        IEnumerable<IPermission> AllPermission();
    }

    public interface IPermissionLifeTime : ISingleton
    {
        void Operation(IEnumerable<IPermission> permissionsFromService, IEnumerable<IPermission> permissionsFromDB);
    }
}
