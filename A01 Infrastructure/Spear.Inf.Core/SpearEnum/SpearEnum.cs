﻿using System;
using System.Collections.Generic;

using Spear.Inf.Core.Injection;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.Interface
{
    public interface ISpearEnum : ISingleton { }
}

namespace Spear.Inf.Core.CusEnum
{
    [Serializable]
    public abstract class SpearEnum : ISpearEnum
    {
        protected readonly SpearEnumFactory Factory;

        public SpearEnum() { Factory = new SpearEnumFactory(); }
    }

    [Serializable]
    public class SpearEnumItem
    {
        public string Name { get; set; }

        public int Value { get; set; }

        internal SpearEnumItem() { }

        internal string ToStr() { return Name; }

        internal int ToInt() { return Value; }

        public string ToIntString() { return Value.ToString(); }
    }

    public class SpearEnumFactory
    {
        internal List<SpearEnumItem> Members = new List<SpearEnumItem>();
        internal Dictionary<string, SpearEnumItem> Keys = new Dictionary<string, SpearEnumItem>();
        internal Dictionary<int, SpearEnumItem> Values = new Dictionary<int, SpearEnumItem>();
        internal int Counter = -1;
    }
}
