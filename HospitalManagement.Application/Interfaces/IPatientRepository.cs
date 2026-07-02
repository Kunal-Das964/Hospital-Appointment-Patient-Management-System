using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces
{
    public interface IPatientRepository
    {
        Task<int> RegisterAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeactivateAsync(int patientId);
        Task<IEnumerable<Patient>> GetAllActiveAsync();
        Task<Patient?> GetByIdAsync(int patientId);
    }
}
