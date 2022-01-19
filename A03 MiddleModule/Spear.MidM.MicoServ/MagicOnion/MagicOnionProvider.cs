using System.Reflection;

using Spear.Inf.Core.Interface;

namespace Spear.MidM.MicoServ.MagicOnion
{
    public class MagicOnionProvider : IMicoServProvider
    {
        private MethodInfo _methodInfo4Resolve;

        public MagicOnionProvider()
        {
            _methodInfo4Resolve = typeof(MagicOnionExtend).GetMethod("Resolve");
        }

        public TContainer Resolve<TContainer>(string address) where TContainer : IMicoServContainer
        {
            var mi = _methodInfo4Resolve.MakeGenericMethod(typeof(TContainer));
            var result = mi.Invoke(typeof(TContainer), new object[] { address });
            return (TContainer)result;
        }
    }
}
