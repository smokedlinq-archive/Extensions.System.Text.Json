using A3;
using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace Extensions.System.Text.Json.Tests
{
    public abstract class DynamicJsonElementValueTypeTests<T> : DynamicJsonElementBaseTests<T>
        where T : struct
    {
        [Fact]
        public void NullIsSupported()
            => A3<dynamic>
            .Arrange(setup => setup.Sut(_ => DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize<T?>(null, JsonSerializerOptions), JsonSerializerOptions))))
            .Act(sut => (T?)sut)
            .Assert((_, result) => result.Should().BeNull());
    }
}
