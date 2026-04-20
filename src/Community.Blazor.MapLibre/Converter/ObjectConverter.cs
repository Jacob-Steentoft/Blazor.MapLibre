using System.Text.Json;
using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Converter;

public class ObjectConverter : JsonConverter<Dictionary<string, object?>>
{
	public override Dictionary<string, object?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.StartObject)
		{
			throw new JsonException();
		}

		var dictionary = new Dictionary<string, object?>();

		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndObject)
			{
				return dictionary;
			}

			if (reader.TokenType != JsonTokenType.PropertyName)
			{
				throw new JsonException();
			}

			var propertyName = reader.GetString()!;
			reader.Read();
			dictionary.Add(propertyName, ExtractValue(ref reader));
		}

		return dictionary;
	}

	private static object? ExtractValue(ref Utf8JsonReader reader) => reader.TokenType switch
	{
		JsonTokenType.String => ExtractString(ref reader),
		JsonTokenType.Number => ExtractNumber(ref reader),
		JsonTokenType.True => true,
		JsonTokenType.False => false,
		JsonTokenType.Null => null,
		JsonTokenType.StartObject or JsonTokenType.StartArray => JsonElement.ParseValue(ref reader),
		_ => throw new JsonException(),
	};

	private static object? ExtractString(ref Utf8JsonReader reader) => reader.TryGetGuid(out var guid) ? guid : reader.GetString();

	private static object ExtractNumber(ref Utf8JsonReader reader)
	{
		if (reader.TryGetInt32(out var i))
		{
			return i;
		}

		if (reader.TryGetInt64(out var l))
		{
			return l;
		}

		return reader.GetDouble();
	}

	public override void Write(Utf8JsonWriter writer, Dictionary<string, object?> value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();

		foreach (var pair in value)
		{
			writer.WritePropertyName(pair.Key);
			JsonSerializer.Serialize(writer, pair.Value, options);
		}

		writer.WriteEndObject();
	}
}