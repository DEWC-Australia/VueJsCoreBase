--Scaffold-DbContext -Connection "Server=(localdb)\mssqllocaldb; Database='VueCoreBase'; Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data\DatabaseLogger -Schemas DatabaseLogger -Force


CREATE TABLE DatabaseLogger.DatabaseLog(
	[ID] uniqueidentifier default NEWID() NOT NULL PRIMARY KEY CLUSTERED ([ID] ASC),
	[MachineName] [nvarchar](200) NULL,
	[Logged] [datetime2] NOT NULL,
	[Level] [varchar](5) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Logger] [nvarchar](300) NULL,
	[Properties] [nvarchar](max) NULL,
	[Callsite] [nvarchar](300) NULL,
	[Exception] [nvarchar](max) NULL
  );