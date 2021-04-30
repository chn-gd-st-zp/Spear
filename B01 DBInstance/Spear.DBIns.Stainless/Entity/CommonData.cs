using System;

using Spear.Inf.Core.DBRef;

namespace Spear.DBIns.Stainless.Entity
{
    public abstract class CommonData : DBEntity_Basic
    {
        public abstract DateTime CreateTime { get; set; }
    }
}
