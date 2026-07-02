using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Application.DTOs.Doctor
{
    public class DoctorResponseDto
    {
        public int DoctorId { get; set; }
        public string DoctorCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public decimal ConsultationFee { get; set; }
        public bool IsAvailable { get; set; }
    }
}
