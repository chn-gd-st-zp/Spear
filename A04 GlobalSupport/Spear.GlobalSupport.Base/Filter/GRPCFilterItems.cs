﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;
using Spear.MidM.SessionNAuth;

using MS = MagicOnion.Server;

namespace Spear.GlobalSupport.Base.Filter
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(IRequestFilterItems), Enum_FilterType.GRPC)]
    public class GRPCFilterItems : List<IRequestFilterItem>, IRequestFilterItems
    {
        public GRPCFilterItems()
        {
            Add(new GRPCFilterItemForAuth());
        }
    }

    public class GRPCFilterItemForAuth : GRPCFilterItemBase
    {
        private SessionNAuth<HTTPTokenProvider> _sessionNAuth { get; set; }

        public GRPCFilterItemForAuth()
        {
            _sessionNAuth = ServiceContext.Resolve<SessionNAuth<HTTPTokenProvider>>();
        }

        public override void OnExecuting(object context)
        {
            MS.ServiceContext realContext = context as MS.ServiceContext;

            base.OnExecuting(realContext);

            var paList = realContext.MethodInfo.GetCustomAttributes<PermissionAuthAttribute>().ToList();
            if (paList != null && paList.Count() != 0)
                Auth(paList);
        }

        protected void Auth(List<PermissionAuthAttribute> paList)
        {
            foreach (var pa in paList)
                _sessionNAuth.PermissionAuth(pa.Code);
        }
    }
}
