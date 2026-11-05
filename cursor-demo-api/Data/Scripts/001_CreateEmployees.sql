IF OBJECT_ID(N'dbo.Employees', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Employees
    (
        Id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Age INT NOT NULL,
        Salary DECIMAL(18, 2) NOT NULL,
        PhoneNumber NVARCHAR(30) NOT NULL
    );
END
GO
