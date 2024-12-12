USE [master]
GO
IF NOT EXISTS ( SELECT [name] FROM sys.databases WHERE [name] = 'identitydb' )
CREATE DATABASE [identitydb];	 
GO

USE [identitydb]
GO

DROP PROCEDURE IF EXISTS [dbo].[FindActiveByRoleWithCount];
DROP PROCEDURE IF EXISTS [dbo].[FindActiveOrBlockedWithCount];
DROP PROCEDURE IF EXISTS [dbo].[FindActiveOrDeletedUsersWithCount];
DROP PROCEDURE IF EXISTS [dbo].[FindActiveUsersInRoleAndAllActiveUsersWithCount];
DROP PROCEDURE IF EXISTS [dbo].[FindActiveUsersWithCount];
DROP PROCEDURE IF EXISTS [dbo].[FindAllUsersWithCount];
DROP PROCEDURE IF EXISTS [dbo].[FindBlockedOrDeletedWithCount];
DROP PROCEDURE IF EXISTS [dbo].[FindBlockedUsersWithCount];
DROP PROCEDURE IF EXISTS [dbo].[FindDeletedUsersWithCount];
DROP PROCEDURE IF EXISTS [dbo].[PurgeIdentityLogs];
DROP VIEW IF EXISTS [dbo].[ActiveUsers];
DROP TABLE IF EXISTS [dbo].[__EFMigrationsHistory];
DROP TABLE IF EXISTS [dbo].[AdGroups];
DROP TABLE IF EXISTS [dbo].[ApiResourceClaims];
DROP TABLE IF EXISTS [dbo].[ApiResourceProperties];
DROP TABLE IF EXISTS [dbo].[ApiResourceScopes];
DROP TABLE IF EXISTS [dbo].[ApiResourceSecrets];
DROP TABLE IF EXISTS [dbo].[ApiResources];
DROP TABLE IF EXISTS [dbo].[ApiScopeClaims];
DROP TABLE IF EXISTS [dbo].[ApiScopeProperties];
DROP TABLE IF EXISTS [dbo].[ApiScopes];
DROP TABLE IF EXISTS [dbo].[AspNetRoleClaims];
DROP TABLE IF EXISTS [dbo].[AspNetUserClaims];
DROP TABLE IF EXISTS [dbo].[AspNetUserLogins];
DROP TABLE IF EXISTS [dbo].[AspNetUserTokens];
DROP TABLE IF EXISTS [dbo].[AspNetUserRoles];
DROP TABLE IF EXISTS [dbo].[ClientClaims];
DROP TABLE IF EXISTS [dbo].[ClientCorsOrigins];
DROP TABLE IF EXISTS [dbo].[ClientGrantTypes];
DROP TABLE IF EXISTS [dbo].[ClientIdPRestrictions];
DROP TABLE IF EXISTS [dbo].[ClientPostLogoutRedirectUris];
DROP TABLE IF EXISTS [dbo].[ClientProperties];
DROP TABLE IF EXISTS [dbo].[ClientRedirectUris];
DROP TABLE IF EXISTS [dbo].[ClientScopes];
DROP TABLE IF EXISTS [dbo].[ClientSecrets];
DROP TABLE IF EXISTS [dbo].[ConfigurationEntries];
DROP TABLE IF EXISTS [dbo].[DeviceCodes];
DROP TABLE IF EXISTS [dbo].[EnumClaimTypeAllowedValues];
DROP TABLE IF EXISTS [dbo].[ExtendedApiResources];
DROP TABLE IF EXISTS [dbo].[ExtendedClients];
DROP TABLE IF EXISTS [dbo].[ExtendedIdentityResources];
DROP TABLE IF EXISTS [dbo].[IdentityResourceClaims];
DROP TABLE IF EXISTS [dbo].[IdentityResourceProperties];
DROP TABLE IF EXISTS [dbo].[IdentityResources];
DROP TABLE IF EXISTS [dbo].[Keys];
DROP TABLE IF EXISTS [dbo].[Logs];
DROP TABLE IF EXISTS [dbo].[PersistedGrants];
DROP TABLE IF EXISTS [dbo].[AspNetClaimTypes];
DROP TABLE IF EXISTS [dbo].[AspNetRoles];
DROP TABLE IF EXISTS [dbo].[AspNetUsers];
DROP TABLE IF EXISTS [dbo].[Clients];
DROP TABLE IF EXISTS [dbo].[IdentityProviders];
DROP TABLE IF EXISTS [dbo].[AppCache];
DROP TABLE IF EXISTS [dbo].[AuditEntries];

