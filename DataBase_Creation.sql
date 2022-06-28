--- створення бази даних
	CREATE DATABASE IT_SCHOOL_DB_ADO_EXAMPLE_00;
GO
	USE IT_SCHOOL_DB_ADO_EXAMPLE_00;
GO
--- створення таблиць
CREATE TABLE [dbo].[Authors]
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	FirstName VARCHAR(100) NOT NULL,
	LastName VARCHAR(100) NOT NULL
)
GO
CREATE TABLE [dbo].[Books]
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	AuthorId INT NOT NULL,
	FOREIGN KEY (AuthorId) REFERENCES AUTHORS (Id),
	Title VARCHAR(100) NOT NULL,
	PRICE INT,
	PAGES INT
)
GO
--- створення збережуваної процедури
CREATE PROCEDURE getBooksNumber
	@AuthorId int,
	@BookCount int OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT @BookCount = count(b.id)
	FROM Books b, Authors a
	WHERE b.Authorid = a.id AND
	a.id = @AuthorId;
END;