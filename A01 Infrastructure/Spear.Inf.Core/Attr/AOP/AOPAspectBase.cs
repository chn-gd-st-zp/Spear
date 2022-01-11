using System;
using System.Diagnostics;
using System.Reflection;

namespace Spear.Inf.Core.Attr
{
    public abstract class AOPAspectBase
    {
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

        protected virtual void Before(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams) { }

        protected virtual void Error(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, Exception error, out bool throwException) { throwException = true; }

        protected virtual object After(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, object actionResult) { return actionResult; }
    }
}
