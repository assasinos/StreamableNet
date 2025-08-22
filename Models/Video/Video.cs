using System.Text.Json.Serialization;

namespace StreamableNet.Models.Video
{
    public class Video
    {
        // Essential Identifiers
        [JsonPropertyName("file_id")]
        public string FileId { get; set; }

        [JsonPropertyName("shortcode")]
        public string Shortcode { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        // Basic Media Properties
        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        // Status & Processing
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("percent")]
        public int Percent { get; set; }

        // Visual Assets
        [JsonPropertyName("poster_url")]
        public string PosterUrl { get; set; }

        [JsonPropertyName("thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        // Basic Stats
        [JsonPropertyName("plays")]
        public int Plays { get; set; }

        [JsonPropertyName("date_added")]
        public long DateAdded { get; set; }

        // Files
        [JsonPropertyName("files")]
        public Files Files { get; set; }

        // Privacy (simplified)
        [JsonPropertyName("privacy")]
        public int Privacy { get; set; }
    }

    public class Files
    {
        [JsonPropertyName("mp4-mobile")]
        public VideoFile Mp4Mobile { get; set; }

        [JsonPropertyName("mp4")]
        public VideoFile Mp4 { get; set; }
    }

    public class VideoFile
    {
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("bitrate")]
        public long Bitrate { get; set; }

        [JsonPropertyName("framerate")]
        public int Framerate { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("poster_url")]
        public string PosterUrl { get; set; }

        [JsonPropertyName("thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("percent")]
        public int Percent { get; set; }
    }

}
