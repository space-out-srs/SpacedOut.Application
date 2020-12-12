/*
 * Lifted from:
 * https://github.com/dochoffiday/SmartEnum/blob/718b30811b16ffb3f1c6cffe0d6c3d39cb90b6b3/src/SmartEnum.JsonNet/SmartEnumValueConverter.cs
 */
namespace SpacedOut.Api.Helpers
{
    using Ardalis.SmartEnum;
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class SmartEnumValueConverter<TEnum, TValue> : JsonConverter<TEnum>
        where TEnum : SmartEnum<TEnum, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>, IConvertible
    {
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                var jsonValue = reader.GetString();
                var value = (TValue)Convert.ChangeType(jsonValue, typeof(TValue));

                if (value == null)
                {
                    throw new InvalidOperationException("value cannot be null");
                }

                return SmartEnum<TEnum, TValue>.FromValue(value);
            }
            catch (Exception ex)
            {
                throw new JsonException($"Error converting {reader.GetString() ?? "Null"} to {typeToConvert.Name}.", ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.Value.ToString());
            }
        }
    }
}