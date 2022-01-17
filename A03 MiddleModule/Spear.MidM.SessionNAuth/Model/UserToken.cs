using System;

using Newtonsoft.Json;

using Spear.Inf.Core.CusEnum;

namespace Spear.MidM.SessionNAuth
{
    public class SessionInfo4Device
    {
        /// <summary>
        /// 登录设备
        /// </summary>
        [JsonProperty("Entry")]
        public Enum_Entry EEntry { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 推送标识
        /// </summary>
        public string PushToken { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 刷新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiredTime { get; set; }
    }

    public class SessionInfo4Account
    {
        /// <summary>
        /// 角色类型
        /// </summary>
        [JsonProperty("RoleType")]
        public Enum_Role ERoleType { get; set; }

        /// <summary>
        /// 角色代码
        /// </summary>
        public string[] RoleCodes { get; set; }

        /// <summary>
        /// 账号ID
        /// </summary>
        public string AccountID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 刷新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiredTime { get; set; }
    }

    public abstract class UserToken
    {
        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 权限代码
        /// </summary>
        public string[] PermissionCodes { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonIgnore]
        public DateTime CreateTime { get { return DeviceInfo.CreateTime; } set { DeviceInfo.CreateTime = value; AccountInfo.CreateTime = value; } }

        /// <summary>
        /// 刷新时间
        /// </summary>
        [JsonIgnore]
        public DateTime UpdateTime { get { return DeviceInfo.UpdateTime; } set { DeviceInfo.UpdateTime = value; AccountInfo.UpdateTime = value; } }

        /// <summary>
        /// 过期时间
        /// </summary>
        [JsonIgnore]
        public DateTime ExpiredTime { get { return DeviceInfo.ExpiredTime; } set { DeviceInfo.ExpiredTime = value; AccountInfo.ExpiredTime = value; } }

        /// <summary>
        /// 设备信息
        /// </summary>
        public SessionInfo4Device DeviceInfo { get; set; } = new SessionInfo4Device();

        /// <summary>
        /// 账号信息
        /// </summary>
        public SessionInfo4Account AccountInfo { get; set; } = new SessionInfo4Account();
    }

    public class UserTokenCache : UserToken
    {
        //
    }

    public class UserTokenRunTime : UserTokenCache
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheMaintainMinutes">minutes</param>
        public void Extenstion(int cacheMaintainMinutes)
        {
            DateTime now = DateTime.Now;

            DeviceInfo.UpdateTime = now;
            DeviceInfo.ExpiredTime = now.AddMinutes(cacheMaintainMinutes);
            AccountInfo.UpdateTime = now;
            AccountInfo.ExpiredTime = now.AddMinutes(cacheMaintainMinutes);
        }
    }
}
