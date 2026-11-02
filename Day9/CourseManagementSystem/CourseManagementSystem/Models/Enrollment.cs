
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagementSystem.Models
{
    internal class Enrollment
    {
        public int enrollmentId = 0;
        public int EnrollmentId
        {
            get
            {
                return enrollmentId;
            }
            set
            {
                enrollmentId = value;
            }
        }

        public string learnerId;

        public string LearnerId
        {
            get
            {
                return learnerId;
            }
            set
            {

                learnerId = value;
            }
        }

        public string courseCode;

        public string CourseCode
        {
            get
            {
                return courseCode;
            }
            set
            {
                courseCode = value;
            }
        }

        public DateTime enrollmentDate;
        public DateTime EnrollmentDate
        {
            get
            {
                return enrollmentDate;
            }
            set
            {
                enrollmentDate = value;
            }
        }

        public decimal baseFee;
        public decimal BaseFee
        {
            get
            {
                return baseFee;
            }
            set
            {
                baseFee = value;
            }
        }

        public decimal discountAmount;
        public decimal DiscountAmount
        {
            get
            {
                return discountAmount;
            }
            set
            {
                discountAmount = value;
            }
        }

        public decimal finalFee;
        public decimal FinalFee
        {
            get
            {
                return finalFee;
            }
            set
            {
                finalFee = value;
            }
        }
        public string status;

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public Enrollment(int enrollmentId,string learnerId, string courseCode, DateTime enrollmentDate, decimal baseFee, decimal discountAmount, decimal finalFee, string status)
        {
            this.enrollmentId=enrollmentId;
            this.learnerId = learnerId;
            this.courseCode = courseCode;
            this.enrollmentDate = enrollmentDate;
            this.baseFee = baseFee;
            this.discountAmount = discountAmount;
            this.finalFee = finalFee;
            this.status = status;
        }
        public Enrollment()
        {

        }

        public void AddEnrolledCourse(List<Enrollment> enrolledStudents,Dictionary<string,Learner> learnerList,List<Course> courseList)
        {
            string learnerId = "";
            int count = 1;
            while (string.IsNullOrEmpty(learnerId) || !(learnerList.ContainsKey(learnerId)))
            {
                if (count > 1)
                {
                    if (string.IsNullOrEmpty(learnerId))
                    {
                        Console.WriteLine("Invalid LearnerId");
                    }
                    else if (!learnerList.ContainsKey(learnerId) != true)
                    {
                        Console.WriteLine("Learner Id doesn't exit.!!");
                    }

                }
                Console.WriteLine("Enter the Learner Id : ");
                learnerId = Console.ReadLine();
                count++;
            }

            Console.WriteLine("=======================");
            Console.WriteLine("Available Course List");
            Console.WriteLine("=======================");

            Course courseObj = new Course();
            courseObj.displayAllCourses(courseList);
            string courseCode = "";

            bool isVaildCourseCode = false;
            count = 1;
            while (string.IsNullOrEmpty(courseCode) || !isVaildCourseCode)
            {
                if (count > 1)
                {
                    if (string.IsNullOrEmpty(courseCode))
                    {
                        Console.WriteLine("Invalid Course COde!!");
                    }
                    else
                    {
                        for (int i = 0; i < courseList.Count; i++)
                        {
                            if (courseList[i].courseCode == courseCode)
                            {
                                isVaildCourseCode = true;
                            }
                        }
                        if (!isVaildCourseCode)
                        {
                            Console.WriteLine("Course Code doesn't exit!!");
                        }
                    }

                }
                Console.WriteLine("Enter the Course Code : ");
                courseCode = Console.ReadLine();
                count++;
            }
            Console.WriteLine("Enter the Enrollment Date (dd-mm-yyyy): ");
            DateTime enrollmentDate = DateTime.Parse(Console.ReadLine());


            int remainingSeats;

            for (int i = 0; i < enrolledStudents.Count; i++)
            {
                if (enrolledStudents[i].learnerId == learnerId && enrolledStudents[i].courseCode == courseCode)
                {
                    if (enrolledStudents[i].status == "Active")
                    {
                        Console.WriteLine($"{learnerId}  Learner Id is already Active for this Course");
                    }
                    else if (enrolledStudents[i].status == "Cancelled")
                    {
                        Console.WriteLine($"Course is already cancelled by this id");
                    }
                    else if (enrolledStudents[i].status == "Complete")
                    {
                        Console.WriteLine($"Course is already Completed by this Course");
                    }
                }
                else

                {
                    enrollmentId++;
                    remainingSeats = 0;
                    for (int j = 0; j < courseList.Count; j++)
                    {
                        if (courseList[j].courseCode == courseCode)
                        {
                            remainingSeats = courseList[j].totalSeats - courseList[j].entrolledCount;
                        }
                    }

                    if (remainingSeats <= 0)
                    {
                        Console.WriteLine("Seats are Full!!");
                    }
                    else
                    {
                        enrolledStudents.Add(new Enrollment(enrollmentId, learnerId, courseCode, enrollmentDate, courseList[i].fee, 10, courseList[i].fee - (courseList[i].fee / 10), "Active"));
                        Console.WriteLine("Course is sucessfully enrolled!!");
                    }
                }

            }

        }

        public void DisplayEnrolledRecords(List<Enrollment> enrolledStudents)
        {
            Console.WriteLine("===============");
            Console.WriteLine("Enrolled List");
            Console.WriteLine("===============");


            for (int i = 0; i < enrolledStudents.Count; i++)
            {
                Console.WriteLine($"Learner Id : {enrolledStudents[i].learnerId}");
                Console.WriteLine($"Course Code : {enrolledStudents[i].courseCode}");
                Console.WriteLine($"Enrollment Date : {enrolledStudents[i].enrollmentDate}");
                Console.WriteLine($"Base Fee : {enrolledStudents[i].baseFee}");
                Console.WriteLine($"Discount Amount : {enrolledStudents[i].discountAmount}");
                Console.WriteLine($"Total Fee : {enrolledStudents[i].finalFee}");
            }
        }


        public void RemoveStudentFromEnrollment(List<Enrollment> enrolledStudents, List<Enrollment> removeEnrolledStudents)
        {
            Console.WriteLine("=============================");
            Console.WriteLine("Remove From Enrollment List");
            Console.WriteLine("=============================");

            Console.WriteLine("Enter the Learner ID : ");
            string learnerId = Console.ReadLine();

            Console.WriteLine("Enter the Course Code : ");
            string courseCode = Console.ReadLine();

            bool isStudentPresent = false;

            for (int i = 0; i < enrolledStudents.Count; i++)
            {
                if (enrolledStudents[i].learnerId == learnerId && enrolledStudents[i].courseCode == courseCode && 
                    !(enrolledStudents[i].status=="Cancelled") && !(enrolledStudents[i].status=="Completed"))
                {
                    enrolledStudents[i].status = "Cancelled";
                    removeEnrolledStudents.Add(enrolledStudents[i]);
                    enrolledStudents.Remove(enrolledStudents[i]);
                    Console.WriteLine("Removed Successfully from the Enrolled List");
                    isStudentPresent = true;
                    break;
                }
                else if (enrolledStudents[i].learnerId == learnerId && enrolledStudents[i].courseCode == courseCode &&
                    enrolledStudents[i].status == "Cancelled" )
                {
                    Console.WriteLine("You already Cancelled this Course!!");
                    break;
                }
                else if (enrolledStudents[i].learnerId == learnerId && enrolledStudents[i].courseCode == courseCode &&
                    enrolledStudents[i].status == "Completed")
                {
                    Console.WriteLine("You already Completed this Course!!");
                    break;
                }
            }
            if (!isStudentPresent)
            {
                Console.WriteLine($"{learnerId} is not enrolled for this Course!!");
            }
            
        }
    }

}





