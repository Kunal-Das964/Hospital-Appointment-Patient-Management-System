using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // Domain check — cancel ho sakta hai?
        public bool CanBeCancelled()
            => Status == AppointmentStatus.Scheduled;

        // Domain check — past date hai?
        public bool IsInPast()
            => AppointmentDate < DateTime.Now;

        // Readable format — domain mein hoga
        public string FormattedDate
            => AppointmentDate.ToString("dd MMM yyyy hh:mm tt");
    }
}
