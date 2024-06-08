using ServiceSphere.core.Entities.Agreements;
using System.ComponentModel.DataAnnotations;

namespace ServiceSphere.APIs.DTOs
{
    public class GetProjectByIdDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Budget { get; set; }

        public string? Duration { get; set; }

        public DateTime? Deadline { get; set; }

        public int CategoryId { get; set; }

        // Replace SelectedSubCategoryIds and SubCategories with ProjectSubCategories
        public ICollection<ProjectSubCategoryDto> ProjectSubCategories { get; set; } = new List<ProjectSubCategoryDto>();
        public ICollection<ProposalDto> Proposals { get; set; } = new HashSet<ProposalDto>();
    }
}
