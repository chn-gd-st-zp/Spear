using System;
using System.Diagnostics;
using System.Reflection;

namespace Spear.Inf.Core.Attr
{
    public interface IAOPHandler
    {
        void Before(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams);

        void Error(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, Exception error, out bool throwException);

        object After(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, object actionResult);
    }

    public class AOPHandler :  IAOPHandler
    {
        public virtual void Before(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams) { }
        public virtual void Error(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, Exception error, out bool throwException) { throwException = true; }
        public virtual object After(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, object actionResult) { return actionResult; }

        public virtual object HandleMethod(
            object source,
            Func<object[], object> method,
            Attribute[] triggers,
            string actionName,
            object[] actionParams
        )
        {
            object result = null;

            var methodInfo = method.GetMethodInfo();

            try
            {
                Before(source, methodInfo, triggers, actionName, actionParams);

                var sw = Stopwatch.StartNew();
                result = method(actionParams);
                sw.Stop();
            }
            catch (Exception e)
            {
                bool throwException = false;
                Error(source, methodInfo, triggers, actionName, actionParams, e, out throwException);
                if (throwException)
                    throw;
            }
            finally
            {
                result = After(source, methodInfo, triggers, actionName, actionParams, result);
            }

            return result;
        }
    }
}
