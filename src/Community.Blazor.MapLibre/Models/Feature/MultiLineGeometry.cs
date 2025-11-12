using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

public class MultiLineGeometry : IGeometry
{
    [JsonPropertyName("type")]
    [JsonIgnore]
    public GeometryType Type => GeometryType.MultiLine;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public LngLatBounds GetBounds()
    {
        throw new NotImplementedException();
    }
}
