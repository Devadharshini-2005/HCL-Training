using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JSONDemo
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Course> CourseList { get; set; }

        public Student(int id, string name, List<Course> courseList)
        {
            Id = id;
            Name = name;
            CourseList = courseList;
        }
        public Student() { }
    }

    public class Course
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }

        public Course(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public Course() { }
    }
    class JSONPractice
    {
        public void Executer()
        {
            //{
            //    Student student = new Student(5, "Divya");
            //    string json1 = JsonSerializer.Serialize(student);
            //    Console.WriteLine(json1);
            //    Student? obj = JsonSerializer.Deserialize<Student>(json1);
            //    Console.WriteLine($"Student Id : { obj.Id}\nStudent Name : { obj.Name} ");

            Console.WriteLine("Serialization and Deserialization for list of students");
            List<Student> studentList = new List<Student>
            {
                new Student(1,"Gethz",new List<Course>
                {
                    new Course(101,"python",23.45m),
                    new Course(101,"python",23.45m),

                }),
                new Student(2,"Bavz",new List<Course>
                {
                    new Course(101,"python",23.45m),
                    new Course(101,"python",23.45m),

                }),
                new Student(3,"Devz",new List<Course>
                {
                    new Course(101,"python",23.45m),
                    new Course(101,"python",23.45m),

                })
            };
            string json2 = JsonSerializer.Serialize(studentList, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json2);
            Console.WriteLine(new string('-', 50));
            List<Student>? jsonList = JsonSerializer.Deserialize<List<Student>>(json2);
            if (jsonList != null)
            {
                foreach (var s in jsonList)
                {
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine($"Student Id : {s.Id}\nStudent Name : {s.Name}\nCourse List : ");
                    foreach (var c in s.CourseList)
                    {
                        Console.WriteLine($"Course Name : {c.Name}\nPrice : {c.Price}\nId : {c.Id}");
                    }

                }

            }

        }
    }
}
