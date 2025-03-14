using App.Domain.Core.DTO.Reviews;
using App.Domain.Core.DTO.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.DTO.Users.Experts
{
    public class ExpertProfileViewModel
    {
        public ExpertDto Expert { get; set; }
        public List<SkillDto> Skills { get; set; } = new List<SkillDto>();
        public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();

        public double AverageRating
        {
            get
            {
                if (Reviews == null || !Reviews.Any())
                    return 0;

                return Math.Round(Reviews.Average(r => r.Rating), 1);
            }
        }
    }
}
