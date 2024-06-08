using System.ComponentModel.DataAnnotations;

namespace ServiceSphere.APIs.DTOs
{
    public class ProjectSubCategoryDto
    {
        [Required]
        public int SubCategoryId { get; set; }

        [Required]
        public int TeamMembersRequired { get; set; }
    }

}
