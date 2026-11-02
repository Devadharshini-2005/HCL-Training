--101.	Return page 1 of students with 10 students per page, ordered by StudentId.

SELECT * FROM Students
ORDER BY StudentId
OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;

--102.	Return page 2 of students with 10 students per page.

SELECT * FROM Students
ORDER BY StudentId
OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY;

--103.	Return page 5 of students with 10 students per page.

SELECT * FROM Students
ORDER BY StudentId
OFFSET 40 ROWS FETCH NEXT 10 ROWS ONLY;

--104.	Return page 3 of courses with 4 courses per page, ordered by CourseName.

SELECT * FROM Courses
ORDER BY CourseName, CourseId
OFFSET 8 ROWS FETCH NEXT 4 ROWS ONLY;

--105.	Return the first 15 payments by PaymentDate, newest first.

SELECT TOP (15) * FROM Payments
ORDER BY PaymentDate DESC, PaymentId DESC;

--106.	Return page 2 of payments with 20 rows per page, newest first.

SELECT * FROM Payments
ORDER BY PaymentDate DESC, PaymentId DESC
OFFSET 20 ROWS FETCH NEXT 20 ROWS ONLY;

--107.	Return page 4 of assessment results with 25 rows per page.

SELECT * FROM AssessmentResults
ORDER BY AssessmentId
OFFSET 75 ROWS FETCH NEXT 25 ROWS ONLY;

--108.	Return page 2 of enrollments with 12 rows per page, ordered by date.

SELECT * FROM Enrollments
ORDER BY EnrollmentId
OFFSET 12 ROWS FETCH NEXT 12 ROWS ONLY;

--109.	Return page 1 of students aged 20 or older, 8 rows per page.

SELECT * FROM Students
WHERE Age >= 20
ORDER BY StudentId
OFFSET 0 ROWS FETCH NEXT 8 ROWS ONLY;

--110.	Return page 2 of active students, 8 rows per page.

SELECT * FROM Students
WHERE IsActive = 1
ORDER BY StudentId
OFFSET 8 ROWS FETCH NEXT 8 ROWS ONLY;

--111.	Return page 1 of courses whose fee is at least 4000, 5 rows per page.

SELECT * FROM Courses
WHERE Fee >= 4000
ORDER BY CourseId
OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY;

--112.	Return page 2 of assessments ordered by AssessmentDate ascending, 6 per page.

SELECT * FROM Assessments
ORDER BY AssessmentDate ASC, AssessmentId
OFFSET 6 ROWS FETCH NEXT 6 ROWS ONLY;

--113.	Return page 3 of students sorted alphabetically by name, 10 per page.

SELECT * FROM Students
ORDER BY Name, StudentId
OFFSET 20 ROWS FETCH NEXT 10 ROWS ONLY;

--114.	Return page 1 of payments whose status is Pending, 10 per page.

SELECT * FROM Payments
WHERE PaymentStatus = 'Pending'
ORDER BY PaymentDate, PaymentId
OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;

--115.	Return page 2 of results with submitted status, 15 per page.

SELECT * FROM AssessmentResults
WHERE SubmissionStatus = 'Submitted'
ORDER BY AssessmentId
OFFSET 15 ROWS FETCH NEXT 15 ROWS ONLY;

--116.	Return page 1 of enrollments for CourseId 3, 10 per page.

SELECT * FROM Enrollments
WHERE CourseId = 3
ORDER BY EnrollmentId
OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;

--117.	Return page 2 of students whose names start with A, 7 per page.

SELECT * FROM Students
WHERE Name LIKE 'A%'
ORDER BY Name, StudentId
OFFSET 7 ROWS FETCH NEXT 7 ROWS ONLY;

--118.	Return page 1 of assessments with maximum marks 50, 5 per page.

SELECT * FROM Assessments
WHERE MaxMarks = 50
ORDER BY AssessmentId
OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY;

--119.	Return page 3 of students who have a department assigned, 9 per page.

SELECT * FROM Students
WHERE DepartmentId IS NOT NULL
ORDER BY StudentId
OFFSET 18 ROWS FETCH NEXT 9 ROWS ONLY;

--120.	Return page 1 of paid payments with amount over 5000, 10 per page.

SELECT * FROM Payments
WHERE PaymentStatus = 'Paid' AND Amount > 5000
ORDER BY PaymentDate DESC, PaymentId DESC
OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;

--121.	Return page 2 of course names containing 'Data', 3 per page.

SELECT * FROM Courses
WHERE CourseName LIKE '%Data%'
ORDER BY CourseName, CourseId
OFFSET 3 ROWS FETCH NEXT 3 ROWS ONLY;

--122.	Return page 1 of results with marks 80 or higher, 20 per page.

SELECT * FROM AssessmentResults
WHERE MarksObtained >= 80
ORDER BY AssessmentId
OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY;

--123.	Return page 1 of payments from calendar year 2025, 10 per page.

SELECT * FROM Payments
WHERE PaymentDate >= '20250101'
  AND PaymentDate <  '20260101'
ORDER BY PaymentDate, PaymentId
OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;

--124.	Write a parameterized stored procedure that returns a page of students.

/*CREATE OR ALTER PROCEDURE dbo.GetStudentsPage
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageNumber < 1 OR @PageSize < 1
    BEGIN
        THROW 50001, 'PageNumber and PageSize must be greater than zero.', 1;
    END;

    SELECT StudentId, Name, Email, Age, DepartmentId, IsActive
    FROM Students
    ORDER BY StudentId
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;*/

--125.	Calculate the number of pages required for all students when page size is 12.

SELECT (COUNT(*) + 11) / 12 AS PageCount
FROM Students;
