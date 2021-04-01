namespace Spear.Inf.Core.Interface
{
    public interface IKeywordProcessing
    {
        void SetValue(string pattern, object value);

        T GetValue<T>(string pattern);

        void LoadFormObject(object source);

        void SetObject(object target);
    }
}
