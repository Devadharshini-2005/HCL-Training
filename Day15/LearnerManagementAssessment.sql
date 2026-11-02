USE master;
GO

IF DB_ID(N'LearnerManagementAssessment') IS NOT NULL
BEGIN
    ALTER DATABASE LearnerManagementAssessment
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

    DROP DATABASE LearnerManagementAssessment;
END;
GO

CREATE DATABASE LearnerManagementAssessment;
GO

USE LearnerManagementAssessment;
GO

CREATE TABLE Departments
(
    DepartmentId INT IDENTITY(1,1) CONSTRAINT PK_Departments PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL CONSTRAINT UQ_Departments_Name UNIQUE,
    Location NVARCHAR(100) NOT NULL
);
GO
 
/* 2. Students */
CREATE TABLE Students
(
    StudentId INT IDENTITY(1,1) CONSTRAINT PK_Students PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL CONSTRAINT UQ_Students_Email UNIQUE,
    Age INT NOT NULL CONSTRAINT CK_Students_Age CHECK (Age BETWEEN 17 AND 60),
    DepartmentId INT NULL,
    AdmissionDate DATE NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Students_IsActive DEFAULT (1),
    CONSTRAINT FK_Students_Departments FOREIGN KEY (DepartmentId)
        REFERENCES Departments(DepartmentId)
);
GO
 
/* 3. Courses */
CREATE TABLE Courses
(
    CourseId INT IDENTITY(1,1) CONSTRAINT PK_Courses PRIMARY KEY,
    CourseName NVARCHAR(120) NOT NULL CONSTRAINT UQ_Courses_Name UNIQUE,
    Category NVARCHAR(60) NOT NULL,
    Credits INT NOT NULL CONSTRAINT CK_Courses_Credits CHECK (Credits BETWEEN 1 AND 6),
    Fee DECIMAL(10,2) NOT NULL CONSTRAINT CK_Courses_Fee CHECK (Fee >= 0),
    IsActive BIT NOT NULL CONSTRAINT DF_Courses_IsActive DEFAULT (1)
);
GO
 
/* 4. Enrollments: many-to-many relationship between Students and Courses */
CREATE TABLE Enrollments
(
    EnrollmentId INT IDENTITY(1,1) CONSTRAINT PK_Enrollments PRIMARY KEY,
    StudentId INT NOT NULL,
    CourseId INT NOT NULL,
    EnrolledAt DATE NOT NULL,
    EnrollmentStatus NVARCHAR(20) NOT NULL,
    CONSTRAINT CK_Enrollments_Status CHECK (EnrollmentStatus IN ('Active','Completed','Withdrawn')),
    CONSTRAINT UQ_Enrollments_Student_Course UNIQUE (StudentId, CourseId),
    CONSTRAINT FK_Enrollments_Students FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    CONSTRAINT FK_Enrollments_Courses FOREIGN KEY (CourseId) REFERENCES Courses(CourseId)
);
GO
 
/* 5. Assessments */
CREATE TABLE Assessments
(
    AssessmentId INT IDENTITY(1,1) CONSTRAINT PK_Assessments PRIMARY KEY,
    CourseId INT NOT NULL,
    AssessmentName NVARCHAR(120) NOT NULL,
    AssessmentDate DATE NOT NULL,
    MaxMarks INT NOT NULL CONSTRAINT CK_Assessments_MaxMarks CHECK (MaxMarks > 0),
    CONSTRAINT FK_Assessments_Courses FOREIGN KEY (CourseId) REFERENCES Courses(CourseId)
);
GO
 
/* 6. AssessmentResults */
CREATE TABLE AssessmentResults
(
    ResultId INT IDENTITY(1,1) CONSTRAINT PK_AssessmentResults PRIMARY KEY,
    StudentId INT NOT NULL,
    AssessmentId INT NOT NULL,
    MarksObtained DECIMAL(5,2) NULL,
    SubmissionStatus NVARCHAR(20) NOT NULL,
    CONSTRAINT CK_Results_Marks CHECK (MarksObtained IS NULL OR MarksObtained BETWEEN 0 AND 100),
    CONSTRAINT CK_Results_Status CHECK (SubmissionStatus IN ('Submitted','Pending','Absent')),
    CONSTRAINT UQ_Results_Student_Assessment UNIQUE (StudentId, AssessmentId),
    CONSTRAINT FK_Results_Students FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    CONSTRAINT FK_Results_Assessments FOREIGN KEY (AssessmentId) REFERENCES Assessments(AssessmentId)
);
GO
 
