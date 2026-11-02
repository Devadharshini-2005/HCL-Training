/*26.Count all students.

Select Count(StudentId) from Students; */

/*27.Count students who have an email value.

Select Count(Email) from Students; */

/*28.Count students who have a department assigned.

Select Count(DepartmentId) from Students where DepartmentId is not null; */

/*29.Find the youngest student age.

Select Min(Age) from Students; */

/*30.Find the oldest student age.

Select Max(Age) from Students; */

/*31.Calculate the average student age with two decimal places.

Select Avg(Cast(Age as decimal(5,2))) as Average_Age from Students ; */

/*32.Find the total number of credits across all courses.

Select Count(Credits) from Courses; */

/*33.Find the average course fee.

Select Avg(Fee) from Courses;*/

/*34.Find the lowest and highest course fees.

Select Min(Fee) as Lowest_Fee , Max(Fee) as Highest_Fee from Courses; */
 
 /*35.Count distinct student ages.

 Select Count(distinct Age) as distinct_Age_Count from Students; */

 /*36.Count distinct non-NULL department IDs represented in Students.

 Select Count(DepartmentId) as Distinct_Department_Count from Students where DepartmentId is not null;*/

 /*37.Count all enrollment records.

 Select Count(*) from Enrollments; */

 /*38.Count completed enrollments.

 Select Count(EnrollmentStatus) from Enrollments where EnrollmentStatus='Completed'; */

 /*39.Calculate the total amount of all payments.

 Select Sum(Amount) from Payments; */

 /*40.Calculate the total amount of payments with status Paid.

 Select Sum(Amount) from Payments where PaymentStatus = 'Paid'; */

 /*41.Find the average marks obtained across non-NULL assessment results.

 Select Avg(MarksObtained) as Avg_Mark from AssessmentResults where MarksObtained is not null; */


 /*42.Find the highest mark obtained.

 Select Max(MarksObtained) as Highest_Mark from AssessmentResults; */

 /*43.Find the lowest non-NULL mark obtained.

 Select Min(MarksObtained) as Lowest_Mark from AssessmentResults where MarksObtained is not null; */

 /*44.Count assessment results with no marks.

 Select Count(*) from AssessmentResults; */

 /*45.Count submitted results.

 Select Count(SubmissionStatus) from AssessmentResults where SubmissionStatus='Submitted'; */

 /*46.Calculate the average marks for submitted results only.

 Select Avg(MarksObtained) as Avg_Mark from AssessmentResults where SubmissionStatus='Submitted';  */

 /*47.Find the total number of assessments.

 Select Count(*) from Assessments;*/

 /*48.Find the total maximum marks across all assessments.

 Select Count(MaxMarks) from Assessments; */

 /*49.Find the most recent payment date.

 Select Max(PaymentDate) from Payments; */

 /*50.Find the total number of active courses.

 Select Count(IsActive) from Courses where IsActive=1; */