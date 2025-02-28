using App.Domain.Core.DTO.Proposals;
using App.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.DTO.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public int ExpertId { get; set; }
        public string ExpertName { get; set; } = string.Empty;
        public int RequestId { get; set; }
        public string RequestDescription { get; set; } = string.Empty;
        public string SubHomeServiceName { get; set; }
        public int ProposalId { get; set; }
        public List<ProposalDto> Proposals { get; set; } = new List<ProposalDto>();
        public decimal FinalPrice { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }

}
