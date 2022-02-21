using System;

using MessagePack;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.DTO
{
    [MessagePackObject(true)]
    public abstract class IDTO_Input
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        [Remark("时间戳")]
        public long? Timestamp { get; set; };

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public virtual bool Validation(out string errorMsg) { errorMsg = string.Empty; return true; }
    }
}
