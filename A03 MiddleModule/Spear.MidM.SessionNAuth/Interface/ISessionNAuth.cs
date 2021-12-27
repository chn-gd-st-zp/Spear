namespace Spear.MidM.SessionNAuth
{
    public interface ISessionNAuth<T> : ITokenProvider
    {
        UserTokenRunTime CurUserToken { get; }
    }
}