/****** Object:  Table [dbo].[AuditEntries]    Script Date: 11/30/2021 2:47:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AuditEntries](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[When] [datetime2](7) NOT NULL,
	[Source] [nvarchar](max) NULL,
	[SubjectType] [nvarchar](max) NULL,
	[SubjectIdentifier] [nvarchar](max) NULL,
	[Subject] [nvarchar](max) NULL,
	[Action] [nvarchar](max) NULL,
	[ResourceType] [nvarchar](max) NULL,
	[Resource] [nvarchar](max) NULL,
	[ResourceIdentifier] [nvarchar](max) NULL,
	[Succeeded] [bit] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[NormalisedSubject] [nvarchar](max) NULL,
	[NormalisedAction] [nvarchar](max) NULL,
	[NormalisedResource] [nvarchar](max) NULL,
	[NormalisedSource] [nvarchar](max) NULL,
 CONSTRAINT [PK_AuditEntries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[IsBlocked] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[LastName] [nvarchar](max) NULL,
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
	[NormalizedFirstName] [nvarchar](256) NULL,
	[NormalizedLastName] [nvarchar](256) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE CLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ActiveUsers]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ActiveUsers]
WITH SCHEMABINDING 
AS
SELECT        Id, Email, FirstName, LastName, UserName, NormalizedFirstName, NormalizedLastName, NormalizedUserName, NormalizedEmail
FROM            dbo.AspNetUsers
WHERE        (IsBlocked = 0) AND (IsDeleted = 0)
GO
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET NUMERIC_ROUNDABORT OFF
GO
/****** Object:  Index [ClusteredIndex-20190723-131653]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20190723-131653] ON [dbo].[ActiveUsers]
(
	[NormalizedFirstName] ASC,
	[NormalizedLastName] ASC,
	[NormalizedUserName] ASC,
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 9/10/2021 6:47:19 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApiResourceClaims]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiResourceClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
	[ApiResourceId] [int] NOT NULL,
 CONSTRAINT [PK_ApiResourceClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApiResourceProperties]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiResourceProperties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](250) NOT NULL,
	[Value] [nvarchar](2000) NOT NULL,
	[ApiResourceId] [int] NOT NULL,
 CONSTRAINT [PK_ApiResourceProperties] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApiResources]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiResources](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[DisplayName] [nvarchar](200) NULL,
	[Enabled] [bit] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[LastAccessed] [datetime2](7) NULL,
	[NonEditable] [bit] NOT NULL,
	[Updated] [datetime2](7) NULL,
	[AllowedAccessTokenSigningAlgorithms] [nvarchar](100) NULL,
	[ShowInDiscoveryDocument] [bit] NOT NULL,
	[RequireResourceIndicator] [bit] NOT NULL,
 CONSTRAINT [PK_ApiResources] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApiResourceScopes]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiResourceScopes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Scope] [nvarchar](200) NOT NULL,
	[ApiResourceId] [int] NOT NULL,
 CONSTRAINT [PK_ApiResourceScopes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApiResourceSecrets]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiResourceSecrets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Value] [nvarchar](4000) NOT NULL,
	[Expiration] [datetime2](7) NULL,
	[Type] [nvarchar](250) NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[ApiResourceId] [int] NOT NULL,
 CONSTRAINT [PK_ApiResourceSecrets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApiScopeClaims]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiScopeClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScopeId] [int] NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ApiScopeClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApiScopeProperties]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiScopeProperties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](250) NOT NULL,
	[Value] [nvarchar](2000) NOT NULL,
	[ScopeId] [int] NOT NULL,
 CONSTRAINT [PK_ApiScopeProperties] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApiScopes]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiScopes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[DisplayName] [nvarchar](200) NULL,
	[Emphasize] [bit] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Required] [bit] NOT NULL,
	[ShowInDiscoveryDocument] [bit] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_ApiScopes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetClaimTypes]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetClaimTypes](
	[Id] [nvarchar](450) NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Name] [nvarchar](256) NOT NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[Required] [bit] NOT NULL,
	[Reserved] [bit] NOT NULL,
	[Rule] [nvarchar](max) NULL,
	[RuleValidationFailureDescription] [nvarchar](max) NULL,
	[UserEditable] [bit] NOT NULL,
	[ValueType] [int] NOT NULL,
 CONSTRAINT [PK_AspNetClaimTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_AspNetClaimTypes_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[Reserved] [bit] NOT NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](256) NOT NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientClaims]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[Type] [nvarchar](250) NOT NULL,
	[Value] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_ClientClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientCorsOrigins]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientCorsOrigins](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[Origin] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_ClientCorsOrigins] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientGrantTypes]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientGrantTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[GrantType] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_ClientGrantTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientIdPRestrictions]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientIdPRestrictions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[Provider] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ClientIdPRestrictions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientPostLogoutRedirectUris]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientPostLogoutRedirectUris](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[PostLogoutRedirectUri] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_ClientPostLogoutRedirectUris] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientProperties]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientProperties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[Key] [nvarchar](250) NOT NULL,
	[Value] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_ClientProperties] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientRedirectUris]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientRedirectUris](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[RedirectUri] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_ClientRedirectUris] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AbsoluteRefreshTokenLifetime] [int] NOT NULL,
	[AccessTokenLifetime] [int] NOT NULL,
	[AccessTokenType] [int] NOT NULL,
	[AllowAccessTokensViaBrowser] [bit] NOT NULL,
	[AllowOfflineAccess] [bit] NOT NULL,
	[AllowPlainTextPkce] [bit] NOT NULL,
	[AllowRememberConsent] [bit] NOT NULL,
	[AlwaysIncludeUserClaimsInIdToken] [bit] NOT NULL,
	[AlwaysSendClientClaims] [bit] NOT NULL,
	[AuthorizationCodeLifetime] [int] NOT NULL,
	[BackChannelLogoutSessionRequired] [bit] NOT NULL,
	[BackChannelLogoutUri] [nvarchar](2000) NULL,
	[ClientClaimsPrefix] [nvarchar](200) NULL,
	[ClientId] [nvarchar](200) NOT NULL,
	[ClientName] [nvarchar](200) NULL,
	[ClientUri] [nvarchar](2000) NULL,
	[ConsentLifetime] [int] NULL,
	[Description] [nvarchar](1000) NULL,
	[EnableLocalLogin] [bit] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[FrontChannelLogoutSessionRequired] [bit] NOT NULL,
	[FrontChannelLogoutUri] [nvarchar](2000) NULL,
	[IdentityTokenLifetime] [int] NOT NULL,
	[IncludeJwtId] [bit] NOT NULL,
	[LogoUri] [nvarchar](2000) NULL,
	[PairWiseSubjectSalt] [nvarchar](200) NULL,
	[ProtocolType] [nvarchar](200) NOT NULL,
	[RefreshTokenExpiration] [int] NOT NULL,
	[RefreshTokenUsage] [int] NOT NULL,
	[RequireClientSecret] [bit] NOT NULL,
	[RequireConsent] [bit] NOT NULL,
	[RequirePkce] [bit] NOT NULL,
	[SlidingRefreshTokenLifetime] [int] NOT NULL,
	[UpdateAccessTokenClaimsOnRefresh] [bit] NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[DeviceCodeLifetime] [int] NOT NULL,
	[LastAccessed] [datetime2](7) NULL,
	[NonEditable] [bit] NOT NULL,
	[Updated] [datetime2](7) NULL,
	[UserCodeType] [nvarchar](100) NULL,
	[UserSsoLifetime] [int] NULL,
	[AllowedIdentityTokenSigningAlgorithms] [nvarchar](100) NULL,
	[RequireRequestObject] [bit] NOT NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientScopes]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientScopes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[Scope] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ClientScopes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientSecrets]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientSecrets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[Expiration] [datetime2](7) NULL,
	[Type] [nvarchar](250) NOT NULL,
	[Value] [nvarchar](4000) NOT NULL,
	[Created] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ClientSecrets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConfigurationEntries]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConfigurationEntries](
	[Key] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_ConfigurationEntries] PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeviceCodes]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceCodes](
	[DeviceCode] [nvarchar](200) NOT NULL,
	[UserCode] [nvarchar](200) NOT NULL,
	[SubjectId] [nvarchar](200) NULL,
	[ClientId] [nvarchar](200) NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[Expiration] [datetime2](7) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
	[SessionId] [nvarchar](100) NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_DeviceCodes] PRIMARY KEY CLUSTERED 
(
	[UserCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EnumClaimTypeAllowedValues]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EnumClaimTypeAllowedValues](
	[ClaimTypeId] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_EnumClaimTypeAllowedValues] PRIMARY KEY CLUSTERED 
(
	[ClaimTypeId] ASC,
	[Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExtendedApiResources]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtendedApiResources](
	[Id] [nvarchar](450) NOT NULL,
	[ApiResourceName] [nvarchar](200) NOT NULL,
	[NormalizedName] [nvarchar](200) NOT NULL,
	[Reserved] [bit] NOT NULL,
 CONSTRAINT [PK_ExtendedApiResources] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExtendedClients]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtendedClients](
	[Id] [nvarchar](450) NOT NULL,
	[ClientId] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[NormalizedClientId] [nvarchar](200) NOT NULL,
	[NormalizedClientName] [nvarchar](200) NULL,
	[Reserved] [bit] NOT NULL,
	[ClientType] [int] NULL,
 CONSTRAINT [PK_ExtendedClients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExtendedIdentityResources]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtendedIdentityResources](
	[Id] [nvarchar](450) NOT NULL,
	[IdentityResourceName] [nvarchar](200) NOT NULL,
	[NormalizedName] [nvarchar](200) NOT NULL,
	[Reserved] [bit] NOT NULL,
 CONSTRAINT [PK_ExtendedIdentityResources] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityProviders]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityProviders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Scheme] [nvarchar](200) NOT NULL,
	[DisplayName] [nvarchar](200) NULL,
	[Enabled] [bit] NOT NULL,
	[Type] [nvarchar](20) NOT NULL,
	[Properties] [nvarchar](max) NULL,
 CONSTRAINT [PK_IdentityProviders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityResourceClaims]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityResourceClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
	[IdentityResourceId] [int] NOT NULL,
 CONSTRAINT [PK_IdentityResourceClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityResourceProperties]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityResourceProperties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](250) NOT NULL,
	[Value] [nvarchar](2000) NOT NULL,
	[IdentityResourceId] [int] NOT NULL,
 CONSTRAINT [PK_IdentityResourceProperties] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityResources]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityResources](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[DisplayName] [nvarchar](200) NULL,
	[Emphasize] [bit] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Required] [bit] NOT NULL,
	[ShowInDiscoveryDocument] [bit] NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[NonEditable] [bit] NOT NULL,
	[Updated] [datetime2](7) NULL,
 CONSTRAINT [PK_IdentityResources] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Keys]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Keys](
	[Id] [nvarchar](450) NOT NULL,
	[Version] [int] NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[Use] [nvarchar](450) NULL,
	[Algorithm] [nvarchar](100) NOT NULL,
	[IsX509Certificate] [bit] NOT NULL,
	[DataProtected] [bit] NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Keys] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersistedGrants]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersistedGrants](
	[Key] [nvarchar](200) NOT NULL,
	[ClientId] [nvarchar](200) NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
	[Expiration] [datetime2](7) NULL,
	[SubjectId] [nvarchar](200) NULL,
	[Type] [nvarchar](50) NOT NULL,
	[SessionId] [nvarchar](100) NULL,
	[Description] [nvarchar](200) NULL,
	[ConsumedTime] [datetime2](7) NULL,
 CONSTRAINT [PK_PersistedGrants] PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_ApiResourceClaims_ApiResourceId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ApiResourceClaims_ApiResourceId] ON [dbo].[ApiResourceClaims]
(
	[ApiResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApiResourceProperties_ApiResourceId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ApiResourceProperties_ApiResourceId] ON [dbo].[ApiResourceProperties]
(
	[ApiResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ApiResources_Name]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ApiResources_Name] ON [dbo].[ApiResources]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApiResourceScopes_ApiResourceId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ApiResourceScopes_ApiResourceId] ON [dbo].[ApiResourceScopes]
(
	[ApiResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApiResourceSecrets_ApiResourceId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ApiResourceSecrets_ApiResourceId] ON [dbo].[ApiResourceSecrets]
(
	[ApiResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApiScopeClaims_ScopeId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ApiScopeClaims_ScopeId] ON [dbo].[ApiScopeClaims]
(
	[ScopeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ApiScopeProperties_ScopeId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ApiScopeProperties_ScopeId] ON [dbo].[ApiScopeProperties]
(
	[ScopeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ApiScopes_Name]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ApiScopes_Name] ON [dbo].[ApiScopes]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [ClaimTypeNameIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [ClaimTypeNameIndex] ON [dbo].[AspNetClaimTypes]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_ClaimType]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_ClaimType] ON [dbo].[AspNetUserClaims]
(
	[ClaimType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [CountIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [CountIndex] ON [dbo].[AspNetUsers]
(
	[IsBlocked] ASC,
	[IsDeleted] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [CountIndexReversed]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [CountIndexReversed] ON [dbo].[AspNetUsers]
(
	[IsDeleted] ASC,
	[IsBlocked] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [FirstNameIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [FirstNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedFirstName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [LastNameIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [LastNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedLastName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClientClaims_ClientId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClientClaims_ClientId] ON [dbo].[ClientClaims]
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClientCorsOrigins_ClientId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClientCorsOrigins_ClientId] ON [dbo].[ClientCorsOrigins]
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClientGrantTypes_ClientId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClientGrantTypes_ClientId] ON [dbo].[ClientGrantTypes]
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClientIdPRestrictions_ClientId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClientIdPRestrictions_ClientId] ON [dbo].[ClientIdPRestrictions]
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClientPostLogoutRedirectUris_ClientId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClientPostLogoutRedirectUris_ClientId] ON [dbo].[ClientPostLogoutRedirectUris]
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClientProperties_ClientId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClientProperties_ClientId] ON [dbo].[ClientProperties]
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClientRedirectUris_ClientId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClientRedirectUris_ClientId] ON [dbo].[ClientRedirectUris]
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Clients_ClientId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Clients_ClientId] ON [dbo].[Clients]
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClientScopes_ClientId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClientScopes_ClientId] ON [dbo].[ClientScopes]
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClientSecrets_ClientId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_ClientSecrets_ClientId] ON [dbo].[ClientSecrets]
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DeviceCodes_DeviceCode]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_DeviceCodes_DeviceCode] ON [dbo].[DeviceCodes]
(
	[DeviceCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DeviceCodes_UserCode]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_DeviceCodes_UserCode] ON [dbo].[DeviceCodes]
(
	[UserCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [ApiNameIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [ApiNameIndex] ON [dbo].[ExtendedApiResources]
(
	[ApiResourceName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [ApiResourceNameIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [ApiResourceNameIndex] ON [dbo].[ExtendedApiResources]
(
	[NormalizedName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [ClientIdIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [ClientIdIndex] ON [dbo].[ExtendedClients]
(
	[NormalizedClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [ClientNameIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [ClientNameIndex] ON [dbo].[ExtendedClients]
(
	[NormalizedClientName] ASC
)
WHERE ([NormalizedClientName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IdIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IdIndex] ON [dbo].[ExtendedClients]
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IdentityNameIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IdentityNameIndex] ON [dbo].[ExtendedIdentityResources]
(
	[IdentityResourceName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IdentityResourceNameIndex]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IdentityResourceNameIndex] ON [dbo].[ExtendedIdentityResources]
(
	[NormalizedName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_IdentityResourceClaims_IdentityResourceId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_IdentityResourceClaims_IdentityResourceId] ON [dbo].[IdentityResourceClaims]
(
	[IdentityResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_IdentityResourceProperties_IdentityResourceId]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_IdentityResourceProperties_IdentityResourceId] ON [dbo].[IdentityResourceProperties]
(
	[IdentityResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_IdentityResources_Name]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_IdentityResources_Name] ON [dbo].[IdentityResources]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Keys_Use]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_Keys_Use] ON [dbo].[Keys]
(
	[Use] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PersistedGrants_ConsumedTime]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_PersistedGrants_ConsumedTime] ON [dbo].[PersistedGrants]
(
	[ConsumedTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_PersistedGrants_SubjectId_ClientId_Type]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_PersistedGrants_SubjectId_ClientId_Type] ON [dbo].[PersistedGrants]
(
	[SubjectId] ASC,
	[ClientId] ASC,
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_PersistedGrants_SubjectId_SessionId_Type]    Script Date: 9/10/2021 6:47:19 AM ******/
CREATE NONCLUSTERED INDEX [IX_PersistedGrants_SubjectId_SessionId_Type] ON [dbo].[PersistedGrants]
(
	[SubjectId] ASC,
	[SessionId] ASC,
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApiResources] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [Created]
GO
ALTER TABLE [dbo].[ApiResources] ADD  DEFAULT (CONVERT([bit],(0))) FOR [NonEditable]
GO
ALTER TABLE [dbo].[ApiResources] ADD  DEFAULT (CONVERT([bit],(0))) FOR [RequireResourceIndicator]
GO
ALTER TABLE [dbo].[AspNetClaimTypes] ADD  DEFAULT (CONVERT([bit],(0))) FOR [UserEditable]
GO
ALTER TABLE [dbo].[Clients] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [Created]
GO
ALTER TABLE [dbo].[Clients] ADD  DEFAULT ((0)) FOR [DeviceCodeLifetime]
GO
ALTER TABLE [dbo].[Clients] ADD  DEFAULT (CONVERT([bit],(0))) FOR [NonEditable]
GO
ALTER TABLE [dbo].[ClientSecrets] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [Created]
GO
ALTER TABLE [dbo].[IdentityResources] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [Created]
GO
ALTER TABLE [dbo].[IdentityResources] ADD  DEFAULT (CONVERT([bit],(0))) FOR [NonEditable]
GO
ALTER TABLE [dbo].[ApiResourceClaims]  WITH CHECK ADD  CONSTRAINT [FK_ApiResourceClaims_ApiResources_ApiResourceId] FOREIGN KEY([ApiResourceId])
REFERENCES [dbo].[ApiResources] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApiResourceClaims] CHECK CONSTRAINT [FK_ApiResourceClaims_ApiResources_ApiResourceId]
GO
ALTER TABLE [dbo].[ApiResourceProperties]  WITH CHECK ADD  CONSTRAINT [FK_ApiResourceProperties_ApiResources_ApiResourceId] FOREIGN KEY([ApiResourceId])
REFERENCES [dbo].[ApiResources] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApiResourceProperties] CHECK CONSTRAINT [FK_ApiResourceProperties_ApiResources_ApiResourceId]
GO
ALTER TABLE [dbo].[ApiResourceScopes]  WITH CHECK ADD  CONSTRAINT [FK_ApiResourceScopes_ApiResources_ApiResourceId] FOREIGN KEY([ApiResourceId])
REFERENCES [dbo].[ApiResources] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApiResourceScopes] CHECK CONSTRAINT [FK_ApiResourceScopes_ApiResources_ApiResourceId]
GO
ALTER TABLE [dbo].[ApiResourceSecrets]  WITH CHECK ADD  CONSTRAINT [FK_ApiResourceSecrets_ApiResources_ApiResourceId] FOREIGN KEY([ApiResourceId])
REFERENCES [dbo].[ApiResources] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApiResourceSecrets] CHECK CONSTRAINT [FK_ApiResourceSecrets_ApiResources_ApiResourceId]
GO
ALTER TABLE [dbo].[ApiScopeClaims]  WITH CHECK ADD  CONSTRAINT [FK_ApiScopeClaims_ApiScopes_ScopeId] FOREIGN KEY([ScopeId])
REFERENCES [dbo].[ApiScopes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApiScopeClaims] CHECK CONSTRAINT [FK_ApiScopeClaims_ApiScopes_ScopeId]
GO
ALTER TABLE [dbo].[ApiScopeProperties]  WITH CHECK ADD  CONSTRAINT [FK_ApiScopeProperties_ApiScopes_ScopeId] FOREIGN KEY([ScopeId])
REFERENCES [dbo].[ApiScopes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApiScopeProperties] CHECK CONSTRAINT [FK_ApiScopeProperties_ApiScopes_ScopeId]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetClaimTypes_ClaimType] FOREIGN KEY([ClaimType])
REFERENCES [dbo].[AspNetClaimTypes] ([Name])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetClaimTypes_ClaimType]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[ClientClaims]  WITH CHECK ADD  CONSTRAINT [FK_ClientClaims_Clients_ClientId] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClientClaims] CHECK CONSTRAINT [FK_ClientClaims_Clients_ClientId]
GO
ALTER TABLE [dbo].[ClientCorsOrigins]  WITH CHECK ADD  CONSTRAINT [FK_ClientCorsOrigins_Clients_ClientId] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClientCorsOrigins] CHECK CONSTRAINT [FK_ClientCorsOrigins_Clients_ClientId]
GO
ALTER TABLE [dbo].[ClientGrantTypes]  WITH CHECK ADD  CONSTRAINT [FK_ClientGrantTypes_Clients_ClientId] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClientGrantTypes] CHECK CONSTRAINT [FK_ClientGrantTypes_Clients_ClientId]
GO
ALTER TABLE [dbo].[ClientIdPRestrictions]  WITH CHECK ADD  CONSTRAINT [FK_ClientIdPRestrictions_Clients_ClientId] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClientIdPRestrictions] CHECK CONSTRAINT [FK_ClientIdPRestrictions_Clients_ClientId]
GO
ALTER TABLE [dbo].[ClientPostLogoutRedirectUris]  WITH CHECK ADD  CONSTRAINT [FK_ClientPostLogoutRedirectUris_Clients_ClientId] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClientPostLogoutRedirectUris] CHECK CONSTRAINT [FK_ClientPostLogoutRedirectUris_Clients_ClientId]
GO
ALTER TABLE [dbo].[ClientProperties]  WITH CHECK ADD  CONSTRAINT [FK_ClientProperties_Clients_ClientId] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClientProperties] CHECK CONSTRAINT [FK_ClientProperties_Clients_ClientId]
GO
ALTER TABLE [dbo].[ClientRedirectUris]  WITH CHECK ADD  CONSTRAINT [FK_ClientRedirectUris_Clients_ClientId] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClientRedirectUris] CHECK CONSTRAINT [FK_ClientRedirectUris_Clients_ClientId]
GO
ALTER TABLE [dbo].[ClientScopes]  WITH CHECK ADD  CONSTRAINT [FK_ClientScopes_Clients_ClientId] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClientScopes] CHECK CONSTRAINT [FK_ClientScopes_Clients_ClientId]
GO
ALTER TABLE [dbo].[ClientSecrets]  WITH CHECK ADD  CONSTRAINT [FK_ClientSecrets_Clients_ClientId] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClientSecrets] CHECK CONSTRAINT [FK_ClientSecrets_Clients_ClientId]
GO
ALTER TABLE [dbo].[EnumClaimTypeAllowedValues]  WITH CHECK ADD  CONSTRAINT [FK_EnumClaimTypeAllowedValues_AspNetClaimTypes_ClaimTypeId] FOREIGN KEY([ClaimTypeId])
REFERENCES [dbo].[AspNetClaimTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EnumClaimTypeAllowedValues] CHECK CONSTRAINT [FK_EnumClaimTypeAllowedValues_AspNetClaimTypes_ClaimTypeId]
GO
ALTER TABLE [dbo].[IdentityResourceClaims]  WITH CHECK ADD  CONSTRAINT [FK_IdentityResourceClaims_IdentityResources_IdentityResourceId] FOREIGN KEY([IdentityResourceId])
REFERENCES [dbo].[IdentityResources] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IdentityResourceClaims] CHECK CONSTRAINT [FK_IdentityResourceClaims_IdentityResources_IdentityResourceId]
GO
ALTER TABLE [dbo].[IdentityResourceProperties]  WITH CHECK ADD  CONSTRAINT [FK_IdentityResourceProperties_IdentityResources_IdentityResourceId] FOREIGN KEY([IdentityResourceId])
REFERENCES [dbo].[IdentityResources] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IdentityResourceProperties] CHECK CONSTRAINT [FK_IdentityResourceProperties_IdentityResources_IdentityResourceId]
GO
/****** Object:  StoredProcedure [dbo].[FindActiveByRoleWithCount]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FindActiveByRoleWithCount]
	@RoleId nvarchar(450),
	@SearchTerm nvarchar(256) = null, 
	@PageNumber int,
	@PageSize int
AS
BEGIN
/* Searchs for all Active (IsBlocked = 0 && IsDeleted = 0) Users within a role				  */
/* If @SearchTerm is given it will search for LIKE on Email, Username, firstname and lastname */
/* If no @SearchTerm is given it will return all Active Users in that Role					  */
	SET NOCOUNT ON;

	IF @SearchTerm IS null
		BEGIN
		 -- Select Count of all users within role
			SELECT Count(*)
			FROM [AspNetUserRoles] ur
			RIGHT JOIN [ActiveUsers] u
			ON u.Id = ur.UserId
			WHERE (ur.[RoleId] = @RoleId) 

		--Select all users in role with SearchTerm
				SELECT  u.Id,
						u.[Email], 
						u.[FirstName], 
						u.[LastName], 	
						u.[UserName]
				FROM [AspNetUserRoles] ur
				RIGHT JOIN [ActiveUsers] u
				ON u.Id = ur.UserId			
				WHERE (ur.[RoleId] = @RoleId) 
				ORDER BY u.[UserName]
				OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE --IF @SearchTerm is not Null
		BEGIN

			SET @SearchTerm = UPPER(@SearchTerm) + N'%'
			
			DECLARE @NameSplitCount int;
			SET @NameSplitCount = (SELECT LEN(@SearchTerm) - LEN(REPLACE(@SearchTerm, ' ', '')))

			IF(@NameSplitCount = 0)
				BEGIN
								--Select Count With Search
					SELECT Count(*)
					FROM [AspNetUserRoles] ur
					RIGHT JOIN [ActiveUsers] u
					ON u.Id = ur.UserId
					WHERE (ur.[RoleId] = @RoleId) 
					AND
						([NormalizedUserName] LIKE @SearchTerm  OR 
						[NormalizedEmail] LIKE @SearchTerm  OR 
						[NormalizedFirstName] LIKE @SearchTerm  OR 
						[NormalizedLastName] LIKE @SearchTerm) 

				--Select all users in role with Search
					SELECT u.Id,
							u.[Email], 
							u.[FirstName], 
							u.[LastName], 	
							u.[UserName]
					FROM [AspNetUserRoles] ur
					RIGHT JOIN [ActiveUsers] u
					ON u.Id = ur.UserId
					WHERE (ur.[RoleId] = @RoleId) 
						AND
							([NormalizedUserName] LIKE @SearchTerm  OR 
							[NormalizedEmail] LIKE @SearchTerm  OR 
							[NormalizedFirstName] LIKE @SearchTerm  OR 
							[NormalizedLastName] LIKE @SearchTerm) 
					ORDER BY u.[UserName]
					OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
				END
			ELSE
				BEGIN
					DECLARE @FirstName nvarchar(256);
					DECLARE @FirstNameCount int;
					DECLARE @LastName nvarchar(256);

					SET @FirstName = left(@SearchTerm, charindex(' ', @SearchTerm) - 1) + N'%'
					SET @LastName = substring(@SearchTerm, charindex(' ', @SearchTerm) + 1, len(@SearchTerm))

					SELECT Count(*)
					FROM [AspNetUserRoles] ur
					RIGHT JOIN [ActiveUsers] u
					ON u.Id = ur.UserId
					WHERE (ur.[RoleId] = @RoleId) 
					AND
					   ([NormalizedFirstName] LIKE @FirstName AND
					   [NormalizedLastName] LIKE @LastName)

				--Select all users in role with Search
					SELECT u.Id,
							u.[Email], 
							u.[FirstName], 
							u.[LastName], 	
							u.[UserName]
					FROM [AspNetUserRoles] ur
					RIGHT JOIN [ActiveUsers] u
					ON u.Id = ur.UserId
					WHERE (ur.[RoleId] = @RoleId) 
					AND
					   ([NormalizedFirstName] LIKE @FirstName AND
					   [NormalizedLastName] LIKE @LastName)
					ORDER BY u.[UserName]
					OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
				END

		END
