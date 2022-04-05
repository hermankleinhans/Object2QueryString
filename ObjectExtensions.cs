using System.Collections;

namespace Object2QueryString
{
    public static class ObjectExtensions
    {
        public static string ToQueryString(this object source)
        {
            var properties = new List<KeyValuePair<string, string>>();
            CreateProperties(source, properties);
            return new FormUrlEncodedContent(properties).ReadAsStringAsync().Result;
        }

        private static void CreateProperties(object source, List<KeyValuePair<string, string>> properties, bool useFullname = false)
        {
            var objType = source.GetType();
            foreach (var property in source.GetType().GetProperties())
            {
                var key = useFullname ? $"{property.ReflectedType?.Name}.{property.Name}" : property.Name;
                var value = property.GetValue(source, null) ?? "";          
                var elems = value as IList;
                if (elems != null)
                {
                    if (value is IEnumerable<string>)
                    {
                        properties.Add(new KeyValuePair<string, string>(key, string.Join(",", value as IEnumerable<string>)));
                    }
                    else if (GetEnumerableTypes(elems.GetType()).Any(t => t.IsEnum))
                    {
                        var list = new List<string>();
                        foreach (object item in elems)
                        {
                            list.Add(item?.ToString() ?? "");
                        }
                        properties.Add(new KeyValuePair<string, string>(key, string.Join(",", list)));
                    }
                    else
                    {
                        foreach (var item in elems)
                        {
                            CreateProperties(item, properties);
                        }
                    }
                }
                else
                {
                    // This will not cut-off System.Collections because of the first check
                    if (property.PropertyType.Assembly == objType.Assembly)
                    {
                        CreateProperties(value, properties, true);
                    }
                    else
                    {
                        properties.Add(new KeyValuePair<string, string>(key, value?.ToString() ?? ""));
                    }
                }
            }
        }

        private static IEnumerable<Type> GetEnumerableTypes(Type type)
        {
            if (type.IsInterface)
            {
                if (type.IsGenericType
                    && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    yield return type.GetGenericArguments()[0];
                }
            }
            foreach (Type intType in type.GetInterfaces())
            {
                if (intType.IsGenericType
                    && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    yield return intType.GetGenericArguments()[0];
                }
            }
        }
    }
}
