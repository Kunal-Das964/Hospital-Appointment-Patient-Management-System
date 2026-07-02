using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<int> BookAsync(int patientId, int doctorId, DateTime date);
        Task CancelAsync(int appointmentId);
        Task<IEnumerable<AppointmentResponseDto>> GetUpcomingAsync();
        Task<IEnumerable<AppointmentResponseDto>> GetByDoctorAsync(int doctorId);
        Task<IEnumerable<AppointmentResponseDto>> GetConsolidatedReportAsync();
        Task<IEnumerable<DoctorAppointmentCountDto>> GetDoctorCountsAsync();
        Task<IEnumerable<RevenueBySpecializationDto>> GetRevenueBySpecAsync();
        Task<IEnumerable<DuplicateBookingDto>> GetDuplicateBookingsAsync();
    }
}
