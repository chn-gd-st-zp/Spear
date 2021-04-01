namespace Spear.MidM.RabbitMQ
{
    /// <summary>
    /// 优先级
    /// </summary>
    public enum Enum_Priority
    {
        None = -1,
        Zero = 0,
        //None,
        //Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
    }

    /// <summary>
    /// 交换机类型
    /// </summary>
    public enum Enum_ExchangeType
    {
        /// <summary>
        /// 路由模式
        /// 如果路由键完全匹配的话,消息才会被投放到相应的队列
        /// </summary>
        Direct,

        /// <summary>
        /// 发布订阅模式
        /// 当发送一条消息到fanout交换器上时,它会把消息投放到所有附加在此交换器的上的队列
        /// </summary>
        Fanout,

        /// <summary>
        /// 通配符模式
        /// 设置模糊的绑定方式,"*"操作符将"."视为分隔符,匹配单个字符;"#"操作符没有分块的概念,它将任意"."均视为关键字的匹配部分,能够匹配多个字符
        /// </summary>
        Topic,

        /// <summary>
        /// Headers模式
        /// 交换器允许匹配AMQP消息的header而非路由键,除此之外,header交换器和direct交换器完全一致,但是性能却差很多
        /// </summary>
        Headers,
    }
}
