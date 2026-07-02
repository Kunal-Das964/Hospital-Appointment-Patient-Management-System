using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age))
                    age--;
                return age;
            }
        }
    }
}