/* 7. Payments */
CREATE TABLE Payments
(
    PaymentId INT IDENTITY(1,1) CONSTRAINT PK_Payments PRIMARY KEY,
    StudentId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL CONSTRAINT CK_Payments_Amount CHECK (Amount >= 0),
    PaymentDate DATE NOT NULL,
    PaymentStatus NVARCHAR(20) NOT NULL,
    PaymentMethod NVARCHAR(30) NOT NULL,
    CONSTRAINT CK_Payments_Status CHECK (PaymentStatus IN ('Paid','Pending','Failed','Refunded')),
    CONSTRAINT FK_Payments_Students FOREIGN KEY (StudentId) REFERENCES Students(StudentId)
);
GO
 
/* INSERT DATA */
 
/* Departments: IDs 1-8 */
INSERT INTO Departments (DepartmentName, Location)
VALUES
(N'Computer Science', N'Block A'),
(N'Information Technology', N'Block B'),
(N'Electronics', N'Block C'),
(N'Mechanical', N'Block D'),
(N'Civil', N'Block E'),
(N'Data Science', N'Block F'),
(N'Cybersecurity', N'Block G'),
(N'Architecture', N'Block H');
GO
 
/* Courses: IDs 1-15 */
INSERT INTO Courses (CourseName, Category, Credits, Fee, IsActive)
VALUES
(N'C# Programming', N'Programming', 4, 4500, 1),
(N'SQL Server Fundamentals', N'Database', 3, 3000, 1),
(N'.NET Backend Development', N'Backend', 5, 6500, 1),
(N'Entity Framework Core', N'Backend', 3, 4000, 1),
(N'Java Programming', N'Programming', 4, 4500, 1),
(N'Python Programming', N'Programming', 4, 4200, 1),
(N'Web Development', N'Web', 4, 5000, 1),
(N'Data Structures', N'Computer Science', 5, 5500, 1),
(N'Data Analytics', N'Data Science', 4, 6000, 1),
(N'Machine Learning', N'Data Science', 5, 7500, 1),
(N'Cloud Fundamentals', N'Cloud', 3, 3500, 1),
(N'Cybersecurity Basics', N'Security', 3, 3800, 1),
(N'Legacy Programming', N'Programming', 2, 2000, 0),
(N'Power BI', N'Data Science', 2, 2800, 1),
(N'API Testing', N'Testing', 2, 2500, 1);
GO
 
/* Generate 100 students deterministically. Some students have no department.
   Names/emails remain unique. */
;WITH N AS
(
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM N WHERE n < 100
)
INSERT INTO Students (Name, Email, Age, DepartmentId, AdmissionDate, IsActive)
SELECT
    CASE (n % 12)
        WHEN 0 THEN N'Aarav'
        WHEN 1 THEN N'Ananya'
        WHEN 2 THEN N'Kavin'
        WHEN 3 THEN N'Meera'
        WHEN 4 THEN N'Rahul'
        WHEN 5 THEN N'Priya'
        WHEN 6 THEN N'Vikram'
        WHEN 7 THEN N'Nila'
        WHEN 8 THEN N'Arjun'
        WHEN 9 THEN N'Divya'
        WHEN 10 THEN N'Farhan'
        ELSE N'Sanjay'
    END + N' ' + CONVERT(NVARCHAR(10), n) AS Name,
    N'student' + RIGHT(N'000' + CONVERT(NVARCHAR(3), n), 3) + N'@example.com',
    17 + (n % 19),
    CASE WHEN n % 17 = 0 THEN NULL ELSE 1 + (n % 7) END,
    DATEADD(DAY, -(n * 11) % 1400, CONVERT(DATE, '2026-10-01')),
    CASE WHEN n % 13 = 0 THEN 0 ELSE 1 END
FROM N
OPTION (MAXRECURSION 100);
GO
 
