using System;

using Spear.Inf.Core.DBRef;

namespace Spear.Demo.DBIns.Stainless.Entity
{
    public abstract class CommonData : IDBEntity
    {
        public abstract DateTime CreateTime { get; set; }
    }
}
