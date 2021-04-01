using System;

using Spear.Inf.Core.CusEnum;

namespace Spear.Inf.Core.Interface
{
    /// <summary>
    /// 消息队列方法
    /// </summary>
    public enum Enum_MQFunc
    {
        /// <summary>
        /// 发布、订阅
        /// </summary>
        Exec,

        /// <summary>
        /// 退出队列、交换机
        /// </summary>
        Quit,
    }

    /// <summary>
    /// 消息队列服务
    /// </summary>
    /// <typeparam name="THandle"></typeparam>
    /// <typeparam name="TParam"></typeparam>
    public interface IMsgQueueService<THandle, TParam> : IDisposable
        where THandle : IMsgQueueHandle<TParam>
        where TParam : class, IMsgQueueParam
    {
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="mqFunc"></param>
        /// <param name="mqParam"></param>
        void Run(Enum_MQFunc mqFunc, TParam mqParam = null);
    }

    /// <summary>
    /// 消息队列处理器
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    public interface IMsgQueueHandle<TParam> : IDisposable
        where TParam : class, IMsgQueueParam
    {
        /// <summary>
        /// 处理器状态
        /// </summary>
        Enum_Process EStatus { get; set; }
    }

    /// <summary>
    /// 消息队列参数
    /// </summary>
    public interface IMsgQueueParam
    {
        //
    }
}
