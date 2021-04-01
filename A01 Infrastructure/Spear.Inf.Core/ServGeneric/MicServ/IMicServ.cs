using MagicOnion;

namespace Spear.Inf.Core.ServGeneric.MicServ
{
    public interface IMicServ<T> : IServ, IService<T>
    {
        //
    }
    public interface IMicServGener
    {
        T GetServ<T>(params object[] paramsArray) where T : IMicServ<T>;
    }
}
