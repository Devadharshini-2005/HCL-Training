using System.Xml.Linq;
using WebAPIPractice.Models;

namespace WebAPIPractice.Services
{
    public class StudentService
    {
        private readonly List<Student> _students = new()
    {
        new Student
        {
            Id = 1,
            Name = "Arun",
            Email = "arun@example.com",
            Age = 21
        },
        new Student
        {
            Id = 2,
            Name = "Priya",
            Email = "priya@example.com",
            Age = 22
        }
    };

        private int _nextId = 3;

        // GET: Retrieve all students
        public List<Student> GetAllStudents()
        {
            return _students;
        }

        // GET: Retrieve student by ID
        public Student? GetStudentById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        // POST: Add a student
        public Student AddStudent(Student student)
        {
            student.Id = _nextId++;

            _students.Add(student);

            return student;
        }

        // PUT: Update a student
        public Student? UpdateStudent(int id, Student updatedStudent)
        {
            var existingStudent = GetStudentById(id);

            if (existingStudent == null)
                return null;

            existingStudent.Name = updatedStudent.Name;
            existingStudent.Email = updatedStudent.Email;
            existingStudent.Age = updatedStudent.Age;

            return existingStudent;
        }

        // DELETE: Delete a student
        public bool DeleteStudent(int id)
        {
            var student = GetStudentById(id);

            if (student == null)
                return false;

            _students.Remove(student);

            return true;
        }
    }
}
