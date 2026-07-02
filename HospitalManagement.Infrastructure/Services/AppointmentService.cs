using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Exceptions;

namespace HospitalManagement.Infrastructure.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        // Book Appointment 
        public async Task<int> BookAppointmentAsync(BookAppointmentDto dto)
        {
            if (dto.AppointmentDate < DateTime.Now)
                throw new PastAppointmentDateException();

            return await _repo.BookAsync(dto.PatientId, dto.DoctorId, dto.AppointmentDate);
        }

        // Cancel Appointment 
        public async Task CancelAppointmentAsync(int appointmentId)
        {
            await _repo.CancelAsync(appointmentId);
        }

        // Upcoming Appointments 
        public async Task<IEnumerable<AppointmentResponseDto>> GetUpcomingAsync()
        {
            return await _repo.GetUpcomingAsync();
        }

        // By Doctor 
        public async Task<IEnumerable<AppointmentResponseDto>> GetByDoctorAsync(int doctorId)
        {
            return await _repo.GetByDoctorAsync(doctorId);
        }

        // Consolidated Report 
        public async Task<IEnumerable<AppointmentResponseDto>> GetConsolidatedReportAsync()
        {
            return await _repo.GetConsolidatedReportAsync();
        }

        // Doctor Counts 
        public async Task<IEnumerable<DoctorAppointmentCountDto>> GetDoctorCountsAsync()
        {
            return await _repo.GetDoctorCountsAsync();
        }

        // Revenue By Specialization 
        public async Task<IEnumerable<RevenueBySpecializationDto>> GetRevenueBySpecAsync()
        {
            return await _repo.GetRevenueBySpecAsync();
        }

        // Duplicate Bookings 
        public async Task<IEnumerable<DuplicateBookingDto>> GetDuplicateBookingsAsync()
        {
            return await _repo.GetDuplicateBookingsAsync();
        }
    }
}
