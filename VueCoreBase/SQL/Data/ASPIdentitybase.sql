
INSERT INTO dbo.__EFMigrationsHistory(MigrationId,ProductVersion) VALUES
('00000000000000_CreateIdentitySchema', '2.2.0-rtm-35687');

INSERT INTO [dbo].[AspNetRoles] ([ConcurrencyStamp], [Name], [NormalizedName]) VALUES 
( CONVERT(NVARCHAR(max), NEWID()), N'User', N'USER'),
( CONVERT(NVARCHAR(max), NEWID()), N'Employee', N'EMPLOYEE'),
( CONVERT(NVARCHAR(max), NEWID()), N'Admin', N'ADMIN');

INSERT INTO dbo.[AspNetUsers] ([AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName], [FirstName], [LastName]) 
VALUES (0, N'd99b82a7-8a1f-423e-9d53-0a640e720011', N'joe.bloggs@company.com', 1, 1, NULL, N'JOE.BLOGGS@COMPANY.COM', N'JOE.BLOGGS@COMPANY.COM', N'AQAAAAEAACcQAAAAEHKAnlTYUAp8F5ERBP1akzpjEdHBtGuwjQM7zTZ3zJOdyyLcvgUJVfzYEzjWXomV3A==', N'0403478235', 0, N'78f8759f-684b-47bb-8281-3158b3f5cad8', 0, N'joe.bloggs@dewc.com', 'Joe', 'Bloggs')

INSERT INTO [dbo].[AspNetUserRoles]([UserId], [RoleId]) VALUES
((SELECT ID FROM [dbo].[AspNetUsers] WHERE [UserName] = 'joe.bloggs@dewc.com'),(SELECT ID FROM [dbo].[AspNetRoles] WHERE [Name] = N'User')),
((SELECT ID FROM [dbo].[AspNetUsers] WHERE [UserName] = 'joe.bloggs@dewc.com'),(SELECT ID FROM [dbo].[AspNetRoles] WHERE [Name] = N'Employee')),
((SELECT ID FROM [dbo].[AspNetUsers] WHERE [UserName] = 'joe.bloggs@dewc.com'),(SELECT ID FROM [dbo].[AspNetRoles] WHERE [Name] = N'Admin'));