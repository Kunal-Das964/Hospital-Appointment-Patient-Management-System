using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Application.DTOs.Appointment
{
    public class DoctorAppointmentCountDto
    {
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int TotalAppointments { get; set; }
    }

    public class RevenueBySpecializationDto
    {
        public string Specialization { get; set; } = string.Empty;
        public int TotalAppointments { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class DuplicateBookingDto
    {
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public int BookingCount { get; set; }
    }
}
