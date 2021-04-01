using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using AspectInjector.Broker;

using Spear.Inf.Core.CusException;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.Attr
{
    [Aspect(Scope.Global)]
    [Injection(typeof(InputVerifyAttribute))]
    public class InputVerifyAttribute : AOPAttribute
    {
        public override void Before(object source, MethodInfo methodInfo, Attribute[] triggers, string actionName, object[] actionParams)
        {
            actionParams.Verify();
        }

        [Advice(Kind.Around)]
        public override object HandleMethod(
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

    public static class InputVerifyAttributeExtend
    {
        public static void Verify(this object paramObj)
        {
            string errorMsg = "";

            List<IDTO_Input> actionParams = new List<IDTO_Input>();
            LoadParams(paramObj, typeof(IDTO_Input), actionParams);

            foreach (object actionParam in actionParams)
            {
                IDTO_Input input = actionParam as IDTO_Input;
                if (input == null)
                    continue;

                if (!input.VerifyField(out errorMsg))
                    throw new Exception_VerifyError(errorMsg);
            }
        }

        private static void LoadParams(this object paramObj, Type inputType, List<IDTO_Input> paramList)
        {
            if (paramObj == null)
                return;

            Type paramType = paramObj.GetType();

            if (paramType.IsImplementedType(inputType))
            {
                paramList.Add((IDTO_Input)paramObj);
            }
            else
            {
                if (paramType.IsImplementedType(typeof(ICollection)))
                {
                    ICollection objs = null;

                    if (paramType.IsImplementedType(typeof(IDictionary)))
                        objs = ((IDictionary)paramObj).Values;
                    else
                        objs = (ICollection)paramObj;

                    foreach (var obj in objs)
                        LoadParams(obj, inputType, paramList);
                }
                else
                {
                    var piArray = paramType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var pi in piArray)
                    {
                        if (pi.PropertyType.IsImplementedType(inputType))
                        {
                            var value = pi.GetValue(paramObj);
                            paramList.Add((IDTO_Input)value);
                        }
                    }
                }
            }
        }
    }
}
