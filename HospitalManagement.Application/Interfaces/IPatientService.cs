using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Application.DTOs.Patient;

namespace HospitalManagement.Application.Interfaces
{
    public interface IPatientService
    {
        Task<int> RegisterPatientAsync(CreatePatientDto dto);
        Task UpdatePatientAsync(int patientId, UpdatePatientDto dto);
        Task DeactivatePatientAsync(int patientId);
        Task<IEnumerable<PatientResponseDto>> GetAllActivePatientsAsync();
        Task<PatientResponseDto?> GetPatientByIdAsync(int patientId);
    }
}
