
USE identitydb
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

BEGIN TRANSACTION 
BEGIN TRY

PRINT 'Migrating __EFMigrationsHistory'


MERGE [identitydb].[dbo].[__EFMigrationsHistory] as TARGET
USING [identity].[dbo].[__EFMigrationsHistory] AS SOURCE
ON (TARGET.MigrationID =  SOURCE.MigrationId)
WHEN NOT MATCHED BY TARGET
THEN INSERT(MigrationId, ProductVersion) VALUES (SOURCE.MigrationID, Source.ProductVersion);


PRINT 'Migrating  AdGroups'


/****** Object:  Table [dbo].[AdGroups]    Script Date: 9/10/2021 8:42:10 AM ******/
IF NOT EXISTS (SELECT * FROM [identitydb].[dbo].sysobjects WHERE name='AdGroups' and xtype='U')
CREATE TABLE [identitydb].[dbo].[AdGroups](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NULL,
 CONSTRAINT [PK_AdGroups] PRIMARY KEY CLUSTERED ([Id] ASC)); 


MERGE [identitydb].[dbo].[AdGroups] as TARGET
USING [identity].[dbo].[AdGroups] AS SOURCE
ON (TARGET.ID =  SOURCE.Id)
WHEN NOT MATCHED BY TARGET
THEN INSERT(Id, Name) VALUES (SOURCE.ID, Source.Name);

PRINT 'Migrating Clients'
SET IDENTITY_INSERT [identitydb].[dbo].[clients] ON;
MERGE [identitydb].[dbo].[clients] as TARGET
USING [identity].[dbo].[clients] AS SOURCE
ON (TARGET.ID =  SOURCE.Id)
WHEN MATCHED THEN 
    UPDATE SET  AbsoluteRefreshTokenLifetime=SOURCE.AbsoluteRefreshTokenLifetime,
				AccessTokenLifetime = SOURCE.AccessTokenLifetime, 
				AccessTokenType = SOURCE.AccessTokenType, 
				AllowAccessTokensViaBrowser = SOURCE.AllowAccessTokensViaBrowser,
                AllowedIdentityTokenSigningAlgorithms = SOURCE.AllowedIdentityTokenSigningAlgorithms, 
				AllowOfflineAccess = SOURCE.AllowOfflineAccess, 
				AllowPlainTextPkce = SOURCE.AllowPlainTextPkce, 
				AllowRememberConsent = SOURCE.AllowRememberConsent, 
				AlwaysIncludeUserClaimsInIdToken = SOURCE.AlwaysIncludeUserClaimsInIdToken,
				AlwaysSendClientClaims = SOURCE.AlwaysSendClientClaims, 
				AuthorizationCodeLifetime= SOURCE.AuthorizationCodeLifetime, 
				BackChannelLogoutSessionRequired = SOURCE.BackChannelLogoutSessionRequired, 
				BackChannelLogoutUri = SOURCE.BackChannelLogoutUri, 
				ClientClaimsPrefix = SOURCE.ClientClaimsPrefix, 
				ClientId = SOURCE.ClientId, 
				ClientName= SOURCE.ClientName, 
				ClientUri = SOURCE.ClientUri, 
				ConsentLifetime = SOURCE.ConsentLifetime, 
				Created = SOURCE.Created, 
				[Description]=SOURCE.[Description], 
				DeviceCodeLifetime = SOURCE.DeviceCodeLifetime, 
				[Enabled] = SOURCE.[Enabled], 
				EnableLocalLogin = SOURCE.EnableLocalLogin, 
				FrontChannelLogoutSessionRequired = SOURCE.FrontChannelLogoutSessionRequired, 
				FrontChannelLogoutUri = SOURCE.FrontChannelLogoutUri, 
				IdentityTokenLifetime = SOURCE.IdentityTokenLifetime, 
				IncludeJwtId = SOURCE.IncludeJwtId, 
				LastAccessed = SOURCE.LastAccessed, 
				LogoUri = SOURCE.LogoUri, 
				NonEditable = SOURCE.NonEditable, 
				PairWiseSubjectSalt = SOURCE.PairWiseSubjectSalt, 
				ProtocolType = SOURCE.ProtocolType, 
				RefreshTokenExpiration = SOURCE.RefreshTokenExpiration, 
				RefreshTokenUsage = SOURCE.RefreshTokenUsage, 
				RequireClientSecret = SOURCE.RequireClientSecret,
                RequireConsent = SOURCE.RequireConsent, 
				RequirePkce = SOURCE.RequirePkce, 
				RequireRequestObject = SOURCE.RequireRequestObject, 
				SlidingRefreshTokenLifetime = SOURCE.SlidingRefreshTokenLifetime, 
				UpdateAccessTokenClaimsOnRefresh = SOURCE.UpdateAccessTokenClaimsOnRefresh, 
				Updated = SOURCE.Updated, 
				UserCodeType = SOURCE.UserCodeType, 
				UserSsoLifetime = SOURCE.UserSsoLifetime
