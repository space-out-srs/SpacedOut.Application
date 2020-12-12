using SpacedOut.Api.Helpers;
using System.Text.Json.Serialization;

namespace SpacedOut.Api.UnitTests.SmartEnumValueConverter
{
    public class TestClass
    {
        [JsonConverter(typeof(SmartEnumValueConverter<TestEnumBoolean, bool>))]
        public TestEnumBoolean Bool { get; set; } = null!;

        [JsonConverter(typeof(SmartEnumValueConverter<TestEnumInt16, short>))]
        public TestEnumInt16 Int16 { get; set; } = null!;

        [JsonConverter(typeof(SmartEnumValueConverter<TestEnumInt32, int>))]
        public TestEnumInt32 Int32 { get; set; } = null!;

        [JsonConverter(typeof(SmartEnumValueConverter<TestEnumDouble, double>))]
        public TestEnumDouble Double { get; set; } = null!;

        [JsonConverter(typeof(SmartEnumValueConverter<TestEnumString, string>))]
        public TestEnumString String { get; set; } = null!;

        internal static TestClass TestInstance => new TestClass
        {
            Bool = TestEnumBoolean.Instance,
            Int16 = TestEnumInt16.Instance,
            Int32 = TestEnumInt32.Instance,
            Double = TestEnumDouble.Instance,
            String = TestEnumString.Instance,
        };

        internal static string TestInstanceJson =>
@"{
  ""Bool"": true,
  ""Int16"": 1,
  ""Int32"": 1,
  ""Double"": 1.2,
  ""String"": ""1.5""
}";
    }
}