namespace Spear.Inf.Core.ServGeneric.IOC
{
    public interface IAutoFac
    {
        //
    }

    public interface IIOCIgnore : IAutoFac
    {
        //
    }

    /// <summary>
    /// 每次获取都是新对象
    /// </summary>
    public interface ITransient : IAutoFac
    {
        //
    }

    /// <summary>
    /// 依赖于某个对象的生命周期，在这个对象的生命周期内，所获取的目标对象，都是同一个
    /// </summary>
    public interface IScoped : IAutoFac
    {
        //
    }

    /// <summary>
    /// 单例
    /// </summary>
    public interface ISingleton : IAutoFac
    {
        //
    }
}
