namespace Spear.Inf.Core.Interface
{
    public interface IMicoServContainer
    {
        //
    }

    public interface IMicoServContainer<TContainer> : IMicoServContainer
    {
        //
    }

    public interface IMicoServProvider
    {
        TContainer Resolve<TContainer>(params object[] paramArray) where TContainer : IMicoServContainer;
    }
}
