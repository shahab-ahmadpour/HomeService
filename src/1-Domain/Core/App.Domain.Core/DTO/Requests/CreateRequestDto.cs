using App.Domain.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.DTO.Requests
{
    public class CreateRequestDto
    {
        [Required(ErrorMessage = "شناسه مشتری الزامی است.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "شناسه زیرسرویس الزامی است.")]
        public int SubHomeServiceId { get; set; }

        [Required(ErrorMessage = "توضیحات درخواست الزامی است.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "وضعیت درخواست الزامی است.")]
        public RequestStatus Status { get; set; }

        public string SubHomeServiceName { get; set; }

        [Required(ErrorMessage = "تاریخ پایان الزامی است.")]
        public DateTime Deadline { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "تاریخ اجرا الزامی است.")]
        public DateTime ExecutionDate { get; set; } = DateTime.Today;

        public IFormFile? EnvironmentImage { get; set; }
        public string? EnvironmentImagePath { get; set; }
    }
}
