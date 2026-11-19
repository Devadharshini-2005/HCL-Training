using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalAppointmentManagementSystem.Models
{
    internal class Doctors
    {

        public int DoctorId { get; set; }
        public string? FullName { get; set; }
        public string? Specialization {  get; set; }
        public decimal ConsultationFee {  get; set; }

        public Doctors() { }

        public Doctors(int doctorId,string fullname,string specialization,decimal consultationFee)
        {
            DoctorId = doctorId;
            FullName = fullname;
            Specialization = specialization;
            ConsultationFee= consultationFee;
        }

    }
}
