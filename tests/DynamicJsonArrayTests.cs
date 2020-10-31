using A3;
using AutoFixture.Xunit2;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Extensions.System.Text.Json.Tests
{
    public class DynamicJsonArrayTests
    {
        [Theory]
        [AutoData]
        public void Int32ArrayIsSupported(int[] input)
            => A3<dynamic>
            .Arrange(setup => setup.Sut(DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(input)))))
            .Act(sut => (int[])sut)
            .Assert(result => result.Should().BeEquivalentTo(input));

        [Theory]
        [AutoData]
        public void IEnumerableIsSupported(int[] input)
            => A3<dynamic>
            .Arrange(setup => setup.Sut(DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(input)))))
            .Act(sut =>
            {
                IEnumerable<int> result = sut;
                return result;
            })
            .Assert(result => result.Should().BeEquivalentTo(input));

        [Theory]
        [AutoData]
        public void IEnumerableOfJsonElementIsSupported(int[] input)
            => A3<dynamic>
            .Arrange(setup => setup.Sut(DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(input)))))
            .Act(sut =>
            {
                IEnumerable<JsonElement> result = sut;
                return result;
            })
            .Assert(result => result.Count().Should().Be(input.Length));

        [Theory]
        [AutoData]
        public void DynamicArrayIsSupported(int[] input)
            => A3<dynamic>
            .Arrange(setup => setup.Sut(DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(input)))))
            .Act(sut =>
            {
                dynamic[] result = sut;
                return result;
            })
            .Assert(result => result.Length.Should().Be(input.Length));

        [Theory]
        [AutoData]
        public void CanReadLengthProperty(int[] input)
            => A3<dynamic>
            .Arrange(setup => setup.Sut(DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(input)))))
            .Act(sut => (int)sut.Length)
            .Assert(result => result.Should().Be(input.Length));

        [Theory]
        [AutoData]
        public void CanReadCountProperty(int[] input)
            => A3<dynamic>
            .Arrange(setup => setup.Sut(DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(input)))))
            .Act(sut => (int)sut.Count)
            .Assert(result => result.Should().Be(input.Length));

        [Theory]
        [AutoData]
        public void CanInvokeCountMethod(int[] input)
            => A3<dynamic>
            .Arrange(setup => setup.Sut(DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(input)))))
            .Act(sut => (int)sut.Count())
            .Assert(result => result.Should().Be(input.Length));
    }
}