END
GO
/****** Object:  StoredProcedure [dbo].[FindActiveOrBlockedWithCount]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FindActiveOrBlockedWithCount]
	@SearchTerm nvarchar(256),
	@PageNumber int,
	@PageSize int
AS
BEGIN
	SET NOCOUNT ON;
	SET @SearchTerm = UPPER(@SearchTerm) + N'%'
	DECLARE @NameSplitCount int;
	SET @NameSplitCount = (SELECT LEN(@SearchTerm) - LEN(REPLACE(@SearchTerm, ' ', '')))

	IF(@NameSplitCount = 0)
		BEGIN
			SELECT Count(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm OR 
				   [NormalizedLastName] LIKE @SearchTerm) AND
				   [IsDeleted] = 0;
			SELECT [Id], 
				   [AccessFailedCount], 
				   [ConcurrencyStamp], 
				   [Email], 
				   [EmailConfirmed], 
				   [FirstName], 
				   [IsBlocked], 
				   [IsDeleted], 
				   [LastName], 
				   [LockoutEnabled], 
				   [LockoutEnd], 
				   [NormalizedEmail], 
				   [NormalizedFirstName], 
				   [NormalizedLastName], 
				   [NormalizedUserName], 
				   [PasswordHash], 
				   [PhoneNumber], 
				   [PhoneNumberConfirmed], 
				   [SecurityStamp], 
				   [TwoFactorEnabled], 
				   [UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm  OR 
				   [NormalizedLastName] LIKE @SearchTerm) AND
				   [IsDeleted] = 0
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE
		BEGIN
			DECLARE @FirstName nvarchar(256);
			DECLARE @FirstNameCount int;
			DECLARE @LastName nvarchar(256);

			SET @FirstName = left(@SearchTerm, charindex(' ', @SearchTerm) - 1) + N'%'
			SET @LastName = substring(@SearchTerm, charindex(' ', @SearchTerm) + 1, len(@SearchTerm))

			SELECT COUNT(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName
					AND [NormalizedLastName] LIKE @LastName) AND [IsDeleted] = 0

			SELECT [Id],
					[AccessFailedCount], 
					[ConcurrencyStamp], 
					[Email], 
					[EmailConfirmed], 
					[FirstName], 
					[IsBlocked], 
					[IsDeleted], 
					[LastName], 
					[LockoutEnabled], 
					[LockoutEnd], 
					[NormalizedEmail], 
					[NormalizedFirstName], 
					[NormalizedLastName], 
					[NormalizedUserName], 
					[PasswordHash], 
					[PhoneNumber], 
					[PhoneNumberConfirmed], 
					[SecurityStamp], 
					[TwoFactorEnabled], 
					[UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName AND
					[NormalizedLastName] LIKE @LastName) AND
					[IsDeleted] = 0
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
END


GO
/****** Object:  StoredProcedure [dbo].[FindActiveOrDeletedUsersWithCount]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FindActiveOrDeletedUsersWithCount]
	@SearchTerm nvarchar(256),
	@PageNumber int,
	@PageSize int
AS
BEGIN
	SET NOCOUNT ON;
	SET @SearchTerm = UPPER(@SearchTerm) + N'%'
	DECLARE @NameSplitCount int;
	SET @NameSplitCount = (SELECT LEN(@SearchTerm) - LEN(REPLACE(@SearchTerm, ' ', '')))

	IF(@NameSplitCount = 0)
		BEGIN
			SELECT Count(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm OR 
				   [NormalizedLastName] LIKE @SearchTerm) AND
					[IsBlocked] = 0;
			SELECT [Id], 
				   [AccessFailedCount], 
				   [ConcurrencyStamp], 
				   [Email], 
				   [EmailConfirmed], 
				   [FirstName], 
				   [IsBlocked], 
				   [IsDeleted], 
				   [LastName], 
				   [LockoutEnabled], 
				   [LockoutEnd], 
				   [NormalizedEmail], 
				   [NormalizedFirstName], 
				   [NormalizedLastName], 
				   [NormalizedUserName], 
				   [PasswordHash], 
				   [PhoneNumber], 
				   [PhoneNumberConfirmed], 
				   [SecurityStamp], 
				   [TwoFactorEnabled], 
				   [UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm  OR 
				   [NormalizedLastName] LIKE @SearchTerm) AND
					[IsBlocked] = 0
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE
		BEGIN
			DECLARE @FirstName nvarchar(256);
			DECLARE @FirstNameCount int;
			DECLARE @LastName nvarchar(256);

			SET @FirstName = left(@SearchTerm, charindex(' ', @SearchTerm) - 1) + N'%'
			SET @LastName = substring(@SearchTerm, charindex(' ', @SearchTerm) + 1, len(@SearchTerm))

			SELECT COUNT(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName
				   AND [NormalizedLastName] LIKE @LastName)
				   AND  [IsBlocked] = 0

			SELECT [Id],
					[AccessFailedCount], 
					[ConcurrencyStamp], 
					[Email], 
					[EmailConfirmed], 
					[FirstName], 
					[IsBlocked], 
					[IsDeleted], 
					[LastName], 
					[LockoutEnabled], 
					[LockoutEnd], 
					[NormalizedEmail], 
					[NormalizedFirstName], 
					[NormalizedLastName], 
					[NormalizedUserName], 
					[PasswordHash], 
					[PhoneNumber], 
					[PhoneNumberConfirmed], 
					[SecurityStamp], 
					[TwoFactorEnabled], 
					[UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName AND
				   [NormalizedLastName] LIKE @LastName)
				   AND [IsBlocked] = 0
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
END

GO
/****** Object:  StoredProcedure [dbo].[FindActiveUsersInRoleAndAllActiveUsersWithCount]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FindActiveUsersInRoleAndAllActiveUsersWithCount]
	@RoleId nvarchar(450),
	@SearchTerm nvarchar(256) = null, 
	@PageNumber int,
	@PageSize int
AS
BEGIN
/* Searchs for all Active (IsBlocked = 0 && IsDeleted = 0) Users and if they are in the role  */
/* If @SearchTerm is given it will search for LIKE on Email, Username, firstname and lastname */
/* If no @SearchTerm is given it will return all Active Users in that Role					  */
	SET NOCOUNT ON;

	
	IF @SearchTerm IS null
		BEGIN
		  -- Select Count of all users within role
			SELECT Count(*)
			FROM [AspNetUserRoles] ur
			RIGHT JOIN [ActiveUsers] u
			ON u.Id = ur.UserId
			WHERE (ur.[RoleId] = @RoleId) 

	--Get All UserRole Entries with RoleID

		;WITH UserRoles_CTE
		AS
		(
			SELECT ur.[RoleId],
				   ur.[UserId]
			FROM [AspNetUserRoles] ur
			WHERE ur.[RoleId] = @RoleId
		)
		SELECT  u.Id,
				u.[Email], 
				u.[FirstName], 
				u.[LastName], 	
				u.[UserName],
			    CASE WHEN ur.[RoleId] IS NULL THEN CONVERT(bit, 0) ELSE CONVERT(bit, 1) END AS IsInRole
		FROM UserRoles_CTE ur
		RIGHT JOIN [ActiveUsers] u
		ON u.Id = ur.UserId			
		ORDER BY u.[UserName]

		OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE -- IF @SearchTerm is not Null
		BEGIN
			SET @SearchTerm = UPPER(@SearchTerm) + N'%'
			
			DECLARE @NameSplitCount int;
			SET @NameSplitCount = (SELECT LEN(@SearchTerm) - LEN(REPLACE(@SearchTerm, ' ', '')))

			IF(@NameSplitCount = 0)
				BEGIN
			--Select Count With Search
					SELECT Count(*)
					FROM [AspNetUserRoles] ur
					RIGHT JOIN [ActiveUsers] u
					ON u.Id = ur.UserId
					WHERE [NormalizedUserName] LIKE @SearchTerm  OR 
						[NormalizedEmail] LIKE @SearchTerm  OR 
						[NormalizedFirstName] LIKE @SearchTerm  OR 
						[NormalizedLastName] LIKE @SearchTerm

				--Select users in result with Search
					;WITH UserRoles_CTE
					AS
					(
						SELECT ur.[RoleId],
							   ur.[UserId]
						FROM [AspNetUserRoles] ur
						WHERE ur.[RoleId] = @RoleId

					)
					SELECT u.Id,
							u.[Email], 
							u.[FirstName], 
							u.[LastName], 	
							u.[UserName],
							CASE WHEN ur.[RoleId] IS NULL THEN CONVERT(bit, 0) ELSE CONVERT(bit, 1) END AS IsInRole
					FROM UserRoles_CTE ur
					RIGHT JOIN [ActiveUsers] u
					ON u.Id = ur.UserId
					WHERE (ur.[RoleId] = @RoleId) 
						AND
							[NormalizedUserName] LIKE @SearchTerm  OR 
							[NormalizedEmail] LIKE @SearchTerm  OR 
							[NormalizedFirstName] LIKE @SearchTerm  OR 
							[NormalizedLastName] LIKE @SearchTerm
					ORDER BY u.[UserName]
					OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
				END
		ELSE
			BEGIN
				DECLARE @FirstName nvarchar(256);
				DECLARE @FirstNameCount int;
				DECLARE @LastName nvarchar(256);

				SET @FirstName = left(@SearchTerm, charindex(' ', @SearchTerm) - 1) + N'%'
				SET @LastName = substring(@SearchTerm, charindex(' ', @SearchTerm) + 1, len(@SearchTerm))	
			
				SELECT Count(*)
				FROM [AspNetUserRoles] ur
				RIGHT JOIN [ActiveUsers] u
				ON u.Id = ur.UserId
				WHERE ([NormalizedFirstName] LIKE @FirstName AND
					   [NormalizedLastName] LIKE @LastName)

			--Select users in result with Search
				;WITH UserRoles_CTE
				AS
				(
					SELECT ur.[RoleId],
							ur.[UserId]
					FROM [AspNetUserRoles] ur
					WHERE ur.[RoleId] = @RoleId

				)
				SELECT u.Id,
						u.[Email], 
						u.[FirstName], 
						u.[LastName], 	
						u.[UserName],
						CASE WHEN ur.[RoleId] IS NULL THEN CONVERT(bit, 0) ELSE CONVERT(bit, 1) END AS IsInRole
				FROM UserRoles_CTE ur
				RIGHT JOIN [ActiveUsers] u
				ON u.Id = ur.UserId
				WHERE ([NormalizedFirstName] LIKE @FirstName AND
					   [NormalizedLastName] LIKE @LastName)
				ORDER BY u.[UserName]
				OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
			END
	END
