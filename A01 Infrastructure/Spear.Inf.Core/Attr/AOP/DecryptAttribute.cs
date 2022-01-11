using System;
using System.Linq;
using System.Reflection;

using AspectInjector.Broker;
using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.Attr
{
    [Injection(typeof(DecryptAspect))]
    [AttributeUsage(AttributeTargets.Property)]
    public class DecryptAttribute : Attribute
    {
        public Enum_EncryptionNDecrypt EDecryptType { get; private set; }

        public Enum_Environment[] EEnvs { get; private set; }

        public DecryptAttribute(Enum_EncryptionNDecrypt eDecryptType, params Enum_Environment[] eEnvs) { EDecryptType = eDecryptType; EEnvs = eEnvs; }
    }

    [Aspect(Scope.Global)]
    public class DecryptAspect : AOPAspectBase
    {
        public override object After(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams, object actionResult)
        {
            if (
                methodInfo.IsSpecialName
                && methodInfo.Name.Contains("_get_", StringComparison.OrdinalIgnoreCase)
                && actionResult != null
            )
            {
                var attr = triggers.Where(o => o is DecryptAttribute).FirstOrDefault();
                if (attr != null)
                {
                    var realAttr = attr as DecryptAttribute;
                    if (realAttr.EEnvs.Contains(AppInitHelper.EEnvironment))
                    {
                        var encryptionNDecrypt = ServiceContext.ResolveByKeyed<IEncryptionNDecrypt>(realAttr.EDecryptType);
                        if (encryptionNDecrypt != null)
                            actionResult = encryptionNDecrypt.Decrypt(actionResult.ToString());
                    }
                }
            }

            return actionResult;
        }
        /*
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
        }*/
    }
}
