using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace HospitalManagement.Application.DTOs.Patient
{
    public class CreatePatientDto
    {
        [Required]
        [StringLength(20)]
        public string PatientCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^[MFO]$", ErrorMessage = "Gender must be M, F, or O")]
        public char Gender { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }
    }
}
