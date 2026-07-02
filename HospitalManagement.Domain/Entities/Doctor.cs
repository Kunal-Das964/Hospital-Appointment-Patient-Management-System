using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public string Specialization { get; set; } = string.Empty;
        public decimal ConsultationFee { get; set; }
        public bool IsAvailable { get; set; }
    }
}
