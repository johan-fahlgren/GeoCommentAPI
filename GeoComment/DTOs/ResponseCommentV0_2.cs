using GeoComment.Models;

namespace GeoComment.DTOs
{
    public class ResponseCommentV0_2
    {
        /// <example>1</example>>
        public int Id { get; set; }
        /// <example>5</example>>
        public decimal Longitude { get; set; }
        /// <example>5</example>>
        public decimal Latitude { get; set; }
        public ResponseBody Body { get; set; }

        public static ResponseCommentV0_2 CreateReturn(Comment comment)
        {
            var newComment = new ResponseCommentV0_2()
            {
                Id = comment.Id,
                Latitude = comment.Latitude,
                Longitude = comment.Longitude,
                Body = new ResponseBody()
                {
                    Author = comment.Author,
                    Title = comment.Title,
                    Message = comment.Message,
                }
            };

            return newComment;
        }

        public class ResponseBody
        {
            /// <example>Kalle</example>>
            public string? Author { get; set; }
            /// <example>Lorem</example>>
            public string Title { get; set; }
            /// <example>Lorem ipsum dolor sit amet</example>>
            public string Message { get; set; }

        }
    }


}

