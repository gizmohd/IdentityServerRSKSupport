USE [identity]
GO
BEGIN TRANSACTION

DECLARE @ClientID nvarchar(200)= 'mulesoft'
DECLARE @ClientName nvarchar(200) = 'mulesoft'
DECLARE @ClientDescription nvarchar(1000) = ''
DECLARE @ClientUri nvarchar(2000) = 'http://client_application_url' -- if this is an Angular UI app set this to the full url to the app.  ie.. https://<app>.landstaronline.com
DECLARE @SharedSecret nvarchar(100) = newid()
DECLARE @UiApp bit = 1  --If this is an Angular UI app set this value to 1, 0 if it's just an engine
DECLARE @AuthMode varchar(10) = 'lsol' --Modes:  'lsol' Landstar Online Auth, 'adfs' Azure Active Directory, 'local' Local to Identity Server (ie: Landstar Blue)

DECLARE @ConnectURL nvarchar(500) = 'https://connectdev.landstaronline.com'
--DECLARE @ConnectURL nvarchar(500) = 'https://connectqa.landstaronline.com'
--DECLARE @ConnectURL nvarchar(500) = 'https://connect.landstaronline.com'

DECLARE @PostLogoutRedirectUri nvarchar(500) = 'https://dev.landstaronline.com/public/logoff.aspx'
--DECLARE @PostLogoutRedirectUri nvarchar(500) = 'https://qa.landstaronline.com/public/logoff.aspx'
--DECLARE @PostLogoutRedirectUri nvarchar(500) = 'https://www.landstaronline.com/public/logoff.aspx'


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
           ('http://localhost:4595'
           ,@id)

INSERT INTO [dbo].[ClientCorsOrigins]
           ([Origin]
           ,[ClientId])
     VALUES
           ('http://localdev.landstaronline.com:4595'
           ,@id)

INSERT INTO [dbo].[ClientCorsOrigins]
           ([Origin]
           ,[ClientId])
     VALUES
           ('https://localdev.landstaronline.com:4596'
           ,@id)
INSERT INTO [dbo].[ClientCorsOrigins]
           ([Origin]
           ,[ClientId])
     VALUES
           ('https://localhost:4596'
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

IF  @ClientUri is not null AND @UiApp = 1
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

IF @UiApp = 1 
BEGIN
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localdev.landstaronline.com:4596/assets/silent-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localdev.landstaronline.com:4596/assets/signin-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localdev.landstaronline.com:4596/auth/silent-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localdev.landstaronline.com:4596/auth/signin-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localdev.landstaronline.com:4596/signin-oidc'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localdev.landstaronline.com:4595/assets/silent-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localdev.landstaronline.com:4595/assets/signin-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localdev.landstaronline.com:4595/auth/silent-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localdev.landstaronline.com:4595/auth/signin-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localdev.landstaronline.com:4595/signin-oidc'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localhost:4596/assets/silent-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localhost:4596/assets/signin-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localhost:4596/auth/silent-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localhost:4596/auth/signin-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('https://localhost:4596/signin-oidc'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localhost:4595/assets/silent-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localhost:4595/assets/signin-callback.html'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localhost:4595/auth/silent-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localhost:4595/auth/signin-callback'
           ,@ID)
INSERT INTO [dbo].[ClientRedirectUris]
           ([RedirectUri]
           ,[ClientId])
     VALUES
           ('http://localhost:4595/signin-oidc'
           ,@ID)
END

INSERT INTO [dbo].[ClientScopes]
           (Scope
           ,[ClientId])
     VALUES
           ('api1'
           ,@ID)

IF @UiApp = 1 
BEGIN
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
END

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
 
 


