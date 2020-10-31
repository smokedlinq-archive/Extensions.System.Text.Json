using System;

namespace Extensions.System.Text.Json.Tests
{
    public class DynamicJsonElementStringTests : DynamicJsonElementWithEnumerableTests<string> { }
    public class DynamicJsonElementBooleanTests : DynamicJsonElementValueTypeTests<bool> { }
    public class DynamicJsonElementByteTests : DynamicJsonElementValueTypeTests<byte> { }
    public class DynamicJsonElementSByteTests : DynamicJsonElementValueTypeTests<sbyte> { }
    public class DynamicJsonElementInt16Tests : DynamicJsonElementValueTypeTests<short> { }
    public class DynamicJsonElementUInt16Tests : DynamicJsonElementValueTypeTests<ushort> { }
    public class DynamicJsonElementInt32Tests : DynamicJsonElementValueTypeTests<int> { }
    public class DynamicJsonElementUInt32Tests : DynamicJsonElementValueTypeTests<uint> { }
    public class DynamicJsonElementInt64Tests : DynamicJsonElementValueTypeTests<long> { }
    public class DynamicJsonElementUInt64Tests : DynamicJsonElementValueTypeTests<ulong> { }
    public class DynamicJsonElementSingleTests : DynamicJsonElementValueTypeTests<float> { }
    public class DynamicJsonElementDoubleTests : DynamicJsonElementValueTypeTests<double> { }
    public class DynamicJsonElementDecimalTests : DynamicJsonElementValueTypeTests<decimal> { }
    public class DynamicJsonElementDateTimeTests : DynamicJsonElementValueTypeTests<DateTime> { }
    public class DynamicJsonElementDateTimeOffsetTests : DynamicJsonElementValueTypeTests<DateTimeOffset> { }
    public class DynamicJsonElementTimeSpanTests : DynamicJsonElementValueTypeTests<TimeSpan> { }
    public class DynamicJsonElementGuidTests : DynamicJsonElementValueTypeTests<Guid> { }
    public class DynamicJsonElementUriTests : DynamicJsonElementBaseTests<Uri> { }
    public class DynamicJsonElementEnumAsByteTests : DynamicJsonElementBaseTests<EnumAsByte> { }
    public class DynamicJsonElementEnumAsSByteTests : DynamicJsonElementBaseTests<EnumAsSByte> { }
    public class DynamicJsonElementEnumAsInt16Tests : DynamicJsonElementBaseTests<EnumAsInt16> { }
    public class DynamicJsonElementEnumAsUInt16Tests : DynamicJsonElementBaseTests<EnumAsUInt16> { }
    public class DynamicJsonElementEnumAsInt32Tests : DynamicJsonElementBaseTests<EnumAsInt32> { }
    public class DynamicJsonElementEnumAsUInt32Tests : DynamicJsonElementBaseTests<EnumAsUInt32> { }
    public class DynamicJsonElementEnumAsInt64Tests : DynamicJsonElementBaseTests<EnumAsInt64> { }
    public class DynamicJsonElementEnumAsUInt64Tests : DynamicJsonElementBaseTests<EnumAsUInt64> { }
}
