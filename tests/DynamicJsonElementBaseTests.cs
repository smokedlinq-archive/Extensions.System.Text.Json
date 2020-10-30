using A3;
using AutoFixture.Xunit2;
using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace Extensions.System.Text.Json.Tests
{
    public abstract class DynamicJsonElementBaseTests<T>
    {
        protected JsonSerializerOptions JsonSerializerOptions { get; } = CreateJsonSerializerOptions();

        private static JsonSerializerOptions CreateJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonTimeSpanConverter());
            return options;
        }

        [Theory]
        [AutoData]
        public void TypeIsSupported(T input)
            => A3<dynamic>
            .Arrange(setup => setup.Sut(_ => DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(input, JsonSerializerOptions), JsonSerializerOptions))))
            .Act(sut => (T)sut)
            .Assert((_, result) => result.Should().BeEquivalentTo(input));
    }
}
