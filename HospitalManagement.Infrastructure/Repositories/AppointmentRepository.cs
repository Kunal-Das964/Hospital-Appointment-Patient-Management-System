using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HospitalManagement.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly string _connectionString;

        public AppointmentRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        // Book Appointment 
        public async Task<int> BookAsync(int patientId, int doctorId, DateTime date)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_BookAppointment", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PatientId", patientId);
            cmd.Parameters.AddWithValue("@DoctorId", doctorId);
            cmd.Parameters.AddWithValue("@AppointmentDate", date);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        // Cancel Appointment
        public async Task CancelAsync(int appointmentId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_CancelAppointment", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // Get Upcoming (Next 7 Days) 
        public async Task<IEnumerable<AppointmentResponseDto>> GetUpcomingAsync()
        {
            return await ExecuteAppointmentReaderAsync("sp_GetUpcomingAppointments");
        }

        // Get By Doctor 
        public async Task<IEnumerable<AppointmentResponseDto>> GetByDoctorAsync(int doctorId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetAppointmentsByDoctor", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@DoctorId", doctorId);

            await conn.OpenAsync();
            return await ReadAppointmentsAsync(cmd);
        }

        // Consolidated Report 
        public async Task<IEnumerable<AppointmentResponseDto>> GetConsolidatedReportAsync()
        {
            return await ExecuteAppointmentReaderAsync("sp_GetConsolidatedReport");
        }

        // Doctor Appointment Counts (> 2) 
        public async Task<IEnumerable<DoctorAppointmentCountDto>> GetDoctorCountsAsync()
        {
            var result = new List<DoctorAppointmentCountDto>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetDoctorAppointmentCounts", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new DoctorAppointmentCountDto
                {
                    DoctorName = reader.GetString(reader.GetOrdinal("DoctorName")),
                    Specialization = reader.GetString(reader.GetOrdinal("Specialization")),
                    TotalAppointments = reader.GetInt32(reader.GetOrdinal("TotalAppointments"))
                });
            }
            return result;
        }

        // Revenue By Specialization
        public async Task<IEnumerable<RevenueBySpecializationDto>> GetRevenueBySpecAsync()
        {
            var result = new List<RevenueBySpecializationDto>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetRevenueBySpecialization", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new RevenueBySpecializationDto
                {
                    Specialization = reader.GetString(reader.GetOrdinal("Specialization")),
                    TotalAppointments = reader.GetInt32(reader.GetOrdinal("TotalAppointments")),
                    TotalRevenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue"))
                });
            }
            return result;
        }

        // Duplicate Bookings
        public async Task<IEnumerable<DuplicateBookingDto>> GetDuplicateBookingsAsync()
        {
            var result = new List<DuplicateBookingDto>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetDuplicateBookings", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new DuplicateBookingDto
                {
                    PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                    DoctorName = reader.GetString(reader.GetOrdinal("DoctorName")),
                    Specialization = reader.GetString(reader.GetOrdinal("Specialization")),
                    BookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate")),
                    BookingCount = reader.GetInt32(reader.GetOrdinal("BookingCount"))
                });
            }
            return result;
        }

        // Private Helper Methods. No-parameter SPs ke liye helper
        private async Task<IEnumerable<AppointmentResponseDto>> ExecuteAppointmentReaderAsync(string spName)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            return await ReadAppointmentsAsync(cmd);
        }

        // Reader logic ek jagah — duplicate code nahi hoga
        private async Task<List<AppointmentResponseDto>> ReadAppointmentsAsync(SqlCommand cmd)
        {
            var list = new List<AppointmentResponseDto>();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new AppointmentResponseDto
                {
                    AppointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                    PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                    DoctorName = reader.GetString(reader.GetOrdinal("DoctorName")),
                    Specialization = reader.GetString(reader.GetOrdinal("Specialization")),
                    AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                    Status = reader.GetString(reader.GetOrdinal("Status")),
                    ConsultationFee = reader.GetDecimal(reader.GetOrdinal("Fee"))
                });
            }
            return list;
        }
    }
}
