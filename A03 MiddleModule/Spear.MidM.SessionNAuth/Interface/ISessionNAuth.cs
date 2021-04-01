namespace Spear.MidM.SessionNAuth
{
    public interface ISessionNAuth<T> : ITokenProvider
    {
        string CurToken { get; }

        UserTokenRunTime CurUserToken { get; }
    }
}