WHEN NOT MATCHED BY TARGET
THEN INSERT( AbsoluteRefreshTokenLifetime, AccessTokenLifetime, AccessTokenType, AllowAccessTokensViaBrowser,
                         AllowedIdentityTokenSigningAlgorithms, AllowOfflineAccess, AllowPlainTextPkce, AllowRememberConsent, AlwaysIncludeUserClaimsInIdToken,
                         AlwaysSendClientClaims, AuthorizationCodeLifetime, BackChannelLogoutSessionRequired, BackChannelLogoutUri, ClientClaimsPrefix, ClientId, 
						 ClientName, ClientUri, ConsentLifetime, Created, [Description], DeviceCodeLifetime, [Enabled], EnableLocalLogin, 
						 FrontChannelLogoutSessionRequired, FrontChannelLogoutUri, Id, IdentityTokenLifetime, IncludeJwtId, LastAccessed, LogoUri, NonEditable, 
						 PairWiseSubjectSalt, ProtocolType, RefreshTokenExpiration, RefreshTokenUsage, RequireClientSecret,
                         RequireConsent, RequirePkce, RequireRequestObject, SlidingRefreshTokenLifetime, UpdateAccessTokenClaimsOnRefresh, Updated, UserCodeType, 
						 UserSsoLifetime)
	 VALUES (SOURCE.AbsoluteRefreshTokenLifetime, SOURCE.AccessTokenLifetime, SOURCE.AccessTokenType, SOURCE.AllowAccessTokensViaBrowser,
                         SOURCE.AllowedIdentityTokenSigningAlgorithms, SOURCE.AllowOfflineAccess, SOURCE.AllowPlainTextPkce, SOURCE.AllowRememberConsent, SOURCE.AlwaysIncludeUserClaimsInIdToken, 
						 SOURCE.AlwaysSendClientClaims, SOURCE.AuthorizationCodeLifetime, SOURCE.BackChannelLogoutSessionRequired, SOURCE.BackChannelLogoutUri, SOURCE.ClientClaimsPrefix, SOURCE.ClientId, 
						 SOURCE.ClientName, SOURCE.ClientUri, SOURCE.ConsentLifetime, SOURCE.Created, SOURCE.[Description], SOURCE.DeviceCodeLifetime, SOURCE.[Enabled], SOURCE.EnableLocalLogin, 
						 SOURCE.FrontChannelLogoutSessionRequired, SOURCE.FrontChannelLogoutUri, SOURCE.Id, SOURCE.IdentityTokenLifetime, SOURCE.IncludeJwtId, SOURCE.LastAccessed, SOURCE.LogoUri, NonEditable, 
						 PairWiseSubjectSalt, ProtocolType, RefreshTokenExpiration, RefreshTokenUsage, RequireClientSecret, 
						 RequireConsent, RequirePkce, RequireRequestObject, SlidingRefreshTokenLifetime, UpdateAccessTokenClaimsOnRefresh, Updated, UserCodeType,
                         UserSsoLifetime);
;


SET IDENTITY_INSERT [identitydb].[dbo].[clients] OFF;

