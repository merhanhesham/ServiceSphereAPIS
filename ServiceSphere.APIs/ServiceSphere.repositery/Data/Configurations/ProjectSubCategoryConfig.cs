using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using ServiceSphere.core.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.repositery.Data.Configurations
{
    public class ProjectSubCategoryConfig : IEntityTypeConfiguration<ProjectSubCategory>
    {
        public void Configure(EntityTypeBuilder<ProjectSubCategory> builder)
        {
            builder.HasKey(psc => new { psc.ProjectPostingId, psc.SubCategoryId });

            builder
                .HasOne(psc => psc.ProjectPosting)
                .WithMany(pp => pp.ProjectSubCategories)
                .HasForeignKey(psc => psc.ProjectPostingId);

            builder
                .HasOne(psc => psc.SubCategory)
                .WithMany() // Assuming SubCategory does not directly reference ProjectSubCategory
                .HasForeignKey(psc => psc.SubCategoryId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
