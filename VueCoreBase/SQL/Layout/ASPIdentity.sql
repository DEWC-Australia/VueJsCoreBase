--Scaffold-DbContext -Connection "Server=(localdb)\mssqllocaldb; Database='VueCoreBase'; Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data\ASPIdentity -Schemas dbo -Force

GO
/****** Object: Table [dbo].[__EFMigrationsHistory] Script Date: 30/12/2018 12:19:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
[MigrationId] [nvarchar](150) NOT NULL,
[ProductVersion] [nvarchar](32) NOT NULL,
CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object: Table [dbo].[AspNetRoleClaims] Script Date: 30/12/2018 12:19:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
[Id] [int] IDENTITY(1,1) NOT NULL,
[ClaimType] [nvarchar](max) NULL,
[ClaimValue] [nvarchar](max) NULL,
[RoleId] uniqueidentifier NOT NULL,
CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]



GO
/****** Object: Table [dbo].[AspNetRoles] Script Date: 30/12/2018 12:19:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
[Id] uniqueidentifier default NEWID() NOT NULL,
[ConcurrencyStamp] [nvarchar](max) NULL,
[Name] [nvarchar](256) NULL,
[NormalizedName] [nvarchar](256) NULL,
CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object: Table [dbo].[AspNetUserClaims] Script Date: 30/12/2018 12:19:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
[Id] [int] IDENTITY(1,1) NOT NULL,
[ClaimType] [nvarchar](max) NULL,
[ClaimValue] [nvarchar](max) NULL,
[UserId] uniqueidentifier NOT NULL,
CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object: Table [dbo].[AspNetUserLogins] Script Date: 30/12/2018 12:19:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
[LoginProvider] [nvarchar](450) NOT NULL,
[ProviderKey] [nvarchar](450) NOT NULL,
[ProviderDisplayName] [nvarchar](max) NULL,
[UserId] uniqueidentifier NOT NULL,
CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
[LoginProvider] ASC,
[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object: Table [dbo].[AspNetUserRoles] Script Date: 30/12/2018 12:19:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
[UserId] uniqueidentifier NOT NULL,
[RoleId] uniqueidentifier NOT NULL,
CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
[UserId] ASC,
[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object: Table [dbo].[AspNetUsers] Script Date: 30/12/2018 12:19:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
[Id] uniqueidentifier default NEWID() NOT NULL,
[AccessFailedCount] [int] NOT NULL,
[ConcurrencyStamp] [nvarchar](max) NULL,
[Email] [nvarchar](256) NULL,
[EmailConfirmed] [bit] NOT NULL,
[LockoutEnabled] [bit] NOT NULL,
[LockoutEnd] [datetimeoffset](7) NULL,
[NormalizedEmail] [nvarchar](256) NULL,
[NormalizedUserName] [nvarchar](256) NULL,
[PasswordHash] [nvarchar](max) NULL,
[PhoneNumber] [nvarchar](max) NULL,
[PhoneNumberConfirmed] [bit] NOT NULL,
[SecurityStamp] [nvarchar](max) NULL,
[TwoFactorEnabled] [bit] NOT NULL,
[UserName] [nvarchar](256) NULL,
[FirstName] [nvarchar](256) NULL,
[LastName] [nvarchar](256) NULL,
CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object: Table [dbo].[AspNetUserTokens] Script Date: 30/12/2018 12:19:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
[UserId] uniqueidentifier NOT NULL,
[LoginProvider] [nvarchar](450) NOT NULL,
[Name] [nvarchar](450) NOT NULL,
[Value] [nvarchar](max) NULL,
CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
[UserId] ASC,
[LoginProvider] ASC,
[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]



GO
ALTER TABLE [dbo].[AspNetRoleClaims] WITH CHECK ADD CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims] WITH CHECK ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins] WITH CHECK ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles] WITH CHECK ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles] WITH CHECK ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO