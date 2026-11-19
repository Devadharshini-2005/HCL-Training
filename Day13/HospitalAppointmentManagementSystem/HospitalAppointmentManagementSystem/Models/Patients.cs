using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalAppointmentManagementSystem.Models
{
    internal class Patients
    {

        public Patients()
        {

        }

        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public int PatientAge { get; set; }
        public string? PatientPhoneNo { get; set; }

        public Patients(int patientId,string patientName,int patientAge,string patientPhoneNo)
        {
            PatientId = patientId;
            PatientName = patientName;
            PatientAge = patientAge;
            PatientPhoneNo = patientPhoneNo;
        }

    }
}
