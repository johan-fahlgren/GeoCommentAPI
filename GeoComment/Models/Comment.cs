namespace GeoComment.Models
{
    public class Comment
    {
        /// <example>1</example>>
        public int Id { get; set; }
        /// <example>Kalle</example>>
        public string Author { get; set; }
        /// <example>Lorem</example>>
        public string? Title { get; set; }
        /// <example>Lorem ipsum dolor sit amet</example>>
        public string Message { get; set; }

        /// <example>5</example>>
        public decimal Longitude { get; set; }
        /// <example>5</example>>
        public decimal Latitude { get; set; }

        public GeoUser User { get; set; }

    }
}
