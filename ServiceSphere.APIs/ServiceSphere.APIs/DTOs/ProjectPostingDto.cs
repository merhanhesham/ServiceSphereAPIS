using ServiceSphere.APIs.DTOs;
using System.ComponentModel.DataAnnotations;

public class ProjectPostingDto
{
    public int Id { get; set; }
    public string Title { get; set; }

    public string Description { get; set; }
    public decimal? Budget { get; set; }

    public string? Duration { get; set; }

    public DateTime? Deadline { get; set; }

    public int CategoryId { get; set; }

    public string? ClientId { get; set; }

    public string? UserId { get; set; }

    // Replace SelectedSubCategoryIds and SubCategories with ProjectSubCategories
    public ICollection<ProjectSubCategoryDto> ProjectSubCategories { get; set; } = new List<ProjectSubCategoryDto>();
}
