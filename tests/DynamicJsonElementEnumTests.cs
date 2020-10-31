using A3;
using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Text.Json;
using Xunit;

namespace Extensions.System.Text.Json.Tests
{
    public abstract class DynamicJsonElementEnumTests<T> : DynamicJsonElementBaseTests<T>
        where T : Enum
    {
        [Theory]
        [AutoData]
        public void ValueAsNumericIsSupported(T input)
            => A3<dynamic>
            .Arrange(setup => setup.Sut(DynamicJsonElement.From(JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(Convert.ChangeType(input, typeof(ulong)))))))
            .Act(sut => (T)sut)
            .Assert(result => result.Should().Be(input));
    }

    public enum EnumAsByte : byte { Value = byte.MaxValue }
    public enum EnumAsSByte : sbyte { Value = sbyte.MaxValue }
    public enum EnumAsInt16 : short { Value = short.MaxValue }
    public enum EnumAsUInt16 : ushort { Value = ushort.MaxValue }
    public enum EnumAsInt32 { Value = int.MaxValue }
    public enum EnumAsUInt32 : uint { Value = uint.MaxValue }
    public enum EnumAsInt64 : long { Value = long.MaxValue }
    public enum EnumAsUInt64 : ulong { Value = ulong.MaxValue }
}
