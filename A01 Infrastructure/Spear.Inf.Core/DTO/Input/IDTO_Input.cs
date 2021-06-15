﻿using MessagePack;

namespace Spear.Inf.Core.DTO
{
    [MessagePackObject(true)]
    public abstract class IDTO_Input
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool VerifyField(out string errorMsg) { errorMsg = ""; return true; }
    }
}
