--Scaffold-DbContext -Connection "Server=(localdb)\MSSQLLocalDB;Database=VueCoreBase;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data\VueCoreBase -Schemas VueCoreBase -Force

CREATE TABLE VueCoreBase.Logging(
	LogId uniqueidentifier default NEWID() NOT NULL PRIMARY KEY,
	Created DATETIME2 DEFAULT GETUTCDate() NOT NULL,
	RequestMethod NVARCHAR(50) NOT NULL,
	RequestPath NVARCHAR(max) NOT NULL,
	ResponseStatusCode INTEGER NOT NULL
);