using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace System.Text.Json
{
    // Adds support for adding to ICollection<T> properties that are read-only
    // TODO: Remove this if this feature is ever natively added to System.Text.Json
    // https://github.com/dotnet/runtime/issues/30258
    public sealed class JsonReadOnlyCollectionConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => !(typeToConvert is null)
            && !typeToConvert.IsAbstract
            && !(typeToConvert.GetConstructor(Type.EmptyTypes) is null)
            && typeToConvert
                .GetProperties()
                .Any(x => !x.CanWrite && !(GetReadOnlyCollectionInterface(x.PropertyType) is null));

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            => (JsonConverter)Activator.CreateInstance(typeof(ReadOnlyCollectionConverter<>).MakeGenericType(typeToConvert))!;

        internal static Type? GetReadOnlyCollectionInterface(Type type)
            => type.IsGenericType && IsCollectionType(type.GetGenericTypeDefinition())
                ? type
                : type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && IsCollectionType(x.GetGenericTypeDefinition()));

        internal static bool IsCollectionType(Type type)
            => type == typeof(ICollection<>);

#pragma warning disable CA1812 // Dynamically activated type per generic type
        private class ReadOnlyCollectionConverter<T> : JsonConverter<T>
#pragma warning restore CA1812
            where T : new()
        {
            private readonly IDictionary<string, (Type PropertyType, Type? CollectionElementType, Action<T, object> Handler)> _properties;

            public ReadOnlyCollectionConverter()
            {
                _properties = typeof(T)
                    .GetProperties()
                    .Select(x => new
                    {
                        Property = x,
                        CollectionInterface = x.CanWrite ? null : GetReadOnlyCollectionInterface(x.PropertyType)
                    })
                    .Where(x => x.Property.CanWrite || !(x.CollectionInterface is null))
                    .Select(x =>
                    {
                        var typeParameter = Expression.Parameter(typeof(T));
                        var valueParameter = Expression.Parameter(typeof(object));
                        Action<T, object> handler;

                        if (x.Property.CanWrite)
                        {
                            var assign = Expression.Assign(
                                Expression.Property(typeParameter, x.Property),
                                Expression.Convert(valueParameter, x.Property.PropertyType));

                            handler = Expression.Lambda<Action<T, object>>(assign, typeParameter, valueParameter).Compile();
                        }
                        else
                        {
                            var add = Expression.Call(
                                Expression.Property(typeParameter, x.Property),
                                x.CollectionInterface!.GetMethod(nameof(ICollection<T>.Add)),
                                Expression.Convert(valueParameter, x.CollectionInterface.GetGenericArguments().First()));

                            handler = Expression.Lambda<Action<T, object>>(add, typeParameter, valueParameter).Compile();
                        }

                        return new
                        {
                            PropertyName = x.Property.Name,
                            x.Property.PropertyType,
                            CollectionElementType = x.CollectionInterface?.GetGenericArguments()?.First(),
                            Handler = handler
                        };
                    })
                    .ToDictionary(x => x.PropertyName, x => (x.PropertyType, x.CollectionElementType, x.Handler));
            }

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var item = new T();
                var properties = options.PropertyNameCaseInsensitive
                    ? new Dictionary<string, (Type PropertyType, Type? CollectionElementType, Action<T, object> Handler)>(_properties, StringComparer.InvariantCultureIgnoreCase)
                    : _properties;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        break;
                    }
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        if (properties.TryGetValue(reader.GetString(), out var info))
                        {
                            if (!reader.Read())
                            {
                                throw new JsonException("Missing property value.");
                            }

                            if (info.CollectionElementType is null)
                            {
                                info.Handler(item, JsonSerializer.Deserialize(ref reader, info.PropertyType, options));
                            }
                            else
                            {
                                if (reader.TokenType == JsonTokenType.StartArray)
                                {
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            throw new JsonException($"Missing end of array.");
                                        }

                                        if (reader.TokenType == JsonTokenType.EndArray)
                                        {
                                            break;
                                        }

                                        info.Handler(item, JsonSerializer.Deserialize(ref reader, info.CollectionElementType, options));
                                    }
                                }
                                else
                                {
                                    reader.Skip();
                                }
                            }
                        }
                        else
                        {
                            reader.Skip();
                        }
                    }
                }
                return item;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
                => throw new NotSupportedException();
        }
    }
}
