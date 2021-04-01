using Spear.Inf.Core.Attr;

namespace Spear.Inf.Core.CusEnum
{
    public enum Enum_DateCycle
    {
        [Remark("默认、无")]
        None,

        [Remark("天")]
        Day,

        //[Remark("周")]
        //Week,

        [Remark("月")]
        Month,

        //[Remark("季")]
        //Season,

        [Remark("年")]
        Year,
    }

    public enum Enum_TimeUnit
    {
        [Remark("默认、无")]
        None,

        [Remark("小时")]
        Hour,

        [Remark("分钟")]
        Minute,

        [Remark("秒")]
        Second,
    }
}
