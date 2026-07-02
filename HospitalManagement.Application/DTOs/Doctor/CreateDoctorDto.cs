using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Doctor
{
    public class CreateDoctorDto
    {
        [Required]
        [StringLength(20)]
        public string DoctorCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Specialization { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Range(0, 99999)]
        public decimal ConsultationFee { get; set; }
    }
}