/* Generate 250 enrollments, at most one per student/course pair.
   Deterministic sequence; leaves some courses with no enrollments. */
;WITH N AS
(
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM N WHERE n < 250
)
INSERT INTO Enrollments (StudentId, CourseId, EnrolledAt, EnrollmentStatus)
SELECT
    ((n - 1) % 100) + 1,
    ((n - 1) % 12) + 1,
    DATEADD(DAY, -(n * 7) % 1000, CONVERT(DATE, '2026-10-01')),
    CASE WHEN n % 10 = 0 THEN 'Withdrawn'
         WHEN n % 4 = 0 THEN 'Completed'
         ELSE 'Active' END
FROM N
WHERE NOT EXISTS
(
    SELECT 1
    FROM Enrollments e
    WHERE e.StudentId = ((N.n - 1) % 100) + 1
      AND e.CourseId = ((N.n - 1) % 12) + 1
)
OPTION (MAXRECURSION 250);
GO
 
/* 30 assessments across the first 10 courses */
;WITH N AS
(
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM N WHERE n < 30
)
INSERT INTO Assessments (CourseId, AssessmentName, AssessmentDate, MaxMarks)
SELECT
    ((n - 1) % 10) + 1,
    N'Assessment ' + CONVERT(NVARCHAR(10), n),
    DATEADD(DAY, -(n * 13) % 800, CONVERT(DATE, '2026-10-01')),
    CASE WHEN n % 3 = 0 THEN 50 ELSE 100 END
FROM N
OPTION (MAXRECURSION 30);
GO
 
/* Generate 500 unique student-assessment result rows.
   There are 100 students and 30 assessments; a deterministic subset is selected.
   Marks are NULL for pending/absent results. */
;WITH S AS
(
    SELECT 1 AS n
    UNION ALL SELECT n + 1 FROM S WHERE n < 100
),
A AS
(
    SELECT 1 AS n
    UNION ALL SELECT n + 1 FROM A WHERE n < 30
)
INSERT INTO AssessmentResults (StudentId, AssessmentId, MarksObtained, SubmissionStatus)
SELECT
    S.n,
    A.n,
    CASE WHEN (S.n + A.n) % 11 = 0 OR (S.n + A.n) % 13 = 0 THEN NULL
         ELSE CONVERT(DECIMAL(5,2), (S.n * 17 + A.n * 9) % 101) END,
    CASE WHEN (S.n + A.n) % 11 = 0 THEN 'Pending'
         WHEN (S.n + A.n) % 13 = 0 THEN 'Absent'
         ELSE 'Submitted' END
FROM S
CROSS JOIN A
WHERE (S.n + A.n * 7) % 6 = 0
OPTION (MAXRECURSION 100);
GO
 
/* Generate 200 payments */
;WITH N AS
(
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM N WHERE n < 200
)
INSERT INTO Payments (StudentId, Amount, PaymentDate, PaymentStatus, PaymentMethod)
SELECT
    ((n - 1) % 100) + 1,
    CONVERT(DECIMAL(10,2), 1000 + ((n * 347) % 9000)),
    DATEADD(DAY, -(n * 9) % 1200, CONVERT(DATE, '2026-10-01')),
    CASE WHEN n % 10 = 0 THEN 'Failed'
         WHEN n % 7 = 0 THEN 'Pending'
         WHEN n % 17 = 0 THEN 'Refunded'
         ELSE 'Paid' END,
    CASE WHEN n % 3 = 0 THEN 'UPI'
         WHEN n % 3 = 1 THEN 'Card'
         ELSE 'Bank Transfer' END
FROM N
OPTION (MAXRECURSION 200);
GO
 
/* Verification counts */
SELECT 'Departments' AS TableName, COUNT(*) AS [RowCount] FROM Departments
UNION ALL SELECT 'Students', COUNT(*) FROM Students
UNION ALL SELECT 'Courses', COUNT(*) FROM Courses
UNION ALL SELECT 'Enrollments', COUNT(*) FROM Enrollments
UNION ALL SELECT 'Assessments', COUNT(*) FROM Assessments
UNION ALL SELECT 'AssessmentResults', COUNT(*) FROM AssessmentResults
UNION ALL SELECT 'Payments', COUNT(*) FROM Payments;
GO









