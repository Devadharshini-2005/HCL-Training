using HospitalAppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalAppointmentManagementSystem.Services
{
    internal class ReportsServices
    {

        List<Patients>? patients = new List<Patients>();
        List<Doctors>? doctors = new List<Doctors>();
        List<Appointment>? appointments = new List<Appointment>();

        public ReportsServices(List<Patients>? patients, List<Doctors>? doctors, List<Appointment>? appointments)
        {
            this.patients = patients;
            this.doctors = doctors;
            this.appointments = appointments;
        }

        public void AppointmentsByStatusAndDatetime(AppointmentStatus status,DateTime appointmentDateTime)
        {
            if (!appointments!.Any(p => p.Status == status && p.AppointmentDateTime == appointmentDateTime))
            {
                Console.WriteLine("Appointment is Not Exist!!");
                return;
            }
            var appointment = appointments!.Where(p => p.AppointmentDateTime == appointmentDateTime && p.Status == status);

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

        public void ShecduledAndCompletedAppointments()
        {
            var appointment = appointments!.Where(a => a.Status == AppointmentStatus.Scheduled && a.Status == AppointmentStatus.Completed).ToList();

            var groupbyDoctors = doctors!.GroupJoin(
                appointment!,
                d => d.DoctorId,
                a => a.DoctorId,
                (d, Matches) => new
                {
                    doctorId = d.DoctorId,
                    doctorName = d.FullName,
                    specilization = d.Specialization,
                    shecduledCount = Matches.Where(m => m.Status == AppointmentStatus.Scheduled).Count(),
                    completedCount = Matches.Where(m => m.Status == AppointmentStatus.Completed).Count()
                }
                ).ToList();

            foreach (var k in groupbyDoctors)
            {
                Console.WriteLine($"Doctor Id : {k.doctorId}  " +
                    $"Doctor Name : {k.doctorName}  " +
                    $"Specilization : {k.specilization}  " +
                    $"Shecduled Count : {k.shecduledCount}  " +
                    $"Completed Count : {k.completedCount}");
            }

        }

        public void CompletedXConsultationRevenue()
        {
            if (!appointments!.Any(a => a.Status == AppointmentStatus.Completed))
            {
                Console.WriteLine("Appointment Not Exist!!");
                return;
            }
            var appointment = appointments!.Where(a => a.Status == AppointmentStatus.Completed).ToList();

            var totalFee = doctors!.LeftJoin(

                appointment,
                d => d.DoctorId,
                a => a.DoctorId,
                (d, a) => new
                {
                    consultationFee = d.ConsultationFee
                }
                ).ToList();

            decimal consultationFee = 0;
            foreach (var r in totalFee)
            {
                consultationFee += r.consultationFee;
            }

            Console.WriteLine($"Consultation Revenue : {consultationFee}");
        }

        public void AppointmemtsForPatientOrDoctors()
        {
            Console.WriteLine("1.Patient Id\n" +
                "2.Doctor Id");

            Console.WriteLine("Enter the Option : ");
            int.TryParse(Console.ReadLine(), out int option);

            switch (option)
            {
                case 1:
                    {
                        Console.WriteLine("Enter Patient Id :");
                        int.TryParse(Console.ReadLine(), out int patientId);

                        if (!patients!.Any(p => p.PatientId == patientId))
                        {
                            Console.WriteLine("Patient is Not Exist!!");
                            return;
                        }
                        var appointment = appointments!.Where(p => p.PatientId == patientId);

                        foreach (var k in appointment)
                        {
                            Console.WriteLine($"Appointment Id : {k.AppointmentId}\n" +
                                $"Patient Id : {k.PatientId}\n" +
                                $"Doctor Id : {k.DoctorId}\n" +
                                $"Appointment DateTime : {k.AppointmentDateTime}\n" +
                                $"Reason : {k.Reason}\n" +
                                $"Status : {k.Status}");
                        }
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Enter Doctor Id :");
                        int.TryParse(Console.ReadLine(), out int doctorId);

                        if (!doctors!.Any(p => p.DoctorId == doctorId))
                        {
                            Console.WriteLine("Doctor is Not Exist!!");
                            return;
                        }

                        var appointment = appointments!.Where(p => p.DoctorId == doctorId);

                        foreach (var k in appointment)
                        {
                            Console.WriteLine($"Appointment Id : {k.AppointmentId}\n" +
                                $"Patient Id : {k.PatientId}\n" +
                                $"Doctor Id : {k.DoctorId}\n" +
                                $"Appointment DateTime : {k.AppointmentDateTime}\n" +
                                $"Reason : {k.Reason}\n" +
                                $"Status : {k.Status}");
                        }
                        break;
                    }
            }

        }

            



    }
}
