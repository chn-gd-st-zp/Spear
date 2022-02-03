using System;

using System.Collections.Generic;

using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Injection;

namespace Spear.MidM.Permission
{
    public interface IPermissionEnum
    {
        Type EnumType { get; }
    }

    public interface IPermission : IDBField_Status
    {
        bool AccessLogger { get; set; }
    }

    public interface IPermissionRepository : IDBRepository
    {
        bool Create(IEnumerable<IPermission> permissions);

        bool Delete(IEnumerable<IPermission> permissions);

        IPermission Permission(string code);

        IEnumerable<IPermission> AllPermission();
    }

    public interface IAccessLoggerRepository : IDBRepository
    {
        bool Create(AccessRecord accessRecord);

        DBEntity_Base GetDataObj(Type dbType, string tbName, string primeryKeyName, object primeryKeyValue);
    }

    public interface IPermissionInitialization : ISingleton
    {
        void Operation(IEnumerable<IPermission> permissionsFromService, IEnumerable<IPermission> permissionsFromDB);
    }
}
