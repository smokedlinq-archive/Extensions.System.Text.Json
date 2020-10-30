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
                setup.Sut(_ => DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(json)));
            })
            .Act(sut =>
            {
                JsonObject result = sut;
                return result;
            })
            .Assert((_, result) => result.Should().BeEquivalentTo(input));

        [Theory]
        [AutoData]
        public void DictionaryIsSupported(IDictionary<string, string> input)
            => A3<dynamic>
            .Arrange(setup =>
            {
                var json = JsonSerializer.Serialize(input);
                setup.Sut(_ => DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(json)));
            })
            .Act(sut =>
            {
                IDictionary<string, string> result = sut;
                return result;
            })
            .Assert((_, result) => result.Should().BeEquivalentTo(input));

        public class JsonObject
        {
            public string Value { get; set; }
        }
    }
}