END
GO
/****** Object:  StoredProcedure [dbo].[FindActiveUsersWithCount]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FindActiveUsersWithCount]
	@SearchTerm nvarchar(256),
	@PageNumber int,
	@PageSize int
AS
BEGIN
	SET NOCOUNT ON;
	SET @SearchTerm = UPPER(@SearchTerm) + N'%'

	DECLARE @NameSplitCount int;
	SET @NameSplitCount = (SELECT LEN(@SearchTerm) - LEN(REPLACE(@SearchTerm, ' ', '')))

	IF(@NameSplitCount = 0)
		BEGIN
			SELECT Count(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm OR 
				   [NormalizedLastName] LIKE @SearchTerm) AND
					[IsBlocked] = 0 AND [IsDeleted] = 0;
			SELECT [Id], 
				   [AccessFailedCount], 
				   [ConcurrencyStamp], 
				   [Email], 
				   [EmailConfirmed], 
				   [FirstName], 
				   [IsBlocked], 
				   [IsDeleted], 
				   [LastName], 
				   [LockoutEnabled], 
				   [LockoutEnd], 
				   [NormalizedEmail], 
				   [NormalizedFirstName], 
				   [NormalizedLastName], 
				   [NormalizedUserName], 
				   [PasswordHash], 
				   [PhoneNumber], 
				   [PhoneNumberConfirmed], 
				   [SecurityStamp], 
				   [TwoFactorEnabled], 
				   [UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm  OR 
				   [NormalizedLastName] LIKE @SearchTerm) AND
					([IsBlocked] = 0 AND [IsDeleted] = 0)
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE
		BEGIN
			DECLARE @FirstName nvarchar(256);
			DECLARE @FirstNameCount int;
			DECLARE @LastName nvarchar(256);

			SET @FirstName = left(@SearchTerm, charindex(' ', @SearchTerm) - 1) + N'%'
			SET @LastName = substring(@SearchTerm, charindex(' ', @SearchTerm) + 1, len(@SearchTerm))

			SELECT COUNT(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName AND
					[NormalizedLastName] LIKE @LastName)  AND
					([IsBlocked] = 0 AND [IsDeleted] = 0)

			SELECT [Id],
					[AccessFailedCount], 
					[ConcurrencyStamp], 
					[Email], 
					[EmailConfirmed], 
					[FirstName], 
					[IsBlocked], 
					[IsDeleted], 
					[LastName], 
					[LockoutEnabled], 
					[LockoutEnd], 
					[NormalizedEmail], 
					[NormalizedFirstName], 
					[NormalizedLastName], 
					[NormalizedUserName], 
					[PasswordHash], 
					[PhoneNumber], 
					[PhoneNumberConfirmed], 
					[SecurityStamp], 
					[TwoFactorEnabled], 
					[UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName AND
					[NormalizedLastName] LIKE @LastName) AND
					([IsBlocked] = 0 AND [IsDeleted] = 0)
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
END


GO
/****** Object:  StoredProcedure [dbo].[FindAllUsersWithCount]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FindAllUsersWithCount] 
	@SearchTerm nvarchar(256),
	@PageNumber int,
	@PageSize int
AS
BEGIN
	SET NOCOUNT ON;

	SET @SearchTerm = UPPER(@SearchTerm) + N'%'

	DECLARE @NameSplitCount int;
	SET @NameSplitCount = (SELECT LEN(@SearchTerm) - LEN(REPLACE(@SearchTerm, ' ', '')))

	IF(@NameSplitCount = 0)
		BEGIN
			SELECT Count(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm OR 
				   [NormalizedLastName] LIKE @SearchTerm);
			SELECT [Id], 
				   [AccessFailedCount], 
				   [ConcurrencyStamp], 
				   [Email], 
				   [EmailConfirmed], 
				   [FirstName], 
				   [IsBlocked], 
				   [IsDeleted], 
				   [LastName], 
				   [LockoutEnabled], 
				   [LockoutEnd], 
				   [NormalizedEmail], 
				   [NormalizedFirstName], 
				   [NormalizedLastName], 
				   [NormalizedUserName], 
				   [PasswordHash], 
				   [PhoneNumber], 
				   [PhoneNumberConfirmed], 
				   [SecurityStamp], 
				   [TwoFactorEnabled], 
				   [UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm  OR 
				   [NormalizedLastName] LIKE @SearchTerm)
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE
		BEGIN
			DECLARE @FirstName nvarchar(256);
			DECLARE @FirstNameCount int;
			DECLARE @LastName nvarchar(256);

			SET @FirstName = left(@SearchTerm, charindex(' ', @SearchTerm) - 1) + N'%'
			SET @LastName = substring(@SearchTerm, charindex(' ', @SearchTerm) + 1, len(@SearchTerm))

			SELECT COUNT(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName AND [NormalizedLastName] LIKE @LastName)

			SELECT [Id],
					[AccessFailedCount], 
					[ConcurrencyStamp], 
					[Email], 
					[EmailConfirmed], 
					[FirstName], 
					[IsBlocked], 
					[IsDeleted], 
					[LastName], 
					[LockoutEnabled], 
					[LockoutEnd], 
					[NormalizedEmail], 
					[NormalizedFirstName], 
					[NormalizedLastName], 
					[NormalizedUserName], 
					[PasswordHash], 
					[PhoneNumber], 
					[PhoneNumberConfirmed], 
					[SecurityStamp], 
					[TwoFactorEnabled], 
					[UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName AND [NormalizedLastName] LIKE @LastName)
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
END

GO
/****** Object:  StoredProcedure [dbo].[FindBlockedOrDeletedWithCount]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FindBlockedOrDeletedWithCount]
	@SearchTerm nvarchar(256),
	@PageNumber int,
	@PageSize int
AS
BEGIN
	SET NOCOUNT ON;
	SET @SearchTerm = UPPER(@SearchTerm) + N'%'
	DECLARE @NameSplitCount int;
	SET @NameSplitCount = (SELECT LEN(@SearchTerm) - LEN(REPLACE(@SearchTerm, ' ', '')))

	IF(@NameSplitCount = 0)
		BEGIN
			SELECT Count(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm OR 
				   [NormalizedLastName] LIKE @SearchTerm) AND
					([IsDeleted] = 1 OR [IsBlocked] = 1);
			SELECT [Id], 
				   [AccessFailedCount], 
				   [ConcurrencyStamp], 
				   [Email], 
				   [EmailConfirmed], 
				   [FirstName], 
				   [IsBlocked], 
				   [IsDeleted], 
				   [LastName], 
				   [LockoutEnabled], 
				   [LockoutEnd], 
				   [NormalizedEmail], 
				   [NormalizedFirstName], 
				   [NormalizedLastName], 
				   [NormalizedUserName], 
				   [PasswordHash], 
				   [PhoneNumber], 
				   [PhoneNumberConfirmed], 
				   [SecurityStamp], 
				   [TwoFactorEnabled], 
				   [UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm  OR 
				   [NormalizedLastName] LIKE @SearchTerm) AND
					([IsDeleted] = 1 OR [IsBlocked] = 1)
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE
		BEGIN
			DECLARE @FirstName nvarchar(256);
			DECLARE @FirstNameCount int;
			DECLARE @LastName nvarchar(256);

			SET @FirstName = left(@SearchTerm, charindex(' ', @SearchTerm) - 1) + N'%'
			SET @LastName = substring(@SearchTerm, charindex(' ', @SearchTerm) + 1, len(@SearchTerm))

			SELECT COUNT(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName AND
				   [NormalizedLastName] LIKE @LastName)AND
				   ([IsDeleted] = 1 OR [IsBlocked] = 1)

			SELECT [Id],
					[AccessFailedCount], 
					[ConcurrencyStamp], 
					[Email], 
					[EmailConfirmed], 
					[FirstName], 
					[IsBlocked], 
					[IsDeleted], 
					[LastName], 
					[LockoutEnabled], 
					[LockoutEnd], 
					[NormalizedEmail], 
					[NormalizedFirstName], 
					[NormalizedLastName], 
					[NormalizedUserName], 
					[PasswordHash], 
					[PhoneNumber], 
					[PhoneNumberConfirmed], 
					[SecurityStamp], 
					[TwoFactorEnabled], 
					[UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName AND
				   [NormalizedLastName] LIKE @LastName) AND
					([IsDeleted] = 1 OR [IsBlocked] = 1)
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
END
GO
/****** Object:  StoredProcedure [dbo].[FindBlockedUsersWithCount]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FindBlockedUsersWithCount]
	@SearchTerm nvarchar(256),
	@PageNumber int,
	@PageSize int
AS
BEGIN
	SET NOCOUNT ON;
	SET @SearchTerm = UPPER(@SearchTerm) + N'%'
	DECLARE @NameSplitCount int;
	SET @NameSplitCount = (SELECT LEN(@SearchTerm) - LEN(REPLACE(@SearchTerm, ' ', '')))

	IF(@NameSplitCount = 0)
		BEGIN
			SELECT Count(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm OR 
				   [NormalizedLastName] LIKE @SearchTerm)AND
				   [IsBlocked] = 1;
			SELECT [Id], 
				   [AccessFailedCount], 
				   [ConcurrencyStamp], 
				   [Email], 
				   [EmailConfirmed], 
				   [FirstName], 
				   [IsBlocked], 
				   [IsDeleted], 
				   [LastName], 
				   [LockoutEnabled], 
				   [LockoutEnd], 
				   [NormalizedEmail], 
				   [NormalizedFirstName], 
				   [NormalizedLastName], 
				   [NormalizedUserName], 
				   [PasswordHash], 
				   [PhoneNumber], 
				   [PhoneNumberConfirmed], 
				   [SecurityStamp], 
				   [TwoFactorEnabled], 
				   [UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm  OR 
				   [NormalizedLastName] LIKE @SearchTerm)AND
				   [IsBlocked] = 1
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE
		BEGIN
			DECLARE @FirstName nvarchar(256);
			DECLARE @FirstNameCount int;
			DECLARE @LastName nvarchar(256);

			SET @FirstName = left(@SearchTerm, charindex(' ', @SearchTerm) - 1) + N'%'
			SET @LastName = substring(@SearchTerm, charindex(' ', @SearchTerm) + 1, len(@SearchTerm))

			SELECT COUNT(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName AND
				   [NormalizedLastName] LIKE @LastName)AND
				   [IsBlocked] = 1

			SELECT [Id],
					[AccessFailedCount], 
					[ConcurrencyStamp], 
					[Email], 
					[EmailConfirmed], 
					[FirstName], 
					[IsBlocked], 
					[IsDeleted], 
					[LastName], 
					[LockoutEnabled], 
					[LockoutEnd], 
					[NormalizedEmail], 
					[NormalizedFirstName], 
					[NormalizedLastName], 
					[NormalizedUserName], 
					[PasswordHash], 
					[PhoneNumber], 
					[PhoneNumberConfirmed], 
					[SecurityStamp], 
					[TwoFactorEnabled], 
					[UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName AND
				   [NormalizedLastName] LIKE @LastName) AND
					[IsBlocked] = 1
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
END
GO
/****** Object:  StoredProcedure [dbo].[FindDeletedUsersWithCount]    Script Date: 9/10/2021 6:47:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FindDeletedUsersWithCount] 
	@SearchTerm nvarchar(256),
	@PageNumber int,
	@PageSize int
AS
BEGIN
	SET NOCOUNT ON;

	SET @SearchTerm = UPPER(@SearchTerm) + N'%'

	DECLARE @NameSplitCount int;
	SET @NameSplitCount = (SELECT LEN(@SearchTerm) - LEN(REPLACE(@SearchTerm, ' ', '')))

	IF(@NameSplitCount = 0)
		BEGIN
			SELECT Count(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm OR 
				   [NormalizedLastName] LIKE @SearchTerm)AND
				   [IsDeleted] = 1;
			SELECT [Id], 
				   [AccessFailedCount], 
				   [ConcurrencyStamp], 
				   [Email], 
				   [EmailConfirmed], 
				   [FirstName], 
				   [IsBlocked], 
				   [IsDeleted], 
				   [LastName], 
				   [LockoutEnabled], 
				   [LockoutEnd], 
				   [NormalizedEmail], 
				   [NormalizedFirstName], 
				   [NormalizedLastName], 
				   [NormalizedUserName], 
				   [PasswordHash], 
				   [PhoneNumber], 
				   [PhoneNumberConfirmed], 
				   [SecurityStamp], 
				   [TwoFactorEnabled], 
				   [UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedUserName] LIKE @SearchTerm  OR 
				   [NormalizedEmail] LIKE @SearchTerm  OR 
				   [NormalizedFirstName] LIKE @SearchTerm  OR 
				   [NormalizedLastName] LIKE @SearchTerm)AND
				   [IsDeleted] = 1
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
	ELSE
		BEGIN
			DECLARE @FirstName nvarchar(256);
			DECLARE @FirstNameCount int;
			DECLARE @LastName nvarchar(256);

			SET @FirstName = left(@SearchTerm, charindex(' ', @SearchTerm) - 1) + N'%'
			SET @LastName = substring(@SearchTerm, charindex(' ', @SearchTerm) + 1, len(@SearchTerm))

			SELECT COUNT(*)
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName AND
				   [NormalizedLastName] LIKE @LastName)AND
				   [IsDeleted] = 1

			SELECT [Id],
					[AccessFailedCount], 
					[ConcurrencyStamp], 
					[Email], 
					[EmailConfirmed], 
					[FirstName], 
					[IsBlocked], 
					[IsDeleted], 
					[LastName], 
					[LockoutEnabled], 
					[LockoutEnd], 
					[NormalizedEmail], 
					[NormalizedFirstName], 
					[NormalizedLastName], 
					[NormalizedUserName], 
					[PasswordHash], 
					[PhoneNumber], 
					[PhoneNumberConfirmed], 
					[SecurityStamp], 
					[TwoFactorEnabled], 
					[UserName]
			FROM [AspNetUsers]
			WHERE ([NormalizedFirstName] LIKE @FirstName AND
				   [NormalizedLastName] LIKE @LastName) AND
					[IsDeleted] = 1
			ORDER BY [UserName]
			OFFSET @PageSize * @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY
		END
END

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "AspNetUsers"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 262
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ActiveUsers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ActiveUsers'
GO
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20171026080114_InitialSqlServerConfigurationDbMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20171026080706_InitialSqlServerIdentityDbMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20171026080756_InitialSqlServerOperationalDbMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20171026080835_InitialSqlServerExtendedConfigurationDbMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20171122162730_UserSearchOptimizationMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20171122163636_UserSearchOptimizationConfigurationDbMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20171122163718_UserSearchOptimizationExtendedConfigurationDbMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20171122163758_UserSearchOptimizationOperationalDbMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20180626100745_ConfigurationEntries','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20181109163923_IdentityServer2.3SqlServerConfigurationDbMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20181109164134_IdentityServer2.3SqlServerSqlServerOperationalDbMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20181205163055_ExtendedDataMigration2.3','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20190401104724_ClientType','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20190723135545_RoleSearchOptimizationMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20200702075849_V3toV4SqlServerConfigurationDbMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20200702080430_V3toV4SqlServerOperationalDbMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20200706104335_UserSearchOptimizationUpdateMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20200706104406_RoleSearchOptimizationUpdateMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20210105164242_DuendeSqlServerMigrationOperational','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20210106132452_DuendeSqlServerConfigurationMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20210430141851_EnumeratedClaimTypeMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20210602104947_IdentityProvidersSqlServerConfigurationMigration','3.1.15')
INSERT into  [dbo].[__EFMigrationsHistory] (MigrationId, ProductVersion) Values ('20210602110024_PersistedGrantConsumeTimeSqlServerOperationalMigration','3.1.15')
GO

CREATE TABLE [dbo].[AppCache](
	[Id] [nvarchar](449) NOT NULL,
	[Value] [varbinary](max) NOT NULL,
	[ExpiresAtTime] [datetimeoffset](7) NOT NULL,
	[SlidingExpirationInSeconds] [bigint] NULL,
	[AbsoluteExpiration] [datetimeoffset](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))
GO
/****** Object:  Index [Index_ExpiresAtTime]    Script Date: 9/10/2021 12:37:46 PM ******/
CREATE NONCLUSTERED INDEX [Index_ExpiresAtTime] ON [dbo].[AppCache]
(
	[ExpiresAtTime] ASC
)
GO
 
