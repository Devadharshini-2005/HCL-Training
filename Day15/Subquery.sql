--126.	Find students older than the average student age.

SELECT * FROM Students
WHERE Age > (SELECT AVG(CAST(Age AS DECIMAL(10,2))) FROM Students);

--127.	Find students younger than the average age.

SELECT * FROM Students
WHERE Age < (SELECT AVG(CAST(Age AS DECIMAL(10,2))) FROM Students);

--128.	Find courses whose fee is greater than the average course fee.

SELECT * FROM Courses
WHERE Fee > (SELECT AVG(Fee) FROM Courses);

--129.	Find students whose DepartmentId belongs to Computer Science or Data Science.

SELECT * FROM Students
WHERE DepartmentId IN (
    SELECT DepartmentId FROM Departments
    WHERE DepartmentName IN ('Computer Science', 'Data Science')
);

--130.	Find students who have at least one enrollment using EXISTS.

SELECT s.*
FROM Students AS s
WHERE EXISTS (
    SELECT 1 FROM Enrollments AS e
    WHERE e.StudentId = s.StudentId
);

--131.	Find students who have no enrollment using NOT EXISTS.

SELECT s.*
FROM Students AS s
WHERE NOT EXISTS (
    SELECT 1 FROM Enrollments AS e
    WHERE e.StudentId = s.StudentId
);

--132.	Find courses that have no enrollments using NOT EXISTS.

SELECT c.*
FROM Courses AS c
WHERE NOT EXISTS (
    SELECT 1 FROM Enrollments AS e
    WHERE e.CourseId = c.CourseId
);

--133.	Find students whose age is above the average age in their own department.

SELECT s.*
FROM Students AS s
WHERE s.DepartmentId IS NOT NULL
  AND s.Age > (
      SELECT AVG(CAST(s2.Age AS DECIMAL(10,2)))
      FROM Students AS s2
      WHERE s2.DepartmentId = s.DepartmentId
  );

--134.	Find assessment results with marks greater than the average marks for that assessment.

SELECT ar.*
FROM AssessmentResults AS ar
WHERE ar.MarksObtained > (
    SELECT AVG(CAST(ar2.MarksObtained AS DECIMAL(10,2)))
    FROM AssessmentResults AS ar2
    WHERE ar2.AssessmentId = ar.AssessmentId
);

--135.	Find students who have at least one payment above 7000.

SELECT s.*
FROM Students AS s
WHERE EXISTS (
    SELECT 1 FROM Payments AS p
    WHERE p.StudentId = s.StudentId AND p.Amount > 7000
);

--136.	Find students whose total payments are above the average total payment per student among students who have payments.

WITH StudentTotals AS (
    SELECT StudentId, SUM(Amount) AS TotalAmount
    FROM Payments
    GROUP BY StudentId
)
SELECT s.StudentId, s.Name, st.TotalAmount
FROM StudentTotals AS st
JOIN Students AS s ON s.StudentId = st.StudentId
WHERE st.TotalAmount > (SELECT AVG(TotalAmount * 1.0) FROM StudentTotals);

--137.	Find departments whose student count is greater than the average student count per department among departments with students.

WITH DepartmentCounts AS (
    SELECT DepartmentId, COUNT(*) AS StudentCount
    FROM Students
    WHERE DepartmentId IS NOT NULL
    GROUP BY DepartmentId
)
SELECT d.DepartmentId, d.DepartmentName, dc.StudentCount
FROM DepartmentCounts AS dc
JOIN Departments AS d ON d.DepartmentId = dc.DepartmentId
WHERE dc.StudentCount > (SELECT AVG(StudentCount * 1.0) FROM DepartmentCounts);

--138.	Find assessments that have at least one pending result.

SELECT a.*
FROM Assessments AS a
WHERE EXISTS (
    SELECT 1 FROM AssessmentResults AS ar
    WHERE ar.AssessmentId = a.AssessmentId
      AND ar.SubmissionStatus = 'Pending'
);

--139.	Find students who have submitted at least one result with marks 90 or higher.

