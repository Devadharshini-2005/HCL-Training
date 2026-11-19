
using HospitalAppointmentManagementSystem.Models;
using HospitalAppointmentManagementSystem.Services;
using System.Transactions;

public class Program

{
    public static void Main(string[] args)
    {
        

        int option;

        do
        {
            Console.WriteLine($"-----Hospital Appointment Management System-----\n" +
            $"1.Patient Management\n" +
            $"2.Doctors Management\n" +
            $"3.Appointment Management\n" +
            $"4.Search and View Records\n" +
            $"5.Reports\n" +
            $"6.Exit");

            Console.WriteLine("Enter the option : ");

            int.TryParse(Console.ReadLine(), out option);

            switch (option)
            {
                case 1:
                    {
                        PatientManagement();
                        break;
                    }
                case 2:
                    {
                        DoctorManagement();
                        break;
                    }
                case 3:
                    {
                        AppointmentManagement();
                        break;
                    }

                case 4:
                    {
                        SearchAndViewReportMAngement();
                        break;
                    }
                case 5:
                    {
                        ReportManagement();
                        break;
                    }
                case 6:
                    {
                        Console.WriteLine("---------------------Exit------------------");
                        break;
                    }
            }
        } while (option!=6);


        
    }
    static public void PatientManagement()
    {
        PatientsServices patientService = new PatientsServices();

        patientService.LoadPatients();

        int option;

        do
        {
            Console.WriteLine("---Patient Management Operations---\n" +
            "1.Register Patient\n" +
            "2.Display patient\n" +
            "3.Display All Patients\n" +
            "4.Search by Patient Name\n" +
            "5.Update Patient by id\n" +
            "6.Delete Patient Records\n" +
            "7.Save and Exit");
            Console.WriteLine("Enter the option : ");

            int.TryParse(Console.ReadLine(), out option);

            switch (option)
            {
                case 1:
                    {
                        Console.WriteLine("Enter Patient Id : ");
                        int.TryParse(Console.ReadLine(), out int patientId);
                        Console.WriteLine("Enter Patient Name : ");
                        string? patientName = Console.ReadLine();
                        Console.WriteLine("Enter Patient Age : ");
                        int.TryParse(Console.ReadLine(), out int patientAge);
                        Console.WriteLine("Enter Patient Phone Number : ");
                        string? patientPhoneNo = Console.ReadLine();

                        patientService.Register(patientId, patientName!, patientAge, patientPhoneNo!);

                        break;
                    }

                case 2:
                    {
                        Console.WriteLine("Enter the Patient Id : ");
                        int patientId;
                        int.TryParse(Console.ReadLine(), out patientId);

                        patientService.GetById(patientId);
                        break;
                    }

                case 3:
                    {
                        patientService.DispalyAllPatients();
                        break;
                    }

                case 4:
                    {
                        Console.WriteLine("Enter Patient Name : ");
                        string? patientName = Console.ReadLine();
                        patientService.SearchByPatientName(patientName!);
                        break;
                    }
                case 5:
                    {
                        Console.WriteLine("Enter Patient Id : ");
                        int.TryParse(Console.ReadLine(), out int patientId);
                        Console.WriteLine("Enter Patient Name : ");
                        string? patientName = Console.ReadLine();
                        Console.WriteLine("Enter Patient Age : ");
                        int.TryParse(Console.ReadLine(), out int patientAge);
                        Console.WriteLine("Enter Patient Phone Number : ");
                        string? patientPhoneNo = Console.ReadLine();
                        patientService.UpdatePatientDeatils(patientId, patientName!, patientAge, patientPhoneNo!);
                        break;
                    }
                case 7:
                    {
                        patientService.SavePatients();
                        Console.WriteLine("----------------Exit--------------------");
                        break;
                    }
            }

        } while (option!=7);

        }

