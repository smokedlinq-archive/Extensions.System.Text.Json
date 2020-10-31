using A3;
using AutoFixture.Xunit2;
using FluentAssertions;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Extensions.System.Text.Json.Tests
{
    public class DynamicJsonObjectTests
    {
        [Theory]
        [AutoData]
        public void JsonCanBeDeserializedToDynamicType(JsonObject input)
            => A3<dynamic>
            .Arrange(setup =>
            {
                var json = JsonSerializer.Serialize(input);
                setup.Sut(DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(json)));
            })
            .Act(sut =>
            {
                JsonObject result = sut;
                return result;
            })
            .Assert(result => result.Should().BeEquivalentTo(input));

        [Theory]
        [AutoData]
        public void DictionaryIsSupported(IDictionary<string, string> input)
            => A3<dynamic>
            .Arrange(setup =>
            {
                var json = JsonSerializer.Serialize(input);
                setup.Sut(DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(json)));
            })
            .Act(sut =>
            {
                IDictionary<string, string> result = sut;
                return result;
            })
            .Assert(result => result.Should().BeEquivalentTo(input));

        [Theory]
        [AutoData]
        public void CanGetMemberNames(JsonObject input)
            => A3<DynamicJsonObject>
            .Arrange(setup =>
            {
                var json = JsonSerializer.Serialize(input);
                setup.Sut((DynamicJsonObject)DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(json)));
            })
            .Act(sut => sut.GetDynamicMemberNames())
            .Assert(result => result.Should().Contain(nameof(JsonObject.Value)));

        [Theory]
        [AutoData]
        public void CanAccessPropertyByIndexer(JsonObject input)
            => A3<dynamic>
            .Arrange(setup =>
            {
                var json = JsonSerializer.Serialize(input);
                setup.Sut(DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(json)));
            })
            .Act(sut => (string?)sut[nameof(JsonObject.Value)])
            .Assert(result => result.Should().Be(input.Value));

        public class JsonObject
        {
            public string? Value { get; set; }
        }
    }
}
