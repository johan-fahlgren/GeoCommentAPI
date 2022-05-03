﻿namespace GeoComment.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string? Title { get; set; }
        public string Message { get; set; }

        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public GeoUser User { get; set; }

    }
}
