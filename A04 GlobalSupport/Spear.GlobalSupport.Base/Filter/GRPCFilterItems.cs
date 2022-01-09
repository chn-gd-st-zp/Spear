using System.Collections.Generic;
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
            Add(new GRPCFilterItem());
        }
    }

    public class GRPCFilterItem : GRPCFilterItemBase
    {
        private ISessionNAuth<HTTPTokenProvider> _sessionNAuth { get; set; }

        public GRPCFilterItem()
        {
            _sessionNAuth = ServiceContext.Resolve<ISessionNAuth<HTTPTokenProvider>>();
        }

        public override void OnExecuting(object context)
        {
            MS.ServiceContext realContext = context as MS.ServiceContext;

            base.OnExecuting(realContext);

            //var permissionAttr = realContext.MethodInfo.GetCustomAttribute<PermissionAttribute>();
            //if (permissionAttr == null)
            //    return;

            //_sessionNAuth.VerifyPermission(permissionAttr.Code);

            //if(permissionAttr.AccessLogger)
        }
    }
}
