using System;

using Newtonsoft.Json;

using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Tool;
using Spear.Demo4WebApi.Basic;

namespace Spear.Demo4WebApi.Contract.DTO.Input
{
    public class IDTO_CommonOrder : IDTO_Page
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        public Enum_UserType EUserType { get; set; }

        /// <summary>
        /// 开始日期/时间
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 结束日期/时间
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [JsonIgnore]
        public DateTime? BeginTime
        {
            get 
            {
                DateTime? result = null;

                if (BeginDate.IsEmptyString())
                    return result;

                DateTime dt;
                if (DateTime.TryParse(BeginDate, out dt))
                    return dt;

                return BeginDate.ToBeginTime();
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        [JsonIgnore]
        public DateTime? EndTime
        {
            get
            {
                DateTime? result = null;

                if (EndDate.IsEmptyString())
                    return result;

                DateTime dt;
                if (DateTime.TryParse(EndDate, out dt))
                    return dt;

                return EndDate.ToEndTime();
            }
        }
    }
}
