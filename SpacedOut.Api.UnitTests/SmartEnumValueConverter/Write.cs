using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace SpacedOut.Api.UnitTests.SmartEnumValueConverter
{
    public class Write
    {
        [Fact]
        public void SerializesValue()
        {
            var json = JsonSerializer.Serialize(
                TestClass.TestInstance,
                new JsonSerializerOptions { WriteIndented = true }
            );

            json.Should().Be(TestClass.TestInstanceJson);
        }
    }
}