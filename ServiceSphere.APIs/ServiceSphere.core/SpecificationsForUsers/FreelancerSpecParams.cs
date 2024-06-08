using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.SpecificationsForUsers
{
    public class FreelancerSpecParams
    {
        //search
        private string? Search;

        public string? search
        {
            get { return Search; }
            set { Search = value.ToLower(); }
        }
        public string? sort { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        
    }
}
