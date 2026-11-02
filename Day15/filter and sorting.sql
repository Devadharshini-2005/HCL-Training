
/*1.Display the StudentId, Name, Email, and Age of all students.

Select StudentId,Name,Email,Age from Students; */

/*2.Find students who are exactly 21 years old.

Select * from Students where Age = 21; */

/*3.Find students younger than 20.

Select * from Students where Age < 20; */

/*4.	Find active students aged 20 or above.

Select * from Students where IsActive=1 and Age >= 20; */

/*5.Find students aged between 18 and 25 inclusive.

Select * from Students where Age between 18 and 25; */

/*6.Find students belonging to departments 1, 3, or 6.

Select * from Students where DepartmentId = 1 or DepartmentId=3 or DepartmentId=6; */

/*7.Find students who have not been assigned a department.

Select * from Students where DepartmentId is null; */

/*8.Find students who have been assigned a department.

Select * from Students where DepartmentId is not null; */

/*9.Find names that begin with A.

Select Name from Students where Name like 'A%'; */

/*10.Find names that end with the digit 5.

Select Name from Students where Name like '%5';  */

/*11.Find names containing the text 'ar' (use case-insensitive matching where supported by the database collation).

Select Name from Students where Name like '%ar%'; */

/*12.Find names whose second character is 'a'.

Select Name from Students where Name like '_a%'; */

/*13.Find names that contain exactly one character before ' ' and then any text.

Select Name From Students where Name like '[a-z] %'; */

/*14.Find emails ending in example.com.

Select Email from Students where Email like '%example.com'; */

/*15.Find students whose age is not 20.

Select * from Students where Age != 20; */

/*16.Find students aged 18–22 who are active.

Select * from Students where Age between 18 and 22 and IsActive = 1;*/

/*17.Find students who are inactive or are not assigned to a department.

Select * from Students where IsActive = 0 and DepartmentId is not null;*/

/*18.Sort students from youngest to oldest.

Select * from Students order by Age Asc; */

/*19.Display the 10 oldest students.

Select top 10 * from Students order by Age Desc ; */

/*20.Sort students by DepartmentId ascending, then Age descending.

Select * from Students order by DepartmentId Asc , Age Desc; */

/*21.Display each distinct age value.

Select distinct Age from Students; */

/*22.Display distinct non-NULL department IDs.

Select distinct DepartmentId from Students where DepartmentId is not null;*/

/*23.Find names containing a digit using a LIKE pattern.

Select Name From Students where Name Like '%[1-100]'; */

/*24.Find names containing any uppercase letter from A through M at the beginning.

Select Name from Students where Name Like '[A-M]%'; */

/*25.Find students admitted during calendar year 2025.

Select * from Students where AdmissionDate like '2025%'; */