PRINT 'Migrating ClientClaims'
MERGE [identitydb].[dbo].[clientclaims] as TARGET
USING [identity].[dbo].[clientclaims] AS SOURCE
ON (TARGET.ClientID =  SOURCE.ClientID AND TARGET.[Type] = SOURCE.[Type] AND TARGET.[Value] = SOURCE.[Value])
WHEN NOT MATCHED BY TARGET
THEN INSERT( ClientId, [Type], [Value])
	 VALUES (SOURCE.ClientId, SOURCE.[Type], SOURCE.[Value]);


PRINT 'Migrating ClientCorsOrigins'
MERGE [identitydb].[dbo].[clientcorsorigins] as TARGET
USING [identity].[dbo].[clientcorsorigins] AS SOURCE
ON (TARGET.ClientID =  SOURCE.ClientID AND TARGET.ORIGIN = SOURCE.ORIGIN)
WHEN NOT MATCHED BY TARGET
THEN INSERT( ClientId, ORIGIN)
	 VALUES (SOURCE.ClientId, SOURCE.ORIGIN);

PRINT 'Migrating ClientGrantTypes'
MERGE [identitydb].[dbo].[clientgranttypes] as TARGET
USING [identity].[dbo].[clientgranttypes] AS SOURCE
ON (TARGET.ClientID =  SOURCE.ClientID AND TARGET.GrantType = SOURCE.GrantType)
WHEN NOT MATCHED BY TARGET
THEN INSERT( ClientId, GrantType)
	 VALUES (SOURCE.ClientId, SOURCE.GrantType);

PRINT 'Migrating ClientIdPRestrictions'
MERGE [identitydb].[dbo].[ClientIdPRestrictions] as TARGET
USING [identity].[dbo].[ClientIdPRestrictions] AS SOURCE
ON (TARGET.ClientID =  SOURCE.ClientID AND TARGET.[Provider] = SOURCE.[Provider])
WHEN NOT MATCHED BY TARGET
THEN INSERT( ClientId, [Provider])
	 VALUES (SOURCE.ClientId, SOURCE.[Provider]);


PRINT 'Migrating ClientPostLogoutRedirectUris'
MERGE [identitydb].[dbo].[ClientPostLogoutRedirectUris] as TARGET
USING [identity].[dbo].[ClientPostLogoutRedirectUris] AS SOURCE
ON (TARGET.ClientID =  SOURCE.ClientID AND TARGET.PostLogoutRedirectUri = SOURCE.PostLogoutRedirectUri)
WHEN NOT MATCHED BY TARGET
THEN INSERT( ClientId, PostLogoutRedirectUri)
	 VALUES (SOURCE.ClientId, SOURCE.PostLogoutRedirectUri);

PRINT 'Migrating ClientRedirectUris'
MERGE [identitydb].[dbo].[ClientRedirectUris] as TARGET
USING [identity].[dbo].[ClientRedirectUris] AS SOURCE
ON (TARGET.ClientID =  SOURCE.ClientID AND TARGET.RedirectUri = SOURCE.RedirectUri)
WHEN NOT MATCHED BY TARGET
THEN INSERT( ClientId, RedirectUri)
	 VALUES (SOURCE.ClientId, SOURCE.RedirectUri);

PRINT 'Migrating ClientRedirectUris Part 2'
MERGE [identitydb].[dbo].[ClientRedirectUris] as TARGET
USING (select id as ClientID, URIS.RedirectUri FROM [identity].[dbo].[Clients] C join (
		  SELECT Distinct [RedirectUri]
		  FROM [identity].[dbo].[ClientRedirectUris]
		  where   redirecturi like '%localhost%') as URIS on 1=1) AS SOURCE
ON (TARGET.ClientID =  SOURCE.ClientID AND TARGET.RedirectUri = SOURCE.RedirectUri)
WHEN NOT MATCHED BY TARGET
THEN INSERT( ClientId, RedirectUri)
	 VALUES (SOURCE.ClientId, SOURCE.RedirectUri);

PRINT 'Migrating ClientProperties'

MERGE [identitydb].[dbo].[ClientProperties] as TARGET
USING [identity].[dbo].[ClientProperties] AS SOURCE
ON (TARGET.ClientID =  SOURCE.ClientID AND TARGET.[Key] = SOURCE.[Key])
WHEN MATCHED AND TARGET.[Value] <> SOURCE.[Value]
THEN UPDATE SET TARGET.[Value] = SOURCE.[Value]
WHEN NOT MATCHED BY TARGET
THEN INSERT( ClientId, [Key],[Value])
	 VALUES (SOURCE.ClientId, SOURCE.[Key], SOURCE.[Value]);


