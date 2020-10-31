using A3;
using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Text.Json;
using Xunit;

namespace Extensions.System.Text.Json.Tests
{
    public class JsonTimeSpanConverterTests
    {
        [Theory]
        [AutoData]
        public void CanSerializeAndDeserializeTimeSpan(TimeSpan input)
            => A3<JsonSerializerOptions>
            .Arrange(setup =>
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new JsonTimeSpanConverter());

                var json = JsonSerializer.Serialize(input, options);

                setup.Sut(_ => options);
                setup.Parameter(json);
            })
            .Act((JsonSerializerOptions sut, string json) => JsonSerializer.Deserialize<TimeSpan>(json, sut))
            .Assert((_, result) => result.Should().Be(input));

        [Theory]
        [AutoData]
        public void WhenNullableTimeSpanIsNullThenCanSerializeAndDeserialize(JsonObject input)
            => A3<JsonSerializerOptions>
            .Arrange(setup =>
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new JsonTimeSpanConverter());

                input.Timestamp = null;

                var json = JsonSerializer.Serialize(input, options);

                setup.Sut(_ => options);
                setup.Parameter(json);
            })
            .Act((JsonSerializerOptions sut, string json) => JsonSerializer.Deserialize<JsonObject>(json, sut))
            .Assert((_, result) => result.Timestamp.Should().BeNull());

        [Theory]
        [AutoData]
        public void WhenNullableTimeSpanIsNotNullThenCanSerializeAndDeserialize(JsonObject input)
            => A3<JsonSerializerOptions>
            .Arrange(setup =>
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new JsonTimeSpanConverter());

                var json = JsonSerializer.Serialize(input, options);

                setup.Sut(_ => options);
                setup.Parameter(json);
            })
            .Act((JsonSerializerOptions sut, string json) => JsonSerializer.Deserialize<JsonObject>(json, sut))
            .Assert((_, result) => result.Timestamp.Should().NotBeNull());

        public class JsonObject
        {
            public TimeSpan? Timestamp { get; set; }
        }
    }
}
