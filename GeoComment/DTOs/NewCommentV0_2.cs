namespace GeoComment.DTOs
{
    public class NewCommentV0_2
    {
        /// <example>1</example>>
        public int Id { get; set; }
        /// <example>5</example>>
        public decimal Longitude { get; set; }
        /// <example>5</example>>
        public decimal Latitude { get; set; }

        public Body Body { get; set; }
    }

    public class Body
    {
        /// <example>Kalle</example>>
        public string? Author { get; set; }
        /// <example>Lorem</example>>
        public string Title { get; set; }
        /// <example>Lorem ipsum dolor sit amet</example>>
        public string Message { get; set; }

    }
}