PRINT 'Migrating ClientScopes'
MERGE [identitydb].[dbo].[ClientScopes] as TARGET
USING [identity].[dbo].[ClientScopes] AS SOURCE
ON (TARGET.ClientID =  SOURCE.ClientID AND TARGET.[Scope]= SOURCE.[Scope])
WHEN NOT MATCHED BY TARGET
THEN INSERT( ClientId, [Scope])
	 VALUES (SOURCE.ClientId,  SOURCE.[Scope]);


PRINT 'Migrating ClientSecrets'
MERGE [identitydb].[dbo].[ClientSecrets] as TARGET
USING [identity].[dbo].[ClientSecrets] AS SOURCE
ON (TARGET.ClientID =  SOURCE.ClientID AND TARGET.[Value]= SOURCE.[value])
WHEN NOT MATCHED BY TARGET
THEN INSERT( ClientId, [Description],[Value], Expiration, [Type], [Created])
	 VALUES (SOURCE.ClientId, SOURCE.[Description], SOURCE.[Value], Source.Expiration, source.[Type], source.[created]);

	 
PRINT 'Migrating [ApiResources]'
MERGE [identitydb].[dbo].[ApiResources] as TARGET
USING [identity].[dbo].[ApiResources] AS SOURCE
ON (TARGET.[Name] =  SOURCE.[Name])
WHEN NOT MATCHED BY TARGET
THEN INSERT([Description], DisplayName, [Enabled], [Name], Created, LastAccessed, NonEditable, Updated, AllowedAccessTokenSigningAlgorithms, ShowInDiscoveryDocument, RequireResourceIndicator)
	 VALUES (SOURCE.[Description], SOURCE.DisplayName, SOURCE.[Enabled], SOURCE.[Name], SOURCE.Created, SOURCE.LastAccessed, SOURCE.NonEditable, SOURCE.Updated, SOURCE.AllowedAccessTokenSigningAlgorithms, SOURCE.ShowInDiscoveryDocument, 0);


PRINT 'Migrating [ApiResourceClaims]'
MERGE [identitydb].[dbo].[ApiResourceClaims] as TARGET
USING [identity].[dbo].[ApiResourceClaims] AS SOURCE
ON (TARGET.[ApiResourceId] =  SOURCE.[ApiResourceId] AND TARGET.[Type] = SOURCE.[Type])
WHEN NOT MATCHED BY TARGET
THEN INSERT(ApiResourceId,[Type])
	 VALUES (SOURCE.ApiResourceId,SOURCE.[Type]);


PRINT 'Migrating [[ApiResourceProperties]]'
MERGE [identitydb].[dbo].[ApiResourceProperties] as TARGET
USING [identity].[dbo].[ApiResourceProperties] AS SOURCE
ON (TARGET.[ApiResourceId] =  SOURCE.[ApiResourceId] AND TARGET.[Key] = SOURCE.[Key] AND TARGET.[Value] = SOURCE.[Value])
WHEN NOT MATCHED BY TARGET
THEN INSERT(ApiResourceId,[Key],[Value])
	 VALUES (SOURCE.ApiResourceId,SOURCE.[key], SOURCE.[Value]);



PRINT 'Migrating [ApiResourceScopes]'
MERGE [identitydb].[dbo].[ApiResourceScopes] as TARGET
USING   (Select S.Scope, A2.ID as ApiResourceId from [identity].[dbo].[ApiResourceScopes] S inner join 
				  [identity].[dbo].[ApiResources] A on A.Id = S.ApiResourceID LEFT JOIN
				  [identitydb].[dbo].[ApiResources] A2 on A.[Name] = A2.Name)
				  AS SOURCE
ON (TARGET.[ApiResourceId] =  SOURCE.[ApiResourceId] AND TARGET.Scope= SOURCE.SCope )
WHEN NOT MATCHED BY TARGET
THEN INSERT(ApiResourceId,Scope)
	 VALUES (SOURCE.ApiResourceId,SOURCE.Scope);



