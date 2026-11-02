/*51.Count students in each department ID, including students with NULL DepartmentId as a group.

Select Count(*) from Students Group by DepartmentId;*/

/*52.Find the average age for each department ID.

Select Avg(Age) as Avg_AgeByDepartmentId from Students Group by DepartmentId; */

/*53.Find the youngest and oldest student age per department.

Select Min(Age) as Youngest_Age, Max(Age) as Oldest_Age from Students Group by DepartmentId; */

/*54.Show department IDs that have more than 10 students.

Select DepartmentId from Students Group by DepartmentId Having Count(DepartmentId) > 10;*/

/*55.Count students by active status.

Select Count(*) from Students where IsActive=1; */

/*56.Count enrollments by EnrollmentStatus.

Select Count(*) from Enrollments Group by EnrollmentStatus; */

/*57.Find courses with more than 15 enrollments.

Select CourseId,Count(*) from Courses group by CourseId having Count(*) > 5;
*/

/*58.Find the average course fee per category.

Select Avg(Fee) from Courses group by Category;*/

/*59.Count courses per category.

Select Count(Category) from Courses group by Category;*/

/*60.Show payment count and total amount by payment status.

Select Count(*), Sum(Amount) from Payments group by (PaymentStatus); */

/* 61.Show payment count and average amount by payment method.

Select Count(*), Avg(Amount) from Payments group by PaymentMethod; */

/**/

--62.	Find payment statuses with at least 20 payments.

Select PaymentStatus, count(*) AS PaymentCount
from Payments
Group by PaymentStatus
Having Count(*) >= 20;

--63.	Find students with more than 3 enrollments.

Select StudentId, count(*) AS EnrollmentCount
from Enrollments
Group by StudentId
Having count(*) > 3;

--64.	Find students with at least 2 assessment results.

Select StudentId, count(*) AS ResultCount
from AssessmentResults
Group by StudentId
having count(*) >= 2;

--65.	Find average marks per assessment.

Select AssessmentId, avg(MarksObtained) as AverageMark
from AssessmentResults
Group by AssessmentId;

--66.	Find assessments whose average marks are above 60.

Select AssessmentId, avg(MarksObtained) as AverageMark
from AssessmentResults
Group by AssessmentId
having avg(MarksObtained) > 60;

--67.	Count result records by SubmissionStatus.

Select SubmissionStatus, count(*) as ResultCount
from AssessmentResults
Group by SubmissionStatus;

--68.	Find the total fee per course category for active courses.

Select Category, sum(Fee) as TotalFee
From Courses
Where IsActive = 1
Group by Category;

--69.	Find departments with average student age above 22.

Select DepartmentId, avg(Age) as AverageAge
from Students
Where DepartmentId is not null
Group by DepartmentId
Having avg(Age) > 22;

--70.	Count students by DepartmentId and IsActive.

Select DepartmentId, IsActive, count(*) as StudentCount
From Students
Group by DepartmentId, IsActive;

--71.	Find courses with at least 2 assessments.

Select c.CourseId, c.CourseName, count(a.AssessmentId) as AssessmentCount
from Courses as c
Join Assessments as a on a.CourseId = c.CourseId
Group by c.CourseId, c.CourseName
Having count(a.AssessmentId) >= 2;

--72.	Find each student's total payment amount.

Select s.StudentId, s.Name, coalesce(sum(p.Amount), 0) as TotalPayment
From Students as s
Left join Payments as p on p.StudentId = s.StudentId
Group by s.StudentId, s.Name;

--73.	Find students whose total payments exceed 12000.

select s.StudentId, s.Name, sum(p.Amount) as TotalPayment
from Students as s
Join Payments as p on p.StudentId = s.StudentId
Group by s.StudentId, s.Name
having sum(p.Amount) > 12000;

--74.	Count results with non-NULL marks per student.

Select StudentId, count(MarksObtained) as MarkedResultCount
from AssessmentResults
group by StudentId;

--75.	Find payment months with more than 10 payments.

Select year(PaymentDate) as PaymentYear,
       Month(PaymentDate) as PaymentMonth,
       Count(*) as PaymentCount
From Payments
Where PaymentDate is not null
group by year(PaymentDate), month(PaymentDate)
having count(*) > 10
order by PaymentYear, PaymentMonth;


