    static public void DoctorManagement()
    {
        DoctorsService doctorsService = new DoctorsService();

        doctorsService.LoadDoctors();
        int option;

        do
        {
            Console.WriteLine("-----Doctor Management-----\n" +
            "1.Register Doctor\n" +
            "2.Display\n" +
            "3.Get by Id\n" +
            "4.Search\n" +
            "5.Update\n" +
            "6.Delete\n" +
            "7.Save and Exit");

            int.TryParse(Console.ReadLine(), out option);

            switch (option)
            {
                case 1:
                    {
                        Console.WriteLine("Enter the Doctor Id : ");
                        int.TryParse(Console.ReadLine(), out int doctorId);
                        Console.WriteLine("Enter the Name : ");
                        string? fullName = Console.ReadLine();
                        Console.WriteLine("Enter the Specialization : ");
                        string? specialization = Console.ReadLine();
                        Console.WriteLine("Enter the ConsultationFee : ");
                        decimal.TryParse(Console.ReadLine(), out decimal consultationFee);
                        doctorsService.Register(doctorId, fullName!, specialization!, consultationFee);
                        break;
                    }

                case 2:
                    {
                        doctorsService.DisplayAllDoctors();
                        break;
                    }

                case 3:
                    {
                        Console.WriteLine("Enter the Doctor Id : ");
                        int.TryParse(Console.ReadLine(), out int doctorId);
                        doctorsService.GetById(doctorId);
                        break;
                    }
                case 4:
                    {
                        Console.WriteLine("Enter the Name : ");
                        string? fullName = Console.ReadLine();
                        doctorsService.SearchByName(fullName!);
                        break;
                    }

                case 5:
                    {
                        Console.WriteLine("Enter the Doctor Id : ");
                        int.TryParse(Console.ReadLine(), out int doctorId);
                        Console.WriteLine("Enter the Name : ");
                        string? fullName = Console.ReadLine();
                        Console.WriteLine("Enter the Specialization : ");
                        string? specialization = Console.ReadLine();
                        Console.WriteLine("Enter the ConsultationFee : ");
                        decimal.TryParse(Console.ReadLine(), out decimal consultationFee);
                        doctorsService.UpdateDoctorDetails(doctorId, fullName!, specialization!, consultationFee);
                        break;
                    }

                case 7:
                    {
                        doctorsService.SaveDoctors();
                        Console.WriteLine("------------------------------Exit-----------------------------");
                        break;
                    }
            }
        } while (option != 7);
    }

    static public void AppointmentManagement()
    {
        int option;
        PatientsServices patientsServices = new PatientsServices();
        DoctorsService doctorsService = new DoctorsService();
        AppointmentService appointmentService = new AppointmentService(patientsServices.GetPatientsList(),doctorsService.GetDoctorsList());

        appointmentService.LoadAppointments();

        do
        {
            Console.WriteLine("--------Appointment Management---------\n" +
                "1.Book Appointment\n" +
                "2.View All Appointment\n" +
                "3.Get By Id\n" +
                "4.Reshecduled Appointment\n" +
                "5.Cancel Appointment\n" +
                "6.Mark as Completed\n" +
                "7.Save and Exit");

            Console.WriteLine("Enter the Option : ");

            int.TryParse(Console.ReadLine(),out option);

            switch (option)
            {
                case 1:
                    {
                        Console.WriteLine("Enter Appointment Id : ");
                        int.TryParse(Console.ReadLine(),out int appointmentId);
                        Console.WriteLine("Enter Patient Id : ");
                        int.TryParse(Console.ReadLine(),out int patientId);
                        Console.WriteLine("Enter Doctor Id : ");
                        int.TryParse(Console.ReadLine(),out int doctorId);
                        Console.WriteLine("Enter Appointment DateTime : ");
                        DateTime.TryParse(Console.ReadLine(),out DateTime appointmentdatetime);
                        Console.WriteLine("Enter Reason : ");
                        string? reason = Console.ReadLine();
                        appointmentService.BookAppointment(appointmentId,patientId,doctorId,appointmentdatetime,reason!);
                        break;
                    }
                case 2:
                    {
                        appointmentService.ViewAllAppointment();
                        break;
                    }

                case 3:
                    {
                        Console.WriteLine("Enter Appointment Id : ");
                        int.TryParse(Console.ReadLine(), out int appointmentId);
                        appointmentService.GetById(appointmentId);
                        break;
                    }

                case 4:
                    {
                        Console.WriteLine("Enter Appointment Id : ");
                        int.TryParse(Console.ReadLine(), out int appointmentId);
                        Console.WriteLine("Enter Appointment DateTime : ");
                        DateTime.TryParse(Console.ReadLine(), out DateTime appointmentDateTime);
                        appointmentService.ReshecduleAppointment(appointmentId,appointmentDateTime);
                        break;
                    }

                case 5:
                    {
                        Console.WriteLine("Enter Appointment Id : ");
                        int.TryParse(Console.ReadLine(), out int appointmentId);
                        appointmentService.CancelAppointment(appointmentId);
                        break;
                    }

                case 6:
                    {
                        Console.WriteLine("Enter Appointment Id : ");
                        int.TryParse(Console.ReadLine(), out int appointmentId);
                        appointmentService.MarkCompleted(appointmentId);
                        break;
                    }

                case 7:
                    {
                        appointmentService.SaveAppointments();
                        Console.WriteLine("------------------Exit----------------");
                        break;
                    }
            }

        } while (option !=7);
    }

