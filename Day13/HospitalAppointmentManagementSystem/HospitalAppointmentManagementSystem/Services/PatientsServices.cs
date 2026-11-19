using HospitalAppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HospitalAppointmentManagementSystem.Services
{
    internal class PatientsServices
    {
        public List<Patients> patients=new List<Patients>();
        
        public PatientsServices() { }

        public string? filePath = Path.Combine(AppContext.BaseDirectory, "DataFiles","patients.json");

        public void LoadPatients()
        {
            string? folder = Path.GetDirectoryName(filePath);

                Directory.CreateDirectory(folder!);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                patients = JsonSerializer.Deserialize<List<Patients>>(json) ?? new List<Patients>();

            }
            else
            {
                patients = new List<Patients>();
                SavePatients();
            }

            Console.WriteLine("Loaded Successfully!!");
        }

        public void SavePatients()
        {
            string json =JsonSerializer.Serialize(patients,new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath!,json);

            Console.WriteLine("Saved Successfully!!");
        }


        public void Register(int patientId, string patientName, int patientAge, string patientPhoneNo)
        {
            Patients patient = new Patients(patientId, patientName, patientAge, patientPhoneNo);
           
            if (patients.Any(p => p.PatientId == patientId))
            {
                Console.WriteLine("Patient Already Exist!!");
                return;
            }

            patients.Add(patient);

            SavePatients();
            Console.WriteLine("Patient is Added Successfully......");
           
        }

        public void GetById(int patientId)
        {
            var patient = patients.Any(p => p.PatientId == patientId);
            if (!patient)
            {
                Console.WriteLine("Patients is Not Found!!");
                return;
            }

            foreach (var i in patients) 
            {
                if (i.PatientId == patientId)
                {
                    Console.WriteLine($"Patient Id : {i.PatientId}\n" +
                        $"Patient Name : {i.PatientName}\n" +
                        $"Patient Age : {i.PatientAge}\n" +
                        $"Patient Phone Number : {i.PatientPhoneNo}");
                }
            }
        }

        public void DispalyAllPatients()
        {
            Console.WriteLine("---------------------Patients List--------------------------");
            foreach (var i in patients)
            {
                Console.WriteLine($"Patient Id : {i.PatientId}\n" +
                        $"Patient Name : {i.PatientName}\n" +
                        $"Patient Age : {i.PatientAge}\n" +
                        $"Patient Phone Number : {i.PatientPhoneNo}");
            }
        }

        public void SearchByPatientName(string patientName)
        {
            var patient = patients.Where(p => p.PatientName!.ToLower().Contains(patientName.ToLower())).Select(p => p);

            foreach (var i in patient)
            {
                Console.WriteLine($"Patient Id : {i.PatientId}\n" +
                         $"Patient Name : {i.PatientName}\n" +
                         $"Patient Age : {i.PatientAge}\n" +
                         $"Patient Phone Number : {i.PatientPhoneNo}");
            }

        }

        public void UpdatePatientDeatils(int patientId, string patientName, int patientAge, string patientPhoneNo)
        {

            if (!patients.Any(p => p.PatientId == patientId))
            {
                Console.WriteLine("Patient Record Not Found!!");
                return;
            }

            foreach (Patients i in patients)
            {
                if(i.PatientId == patientId)
                {
                    i.PatientName = patientName;
                    i.PatientAge= patientAge;
                    i.PatientPhoneNo = patientPhoneNo;
                    break;
                }
            }

            Console.WriteLine("Updated Successfully!!");

        }

        public List<Patients> GetPatientsList()
        {
            return patients;
        }

    }
}
