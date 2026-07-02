using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Application.DTOs.Patient;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;
        public PatientService(IPatientRepository repo)
        {
            _repo = repo;
        }

        // Register 
        public async Task<int> RegisterPatientAsync(CreatePatientDto dto)
        {
            var patient = new Patient
            {
                Code = dto.PatientCode,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };

            return await _repo.RegisterAsync(patient);
        }

        // Update
        public async Task UpdatePatientAsync(int patientId, UpdatePatientDto dto)
        {
            var patient = new Patient
            {
                Id = patientId,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };

            await _repo.UpdateAsync(patient);
        }

        // Deactivate 
        public async Task DeactivatePatientAsync(int patientId)
        {
            await _repo.DeactivateAsync(patientId);
        }

        // Get All Active 
        public async Task<IEnumerable<PatientResponseDto>> GetAllActivePatientsAsync()
        {
            var patients = await _repo.GetAllActiveAsync();

            // Mapping from Domain to DTO because Age coming from Domain
            return patients.Select(p => new PatientResponseDto
            {
                PatientId = p.Id,
                PatientCode = p.Code,
                FullName = p.FullName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                Age = p.Age,
                IsActive = p.IsActive
            });
        }

        // Get By ID 
        public async Task<PatientResponseDto?> GetPatientByIdAsync(int patientId)
        {
            var p = await _repo.GetByIdAsync(patientId);
            if (p == null) return null;
            return new PatientResponseDto
            {
                PatientId = p.Id,
                PatientCode = p.Code,
                FullName = p.FullName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                Age = p.Age,
                IsActive = p.IsActive
            };
        }
    }
}