CREATE TABLE [dbo].[Logs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](128) NULL,
	[TimeStamp] [datetimeoffset](7) NOT NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [xml] NULL,
	[LogEvent] [nvarchar](max) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))
GO


/****** Object:  Index [IX_Logs_TimeStamp]    Script Date: 9/10/2021 12:39:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_Logs_TimeStamp] ON [dbo].[Logs]
(
	[TimeStamp] ASC
)
GO

/****** Object:  Index [IX_Logs_Level]    Script Date: 9/10/2021 12:40:17 PM ******/
CREATE NONCLUSTERED INDEX [IX_Logs_Level] ON [dbo].[Logs]
(
	[Level] ASC
)
GO



CREATE PROCEDURE [dbo].[PurgeIdentityLogs]
	@DaysToKeep [int]
WITH EXECUTE AS CALLER
AS
SET NOCOUNT ON

DECLARE @ERROR int
DECLARE @ROWCOUNT int
DECLARE @CUTOFFDATE datetime

SET @ROWCOUNT = 1
SET @CUTOFFDATE = DATEADD(day, -@DaysToKeep, GETDATE())

WHILE @ROWCOUNT > 0
BEGIN
  ;WITH CTE AS 
  (
    SELECT TOP 1000 Id
    FROM dbo.Logs WITH (READPAST)
    WHERE TimeStamp < @CUTOFFDATE
    ORDER BY TimeStamp
  )
  DELETE FROM CTE

  SELECT @ERROR = @@ERROR, @ROWCOUNT = @@ROWCOUNT
  
  IF @ERROR <> 0
  BEGIN
    RAISERROR('Error running dbo.PurgeIdentityLogs, @@ERROR=%d', 16, 1, @ERROR)
    RETURN @ERROR
  END

  WAITFOR DELAY '00:00:00.100'
