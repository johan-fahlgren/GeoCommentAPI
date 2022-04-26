namespace GeoComment.Models
{
    public class Comment
    {
        public string Message { get; set; }
        public string Author { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

    }
}
