

using CourseManagementSystem.Models;

public class Program
{
    public static void Main(string[] args)
    {
        Course course1 = new Course("CS01", "Python", 3000, 50, 90, 60);
        Course course2 = new Course("CS02", "Java", 5000, 100, 100, 90);
        Course course3 = new Course("CS03", "C#", 4000, 90, 90, 80);
        Course course4 = new Course("CS04", "C", 3000, 70, 80, 60);
        Course course5 = new Course("CS05", "C++", 2000, 100, 100, 70);

        List<Course> courseList = new List<Course>() { course1, course2, course3, course4, course5 };

        Course courseObj = new Course();

        Learner learnerdisp = new Learner();

        Dictionary<string, Learner> learnerList = new Dictionary<string, Learner>();

        Enrollment enroll1 = new Enrollment(1,"23cs01", "cs01", DateTime.Parse("14-10-2026"), 1000, 100, 2900, "Active");

        List<Enrollment> enrolledStudents = new List<Enrollment>() { enroll1 };

        List<Enrollment> removeEnrolledStudent = new List<Enrollment>();

        FeeCalculator feeCalculator = new FeeCalculator();

        int count = 1;

        while (true)
        {
            Console.WriteLine("1.List all courses");
            Console.WriteLine("2.Search courses");
            Console.WriteLine("3.Sort courses by fee");
            Console.WriteLine("4.Register a learner");
            Console.WriteLine("5.List learners");
            Console.WriteLine("6.Search for a learner");
            Console.WriteLine("7.Enroll in a course");
            Console.WriteLine("8.View enrollment records");
            Console.WriteLine("9.Cancel an enrollment");
            Console.WriteLine("10.Display a fee breakdown");
            Console.WriteLine("11.Process a simulated payment");
            Console.WriteLine("12.View payment history");
            Console.WriteLine("13.Display reports");
            Console.WriteLine("14.Exit");


            int option = 0;
            bool isVaildOption = false;

            count = 1;

            while (option < 0 || option >= 15 || !isVaildOption)
            {
                if (count > 1)
                {
                    if (option < 0 || option >= 15 || !isVaildOption)
                    {
                        Console.WriteLine("Invaild Option!!");
                    }
                }
                Console.WriteLine("Enter Option : ");
                isVaildOption = int.TryParse(Console.ReadLine(), out option);
                count++;
            }
            switch (option)
            {
                case 1:
                    {
                        Console.WriteLine("==================");
                        Console.WriteLine("List all courses");
                        Console.WriteLine("==================");
                        courseObj.displayAllCourses(courseList);
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("===========================================");
                        Console.WriteLine("Search courses by Course Code/Course title");
                        Console.WriteLine("============================================");
                        Console.WriteLine("Enter Course Code or Course Title");
                        string input = Console.ReadLine();
                        if (input.Trim() == "")
                        {
                            Console.WriteLine("Invaild Course Code or Course Title!!");
                        }
                        else
                        {
                            courseObj.SearchCourse(courseList, input);
                        }
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("====================");
                        Console.WriteLine("Sort courses by fee");
                        Console.WriteLine("====================");
                        courseObj.SortByFee(courseList);
                        break;
                    }
                case 4:
                    {
                        Console.WriteLine("=====================");
                        Console.WriteLine("Register a learner");
                        Console.WriteLine("=====================");
                        learnerdisp.AddLearners(learnerList);
                        break;
                    }
                case 5:
                    {
                        Console.WriteLine("=============");
                        Console.WriteLine("List learners");
                        Console.WriteLine("=============");
                        learnerdisp.DisplayALlLearners(learnerList);
                        break;
                    }
                case 6:
                    {
                        Console.WriteLine("======================");
                        Console.WriteLine("6.Search for a learner");
                        Console.WriteLine("======================");
                        learnerdisp.SearchLearnerById(learnerList);
                        break;
                    }
                case 7:
                    {
                        Console.WriteLine("=====================");
                        Console.WriteLine("Enroll in a course");
                        Console.WriteLine("=====================");
                        enroll1.AddEnrolledCourse(enrolledStudents,learnerList,courseList);
                        break;
                    }
                case 8:
                    {
                        Console.WriteLine("=========================");
                        Console.WriteLine("View enrollment records");
                        Console.WriteLine("=========================");
                        enroll1.DisplayEnrolledRecords(enrolledStudents);
                        break;
                    }
                case 9:
                    {
                        Console.WriteLine("======================");
                        Console.WriteLine("Cancel an enrollment");
                        Console.WriteLine("======================");
                        enroll1.RemoveStudentFromEnrollment(enrolledStudents,removeEnrolledStudent);
                        break;
                    }
                case 10:
                    {
                        Console.WriteLine("==========================");
                        Console.WriteLine("Display a fee breakdown");
                        Console.WriteLine("==========================");
                        List<decimal> feeCalculationTuple = new List<decimal>(); 
                        feeCalculator.BaseFeeCalculation(courseList);
                        break;
                    }
                case 14:
                    {
                        Console.WriteLine("----------------------------------------Exit-------------------------------------------");
                        break;
                    }

            }

            if (option == 14)
            {
                break;
            }
        }



        //courseObj.AvailableSeatsInAllCourses(courseList);

        learnerdisp.AddLearners(learnerList);


    }
}
