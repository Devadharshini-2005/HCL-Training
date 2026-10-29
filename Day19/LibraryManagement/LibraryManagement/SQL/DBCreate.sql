USE LibraryManagementDB;
GO

SELECT DB_NAME() AS CurrentDatabase;
SELECT COUNT(*) AS BookCount FROM Books;