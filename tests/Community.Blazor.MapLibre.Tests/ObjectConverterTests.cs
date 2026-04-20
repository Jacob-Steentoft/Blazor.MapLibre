using System.Text.Json;
using AwesomeAssertions;
using Community.Blazor.MapLibre.Converter;
using Xunit;

namespace Community.Blazor.MapLibre.Tests;

public class ObjectConverterTests
{
    [Fact]
    public void Read_ShouldDeserializePrimitivesAndJsonElements()
    {
        // Arrange
        const string json = """
                            {
                                "string": "hello",
                                "int": 42,
                                "long": 4294967296,
                                "double": 3.14,
                                "bool": true,
                                "null": null,
                                "guid": "d0473859-197e-4623-96b6-391d4e028b1e",
                                "object": { "key": "value" },
                                "array": [1, 2, 3]
                            }
                            """;
        var options = new JsonSerializerOptions();
        options.Converters.Add(new ObjectConverter());

        // Act
        var result = JsonSerializer.Deserialize<Dictionary<string, object?>>(json, options);

        // Assert
        result.Should().NotBeNull();
        result["string"].Should().Be("hello");
        result["int"].Should().BeOfType<int>().Which.Should().Be(42);
        result["long"].Should().BeOfType<long>().Which.Should().Be(4294967296L);
        result["double"].Should().BeOfType<double>().Which.Should().BeApproximately(3.14, 2);
        result["bool"].Should().BeOfType<bool>().Which.Should().Be(true);
        result["null"].Should().BeNull();
        result["guid"].Should().BeOfType<Guid>().Which.Should().Be(new Guid("d0473859-197e-4623-96b6-391d4e028b1e"));
        
        result["object"].Should().BeOfType<JsonElement>();
        result["array"].Should().BeOfType<JsonElement>();
    }

    [Fact]
    public void Write_ShouldSerializeCorrectly()
    {
        // Arrange
        var dictionary = new Dictionary<string, object?>
        {
            { "string", "hello" },
            { "int", 42 },
            { "guid", new Guid("d0473859-197e-4623-96b6-391d4e028b1e") },
            { "complex", new { inner = "value" } }
        };
        var options = new JsonSerializerOptions();
        options.Converters.Add(new ObjectConverter());

        // Act
        var json = JsonSerializer.Serialize(dictionary, options);

        // Assert
        json.Should().Contain("\"string\":\"hello\"");
        json.Should().Contain("\"int\":42");
        json.Should().Contain("\"guid\":\"d0473859-197e-4623-96b6-391d4e028b1e\"");
        json.Should().Contain("\"complex\":{\"inner\":\"value\"}");
    }
}
