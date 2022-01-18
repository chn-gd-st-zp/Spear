namespace Spear.Inf.Core.ServGeneric.MicServ
{
    public interface IMicServ<T> : MagicOnion.IService<T>
    {
        //
    }

    public interface IMicServGeneric
    {
        T GetServ<T>(params object[] paramsArray) where T : IMicServ<T>;
    }
}
