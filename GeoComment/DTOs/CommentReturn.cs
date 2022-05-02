namespace GeoComment.DTOs
{
    public class CommentReturn
    {
        public Body Body { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }

    public class Body
    {
        public string Author { get; set; }
        public string Titel { get; set; }
        public string Message { get; set; }

    }
}
