using HospitalAppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalAppointmentManagementSystem.Services
{
    internal class SearchAndReports
    {


        List<Patients>? patients = new List<Patients>();
        List<Doctors>? doctors = new List<Doctors>();
        List<Appointment>? appointments = new List<Appointment>();

        public SearchAndReports() { }

        public SearchAndReports(List<Appointment>? appointments)
        {
            this.appointments = appointments;
        }

        public SearchAndReports(List<Patients>? patients, List<Doctors>? doctors, List<Appointment>? appointments)
        {
            this.patients = patients;
            this.doctors = doctors;
            this.appointments = appointments;
        }

        public void ListByPatient(int patientId)
        {

            if(!patients!.Any(p => p.PatientId == patientId))
            {
                Console.WriteLine("Patient is Not Exist!!");
                return;
            }

            var appointment = appointments!.Where(a => a.PatientId == patientId).ToList(); 

            foreach (var k in appointment)
            {
                Console.WriteLine($"Appointment Id : {k.AppointmentId}\n" +
                    $"Patient Id : {k.PatientId}\n" +
                    $"Doctor Id : {k.DoctorId}\n" +
                    $"Appointment DateTime : {k.AppointmentDateTime}\n" +
                    $"Reason : {k.Reason}\n" +
                    $"Status : {k.Status}" );
            }


            if (!appointment!.Any())
            {
                Console.WriteLine("Patient is Not Exist in the Appointment List!!");
                return;
            }

        }

        public void ListByDoctor(int doctorId)
        {

            if (!doctors!.Any(p => p.DoctorId == doctorId))
            {
                Console.WriteLine("Doctor is Not Exist!!");
                return;
            }

            if (!appointments!.Any(p => p.DoctorId == doctorId))
            {
                Console.WriteLine("Doctor is Not Exist in the Appointment List!!");
                return;
            }

            var appointment = appointments!.Where(a => a.DoctorId == doctorId).ToList();

            foreach (var k in appointment)
            {
                Console.WriteLine($"Appointment Id : {k.AppointmentId}\n" +
                    $"Patient Id : {k.PatientId}\n" +
                    $"Doctor Id : {k.DoctorId}\n" +
                    $"Appointment DateTime : {k.AppointmentDateTime}\n" +
                    $"Reason : {k.Reason}\n" +
                    $"Status : {k.Status}");
            }
        }



        public void ListByDatetime(DateTime appointmentDateTime)
        {
            if (!appointments!.Any(p => p.AppointmentDateTime == appointmentDateTime))
            {
                Console.WriteLine("Appointment DateTime is Not Exist!!");
                return;
            }
            var appointment = appointments!.Where(a => a.AppointmentDateTime == appointmentDateTime).ToList();

            foreach (var k in appointment)
            {
                Console.WriteLine($"Appointment Id : {k.AppointmentId}\n" +
                    $"Patient Id : {k.PatientId}\n" +
                    $"Doctor Id : {k.DoctorId}\n" +
                    $"Appointment DateTime : {k.AppointmentDateTime}\n" +
                    $"Reason : {k.Reason}\n" +
                    $"Status : {k.Status}");
            }

        }

        public void FilterByStauts()
        {
            var appointment = appointments!.GroupBy(a => a.Status).ToList();

            foreach (var j in appointment)
            {
                foreach(var k in j)
                {
                    Console.WriteLine($"Appointment Id : {k.AppointmentId}\n" +
                    $"Patient Id : {k.PatientId}\n" +
                    $"Doctor Id : {k.DoctorId}\n" +
                    $"Appointment DateTime : {k.AppointmentDateTime}\n" +
                    $"Reason : {k.Reason}\n" +
                    $"Status : {k.Status}");
                }
            }
        }

        public void ShecduledAppointmentByDoctor()
        {
            var groupbyDoctors = doctors!.GroupJoin(
                appointments!,
                d => d.DoctorId,
                a => a.DoctorId,
                (d, Matches) => new
                {
                    doctorId = d.DoctorId,
                    doctorName = d.FullName,
                    count = Matches.Count()
                }
                ).ToList();

            foreach(var k in groupbyDoctors)
            {
                Console.WriteLine($"Doctor Id : {k.doctorId}  " +
                    $"Doctor Name : {k.doctorName}  " +
                    $"Appointment Count : {k.count}");
            }

        }
        public void CalulateConsultationFee()
        {
            if (!appointments!.Any(p => p.Status == AppointmentStatus.Completed))
            {
                Console.WriteLine("Completed Appointment is Not Exist!!");
                return;
            }
            var appointment = appointments!.Where(a => a.Status == AppointmentStatus.Completed).ToList();
            var totalFee= doctors!.LeftJoin(
                
                appointment,
                d => d.DoctorId,
                a => a.DoctorId,
                (d,a) => new
                {
                    appointmentId = a!.AppointmentId,
                    consultationFee = d.ConsultationFee
                }
                );

            foreach(var k in totalFee)
            {
                Console.WriteLine($"Appointment Id : {k.appointmentId}  " +
                    $"Consulation Fee : {k.consultationFee}");
            }
        }


        public void DoctorMostCompletedAppointment()
        {
            if (!appointments!.Any(p => p.Status == AppointmentStatus.Completed))
            {
                Console.WriteLine("Completed Appointment is Not Exist!!");
                return;
            }
            var appointment = appointments!.Where(a => a.Status == AppointmentStatus.Completed).ToList();

            var groupbyDoctors = doctors!.GroupJoin(
                appointment!,
                d => d.DoctorId,
                a => a.DoctorId,
                (d, Matches) => new
                {
                    doctorId = d.DoctorId,
                    doctorName = d.FullName,
                    count = Matches.Count()
                }
                ).ToList();
            int max = 0;
            int doctorId=0;
            string? doctorName="";
            int count=0;
            foreach (var k in groupbyDoctors)
            {
                if(k.count > max)
                {
                    max=k.count;
                    doctorId = k.doctorId;
                    doctorName= k.doctorName;
                    count= k.count;
                }
            }

            Console.WriteLine($"Doctor Id : {doctorId}  " +
                    $"Doctor Name : {doctorName}  " +
                    $"Appointment Count : {count}");

        }




    }
}
