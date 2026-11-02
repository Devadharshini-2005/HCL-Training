/*76.List each student with their department name; only students assigned to a department.

Select s.StudentId,s.Name ,d.DepartmentName from Students s 
left join
Departments d
on s.DepartmentId = d.DepartmentId
where s.DepartmentId is not null; 

--77.List all students and their department name, including unassigned students.

Select s.StudentId,s.Name ,d.DepartmentName from Students s 
left join
Departments d
on s.DepartmentId = d.DepartmentId; 

--78.List all departments and any students in them, including departments with no students.

Select * from Departments d 
left join
Students s 
on d.DepartmentId = s.DepartmentId;

--79.Find departments with no students.

Select d.DepartmentId,d.DepartmentName from Departments d 
left join
Students s 
on d.DepartmentId = s.DepartmentId
where s.StudentId is null;  

--80.List each enrollment with student name and course name.

Select s.Name, c.CourseName from Enrollments e 
left join
Students s 
on e.StudentId = s.StudentId
left join 
Courses c
on c.CourseId = e.CourseId; 

--81.List all courses and enrollment counts, including courses with zero enrollments.


Select c.CourseName, Count(c.CourseName) as Enrollment_Count from Courses c 
join 
Enrollments e 
on c.CourseId = e.CourseId
group by c.CourseName; 

--82.Find courses with no enrollments.

Select c.CourseName from Courses c 
join 
Enrollments e 
on c.CourseId = e.CourseId
where e.CourseId is null; 

--83.List each student with assessment name and marks, where a result exists.

Select s.StudentId, s.Name,a.AssessmentName, ar.MarksObtained from Students s
left join
AssessmentResults ar
on s.StudentId = ar.StudentId
left join
Assessments a
on a.AssessmentId=ar.AssessmentId
where ar.MarksObtained is not null; 

--84.List all students and any assessment results, including students with no result.

Select s.StudentId, s.Name,a.AssessmentName, ar.MarksObtained from Students s
left join
AssessmentResults ar
on s.StudentId = ar.StudentId
left join
Assessments a
on a.AssessmentId=ar.AssessmentId; 

--85.List assessment names with course names.

Select a.AssessmentName,c.CourseName from Assessments a
left join
Courses c
on a.CourseId = c.CourseId; 

--86.List all courses and their assessments, including courses without assessments.

Select c.CourseId, c.CourseName, a.AssessmentName from Courses c
left join
Assessments a
on c.CourseId = a.CourseId; 

--87.List each payment with student name and department name if available.

Select p.*,s.Name,d.DepartmentName from Payments p
left join 
Students s
on p.StudentId = s.StudentId
left join
Departments d
on d.DepartmentId = s.DepartmentId; 

--88.Find students who have at least one payment with status Pending.

Select * from Payments p 
left join
Students s
on s.StudentId = p.StudentId
where p.PaymentStatus = 'Pending'; 

--89.Find students who have never made a payment.

Select * from Students s
left join
Payments p
on s.StudentId = p.StudentId
where p.PaymentId is null; 

--90.	Show student name, department, course, and enrollment status.

Select s.StudentId, s.Name, d.DepartmentName, c.CourseName, e.EnrollmentStatus
from Enrollments as e
Join Students as s on s.StudentId = e.StudentId
Join Courses as c on c.CourseId = e.CourseId
Left join Departments as d on d.DepartmentId = s.DepartmentId;

--91.	Count students per department, including departments with zero students.

SELECT d.DepartmentId, d.DepartmentName, COUNT(s.StudentId) AS StudentCount
FROM Departments AS d
LEFT JOIN Students AS s ON s.DepartmentId = d.DepartmentId
GROUP BY d.DepartmentId, d.DepartmentName;

--92.	Find assessment results where the student's department is Computer Science.

SELECT ar.*
FROM AssessmentResults AS ar
JOIN Students AS s ON s.StudentId = ar.StudentId
JOIN Departments AS d ON d.DepartmentId = s.DepartmentId
WHERE d.DepartmentName = 'Computer Science';

--93.	List payments made by students from Data Science.

SELECT p.*
FROM Payments AS p
JOIN Students AS s ON s.StudentId = p.StudentId
JOIN Departments AS d ON d.DepartmentId = s.DepartmentId
WHERE d.DepartmentName = 'Data Science';

--94.	List each course with its assessment count.

SELECT c.CourseId, c.CourseName, COUNT(a.AssessmentId) AS AssessmentCount
FROM Courses AS c
LEFT JOIN Assessments AS a ON a.CourseId = c.CourseId
GROUP BY c.CourseId, c.CourseName;

--95.	List each student and their number of assessment results.

SELECT s.StudentId, s.Name, COUNT(ar.AssessmentId) AS ResultCount
FROM Students AS s
LEFT JOIN AssessmentResults AS ar ON ar.StudentId = s.StudentId
GROUP BY s.StudentId, s.Name;

--96.	Find students enrolled in C# Programming.

SELECT DISTINCT s.StudentId, s.Name
FROM Students AS s
JOIN Enrollments AS e ON e.StudentId = s.StudentId
JOIN Courses AS c ON c.CourseId = e.CourseId
WHERE c.CourseName = 'C# Programming';

--97.	Show students who have results for assessments dated in 2026.

SELECT DISTINCT s.StudentId, s.Name
FROM Students AS s
JOIN AssessmentResults AS ar ON ar.StudentId = s.StudentId
JOIN Assessments AS a ON a.AssessmentId = ar.AssessmentId
WHERE a.AssessmentDate >= '20260101'
  AND a.AssessmentDate <  '20270101';

--98.	List all departments and students using RIGHT JOIN.

SELECT d.DepartmentId, d.DepartmentName, s.StudentId, s.Name
FROM Students AS s
RIGHT JOIN Departments AS d ON d.DepartmentId = s.DepartmentId;

--99.	List all courses and enrollments using FULL OUTER JOIN.

SELECT c.CourseId, c.CourseName, e.EnrollmentId, e.StudentId
FROM Courses AS c
FULL OUTER JOIN Enrollments AS e ON e.CourseId = c.CourseId;

--100.	Find each student's latest payment date.

SELECT s.StudentId, s.Name, MAX(p.PaymentDate) AS LatestPaymentDate
FROM Students AS s
LEFT JOIN Payments AS p ON p.StudentId = s.StudentId
GROUP BY s.StudentId, s.Name;
*/