PRINT 'Migrating [ApiScopes]'
SET IDENTITY_INSERT [identitydb].[dbo].[ApiScopes] ON;
MERGE [identitydb].[dbo].[ApiScopes] as TARGET
USING   [identity].[dbo].[ApiScopes] AS SOURCE   
ON (TARGET.[Id]=  SOURCE.Id )
WHEN NOT MATCHED BY TARGET
THEN  INSERT(Id, [Description], DisplayName, Emphasize, [Name], [Required], ShowInDiscoveryDocument, [Enabled])
	 VALUES (SOURCE.Id, SOURCE.[Description], SOURCE.DisplayName, SOURCE.Emphasize, SOURCE.[Name], SOURCE.[Required], SOURCE.ShowInDiscoveryDocument, SOURCE.[Enabled]);
SET IDENTITY_INSERT [identitydb].[dbo].[ApiScopes] OFF;


PRINT 'Migrating [ApiScopeClaims]'
SET IDENTITY_INSERT [identitydb].[dbo].[ApiScopeClaims] ON;
MERGE [identitydb].[dbo].[ApiScopeClaims] as TARGET
USING   [identity].[dbo].[ApiScopeClaims] AS SOURCE   
ON (TARGET.[Id]=  SOURCE.Id )
WHEN NOT MATCHED BY TARGET
THEN  INSERT(Id, ScopeId, [Type])
	 VALUES (SOURCE.Id, SOURCE.[ScopeId], SOURCE.[Type]);
SET IDENTITY_INSERT [identitydb].[dbo].[ApiScopeClaims] OFF;
 
PRINT 'Migrating [ApiScopeProperties]'
SET IDENTITY_INSERT [identitydb].[dbo].[ApiScopeProperties] ON;
MERGE [identitydb].[dbo].ApiScopeProperties as TARGET
USING   [identity].[dbo].ApiScopeProperties AS SOURCE   
ON (TARGET.[Id]=  SOURCE.Id )
WHEN NOT MATCHED BY TARGET
THEN  INSERT(Id, ScopeId, [Key], [Value])
	 VALUES (SOURCE.Id, SOURCE.[ScopeId], SOURCE.[Key], SOURCE.[Value]);
SET IDENTITY_INSERT [identitydb].[dbo].ApiScopeProperties OFF;

 

PRINT 'Migrating [IdentityResources]'

SET IDENTITY_INSERT [identitydb].[dbo].[IdentityResources] ON;
MERGE [identitydb].[dbo].[IdentityResources] as TARGET
USING   [identity].[dbo].[IdentityResources] AS SOURCE   
ON (TARGET.[Id]=  SOURCE.Id )
WHEN NOT MATCHED BY TARGET
THEN  INSERT(Id, [Enabled], [Name], DisplayName, [Description], [Required], Emphasize, ShowInDiscoveryDocument, Created, Updated, NonEditable)
	 VALUES (SOURCE.Id, SOURCE.[Enabled], SOURCE.[Name], SOURCE.DisplayName, SOURCE.[Description], SOURCE.[Required], SOURCE.Emphasize, SOURCE.ShowInDiscoveryDocument, SOURCE.Created, SOURCE.Updated, SOURCE.NonEditable);
SET IDENTITY_INSERT [identitydb].[dbo].[IdentityResources] OFF;

PRINT 'Migrating [IdentityResourceProperties]'

SET IDENTITY_INSERT [identitydb].[dbo].[IdentityResourceProperties] ON;
MERGE [identitydb].[dbo].[IdentityResourceProperties] as TARGET
USING   [identity].[dbo].[IdentityResourceProperties] AS SOURCE   
ON (TARGET.[Id]=  SOURCE.Id )
WHEN NOT MATCHED BY TARGET
THEN  INSERT(Id, IdentityResourceId, [Key],[Value])
	 VALUES (SOURCE.Id, SOURCE.IdentityResourceId, SOURCE.[Key],SOURCE.[Value]);
SET IDENTITY_INSERT [identitydb].[dbo].[IdentityResourceProperties] OFF;

