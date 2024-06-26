﻿using ServiceSphere.core.Entities.Posting;

namespace ServiceSphere.APIs.DTOs
{
    public class ServicePostingDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        //public string Status { get; set; }
        public decimal? Budget { get; set; }

        public string? Duration { get; set; }

        public DateTime? Deadline { get; set; }
        public string? UserId { get; set; }
        public string? ClientId { get; set; }


    }

}
