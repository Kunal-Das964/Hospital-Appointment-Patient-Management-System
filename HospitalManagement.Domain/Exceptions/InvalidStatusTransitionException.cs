using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Domain.Exceptions
{ 
    public class InvalidStatusTransitionException : DomainException
    {
        public InvalidStatusTransitionException(string message) : base(message)
        {
        }
    }
}