PRINT 'Migrating [IdentityResourceClaims]'

MERGE [identitydb].[dbo].[IdentityResourceClaims] as TARGET
USING   [identity].[dbo].[IdentityResourceClaims] AS SOURCE   
ON (TARGET.[Id]=  SOURCE.Id )
WHEN NOT MATCHED BY TARGET
THEN  INSERT(IdentityResourceId, [Type])
	 VALUES (SOURCE.IdentityResourceId, SOURCE.[Type]);



PRINT 'Migrating [AspNetRoles]'
 
MERGE [identitydb].[dbo].[AspNetRoles] as TARGET
USING   (SELECT        CONVERT(NVARCHAR(450),Id) as Id, Name, NormalizedName, ConcurrencyStamp
FROM            [identity].dbo.AspNetRoles) AS SOURCE   
ON (TARGET.[ID]=  SOURCE.[ID])
WHEN NOT MATCHED BY TARGET
THEN  INSERT(Id, ConcurrencyStamp,  [Name], NormalizedName, Reserved)
	 VALUES (CONVERT(nvarchar(450),SOURCE.Id), SOURCE.ConcurrencyStamp,  SOURCE.[Name], SOURCE.NormalizedName,0);


PRINT 'Migrating [AspNetUsers]'
MERGE [identitydb].[dbo].[AspNetUsers] as TARGET
USING   (
	SELECT        CONVERT(NVARCHAR(450),Id) as Id, UserId, FirstName, LastName, ResetPasswordOnConfirmation, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, 
							 PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount,   UPPER(FirstName) NormalizedFirstName,UPPER(LastName) NormalizedLastName
	FROM            [identity].dbo.AspNetUsers
	WHERE        (UserName LIKE '%@%')

) AS SOURCE   
ON (TARGET.[ID]=  SOURCE.[ID])
WHEN NOT MATCHED BY TARGET
THEN  INSERT(Id, AccessFailedCount, ConcurrencyStamp, Email, EmailConfirmed, FirstName, IsBlocked, IsDeleted, LastName, LockoutEnabled, LockoutEnd, NormalizedEmail, NormalizedUserName, PasswordHash, PhoneNumber, 
                         PhoneNumberConfirmed, SecurityStamp, TwoFactorEnabled, UserName, NormalizedFirstName, NormalizedLastName)
	 VALUES (SOURCE.Id,SOURCE.AccessFailedCount, SOURCE.ConcurrencyStamp, SOURCE.Email, SOURCE.EmailConfirmed, SOURCE.FirstName, 0, 0, SOURCE.LastName, SOURCE.LockoutEnabled, SOURCE.LockoutEnd, SOURCE.NormalizedEmail, SOURCE.NormalizedUserName, SOURCE.PasswordHash, SOURCE.PhoneNumber, 
                         SOURCE.PhoneNumberConfirmed, SOURCE.SecurityStamp, SOURCE.TwoFactorEnabled, SOURCE.UserName, SOURCE.NormalizedFirstName, SOURCE.NormalizedLastName);

PRINT 'Migrating [AspNetUserRoles]'
 

MERGE [identitydb].[dbo].[AspNetUserRoles] as TARGET
USING   (
	SELECT        CONVERT(nvarchar(450),A.UserId) UserID, CONVERT(nvarchar(450),A.RoleId) RoleID
	FROM            [identity].dbo.AspNetUserRoles A INNER JOIN 
					[Identity].dbo.AspNetUsers U on U.Id = A.UserId
    WHERE	U.UserName like '%@%'

) AS SOURCE   
ON (TARGET.[UserId]=  SOURCE.UserId AND TARGET.RoleID = SOURCE.RoleId )
WHEN NOT MATCHED BY TARGET
THEN  INSERT(UserId, RoleId)
	 VALUES (SOURCE.UserID, SOURCE.RoleID);

PRINT 'Migrating [AspNetClaimTypes]';

