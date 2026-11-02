using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagementSystem.Models
{
    internal class Course
    {
        public string courseCode;
        public string CourseCode { get; set; }

        public string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {

                courseCode = value.Trim();

            }
        }

        public decimal fee;

        public decimal Fee
        {
            get
            {
                return fee;
            }
            set
            {

                fee = value;

            }
        }

        public int durationDays;
        public int DurationDays
        {
            get
            {
                return durationDays;
            }
            set
            {
                durationDays = value;

            }
        }

        public int totalSeats;
        public int TotalSeats
        {
            get
            {
                return totalSeats;
            }
            set
            {
                totalSeats = value;

            }
        }

        public int entrolledCount;
        public int EntrolledCount
        {
            get
            {
                return entrolledCount;
            }
            set
            {
                entrolledCount = value;

            }
        }

        public Course(string courseCode, string title, decimal fee, int durationDays, int totalSeats, int entrolledCount)
        {
            this.courseCode = courseCode;
            this.title = title;
            this.fee = fee;
            this.durationDays = durationDays;
            this.totalSeats = totalSeats;
            this.entrolledCount = entrolledCount;
        }
        public Course()
        {

        }

        public void displayAllCourses(List<Course> course)
        {
            foreach (Course i in course)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Course List");
                Console.WriteLine("-----------------------------");
                Console.WriteLine($"Course Code : {i.courseCode}");
                Console.WriteLine($"Title : {i.title}");
                Console.WriteLine($"Fee : {i.fee}");
                Console.WriteLine($"Duration Days : {i.durationDays}");
                Console.WriteLine($"Total Seats : {i.totalSeats}");
                Console.WriteLine($"Entrolled Count : {i.entrolledCount}");
                Console.WriteLine("-----------------------------");
        
            }
        }

        public void SearchCourse(List<Course> course, string input)
        {

            for (int i = 0; i < course.Count; i++)
            {
                if (input.Trim().ToLower() == course[i].courseCode.ToLower() || input.Trim().ToLower() == course[i].title.ToLower())
                {
                    Console.WriteLine("Searched Course is ");
                    Console.WriteLine($"Course Code : {course[i].courseCode}");
                    Console.WriteLine($"Title : {course[i].title}");
                    Console.WriteLine($"Fee : {course[i].fee}");
                    Console.WriteLine($"Duration Dyas : {course[i].durationDays}");
                    Console.WriteLine($"Total Seats : {course[i].totalSeats}");
                    Console.WriteLine($"Entrolled Count : {course[i].entrolledCount}");
                    break;
                }
            }
        }

        public void SortByFee(List<Course> course)
        {
            Console.WriteLine("--------Sorted by Ascending Order---");

            for (int i = 0; i < course.Count - 1; i++)
            {
                for (int j = i; j < course.Count; j++)
                {
                    if (course[i].fee > course[j].fee)
                    {
                        (course[i], course[j]) = (course[j], course[i]);
                    }
                }
            }

            displayAllCourses(course);
        }

        public void AvailableSeatsInAllCourses(List<Course> course)
        {
            Console.WriteLine("------Available Seats-------");

            for (int i = 0; i < course.Count; i++)
            {
                Console.WriteLine($"Available Seats in {course[i].title} = {course[i].totalSeats - course[i].entrolledCount} ");
            }
        }

    }
}
