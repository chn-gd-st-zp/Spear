﻿using System;
using System.Collections.Generic;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.DTO;
using Spear.Demo4GRPC.Pub.TestDemo;
using Spear.Demo4GRPC.Host.Server.Contract;

namespace Spear.Demo4GRPC.Host.Server.Implement
{
    public class Show : IShow
    {
        public ResultBasic<List<ODTOTestDemo>> List(ListParam input)
        {
            var dataList = new List<ODTOTestDemo>();
            return dataList.ResultBasic_Success();
        }

        public ResultBasic<ODTO_Page<ODTOTestDemo>> Page(PageParam input)
        {
            var pageData = new Tuple<List<ODTOTestDemo>, int>(new List<ODTOTestDemo>(), 0);
            var dataList = pageData.ToODTOPage(input);

            return dataList.ResultBasic_Success();
        }

        public ResultBasic<ODTO_Tree<ODTOTestDemo>> Tree(TreeParam input)
        {
            var dataList = new List<ODTOTestDemo>();

            return dataList.ToTree("").ResultBasic_Success();
        }
    }
}