using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Domain.Exceptions
{
    public class PastAppointmentDateException : DomainException
    {
        public PastAppointmentDateException() : base("Appointment date cannot be in the past.")
        {
        }
    }

}