MERGE [identitydb].[dbo].[AspNetClaimTypes] as TARGET
USING   (
	SELECT DISTINCT ClaimType FROM [identity].[dbo].[AspNetUserClaims]

) AS SOURCE   
ON (TARGET.[Name]=  SOURCE.ClaimType)
WHEN NOT MATCHED BY TARGET
THEN  INSERT (Id,ConcurrencyStamp, [Name],NormalizedName,[Required], Reserved, ValueType)
	 VALUES (NEWID(), NEWID(),SOURCE.ClaimType, UPPER(SOURCE.ClaimType), 0,0,0);


PRINT 'Migrating AspNetUserClaims'

MERGE [identitydb].[dbo].AspNetUserClaims as TARGET
USING   (
SELECT       C.UserId, C.ClaimType, C.ClaimValue
FROM         [identity].dbo.AspNetUserClaims AS C INNER JOIN
             [identitydb].dbo.AspNetUsers AS U ON U.Id = C.UserId 
             

) AS SOURCE   
ON (TARGET.[UserID]=  SOURCE.UserID AND TARGET.ClaimType = SOURCE.ClaimType and TARGET.ClaimValue = SOURCE.ClaimValue)
WHEN NOT MATCHED BY TARGET
THEN  INSERT (ClaimType, ClaimValue, UserId)
	 VALUES (SOURCE.ClaimType, SOURCE.ClaimValue, CONVERT(NVARCHAR(450),SOURCE.UserId));



PRINT 'Migrating [AspNetRoleClaims]'


MERGE [identitydb].[dbo].[AspNetRoleClaims] as TARGET
USING   [identity].[dbo].[AspNetRoleClaims] AS SOURCE   
ON (CONVERT(nvarchar(450),TARGET.[RoleID])=  SOURCE.RoleId AND TARGET.ClaimType=  SOURCE.ClaimType AND TARGET.ClaimValue=  SOURCE.ClaimValue )
WHEN NOT MATCHED BY TARGET
THEN  INSERT(RoleId, ClaimType, ClaimValue)
	 VALUES (SOURCE.RoleId, SOURCE.ClaimType, SOURCE.ClaimValue);

PRINT 'Migrating [AspNetUserLogins]'


MERGE [identitydb].[dbo].[AspNetUserLogins] as TARGET
USING   [identity].[dbo].[AspNetUserLogins] AS SOURCE   
ON (TARGET.[LoginProvider]=  SOURCE.[LoginProvider] AND TARGET.ProviderKey=  SOURCE.ProviderKey AND TARGET.UserID=  SOURCE.UserId)
WHEN NOT MATCHED BY TARGET
THEN  INSERT(LoginProvider, ProviderKey, ProviderDisplayName, UserID)
	 VALUES (SOURCE.LoginProvider, SOURCE.ProviderKey, SOURCE.ProviderDisplayName, SOURCE.UserID);

PRINT 'Migrating [PersistedGrants]'
MERGE [identitydb].[dbo].[PersistedGrants] as TARGET
USING   [identity].[dbo].[PersistedGrants] AS SOURCE   
ON (TARGET.[Key]=  SOURCE.[Key])
WHEN NOT MATCHED BY TARGET
THEN  INSERT([Key], ClientId, CreationTime, [Data], Expiration, SubjectId, [Type], SessionId, [Description], ConsumedTime)
	 VALUES (SOURCE.[Key], SOURCE.ClientId, SOURCE.CreationTime, SOURCE.[Data], SOURCE.Expiration, SOURCE.SubjectId, SOURCE.[Type],
			 SOURCE.SessionId, SOURCE.[Description], SOURCE.ConsumedTime);

PRINT 'Deleting Unused Siteminder Claims'
DELETE FROM [dbo].[AspNetUserClaims] where ClaimType in ('fedauth', 'smidentity', 'spsession', 'smsession', 'smroles', 'fedauthauthoring');
DELETE FROM [dbo].[AspNetClaimTypes] where [NAME] in ('fedauth', 'smidentity', 'spsession', 'smsession', 'smroles', 'fedauthauthoring');

PRINT 'Delete Duplicate values from ClientPostLogoutRedirectUris'
DELETE from [dbo].[ClientPostLogoutRedirectUris]
WHERE ID not in (
	select Min(id) from [dbo].[ClientPostLogoutRedirectUris] GROUP BY ClientID, PostLogoutRedirectURI
)

