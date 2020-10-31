using A3;
using AutoFixture.Xunit2;
using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace Extensions.System.Text.Json.Tests
{
    public class DynamicJsonObjectConverterTests
    {
        [Theory]
        [AutoData]
        public void CanConvertJsonToDynamic(string input)
            => A3<dynamic>
            .Arrange(setup =>
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new DynamicJsonConverter());
                var json = JsonSerializer.Serialize(new { Input = input }, options);
                setup.Sut(JsonSerializer.Deserialize<dynamic>(json, options));
            })
            .Act(sut => (string)sut.Input)
            .Assert(result => result.Should().Be(input));
    }
}
