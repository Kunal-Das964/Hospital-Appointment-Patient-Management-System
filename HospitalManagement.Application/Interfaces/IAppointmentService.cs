using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Application.DTOs.Appointment;


namespace HospitalManagement.Application.Interfaces
{

    public interface IAppointmentService
    {
        Task<int> BookAppointmentAsync(BookAppointmentDto dto);
        Task CancelAppointmentAsync(int appointmentId);
        Task<IEnumerable<AppointmentResponseDto>> GetUpcomingAsync();
        Task<IEnumerable<AppointmentResponseDto>> GetByDoctorAsync(int doctorId);
        Task<IEnumerable<AppointmentResponseDto>> GetConsolidatedReportAsync();
        Task<IEnumerable<DoctorAppointmentCountDto>> GetDoctorCountsAsync();
        Task<IEnumerable<RevenueBySpecializationDto>> GetRevenueBySpecAsync();
        Task<IEnumerable<DuplicateBookingDto>> GetDuplicateBookingsAsync();
    }
}
