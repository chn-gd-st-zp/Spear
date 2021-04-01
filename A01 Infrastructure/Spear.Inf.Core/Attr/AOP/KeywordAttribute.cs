using System;
using System.Linq;
using System.Reflection;

using AspectInjector.Broker;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Attr
{
    [AttributeUsage(AttributeTargets.Property)]
    public class KeywordDefineAttribute : Attribute
    {
        public string Pattern { get; private set; }

        public KeywordDefineAttribute(string pattern) { Pattern = pattern; }
    }

    [Injection(typeof(KeywordResetAspect))]
    [AttributeUsage(AttributeTargets.Property)]
    public class KeywordResetAttribute : Attribute
    {
        public string Pattern { get; private set; }

        public KeywordResetAttribute(string pattern) { Pattern = pattern; }
    }

    [Aspect(Scope.Global)]
    public class KeywordResetAspect : AOPAttribute
    {
        public override object After(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, object actionResult)
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
                    var kr = ServiceContext.ResolveServ<IKeywordProcessing>();
                    if (kr != null)
                    {
                        //actionResult = actionResult.ToString().Replace(realAttr.Pattern, kr.GetValue<string>(realAttr.Pattern));
                    }
                }
            }

            return actionResult;
        }

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
    }
}
