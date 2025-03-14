using App.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.DTO.Orders
{
    public class OrderStateDto
    {
        public int OrderId { get; set; }
        public DateTime LastUpdated { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public RequestStatus? Status { get; set; }


        public bool HasReview { get; set; }
    }
}
