﻿using System.Threading.Tasks;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.ServGeneric;

using Spear.Demo.Contract;
using Spear.Demo.Inf.DTO;
using Spear.Demo.DBIns.Stainless;

namespace Spear.Demo.Impl
{
    [DIModeForService(Enum_DIType.Exclusive, typeof(IWebApiService))]
    public class WebApiService : IWebApiService
    {
        public async Task<ODTO_CommonOrder> Page(IDTO_CommonOrder input)
        {
            var rp = ServiceContext.ResolveByKeyed<EFRepository_CommonOrder>(input.EUserType);
            var execResult = rp.Page(input.BeginTime, input.EndTime, input);
            var result = new ODTO_CommonOrder { EUserType = input.EUserType, Result = execResult };
            return result;
        }
    }
}