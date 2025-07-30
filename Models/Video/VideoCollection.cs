using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
