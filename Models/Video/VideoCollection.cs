using System.Text.Json.Serialization;

namespace StreamableNet.Models.Video
{
    public class VideoCollection
    {
        [JsonPropertyName("total")]
        public uint Total { get; set; }

        [JsonPropertyName("videos")]
        public List<Video> Videos { get; set; }
    }
}
