using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class PatientRepository : IPatientRepository
{
    private readonly string _connectionString;

    public PatientRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    // Register 
    public async Task<int> RegisterAsync(Patient patient)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_RegisterPatient", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@PatientCode", patient.Code);
        cmd.Parameters.AddWithValue("@FullName", patient.FullName);
        cmd.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
        cmd.Parameters.AddWithValue("@Gender", patient.Gender.ToString());
        cmd.Parameters.AddWithValue("@PhoneNumber", patient.PhoneNumber);
        cmd.Parameters.AddWithValue("@Email", (object?)patient.Email ?? DBNull.Value);

        await conn.OpenAsync();
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    // Update
    public async Task UpdateAsync(Patient patient)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_UpdatePatient", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@PatientId", patient.Id);
        cmd.Parameters.AddWithValue("@FullName", patient.FullName);
        cmd.Parameters.AddWithValue("@PhoneNumber", patient.PhoneNumber);
        cmd.Parameters.AddWithValue("@Email", (object?)patient.Email ?? DBNull.Value);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    // Deactivate
    public async Task DeactivateAsync(int patientId)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_DeactivatePatient", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@PatientId", patientId);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    // Get All Active 
    public async Task<IEnumerable<Patient>> GetAllActiveAsync()
    {
        var patients = new List<Patient>();

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_GetAllActivePatients", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            patients.Add(new Patient
            {
                Id = reader.GetInt32(reader.GetOrdinal("PatientId")),
                Code = reader.GetString(reader.GetOrdinal("PatientCode")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                Gender = reader.GetString(reader.GetOrdinal("Gender"))[0],
                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                IsActive = true
            });
        }
        return patients;
    }

    // Get By ID 
    public async Task<Patient?> GetByIdAsync(int patientId)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_GetPatientById", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@PatientId", patientId);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Patient
            {
                Id = reader.GetInt32(reader.GetOrdinal("PatientId")),
                Code = reader.GetString(reader.GetOrdinal("PatientCode")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                Gender = reader.GetString(reader.GetOrdinal("Gender"))[0],
                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }
        return null;
    }
}
