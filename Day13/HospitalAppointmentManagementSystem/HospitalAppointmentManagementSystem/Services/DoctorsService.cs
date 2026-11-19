using HospitalAppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;


namespace HospitalAppointmentManagementSystem.Services
{
    internal class DoctorsService
    {

        public List<Doctors>? doctors=new List<Doctors>();

        public string? filePath = Path.Combine(AppContext.BaseDirectory, "DataFiles", "doctors.json");

        public void LoadDoctors()
        {
            string? folder = Path.GetDirectoryName(filePath);
            if (folder != null)
            {
                Directory.CreateDirectory(folder);
            }
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                doctors = JsonSerializer.Deserialize<List<Doctors>>(json) ?? new List<Doctors>();

            }
            else
            {
                doctors = new List<Doctors>();
                SaveDoctors();
            }

            Console.WriteLine("Loaded Successfully!!");
        }

        public void SaveDoctors()
        {
            string json = JsonSerializer.Serialize(doctors, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath!, json);
        }

        public void Register(int doctorId,string fullname,string specialization,decimal consultationFee)
        {
            Doctors doctor = new Doctors(doctorId,fullname,specialization,consultationFee);

            if(doctors!.Any(d => d.DoctorId == doctorId))
            {
                Console.WriteLine("Doctor Already Exit!!");
                return;
            }

            doctors!.Add(doctor);
            Console.WriteLine("Doctor Details are Added Successfully!!");
        }

        public void GetById(int doctorId)
        {

            if (!doctors!.Any(d => d.DoctorId == doctorId))
            {
                Console.WriteLine("Doctor Not Found!!");
                return;
            }

            Console.WriteLine("----------Doctor Details-------------");

            var doctor = doctors!.Where(p => p.DoctorId == doctorId);

            foreach(var j in doctor)
            {
                Console.WriteLine($"Doctor Id : {j.DoctorId}\n" +
                    $"Doctor Name : {j.FullName}\n" +
                    $"Specialization : {j.Specialization}\n" +
                    $"ConsultationFee : {j.ConsultationFee}");
            }
        }

        public void DisplayAllDoctors()
        {
            Console.WriteLine("--------------Doctors List----------------");
            foreach (Doctors j in doctors!)
            {
                Console.WriteLine($"Doctor Id : {j.DoctorId}\n" +
                    $"Doctor Name : {j.FullName}\n" +
                    $"Specialization : {j.Specialization}\n" +
                    $"Consultation Fee : {j.ConsultationFee}");
            }
        }

        public void SearchByName(string doctorName)
        {
            Console.WriteLine("--------------Doctors List----------------");

            var doctor = doctors!.Where(p => p.FullName!.ToLower().Contains(doctorName.ToLower()));

            foreach (var j in doctor)
            {
                Console.WriteLine($"Doctor Id : {j.DoctorId}\n" +
                   $"Doctor Name : {j.FullName}\n" +
                   $"Specialization : {j.Specialization}\n" +
                   $"Consultation Fee : {j.ConsultationFee}");
            }
        }

        public void UpdateDoctorDetails(int doctorId,string doctorName,string specialization,decimal consultationFee)
        {
            if(!doctors!.Any(d => d.DoctorId == doctorId))
            {
                Console.WriteLine("Doctor Details Not Found!!!");
                return;
            }

            foreach (Doctors j in doctors!)
            {
                if (j.DoctorId == doctorId) 
                {
                    j.FullName = doctorName;
                    j.Specialization = specialization;
                    j.ConsultationFee= consultationFee;
                    break;
                }
            }

            Console.WriteLine("Updated Successfully!!");

        }

        public List<Doctors> GetDoctorsList()
        {
            return doctors!;
        }

    }
}