   static public void SearchAndViewReportMAngement()
    {
        int option;

        PatientsServices patientsServices = new PatientsServices();
        DoctorsService doctorsService = new DoctorsService();
        AppointmentService appointmentService=new AppointmentService();

        patientsServices.LoadPatients();
        doctorsService.LoadDoctors();
        appointmentService.LoadAppointments();

        SearchAndReports searchAndReports = new SearchAndReports(patientsServices.GetPatientsList(),doctorsService.GetDoctorsList(),appointmentService.GetAppointmentList());

        do
        {
            Console.WriteLine("------------Search and Reports------------\n" +
                "1.List Appointment By Patient\n" +
                "2.List Appointment By Doctor\n" +
                "3.List Appointment By Date\n" +
                "4.Filter By Status\n" +
                "5.Each Doctor's Appointment Count\n" +
                "6.Calculate Consultation Fee for Completed Appointment\n" +
                "7.Doctor with most Completed Appointmnet\n" +
                "8.Exit");

            Console.WriteLine("Enter the Option : ");
            int.TryParse(Console.ReadLine(),out option);

            switch (option)
            {
                case 1:
                    {
                        Console.WriteLine("Enter Patient Id : ");
                        int.TryParse(Console.ReadLine(), out int patientId);
                        searchAndReports.ListByPatient(patientId);
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Enter Doctor Id : ");
                        int.TryParse(Console.ReadLine(), out int doctorId);
                        searchAndReports.ListByDoctor(doctorId);
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("Enter Appointment DateTime (YYYY-MM-DD) : ");
                        DateTime.TryParse(Console.ReadLine(), out DateTime appointmentDateTime);
                        searchAndReports.ListByDatetime(appointmentDateTime);
                        break;
                    }
                case 4:
                    {
                        searchAndReports.FilterByStauts();
                        break;
                    }
                case 5:
                    {
                        searchAndReports.ShecduledAppointmentByDoctor();
                        break;
                    }
                case 6:
                    {
                        searchAndReports.CalulateConsultationFee();
                        break;
                    }
                case 7:
                    {
                        searchAndReports.DoctorMostCompletedAppointment();
                        break;
                    }
                case 8:
                    {
                        Console.WriteLine("------------------------Exit---------------------------");
                        break;
                    }
            }
        } while (option!=8);
    }

    static public void ReportManagement()
    {
        int option;

        PatientsServices patientsServices = new PatientsServices();
        DoctorsService doctorsService = new DoctorsService();
        AppointmentService appointmentService = new AppointmentService();

        patientsServices.LoadPatients();
        doctorsService.LoadDoctors();
        appointmentService.LoadAppointments();

        ReportsServices reports = new ReportsServices(patientsServices.GetPatientsList(), doctorsService.GetDoctorsList(), appointmentService.GetAppointmentList());

        do
        {

            Console.WriteLine("----------Report Mangement------------\n" +
                "1.Apointment By Date and Staus\n" +
                "2.Doctor-wise scheduled and completed appointment counts\n" +
                "3.Completed consultation revenue.\n" +
                "4.Appointments for a selected patient or doctor\n" +
                "5.Exit");

            Console.WriteLine("Enter the option : ");

            int.TryParse(Console.ReadLine(), out option);

            switch (option)
            {
                case 1:
                    {
                        Console.WriteLine("Enter the DateTime : ");
                        DateTime.TryParse(Console.ReadLine(),out DateTime appointmentDateTime);
                        Console.WriteLine("Enter the Status");
                        string? input = Console.ReadLine();
                        Enum.TryParse<AppointmentStatus>(input,true,out AppointmentStatus status);
                        reports.AppointmentsByStatusAndDatetime(status,appointmentDateTime);
                        break;
                    }
                case 2:
                    {
                        reports.ShecduledAndCompletedAppointments();
                        break;
                    }
                case 3:
                    {
                        reports.CompletedXConsultationRevenue();
                        break;
                    }
                case 4:
                    {
                        reports.AppointmemtsForPatientOrDoctors();
                        break;
                    }
                case 5:
                    {
                        Console.WriteLine("----------------------Exit---------------------");
                        break;
                    }
            }


        } while (option != 5);
    }
}