END
GO


/****** Object:  Table [dbo].[AdGroups]    Script Date: 9/10/2021 12:45:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AdGroups](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_AdGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))
GO


IF EXISTS (
        SELECT type_desc, type
        FROM sys.procedures WITH(NOLOCK)
        WHERE NAME = 'LandstarCreateClient'
            AND type = 'P'
      )
     DROP PROCEDURE [dbo].[LandstarCreateClient]
GO

CREATE PROCEDURE [dbo].[LandstarCreateClient]
	@ClientID nvarchar(200),
	@ClientName nvarchar(200),
	@ClientDescription nvarchar(1000),
	@ClientURI nvarchar(2000),
	@SharedSecret nvarchar(100),
	@AuthMode varchar(10)  = 'lsol',
	@ConnectURL nvarchar(500),
	@PostLogoutRedirectUri nvarchar(500)
AS
BEGIN
BEGIN TRANSACTION
DECLARE @ID int
 
INSERT INTO [dbo].[Clients]
           ([Enabled]
           ,[ClientId]
           ,[ProtocolType]
           ,[RequireClientSecret]
           ,[ClientName]
           ,[Description]
           ,[ClientUri]
           ,[LogoUri]
           ,[RequireConsent]
           ,[AllowRememberConsent]
           ,[AlwaysIncludeUserClaimsInIdToken]
           ,[RequirePkce]
           ,[AllowPlainTextPkce]
           ,[RequireRequestObject]
           ,[AllowAccessTokensViaBrowser]
           ,[FrontChannelLogoutUri]
           ,[FrontChannelLogoutSessionRequired]
           ,[BackChannelLogoutUri]
           ,[BackChannelLogoutSessionRequired]
           ,[AllowOfflineAccess]
           ,[IdentityTokenLifetime]
           ,[AllowedIdentityTokenSigningAlgorithms]
           ,[AccessTokenLifetime]
           ,[AuthorizationCodeLifetime]
           ,[ConsentLifetime]
           ,[AbsoluteRefreshTokenLifetime]
           ,[SlidingRefreshTokenLifetime]
           ,[RefreshTokenUsage]
           ,[UpdateAccessTokenClaimsOnRefresh]
           ,[RefreshTokenExpiration]
           ,[AccessTokenType]
           ,[EnableLocalLogin]
           ,[IncludeJwtId]
           ,[AlwaysSendClientClaims]
           ,[ClientClaimsPrefix]
           ,[PairWiseSubjectSalt]
           ,[Created]
           ,[Updated]
           ,[LastAccessed]
           ,[UserSsoLifetime]
           ,[UserCodeType]
           ,[DeviceCodeLifetime]
           ,[NonEditable])
     VALUES
           (1
           ,@ClientId
           ,'oidc'
           ,1
           ,@ClientName
           ,@ClientDescription
           ,@ClientUri
           ,null
           ,0
           ,1
           ,0
           ,0
           ,0
           ,0
           ,1
           ,NULL
           ,1
           ,NULL
           ,0
           ,1
           ,3500
           ,NULL
           ,3600
           ,3600
           ,NULL
           ,2592000
           ,1296000
           ,1
           ,1
           ,0
           ,0
           ,0
           ,0
           ,1
           ,'client_'
           ,NULL
           ,GETDATE()
           ,GETDATE()
           ,NULL
           ,NULL
           ,NULL
           ,3600
           ,0)
  
SET @ID = SCOPE_IDENTITY()

 
INSERT INTO [dbo].[ClientCorsOrigins]
           ([Origin]
           ,[ClientId])
     VALUES
           (@ClientUri
           ,@id)

INSERT INTO [dbo].[ClientCorsOrigins]
           ([Origin]
           ,[ClientId])
     VALUES
           (@ConnectURL
           ,@id)

INSERT INTO [dbo].[ClientCorsOrigins]
           ([Origin]
           ,[ClientId])
     VALUES
           ('http://localhost:54595'
           ,@id)

INSERT INTO [dbo].[ClientCorsOrigins]
           ([Origin]
           ,[ClientId])
     VALUES
           ('http://localdev.landstaronline.com:54595'
           ,@id)

INSERT INTO [dbo].[ClientCorsOrigins]
           ([Origin]
           ,[ClientId])
     VALUES
           ('https://localdev.landstaronline.com:54596'
           ,@id)

INSERT INTO [dbo].[ClientGrantTypes]
           ([GrantType]
           ,[ClientId])
     VALUES
           ('implicit'
           ,@ID)

INSERT INTO [dbo].[ClientGrantTypes]
           ([GrantType]
           ,[ClientId])
     VALUES
           ('password'
           ,@ID)

INSERT INTO [dbo].[ClientGrantTypes]
           ([GrantType]
           ,[ClientId])
     VALUES
           ('client_credentials'
           ,@ID)

IF @AuthMode = 'lsol'
BEGIN
INSERT INTO [dbo].[ClientIdPRestrictions]
           ([Provider]
           ,[ClientId])
     VALUES
           ('oidc'
           ,@ID)
END
IF @AuthMode = 'adfs'
BEGIN
INSERT INTO [dbo].[ClientIdPRestrictions]
           ([Provider]
           ,[ClientId])
     VALUES
           ('oidc-adfs'
           ,@ID)
END
IF @AuthMode = 'local'
BEGIN
INSERT INTO [dbo].[ClientIdPRestrictions]
           ([Provider]
           ,[ClientId])
     VALUES
           ('local'
           ,@ID)
INSERT INTO [dbo].[ClientIdPRestrictions]
           ([Provider]
           ,[ClientId])
     VALUES
           ('idsrv'
           ,@ID)

END

INSERT INTO [dbo].[ClientPostLogoutRedirectUris]
           ([PostLogoutRedirectUri]
           ,[ClientId])
     VALUES
           (@PostLogoutRedirectUri
           ,@ID)

IF  @ClientUri is not null 
BEGIN

INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           (@ClientUri + '/assets/silent-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           (@ClientUri + '/assets/signin-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           (@ClientUri + '/auth/silent-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           (@ClientUri + '/auth/signin-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           (@ClientUri + '/signin-oidc'
           ,@ID)
END


INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localdev.landstaronline.com:54596/assets/silent-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localdev.landstaronline.com:54596/assets/signin-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localdev.landstaronline.com:54596/auth/silent-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localdev.landstaronline.com:54596/auth/signin-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localdev.landstaronline.com:54596/signin-oidc'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localdev.landstaronline.com:54595/assets/silent-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localdev.landstaronline.com:54595/assets/signin-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localdev.landstaronline.com:54595/auth/silent-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localdev.landstaronline.com:54595/auth/signin-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localdev.landstaronline.com:54595/signin-oidc'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localhost:54596/assets/silent-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localhost:54596/assets/signin-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localhost:54596/auth/silent-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localhost:54596/auth/signin-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localhost:54596/signin-oidc'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localhost:54595/assets/silent-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localhost:54595/assets/signin-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localhost:54595/auth/silent-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localhost:54595/auth/signin-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localhost:54595/signin-oidc'
           ,@ID)

INSERT INTO [dbo].[ClientScopes]
           (Scope
           ,[ClientId])
     VALUES
           ('api1'
           ,@ID)


INSERT INTO [dbo].[ClientScopes]
           (Scope
           ,[ClientId])
     VALUES
           ('email'
           ,@ID)
INSERT INTO [dbo].[ClientScopes]
           (Scope
           ,[ClientId])
     VALUES
           ('profile'
           ,@ID)
INSERT INTO [dbo].[ClientScopes]
           (Scope
           ,[ClientId])
     VALUES
           ('openid'
           ,@ID)
INSERT INTO [dbo].[ClientScopes]
           (Scope
           ,[ClientId])
     VALUES
           ('ptv'
           ,@ID)


INSERT INTO [dbo].[ClientSecrets]
           ([Value]
		   ,[Type]
		   ,[Created]
           ,[ClientId])
     VALUES
       (@SharedSecret
		   ,'SharedSecret'
		   , GETDATE()
           ,@ID)

Select * from Clients where Id = @ID
Select * from [dbo].[ClientClaims] where clientid = @ID
Select * from [dbo].[ClientCorsOrigins] where clientid = @ID
Select * from [dbo].[ClientGrantTypes] where clientid = @ID
Select * from [dbo].[ClientIdPRestrictions] where clientid = @ID
Select * from [dbo].[ClientPostLogoutRedirectUris] where clientid = @ID
 
Select * from [dbo].[ClientScopes] where clientid = @ID
Select * from [dbo].[ClientSecrets]  where clientid = @ID

COMMIT TRANSACTION
END 
 



