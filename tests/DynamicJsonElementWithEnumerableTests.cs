using A3;
using AutoFixture.Xunit2;
using FluentAssertions;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Extensions.System.Text.Json.Tests
{
    public abstract class DynamicJsonElementWithEnumerableTests<T> : DynamicJsonElementBaseTests<T>
    {
        [Theory]
        [AutoData]
        public void IEnumerableOfTypeIsSupported(IEnumerable<T> input)
            => A3<dynamic>
            .Arrange(setup => setup.Sut(_ => DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(input, JsonSerializerOptions), JsonSerializerOptions))))
            .Act(sut =>
            {
                IEnumerable<T> result = sut;
                return result;
            })
            .Assert((_, result) => result.Should().BeEquivalentTo(input));

        [Theory]
        [AutoData]
        public void ICollectionOfTypeIsSupported(ICollection<T> input)
            => A3<dynamic>
            .Arrange(setup => setup.Sut(_ => DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(input, JsonSerializerOptions), JsonSerializerOptions))))
            .Act(sut =>
            {
                IEnumerable<T> result = sut;
                return result;
            })
            .Assert((_, result) => result.Should().BeEquivalentTo(input));

        [Theory]
        [AutoData]
        public void IListOfTypeIsSupported(IList<T> input)
            => A3<dynamic>
            .Arrange(setup => setup.Sut(_ => DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(input, JsonSerializerOptions), JsonSerializerOptions))))
            .Act(sut =>
            {
                IEnumerable<T> result = sut;
                return result;
            })
            .Assert((_, result) => result.Should().BeEquivalentTo(input));
    }
}
