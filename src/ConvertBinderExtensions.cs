using System.Dynamic;

namespace System.Text.Json
{
    internal static class ConvertBinderExtensions
    {
        public static bool IsType<T>(this ConvertBinder binder)
            where T : class
            => binder.Type == typeof(T);

        public static bool IsValueType<T>(this ConvertBinder binder)
            where T : struct
            => binder.Type == typeof(T) || binder.Type == typeof(T?);
    }
}
