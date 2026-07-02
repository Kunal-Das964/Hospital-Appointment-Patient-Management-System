using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Application.DTOs.Doctor;


namespace HospitalManagement.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorResponseDto>> GetDoctorsAsync(string? specialization, bool? isAvailable);
        Task<DoctorResponseDto?> GetDoctorByIdAsync(int doctorId);
    }
}
