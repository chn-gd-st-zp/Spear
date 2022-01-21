using System;
using System.Linq;
using System.Reflection;

using AspectInjector.Broker;

namespace Spear.Inf.Core.Attr
{
    [AspectInjector.Broker.Injection(typeof(KeywordResetAspect))]
    [AttributeUsage(AttributeTargets.Property)]
    public class KeywordResetAttribute : Attribute
    {
        public string Keyword { get; private set; }

        public string ToValue { get; private set; }

        public KeywordResetAttribute(string keyword, string toValue) { Keyword = keyword; ToValue = toValue; }
    }

    [Aspect(Scope.PerInstance)]
    public class KeywordResetAspect : AOPAspectBase
    {
        [Advice(Kind.Around)]
        public new object HandleMethod(
           [Argument(Source.Instance)] object source,
           [Argument(Source.Target)] Func<object[], object> method,
           [Argument(Source.Triggers)] Attribute[] triggers,
           [Argument(Source.Name)] string actionName,
           [Argument(Source.Arguments)] object[] actionParams
        )
        {
            return base.HandleMethod(source, method, triggers, actionName, actionParams);
        }

        protected override object After(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, object actionResult)
        {
            if (
                methodInfo.IsSpecialName
                && methodInfo.Name.Contains("_get_", StringComparison.OrdinalIgnoreCase)
                && actionResult != null
            )
            {
                var attr = triggers.Where(o => o is KeywordResetAttribute).FirstOrDefault();
                if (attr != null)
                {
                    var realAttr = attr as KeywordResetAttribute;
                    actionResult = actionResult.ToString().Replace(realAttr.Keyword, realAttr.ToValue);
                }
            }

            return actionResult;
        }
    }
}
