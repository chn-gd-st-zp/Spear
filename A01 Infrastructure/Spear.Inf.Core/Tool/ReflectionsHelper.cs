namespace Spear.Inf.Core.Tool
{
    public class ReflectionsHelper
    {
        public static T DynamicBestowValue<T>(T target, T source)
        {
            var type = typeof(T);

            foreach (var property in type.GetProperties())
            {
                var value = property.GetValue(source, null);
                if (value == null)
                    continue;

                property.SetValue(target, value, null);
            }

            return target;
        }

        public static T DynamicBestowValue<T>(T target, string name, string value)
        {
            var type = typeof(T);
            type.GetProperty(name)?.SetValue(target, value, null);
            return target;
        }
    }
}
