using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalAppointmentManagementSystem.Models
{
    internal class Appointment
    {

        public int AppointmentId {  get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string? Reason { get; set; }
        public AppointmentStatus? Status { get; set; }

        public Appointment() { }

        public Appointment(int appointmentId,int patientId,int doctorId,DateTime appointmentDateandTime,string reason)
        {
            AppointmentId= appointmentId;
            PatientId= patientId;
            DoctorId= doctorId;
            AppointmentDateTime= appointmentDateandTime;
            Reason= reason;
            Status = AppointmentStatus.Scheduled;
        }
    }
}
