using System.Text.Json.Serialization;

namespace Community.Blazor.MapLibre.Models.Feature;

public class MultiPointGeometry : IGeometry
{
    [JsonPropertyName("type")]
    [JsonIgnore]
    public GeometryType Type => GeometryType.MultiPoint;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public LngLatBounds GetBounds()
    {
        throw new NotImplementedException();
    }
}
