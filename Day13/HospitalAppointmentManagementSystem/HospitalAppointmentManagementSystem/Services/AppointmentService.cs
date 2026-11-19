using HospitalAppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HospitalAppointmentManagementSystem.Services
{
    internal class AppointmentService
    {
        Appointment appointment = new Appointment();

        public string? filePath = Path.Combine(AppContext.BaseDirectory, "DataFiles", "appointments.json");

        public void LoadAppointments()
        {
            string? folder = Path.GetDirectoryName(filePath);
            if (folder != null)
            {
                Directory.CreateDirectory(folder);
            }
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                appointments = JsonSerializer.Deserialize<List<Appointment>>(json) ?? new List<Appointment>();

            }
            else
            {
                appointments = new List<Appointment>();
                SaveAppointments();
            }

            Console.WriteLine("Loaded Successfully!!");
        }

        public void SaveAppointments()
        {
            string json = JsonSerializer.Serialize(appointments, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath!, json);
        }

        public List<Patients>? patients = new List<Patients>();
        public List<Doctors>? doctors = new List<Doctors>();

        public AppointmentService() { }

        public List<Appointment>? appointments = new List<Appointment>();

        public AppointmentService(List<Patients> patients,List<Doctors> doctors)
        {
            this.patients = patients;
            this.doctors= doctors;
        }

        public void BookAppointment(int appointmentId,int patientId,int doctorId,DateTime appointmentdatetime,string reason)
        {
            if(appointments!.Any(a => a.AppointmentId == appointmentId))
            {
                Console.WriteLine("Appointment Already Booked!!");
                return;
            }
            if(patients!.Any(p => p.PatientId == patientId))
            {
                Console.WriteLine("Invaild Patient Id!!");
                return;
            }
            if(doctors!.Any(d => d.DoctorId == doctorId))
            {
                Console.WriteLine("Invaild Doctor Id!!");
                return;
            }
            Appointment appointment = new Appointment(appointmentId,patientId,doctorId,appointmentdatetime,reason);
            appointments!.Add(appointment);
            Console.WriteLine("Appointment Booked Successfully!!");
        }

        public void ViewAllAppointment()
        {

            Console.WriteLine("-----------Appointment List-------------");
 
            foreach (Appointment k in appointments!)
            {
                Console.WriteLine($"Appointment Id : {k.AppointmentId}\n" +
                    $"Patient Id : {k.PatientId}\n" +
                    $"Doctor Id : {k.DoctorId}\n" +
                    $"Appointment DateTime : {k.AppointmentDateTime}\n" +
                    $"Reason : {k.Reason}\n" +
                    $"Status : {k.Status}");
            }
        }

        public void GetById(int appointmentId)
        {
            if(!appointments!.Any(a => a.AppointmentId == appointmentId))
            {
                Console.WriteLine("Appointment Not Found!!!");
                return;
            }

            var appointment = appointments!.Where(a => a.AppointmentId == appointmentId);

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


        public void ReshecduleAppointment(int appointmentId,DateTime appointmentDateTime)
        {
            if (appointments!.Any(a => a.AppointmentId==appointmentId))
            {
                Console.WriteLine("Appointment Not Found!!!");
                return;
            }

            var appointment = appointments!.Where(a => a.AppointmentId==appointmentId);

            foreach (var k in appointment)
            {
                if (k.AppointmentDateTime == appointmentDateTime)
                {
                    k.AppointmentDateTime = appointmentDateTime;
                    Console.WriteLine("Appointment Date is Reshecduled!!");
                    return;
                }
            }

            Console.WriteLine("Appointment Date is Invaild!!");

        }

        public void CancelAppointment(int appointmentId)
        {
            if (!appointments!.Any(a => a.AppointmentId == appointmentId))
            {
                Console.WriteLine("Appointment Not Found!!!");
                return;
            }

            foreach (Appointment k in appointments!)
            {
                if(k.Status== AppointmentStatus.Scheduled && k.AppointmentId==appointmentId)
                {
                    k.Status = AppointmentStatus.Cancelled;
                    Console.WriteLine("Cancelled Successfully!!");
                    return;
                }
            }
            Console.WriteLine("Can't Cancelled!!");
        }

        public void MarkCompleted(int appointmentId)
        {

            if (!appointments!.Any(a => a.AppointmentId == appointmentId))
            {
                Console.WriteLine("Appointment Not Found!!!");
                return;
            }

            foreach (Appointment k in appointments!)
            {
                if (k.Status == AppointmentStatus.Scheduled && k.AppointmentId == appointmentId)
                {
                    k.Status = AppointmentStatus.Completed;
                    Console.WriteLine("Completed Successfully!!");

                    return;
                }
            }
            Console.WriteLine("Not in the Scheduled List so can't change to Completed!!");

        }
        public List<Appointment>  GetAppointmentList()
        {
            return appointments!;
        }

    }
}




