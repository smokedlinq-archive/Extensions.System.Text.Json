using System.Dynamic;
using System.Linq;

namespace System.Text.Json
{
    // Another missing feature out of the box for System.Text.Json
    // TODO: Replace with native support if/when added
    // https://github.com/dotnet/runtime/issues/31175
    public class DynamicJsonElement : DynamicObject
    {
        protected JsonSerializerOptions Options { get; }
        protected JsonElement Element { get; }

        protected DynamicJsonElement(JsonElement element, JsonSerializerOptions options)
        {
            Element = element;
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public static dynamic From(JsonElement element, JsonSerializerOptions? options = null)
        {
            options ??= new JsonSerializerOptions();

            return element.ValueKind switch
            {
                JsonValueKind.Array => new DynamicJsonArray(element, options),
                JsonValueKind.Object => new DynamicJsonObject(element, options),
                _ => new DynamicJsonElement(element, options)
            };
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            _ = binder ?? throw new ArgumentNullException(nameof(binder));

            var isNullable = binder.Type.IsGenericType && binder.Type.GetGenericTypeDefinition() == typeof(Nullable<>);

            bool IsType<T>()
                where T : class
                => binder.Type == typeof(T);

            bool IsValueType<T>()
                where T : struct
                => binder.Type == typeof(T) || binder.Type == typeof(T?);

            bool IsValueKind(params JsonValueKind[] kinds) => kinds.Any(kind => Element.ValueKind == kind);

            if (IsValueKind(JsonValueKind.Null) && (binder.Type.IsClass || isNullable))
            {
                result = null!;
                return true;
            }

            if (IsValueKind(JsonValueKind.String) && IsType<string>())
            {
                result = Element.GetString();
                return true;
            }

            if (IsValueKind(JsonValueKind.True, JsonValueKind.False) && IsValueType<bool>())
            {
                result = Element.GetBoolean();
                return true;
            }

            if (IsValueType<JsonElement>())
            {
                result = Element;
                return true;
            }

            if (binder.Type.IsEnum)
            {
                if (IsValueKind(JsonValueKind.String))
                {
                    result = Enum.Parse(binder.Type, Element.GetString(), true);
                    return true;
                }

                if (IsValueKind(JsonValueKind.Number))
                {
                    bool IsEnumType<T>()
                        where T : struct
                        => binder.Type.GetEnumUnderlyingType() == typeof(T);

                    if (IsEnumType<byte>())
                    {
                        result = Enum.ToObject(binder.Type, Element.GetByte());
                        return true;
                    }

                    if (IsEnumType<short>())
                    {
                        result = Enum.ToObject(binder.Type, Element.GetInt16());
                        return true;
                    }

                    if (IsEnumType<int>())
                    {
                        result = Enum.ToObject(binder.Type, Element.GetInt32());
                        return true;
                    }

                    if (IsEnumType<long>())
                    {
                        result = Enum.ToObject(binder.Type, Element.GetInt64());
                        return true;
                    }

                    if (IsEnumType<sbyte>())
                    {
                        result = Enum.ToObject(binder.Type, Element.GetSByte());
                        return true;
                    }

                    if (IsEnumType<ushort>())
                    {
                        result = Enum.ToObject(binder.Type, Element.GetUInt16());
                        return true;
                    }

                    if (IsEnumType<uint>())
                    {
                        result = Enum.ToObject(binder.Type, Element.GetUInt32());
                        return true;
                    }

                    if (IsEnumType<ulong>())
                    {
                        result = Enum.ToObject(binder.Type, Element.GetUInt64());
                        return true;
                    }
                }
            }
            else if (IsValueKind(JsonValueKind.String))
            {
                if (IsValueType<DateTime>() && Element.TryGetDateTime(out var dateTime))
                {
                    result = dateTime;
                    return true;
                }

                if (IsValueType<DateTimeOffset>() && Element.TryGetDateTimeOffset(out var dateTimeOffset))
                {
                    result = dateTimeOffset;
                    return true;
                }

                if (IsValueType<TimeSpan>() && TimeSpan.TryParse(Element.GetString(), out var timeSpan))
                {
                    result = timeSpan;
                    return true;
                }

                if (IsValueType<Guid>() && Guid.TryParse(Element.GetString(), out var guid))
                {
                    result = guid;
                    return true;
                }

                if (IsType<Uri>() && Uri.TryCreate(Element.GetString(), UriKind.RelativeOrAbsolute, out var uri))
                {
                    result = uri;
                    return true;
                }
            }
            else if (IsValueKind(JsonValueKind.Number))
            {
                if (IsValueType<byte>())
                {
                    result = Element.GetByte();
                }
                else if (IsValueType<short>())
                {
                    result = Element.GetInt16();
                }
                else if (IsValueType<int>())
                {
                    result = Element.GetInt32();
                }
                else if (IsValueType<long>())
                {
                    result = Element.GetInt64();
                }
                else if (IsValueType<float>())
                {
                    result = Element.GetSingle();
                }
                else if (IsValueType<double>())
                {
                    result = Element.GetDouble();
                }
                else if (IsValueType<decimal>())
                {
                    result = Element.GetDecimal();
                }
                else if (IsValueType<sbyte>())
                {
                    result = Element.GetSByte();
                }
                else if (IsValueType<ushort>())
                {
                    result = Element.GetUInt16();
                }
                else if (IsValueType<uint>())
                {
                    result = Element.GetUInt32();
                }
                else if (IsValueType<ulong>())
                {
                    result = Element.GetUInt64();
                }
                else
                {
                    result = null!;
                }

                return !(result is null);
            }

            result = null!;
            return false;
        }

        protected object GetValue(JsonElement element)
            => From(element, Options);

        public override string ToString()
            => Element.GetRawText();
    }
}
