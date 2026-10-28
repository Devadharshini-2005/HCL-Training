using Microsoft.Data.SqlClient;
using System.Data;

namespace StudentCourseManagement;

internal class Program
{
    private const string ConnectionString =
        @"Server=.\SQLEXPRESS;Database=StudentCourseDB;Trusted_Connection=True;TrustServerCertificate=True;";

    static async Task Main()
    {
        Console.Title = "Student Course Management System";

        try
        {
            using SqlConnection con = new(ConnectionString);
            await con.OpenAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Database connection failed!");
            Console.WriteLine(ex.Message);
            Console.WriteLine("Check your SQL Server instance name.");
            return;
        }

        while (true)
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("   STUDENT COURSE MANAGEMENT SYSTEM");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Department Management");
            Console.WriteLine("2. Student Management");
            Console.WriteLine("3. Course Management");
            Console.WriteLine("4. Enrollment Management");
            Console.WriteLine("5. Reports");
            Console.WriteLine("6. View Audit Logs");
            Console.WriteLine("0. Exit");
            Console.WriteLine("========================================");

            int choice = ReadInt("Enter your choice: ");

            try
            {
                switch (choice)
                {
                    case 1:
                        await DepartmentsMenu();
                        break;

                    case 2:
                        await StudentsMenu();
                        break;

                    case 3:
                        await CoursesMenu();
                        break;

                    case 4:
                        await EnrollmentsMenu();
                        break;

                    case 5:
                        await ReportsMenu();
                        break;

                    case 6:
                        await Show(
                            "SELECT TOP 50 * FROM AuditLogs ORDER BY CreatedAt DESC");
                        break;

                    case 0:
                        Console.WriteLine("Thank you!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Pause();
            }
        }
    }

    // COMMON METHODS

    static SqlConnection GetConnection()
    {
        return new SqlConnection(ConnectionString);
    }

    static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out int value))
                return value;

            Console.WriteLine("Please enter a valid number.");
        }
    }

    static decimal ReadDecimal(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (decimal.TryParse(Console.ReadLine(), out decimal value)
                && value >= 0)
                return value;

            Console.WriteLine("Enter a valid non-negative amount.");
        }
    }

    static string ReadText(string message)
    {
        Console.Write(message);
        return (Console.ReadLine() ?? "").Trim();
    }

    static void Pause()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }

    static SqlParameter Param(
        string name, SqlDbType type, object value, int size = 0)
    {
        SqlParameter parameter = size > 0
            ? new SqlParameter(name, type, size)
            : new SqlParameter(name, type);

        parameter.Value = value;
        return parameter;
    }

    static async Task<int> Execute(
        string sql, params SqlParameter[] parameters)
    {
        using SqlConnection con = GetConnection();
        await con.OpenAsync();

        using SqlCommand cmd = new(sql, con);
        cmd.Parameters.AddRange(parameters);

        return await cmd.ExecuteNonQueryAsync();
    }

    static async Task Log(
        string entity, int id, string action, string details)
    {
        await Execute(
            """
            INSERT INTO AuditLogs
                (EntityName, EntityId, Action, Details)
            VALUES (@entity, @id, @action, @details)
            """,
            Param("@entity", SqlDbType.NVarChar, entity, 50),
            Param("@id", SqlDbType.Int, id),
            Param("@action", SqlDbType.NVarChar, action, 20),
            Param("@details", SqlDbType.NVarChar,
                string.IsNullOrWhiteSpace(details)
                    ? DBNull.Value : details, 500)
        );
    }

    static async Task Show(
        string sql, params SqlParameter[] parameters)
    {
        using SqlConnection con = GetConnection();
        await con.OpenAsync();

        using SqlCommand cmd = new(sql, con);
        cmd.Parameters.AddRange(parameters);

        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        for (int i = 0; i < reader.FieldCount; i++)
        {
            Console.Write(reader.GetName(i) + " | ");
        }

        Console.WriteLine();
        Console.WriteLine(new string('-', 100));

        bool found = false;

        while (await reader.ReadAsync())
        {
            found = true;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string value = reader.IsDBNull(i)
                    ? "NULL"
                    : Convert.ToString(reader.GetValue(i)) ?? "";

                Console.Write(value + " | ");
            }

            Console.WriteLine();
        }

        if (!found)
            Console.WriteLine("No records found.");

        Pause();
    }

    // DEPARTMENT MANAGEMENT

    static async Task DepartmentsMenu()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("DEPARTMENT MANAGEMENT");
            Console.WriteLine("1. Add Department");
            Console.WriteLine("2. List Departments");
            Console.WriteLine("3. Search Department");
            Console.WriteLine("4. Update Department");
            Console.WriteLine("5. Deactivate Department");
            Console.WriteLine("0. Back");

            int choice = ReadInt("Enter choice: ");

            if (choice == 0)
                return;

            if (choice == 1)
            {
                string name = ReadText("Department name: ");
                string location = ReadText("Location: ");

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Department name is required.");
                    Pause();
                    continue;
                }

                using SqlConnection con = GetConnection();
                await con.OpenAsync();

                using SqlCommand cmd = new(
                    """
                    INSERT INTO Departments
                        (DepartmentName, Location)
                    VALUES (@name, @location);

                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                    """, con);

                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = name;
                cmd.Parameters.Add("@location", SqlDbType.NVarChar, 100).Value =
                    string.IsNullOrWhiteSpace(location)
                        ? DBNull.Value : location;

                int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                await Log("Department", id, "INSERT", name);

                Console.WriteLine($"Department added. ID: {id}");
                Pause();
            }
            else if (choice == 2)
            {
                await Show("SELECT * FROM Departments ORDER BY DepartmentId");
            }
            else if (choice == 3)
            {
                int id = ReadInt("Department ID: ");

                await Show(
                    "SELECT * FROM Departments WHERE DepartmentId = @id",
                    Param("@id", SqlDbType.Int, id));
            }
            else if (choice == 4)
            {
                int id = ReadInt("Department ID: ");
                string name = ReadText("New department name: ");
                string location = ReadText("New location: ");

                int rows = await Execute(
                    """
                    UPDATE Departments
                    SET DepartmentName = @name, Location = @location
                    WHERE DepartmentId = @id AND IsActive = 1
                    """,
                    Param("@name", SqlDbType.NVarChar, name, 100),
                    Param("@location", SqlDbType.NVarChar,
                        string.IsNullOrWhiteSpace(location)
                            ? DBNull.Value : location, 100),
                    Param("@id", SqlDbType.Int, id));

                if (rows > 0)
                    await Log("Department", id, "UPDATE", name);

                Console.WriteLine(rows > 0 ? "Updated successfully." : "Department not found or inactive.");
                Pause();
            }
            else if (choice == 5)
            {
                int id = ReadInt("Department ID: ");

                int rows = await Execute(
                    "UPDATE Departments SET IsActive = 0 WHERE DepartmentId = @id",
                    Param("@id", SqlDbType.Int, id));

                if (rows > 0)
                    await Log("Department", id, "DEACTIVATE", "Deactivated");

                Console.WriteLine(rows > 0 ? "Department deactivated." : "Department not found.");
                Pause();
            }
        }
    }

    // STUDENT MANAGEMENT

    static async Task StudentsMenu()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("STUDENT MANAGEMENT");
            Console.WriteLine("1. Register Student");
            Console.WriteLine("2. List Students");
            Console.WriteLine("3. Search by ID");
            Console.WriteLine("4. Search by Name");
            Console.WriteLine("5. Update Student");
            Console.WriteLine("6. Deactivate Student");
            Console.WriteLine("0. Back");

            int choice = ReadInt("Enter choice: ");

            if (choice == 0)
                return;

            if (choice == 1)
            {
                string name = ReadText("Student name: ");
                string email = ReadText("Email: ");
                int age = ReadInt("Age (17-60): ");
                int departmentId = ReadInt("Department ID: ");

                if (string.IsNullOrWhiteSpace(name) ||
                    !email.Contains('@') ||
                    age < 17 || age > 60)
                {
                    Console.WriteLine("Invalid student details.");
                    Pause();
                    continue;
                }

                using SqlConnection con = GetConnection();
                await con.OpenAsync();

                using SqlCommand cmd = new(
                    """
                    INSERT INTO Students
                        (Name, Email, Age, DepartmentId)
                    VALUES (@name, @email, @age, @departmentId);

                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                    """, con);

                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = name;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 255).Value = email;
                cmd.Parameters.Add("@age", SqlDbType.Int).Value = age;
                cmd.Parameters.Add("@departmentId", SqlDbType.Int).Value = departmentId;

                int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                await Log("Student", id, "INSERT", name);

                Console.WriteLine($"Student registered. ID: {id}");
                Pause();
            }
            else if (choice == 2)
            {
                await Show(
                    """
                    SELECT s.StudentId, s.Name, s.Email, s.Age,
                           d.DepartmentName, s.AdmissionDate, s.IsActive
                    FROM Students s
                    INNER JOIN Departments d
                        ON s.DepartmentId = d.DepartmentId
                    ORDER BY s.StudentId
                    """);
            }
            else if (choice == 3)
            {
                int id = ReadInt("Student ID: ");

                await Show(
                    "SELECT * FROM Students WHERE StudentId = @id",
                    Param("@id", SqlDbType.Int, id));
            }
            else if (choice == 4)
            {
                string name = ReadText("Enter student name to search: ");

                await Show(
                    "SELECT * FROM Students WHERE Name LIKE @name",
                    Param("@name", SqlDbType.NVarChar, "%" + name + "%", 100));
            }
            else if (choice == 5)
            {
                int id = ReadInt("Student ID: ");
                string name = ReadText("New name: ");
                string email = ReadText("New email: ");
                int age = ReadInt("New age: ");
                int departmentId = ReadInt("Department ID: ");

                if (string.IsNullOrWhiteSpace(name) ||
                    !email.Contains('@') || age < 17 || age > 60)
                {
                    Console.WriteLine("Invalid student details.");
                    Pause();
                    continue;
                }

                int rows = await Execute(
                    """
                    UPDATE Students
                    SET Name = @name, Email = @email,
                        Age = @age, DepartmentId = @departmentId
                    WHERE StudentId = @id AND IsActive = 1
                    """,
                    Param("@name", SqlDbType.NVarChar, name, 100),
                    Param("@email", SqlDbType.NVarChar, email, 255),
                    Param("@age", SqlDbType.Int, age),
                    Param("@departmentId", SqlDbType.Int, departmentId),
                    Param("@id", SqlDbType.Int, id));

                if (rows > 0)
                    await Log("Student", id, "UPDATE", name);

                Console.WriteLine(rows > 0 ? "Student updated." : "Student not found or inactive.");
                Pause();
            }
            else if (choice == 6)
            {
                int id = ReadInt("Student ID: ");

                int rows = await Execute(
                    "UPDATE Students SET IsActive = 0 WHERE StudentId = @id",
                    Param("@id", SqlDbType.Int, id));

                if (rows > 0)
                    await Log("Student", id, "DEACTIVATE", "Student deactivated");

                Console.WriteLine(rows > 0 ? "Student deactivated." : "Student not found.");
                Pause();
            }
        }
    }

    // COURSE MANAGEMENT

    static async Task CoursesMenu()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("COURSE MANAGEMENT");
            Console.WriteLine("1. Add Course");
            Console.WriteLine("2. List Courses");
            Console.WriteLine("3. Search Course");
            Console.WriteLine("4. Update Course");
            Console.WriteLine("5. Deactivate Course");
            Console.WriteLine("0. Back");

            int choice = ReadInt("Enter choice: ");

            if (choice == 0)
                return;

            if (choice == 1)
            {
                string name = ReadText("Course name: ");
                int duration = ReadInt("Duration in months: ");
                decimal fee = ReadDecimal("Course fee: ");
                int departmentId = ReadInt("Department ID: ");

                if (string.IsNullOrWhiteSpace(name) || duration <= 0)
                {
                    Console.WriteLine("Invalid course details.");
                    Pause();
                    continue;
                }

                using SqlConnection con = GetConnection();
                await con.OpenAsync();

                using SqlCommand cmd = new(
                    """
                    INSERT INTO Courses
                        (CourseName, DurationMonths, Fee, DepartmentId)
                    VALUES (@name, @duration, @fee, @departmentId);

                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                    """, con);

                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = name;
                cmd.Parameters.Add("@duration", SqlDbType.Int).Value = duration;
                cmd.Parameters.Add("@fee", SqlDbType.Decimal).Value = fee;
                cmd.Parameters.Add("@departmentId", SqlDbType.Int).Value = departmentId;

                int id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                await Log("Course", id, "INSERT", name);

                Console.WriteLine($"Course added. ID: {id}");
                Pause();
            }
            else if (choice == 2)
            {
                await Show(
                    """
                    SELECT c.CourseId, c.CourseName, c.DurationMonths,
                           c.Fee, d.DepartmentName, c.IsActive
                    FROM Courses c
                    INNER JOIN Departments d
                        ON c.DepartmentId = d.DepartmentId
                    ORDER BY c.CourseId
                    """);
            }
            else if (choice == 3)
            {
                int id = ReadInt("Course ID: ");

                await Show(
                    "SELECT * FROM Courses WHERE CourseId = @id",
                    Param("@id", SqlDbType.Int, id));
            }
            else if (choice == 4)
            {
                int id = ReadInt("Course ID: ");
                string name = ReadText("New course name: ");
                int duration = ReadInt("New duration in months: ");
                decimal fee = ReadDecimal("New fee: ");
                int departmentId = ReadInt("Department ID: ");

                if (string.IsNullOrWhiteSpace(name) || duration <= 0)
                {
                    Console.WriteLine("Invalid course details.");
                    Pause();
                    continue;
                }

                int rows = await Execute(
                    """
                    UPDATE Courses
                    SET CourseName = @name, DurationMonths = @duration,
                        Fee = @fee, DepartmentId = @departmentId
                    WHERE CourseId = @id AND IsActive = 1
                    """,
                    Param("@name", SqlDbType.NVarChar, name, 100),
                    Param("@duration", SqlDbType.Int, duration),
                    Param("@fee", SqlDbType.Decimal, fee),
                    Param("@departmentId", SqlDbType.Int, departmentId),
                    Param("@id", SqlDbType.Int, id));

                if (rows > 0)
                    await Log("Course", id, "UPDATE", name);

                Console.WriteLine(rows > 0 ? "Course updated." : "Course not found or inactive.");
                Pause();
            }
            else if (choice == 5)
            {
                int id = ReadInt("Course ID: ");

                int rows = await Execute(
                    "UPDATE Courses SET IsActive = 0 WHERE CourseId = @id",
                    Param("@id", SqlDbType.Int, id));

                if (rows > 0)
                    await Log("Course", id, "DEACTIVATE", "Course deactivated");

                Console.WriteLine(rows > 0 ? "Course deactivated." : "Course not found.");
                Pause();
            }
        }
    }

    // ENROLLMENT MANAGEMENT

    static async Task EnrollmentsMenu()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("ENROLLMENT MANAGEMENT");
            Console.WriteLine("1. Enroll Student");
            Console.WriteLine("2. List Enrollments");
            Console.WriteLine("3. View Student Courses");
            Console.WriteLine("4. Complete Enrollment");
            Console.WriteLine("5. Cancel Enrollment");
            Console.WriteLine("0. Back");

            int choice = ReadInt("Enter choice: ");

            if (choice == 0)
                return;

            if (choice == 1)
            {
                int studentId = ReadInt("Student ID: ");
                int courseId = ReadInt("Course ID: ");

                using SqlConnection con = GetConnection();
                await con.OpenAsync();

                using SqlTransaction tx = con.BeginTransaction();

                try
                {
                    int enrollmentId;

                    using (SqlCommand cmd = new(
                        """
                        INSERT INTO Enrollments
                            (StudentId, CourseId, Status)
                        SELECT @studentId, @courseId, 'Enrolled'
                        WHERE EXISTS
                        (
                            SELECT 1 FROM Students
                            WHERE StudentId = @studentId AND IsActive = 1
                        )
                        AND EXISTS
                        (
                            SELECT 1 FROM Courses
                            WHERE CourseId = @courseId AND IsActive = 1
                        );

                        SELECT CAST(SCOPE_IDENTITY() AS INT);
                        """, con, tx))
                    {
                        cmd.Parameters.Add("@studentId", SqlDbType.Int).Value = studentId;
                        cmd.Parameters.Add("@courseId", SqlDbType.Int).Value = courseId;

                        object? result = await cmd.ExecuteScalarAsync();

                        if (result == null || result == DBNull.Value)
                        {
                            throw new InvalidOperationException(
                                "Student or course does not exist or is inactive.");
                        }

                        enrollmentId = Convert.ToInt32(result);
                    }

                    using (SqlCommand cmd = new(
                        """
                        INSERT INTO AuditLogs
                            (EntityName, EntityId, Action, Details)
                        VALUES
                            ('Enrollment', @id, 'INSERT', @details)
                        """, con, tx))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = enrollmentId;
                        cmd.Parameters.Add("@details", SqlDbType.NVarChar, 500).Value =
                            $"Student {studentId} enrolled in course {courseId}.";

                        await cmd.ExecuteNonQueryAsync();
                    }

                    tx.Commit();

                    Console.WriteLine($"Enrollment created. ID: {enrollmentId}");
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }

                Pause();
            }
            else if (choice == 2)
            {
                await Show(
                    """
                    SELECT e.EnrollmentId, s.Name AS StudentName,
                           c.CourseName, e.EnrolledAt, e.Status
                    FROM Enrollments e
                    INNER JOIN Students s ON s.StudentId = e.StudentId
                    INNER JOIN Courses c ON c.CourseId = e.CourseId
                    ORDER BY e.EnrollmentId DESC
                    """);
            }
            else if (choice == 3)
            {
                int studentId = ReadInt("Student ID: ");

                await Show(
                    """
                    SELECT s.StudentId, s.Name, c.CourseName,
                           c.Fee, e.EnrolledAt, e.Status
                    FROM Enrollments e
                    INNER JOIN Students s ON s.StudentId = e.StudentId
                    INNER JOIN Courses c ON c.CourseId = e.CourseId
                    WHERE s.StudentId = @id
                    """,
                    Param("@id", SqlDbType.Int, studentId));
            }
            else if (choice == 4 || choice == 5)
            {
                int id = ReadInt("Enrollment ID: ");

                string status = choice == 4 ? "Completed" : "Cancelled";

                int rows = await Execute(
                    """
                    UPDATE Enrollments
                    SET Status = @status
                    WHERE EnrollmentId = @id AND Status = 'Enrolled'
                    """,
                    Param("@status", SqlDbType.NVarChar, status, 20),
                    Param("@id", SqlDbType.Int, id));

                if (rows > 0)
                    await Log("Enrollment", id, "UPDATE", status);

                Console.WriteLine(
                    rows > 0
                        ? $"Enrollment {status.ToLower()} successfully."
                        : "Enrollment not found or not currently enrolled.");

                Pause();
            }
        }
    }

    // REPORTS

    static async Task ReportsMenu()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("REPORTS");
            Console.WriteLine("1. Student Count by Department");
            Console.WriteLine("2. Enrollment Count by Course");
            Console.WriteLine("3. Students Without Enrollments");
            Console.WriteLine("4. Courses Without Enrollments");
            Console.WriteLine("0. Back");

            int choice = ReadInt("Enter choice: ");

            if (choice == 0)
                return;

            switch (choice)
            {
                case 1:
                    await Show(
                        """
                        SELECT d.DepartmentName,
                               COUNT(s.StudentId) AS StudentCount
                        FROM Departments d
                        LEFT JOIN Students s
                            ON s.DepartmentId = d.DepartmentId
                            AND s.IsActive = 1
                        GROUP BY d.DepartmentName
                        ORDER BY d.DepartmentName
                        """);
                    break;

                case 2:
                    await Show(
                        """
                        SELECT c.CourseName,
                               COUNT(e.EnrollmentId) AS EnrollmentCount
                        FROM Courses c
                        LEFT JOIN Enrollments e
                            ON e.CourseId = c.CourseId
                            AND e.Status <> 'Cancelled'
                        GROUP BY c.CourseName
                        ORDER BY EnrollmentCount DESC
                        """);
                    break;

                case 3:
                    await Show(
                        """
                        SELECT s.StudentId, s.Name, s.Email
                        FROM Students s
                        WHERE NOT EXISTS
                        (
                            SELECT 1
                            FROM Enrollments e
                            WHERE e.StudentId = s.StudentId
                        )
                        """);
                    break;

                case 4:
                    await Show(
                        """
                        SELECT c.CourseId, c.CourseName
                        FROM Courses c
                        WHERE NOT EXISTS
                        (
                            SELECT 1
                            FROM Enrollments e
                            WHERE e.CourseId = c.CourseId
                              AND e.Status <> 'Cancelled'
                        )
                        """);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }
}