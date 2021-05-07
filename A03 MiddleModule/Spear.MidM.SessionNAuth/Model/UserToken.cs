using System;

using Spear.Inf.Core.CusEnum;

namespace Spear.MidM.SessionNAuth
{
    public class UserToken
    {
        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 登录设备
        /// </summary>
        public Enum_Entry EFacility { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 刷新时间
        /// </summary>
        public DateTime RefreshTime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public Enum_Role RoleType { get; set; }

        /// <summary>
        /// 角色代码
        /// </summary>
        public string[] RoleCodes { get; set; }
    }

    public class UserTokenCache : UserToken
    {
        public string MenuCodes { get; set; }
    }

    public class UserTokenRunTime : UserTokenCache
    {
        //public List<string> MenuCodeList { get; set; }

        //public List<Permission> Menus { get; set; }

        //public List<Permission_Role> RolePermissions { get; set; }
    }
}
