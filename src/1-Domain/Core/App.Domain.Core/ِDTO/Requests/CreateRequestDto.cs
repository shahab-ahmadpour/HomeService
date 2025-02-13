using App.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core._ِDTO.Requests
{
    public class CreateRequestDto
    {
        public int CustomerId { get; set; }
        public int SubHomeServiceId { get; set; }
        public string Description { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string EnvironmentImage { get; set; }
    }

}