SELECT s.*
FROM Students AS s
WHERE EXISTS (
    SELECT 1 FROM AssessmentResults AS ar
    WHERE ar.StudentId = s.StudentId
      AND ar.SubmissionStatus = 'Submitted'
      AND ar.MarksObtained >= 90
);

--140.	Find students who have no payment records.

SELECT s.*
FROM Students AS s
WHERE NOT EXISTS (
    SELECT 1 FROM Payments AS p
    WHERE p.StudentId = s.StudentId
);

--141.	Find courses whose fee is above the fee of every inactive course.

SELECT c.*
FROM Courses AS c
WHERE c.Fee > (
    SELECT MAX(c2.Fee)
    FROM Courses AS c2
    WHERE c2.IsActive = 0
);

--142.	Find students whose age equals the maximum age.

SELECT *
FROM Students
WHERE Age = (SELECT MAX(Age) FROM Students);

--143.	Find students who enrolled in the course named SQL Server Fundamentals.

SELECT s.*
FROM Students AS s
WHERE EXISTS (
    SELECT 1
    FROM Enrollments AS e
    JOIN Courses AS c ON c.CourseId = e.CourseId
    WHERE e.StudentId = s.StudentId
      AND c.CourseName = 'SQL Server Fundamentals'
);

--144.	Find payments whose amount is greater than the average amount for their payment status.

SELECT p.*
FROM Payments AS p
WHERE p.Amount > (
    SELECT AVG(CAST(p2.Amount AS DECIMAL(12,2)))
    FROM Payments AS p2
    WHERE p2.PaymentStatus = p.PaymentStatus
);

--145.	Find students whose most recent payment is later than 2026-01-01.

SELECT s.StudentId, s.Name, MAX(p.PaymentDate) AS LatestPaymentDate
FROM Students AS s
JOIN Payments AS p ON p.StudentId = s.StudentId
GROUP BY s.StudentId, s.Name
HAVING MAX(p.PaymentDate) > '20260101';

--146.	Find students who have results for every assessment? Return students whose result count equals the total assessment count.

SELECT s.StudentId, s.Name
FROM Students AS s
WHERE (
    SELECT COUNT(DISTINCT ar.AssessmentId)
    FROM AssessmentResults AS ar
    WHERE ar.StudentId = s.StudentId
) = (SELECT COUNT(*) FROM Assessments);

--147.	Find courses with more assessments than the average assessment count per course.

WITH CourseCounts AS (
    SELECT c.CourseId, c.CourseName, COUNT(a.AssessmentId) AS AssessmentCount
    FROM Courses AS c
    LEFT JOIN Assessments AS a ON a.CourseId = c.CourseId
    GROUP BY c.CourseId, c.CourseName
)
SELECT *
FROM CourseCounts
WHERE AssessmentCount > (SELECT AVG(AssessmentCount * 1.0) FROM CourseCounts);

--148.	Find students whose average mark is higher than the overall average mark.

SELECT s.StudentId, s.Name, AVG(CAST(ar.MarksObtained AS DECIMAL(10,2))) AS AverageMark
FROM Students AS s
JOIN AssessmentResults AS ar ON ar.StudentId = s.StudentId
GROUP BY s.StudentId, s.Name
HAVING AVG(CAST(ar.MarksObtained AS DECIMAL(10,2))) >
       (SELECT AVG(CAST(MarksObtained AS DECIMAL(10,2))) FROM AssessmentResults);

--149.	Find departments with at least one student who has a pending payment.

SELECT d.*
FROM Departments AS d
WHERE EXISTS (
    SELECT 1
    FROM Students AS s
    JOIN Payments AS p ON p.StudentId = s.StudentId
    WHERE s.DepartmentId = d.DepartmentId
      AND p.PaymentStatus = 'Pending'
);

--150.	Find students who have enrolled in all courses in the Programming category.

SELECT s.*
FROM Students AS s
WHERE NOT EXISTS (
    SELECT 1
    FROM Courses AS c
    WHERE c.Category = 'Programming'
      AND NOT EXISTS (
          SELECT 1
          FROM Enrollments AS e
          WHERE e.StudentId = s.StudentId
            AND e.CourseId = c.CourseId
      )
);
