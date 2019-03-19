--Scaffold-DbContext -Connection "Server=(localdb)\mssqllocaldb; Database='VueCoreBase'; Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data\DatabaseLogger -Schemas DatabaseLogger -Force

CREATE TABLE DatabaseLogger.ErrorLog(
	[ID] uniqueidentifier default NEWID() NOT NULL PRIMARY KEY CLUSTERED ([ID] ASC),
	[MachineName] [nvarchar](200) NULL,
	[Logged] [datetime2] NOT NULL,
	[Level] [nvarchar](20) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Logger] [nvarchar](300) NULL,
	[Properties] [nvarchar](max) NULL,
	[Callsite] [nvarchar](300) NULL,
	[Exception] [nvarchar](max) NULL
  );

  CREATE TABLE DatabaseLogger.CalDavLog(
	[ID] uniqueidentifier default NEWID() NOT NULL PRIMARY KEY CLUSTERED ([ID] ASC),
	[Start] [datetime2] NOT NULL,
	[Stop] [datetime2] NOT NULL,
	[UserAgent] [nvarchar](256) NOT NULL,
	[Method] [nvarchar](50) NOT NULL,
	[Path] [nvarchar](256) NOT NULL,
	[Request] [nvarchar](max) NOT NULL,
	[Response] [nvarchar](max) NOT NULL,
	[StatusCode] INTEGER NULL,
	[ResponseContentType] [nvarchar](256) NULL
  );

  CREATE TABLE DatabaseLogger.RequestLog(
	[ID] uniqueidentifier default NEWID() NOT NULL PRIMARY KEY CLUSTERED ([ID] ASC),
	[Start] [datetime2] NOT NULL,
	[Stop] [datetime2] NOT NULL,
	[UserAgent] [nvarchar](256) NOT NULL,
	[Method] [nvarchar](50) NOT NULL,
	[Path] [nvarchar](256) NOT NULL,
	[StatusCode] INTEGER NULL,
	[UserName] [nvarchar](max) NULL
  );

  CREATE TABLE DatabaseLogger.UserLog(
	[ID] uniqueidentifier default NEWID() NOT NULL PRIMARY KEY CLUSTERED ([ID] ASC),
	[UserName] [nvarchar](256) NULL,
	[DateTimeStamp] [datetime2] NOT NULL,
	[TokenType] [nvarchar](10) NULL
  );