MERGE [dbo].[ClientPostLogoutRedirectUris] as TARGET
USING
--
	(select ID as ClientID, 'https://dev.landstaronline.com'  As PostLogoutRedirectUri from [identitydb].dbo.clients 
	UNION
	select ID as ClientID, 'https://qa.landstaronline.com' As PostLogoutRedirectUri from [identitydb].dbo.clients 
	UNION
	select ID as ClientID, 'https://www.landstaronline.com' As PostLogoutRedirectUri  from [identitydb].dbo.clients )
AS SOURCE
ON (TARGET.[ClientId]=  SOURCE.[ClientId] AND TARGET.PostLogoutRedirectUri = SOURCE.PostLogoutRedirectUri)
WHEN NOT MATCHED BY TARGET
THEN INSERT (ClientID, PostLogoutRedirectUri) VALUES (SOURCE.ClientId, SOURCE.PostLogoutRedirectUri);

Merge dbo.ClientCorsOrigins as TARGET 
USING (
SELECT ID as clientID, CLIENTURI FROM CLIENTS
WHERE CLIENTURI IS NOT NULL ) as SOURCE
ON TARGET.ClientID = SOURCE.ClientId and Target.Origin = SOURCE.ClientUri
WHEN NOT MATCHED BY TARGET
THEN INSERT (ClientId, Origin) VALUES (Source.ClientID, Source.ClientUri);

DECLARE @adminUiRole as varchar(100)
select @adminUiRole  = id from aspnetroles where name = 'AdminUI Administrator'
if (@adminuirole is null)
BEGIN
select * from [identitydb].[dbo].aspnetroles 
	insert into [identitydb].[dbo].aspnetroles ([Id],	[ConcurrencyStamp],	[Description],	[Name],	[NormalizedName],	[Reserved])
	values ('44128dee-95ff-4ccd-b7bd-ad336eb5c591',	'03a5e34d-9c59-4266-bee2-ec41d8ef7308',	'Administrator for AdminUI',	'AdminUI Administrator',	'ADMINUI ADMINISTRATOR',	1)
	set @adminUiRole = '44128dee-95ff-4ccd-b7bd-ad336eb5c591'
END
 
MERGE [identitydb].[dbo].aspnetuserroles as TARGET
USING   (
Select id from aspnetusers where email in ('dmcdaniel@landstar.com')
)AS SOURCE   
ON (TARGET.[USERID]=  SOURCE.[Id] AND TARGET.RoleId= @adminUiRole )
WHEN NOT MATCHED BY TARGET
THEN  INSERT(UserID,RoleId)
	 VALUES (SOURCE.Id, @AdminUiRole);

PRINT 'Adding User Manager Role and related configuration'
IF NOT EXISTS(select * from [dbo].[AspNetRoles] where [Name] = 'AdminUI UserAdmin')
BEGIN
INSERT into [dbo].[AspNetRoles] (id, concurrencystamp,[Name], [NormalizedName], [Reserved]) VALUES (newid(), newid(),'AdminUI UserAdmin', 'ADMINUI USERADMIN', 1)
END
IF NOT EXISTS(select * from [dbo].[ConfigurationEntries] where [Value] like  '%AdminUI UserAdmin%')
BEGIN
INSERT INTO [dbo].[ConfigurationEntries] ([Key],[Value]) VALUES ('policy','{"PolicyClaims":[{"Type":"role","Value":"AdminUI UserAdmin","Permission":1}],"Version":"11/30/2021 21:39:42 +00:00"}')
END
DELETE from aspnetclaimtypes where name not in (Select distinct claimtype from aspnetuserclaims)
PRINT 'Success'
COMMIT

PRINT 'Renaming any Duplicate Client Names to a unique value'
 update[dbo].[Clients] set clientname = clientname + '['+ clientid + ']' where 
 clientname in (
select  clientname from [dbo].[Clients]
group by clientname
having count(clientname) > 1
)

END TRY
BEGIN CATCH
	Print 'Error'
	  SELECT   
	    ERROR_LINE() AS ErrorLine
        ,ERROR_NUMBER() AS ErrorNumber  
       ,ERROR_MESSAGE() AS ErrorMessage;  
	ROLLBACK
END CATCH
