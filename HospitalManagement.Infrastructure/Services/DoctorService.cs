using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Application.DTOs.Doctor;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HospitalManagement.Infrastructure.Services
{   
    public class DoctorService : IDoctorService
    {
        private readonly string _connectionString;
        public DoctorService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        // Get Doctors (Filter ke saath)
        public async Task<IEnumerable<DoctorResponseDto>> GetDoctorsAsync(string? specialization, bool? isAvailable)
        {
            var doctors = new List<DoctorResponseDto>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetDoctors", conn)
            {
                CommandType = CommandType.StoredProcedure
            };


            cmd.Parameters.AddWithValue("@Specialization", (object?)specialization ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsAvailable", (object?)isAvailable ?? DBNull.Value);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                doctors.Add(new DoctorResponseDto
                {
                    DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                    DoctorCode = reader.GetString(reader.GetOrdinal("DoctorCode")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Specialization = reader.GetString(reader.GetOrdinal("Specialization")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    ConsultationFee = reader.GetDecimal(reader.GetOrdinal("ConsultationFee")),
                    IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable"))
                });
            }
            return doctors;
        }

        // Get Doctor By ID 
        public async Task<DoctorResponseDto?> GetDoctorByIdAsync(int doctorId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetDoctorById", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@DoctorId", doctorId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new DoctorResponseDto
                {
                    DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                    DoctorCode = reader.GetString(reader.GetOrdinal("DoctorCode")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Specialization = reader.GetString(reader.GetOrdinal("Specialization")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    ConsultationFee = reader.GetDecimal(reader.GetOrdinal("ConsultationFee")),
                    IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable"))
                };
            }
            return null;
        }
    }
}
