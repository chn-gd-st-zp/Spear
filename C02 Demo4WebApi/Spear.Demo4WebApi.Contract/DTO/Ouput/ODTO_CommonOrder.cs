using Spear.Demo4WebApi.Base;

namespace Spear.Demo4WebApi.Contract.DTO.Ouput
{
    public class ODTO_CommonOrder
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        public Enum_UserType EUserType { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public object Result { get; set; }
    }
}
