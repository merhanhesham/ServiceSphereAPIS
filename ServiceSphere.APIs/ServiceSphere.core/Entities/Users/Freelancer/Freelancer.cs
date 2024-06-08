using ServiceSphere.core.Entities.Agreements;
using ServiceSphere.core.Entities.Assessments;
using ServiceSphere.core.Entities.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Entities.Users.Freelancer
{
    public enum ExperienceLevel
    {
        [EnumMember(Value = "Beginner")]
        Beginner,
        [EnumMember(Value = "Intermediate")]
        Intermediate,
        [EnumMember(Value = "Advanced")]
        Advanced
    }
    public class Freelancer:User
    {
       // public string DisplayName { get; set; }
        public string UserName { get; set; }

        public string Title { get; set; }

        public string WorkExperience { get; set; }
        public string? Bio { get; set; }
        public ExperienceLevel? experienceLevel { get; set; }
        public string? Education { get; set; }
        public string? Overview { get; set; }

        public string? WorkStyle { get; set; }
        
        public List<Skill>? Skills { get; set; }
       

        //nav prop for proposal
        public ICollection<Proposal> Proposals { get; set; }=new HashSet<Proposal>();

        //nav prop for category
        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public ICollection<Service>? Services { get; set; } = new HashSet<Service>();
        //nav prop for notification 
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        //nav prop for review
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

        public ICollection<SubCategory> SubCategories { get; set; } = new HashSet<SubCategory>();
    }
}
