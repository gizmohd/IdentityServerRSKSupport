IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ApiResources] (
        [Id] int NOT NULL IDENTITY,
        [Enabled] bit NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [DisplayName] nvarchar(200) NULL,
        [Description] nvarchar(1000) NULL,
        [AllowedAccessTokenSigningAlgorithms] nvarchar(100) NULL,
        [ShowInDiscoveryDocument] bit NOT NULL,
        [RequireResourceIndicator] bit NOT NULL,
        [Created] datetime2 NOT NULL,
        [Updated] datetime2 NULL,
        [LastAccessed] datetime2 NULL,
        [NonEditable] bit NOT NULL,
        CONSTRAINT [PK_ApiResources] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ApiScopes] (
        [Id] int NOT NULL IDENTITY,
        [Enabled] bit NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [DisplayName] nvarchar(200) NULL,
        [Description] nvarchar(1000) NULL,
        [Required] bit NOT NULL,
        [Emphasize] bit NOT NULL,
        [ShowInDiscoveryDocument] bit NOT NULL,
        [Created] datetime2 NOT NULL,
        [Updated] datetime2 NULL,
        [LastAccessed] datetime2 NULL,
        [NonEditable] bit NOT NULL,
        CONSTRAINT [PK_ApiScopes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [Clients] (
        [Id] int NOT NULL IDENTITY,
        [Enabled] bit NOT NULL,
        [ClientId] nvarchar(200) NOT NULL,
        [ProtocolType] nvarchar(200) NOT NULL,
        [RequireClientSecret] bit NOT NULL,
        [ClientName] nvarchar(200) NULL,
        [Description] nvarchar(1000) NULL,
        [ClientUri] nvarchar(2000) NULL,
        [LogoUri] nvarchar(2000) NULL,
        [RequireConsent] bit NOT NULL,
        [AllowRememberConsent] bit NOT NULL,
        [AlwaysIncludeUserClaimsInIdToken] bit NOT NULL,
        [RequirePkce] bit NOT NULL,
        [AllowPlainTextPkce] bit NOT NULL,
        [RequireRequestObject] bit NOT NULL,
        [AllowAccessTokensViaBrowser] bit NOT NULL,
        [FrontChannelLogoutUri] nvarchar(2000) NULL,
        [FrontChannelLogoutSessionRequired] bit NOT NULL,
        [BackChannelLogoutUri] nvarchar(2000) NULL,
        [BackChannelLogoutSessionRequired] bit NOT NULL,
        [AllowOfflineAccess] bit NOT NULL,
        [IdentityTokenLifetime] int NOT NULL,
        [AllowedIdentityTokenSigningAlgorithms] nvarchar(100) NULL,
        [AccessTokenLifetime] int NOT NULL,
        [AuthorizationCodeLifetime] int NOT NULL,
        [ConsentLifetime] int NULL,
        [AbsoluteRefreshTokenLifetime] int NOT NULL,
        [SlidingRefreshTokenLifetime] int NOT NULL,
        [RefreshTokenUsage] int NOT NULL,
        [UpdateAccessTokenClaimsOnRefresh] bit NOT NULL,
        [RefreshTokenExpiration] int NOT NULL,
        [AccessTokenType] int NOT NULL,
        [EnableLocalLogin] bit NOT NULL,
        [IncludeJwtId] bit NOT NULL,
        [AlwaysSendClientClaims] bit NOT NULL,
        [ClientClaimsPrefix] nvarchar(200) NULL,
        [PairWiseSubjectSalt] nvarchar(200) NULL,
        [UserSsoLifetime] int NULL,
        [UserCodeType] nvarchar(100) NULL,
        [DeviceCodeLifetime] int NOT NULL,
        [CibaLifetime] int NULL,
        [PollingInterval] int NULL,
        [CoordinateLifetimeWithUserSession] bit NULL,
        [Created] datetime2 NOT NULL,
        [Updated] datetime2 NULL,
        [LastAccessed] datetime2 NULL,
        [NonEditable] bit NOT NULL,
        CONSTRAINT [PK_Clients] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [IdentityProviders] (
        [Id] int NOT NULL IDENTITY,
        [Scheme] nvarchar(200) NOT NULL,
        [DisplayName] nvarchar(200) NULL,
        [Enabled] bit NOT NULL,
        [Type] nvarchar(20) NOT NULL,
        [Properties] nvarchar(max) NULL,
        [Created] datetime2 NOT NULL,
        [Updated] datetime2 NULL,
        [LastAccessed] datetime2 NULL,
        [NonEditable] bit NOT NULL,
        CONSTRAINT [PK_IdentityProviders] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [IdentityResources] (
        [Id] int NOT NULL IDENTITY,
        [Enabled] bit NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [DisplayName] nvarchar(200) NULL,
        [Description] nvarchar(1000) NULL,
        [Required] bit NOT NULL,
        [Emphasize] bit NOT NULL,
        [ShowInDiscoveryDocument] bit NOT NULL,
        [Created] datetime2 NOT NULL,
        [Updated] datetime2 NULL,
        [NonEditable] bit NOT NULL,
        CONSTRAINT [PK_IdentityResources] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ApiResourceClaims] (
        [Id] int NOT NULL IDENTITY,
        [ApiResourceId] int NOT NULL,
        [Type] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_ApiResourceClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApiResourceClaims_ApiResources_ApiResourceId] FOREIGN KEY ([ApiResourceId]) REFERENCES [ApiResources] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ApiResourceProperties] (
        [Id] int NOT NULL IDENTITY,
        [ApiResourceId] int NOT NULL,
        [Key] nvarchar(250) NOT NULL,
        [Value] nvarchar(2000) NOT NULL,
        CONSTRAINT [PK_ApiResourceProperties] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApiResourceProperties_ApiResources_ApiResourceId] FOREIGN KEY ([ApiResourceId]) REFERENCES [ApiResources] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ApiResourceScopes] (
        [Id] int NOT NULL IDENTITY,
        [Scope] nvarchar(200) NOT NULL,
        [ApiResourceId] int NOT NULL,
        CONSTRAINT [PK_ApiResourceScopes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApiResourceScopes_ApiResources_ApiResourceId] FOREIGN KEY ([ApiResourceId]) REFERENCES [ApiResources] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ApiResourceSecrets] (
        [Id] int NOT NULL IDENTITY,
        [ApiResourceId] int NOT NULL,
        [Description] nvarchar(1000) NULL,
        [Value] nvarchar(4000) NOT NULL,
        [Expiration] datetime2 NULL,
        [Type] nvarchar(250) NOT NULL,
        [Created] datetime2 NOT NULL,
        CONSTRAINT [PK_ApiResourceSecrets] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApiResourceSecrets_ApiResources_ApiResourceId] FOREIGN KEY ([ApiResourceId]) REFERENCES [ApiResources] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ApiScopeClaims] (
        [Id] int NOT NULL IDENTITY,
        [ScopeId] int NOT NULL,
        [Type] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_ApiScopeClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApiScopeClaims_ApiScopes_ScopeId] FOREIGN KEY ([ScopeId]) REFERENCES [ApiScopes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ApiScopeProperties] (
        [Id] int NOT NULL IDENTITY,
        [ScopeId] int NOT NULL,
        [Key] nvarchar(250) NOT NULL,
        [Value] nvarchar(2000) NOT NULL,
        CONSTRAINT [PK_ApiScopeProperties] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApiScopeProperties_ApiScopes_ScopeId] FOREIGN KEY ([ScopeId]) REFERENCES [ApiScopes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ClientClaims] (
        [Id] int NOT NULL IDENTITY,
        [Type] nvarchar(250) NOT NULL,
        [Value] nvarchar(250) NOT NULL,
        [ClientId] int NOT NULL,
        CONSTRAINT [PK_ClientClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClientClaims_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ClientCorsOrigins] (
        [Id] int NOT NULL IDENTITY,
        [Origin] nvarchar(150) NOT NULL,
        [ClientId] int NOT NULL,
        CONSTRAINT [PK_ClientCorsOrigins] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClientCorsOrigins_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ClientGrantTypes] (
        [Id] int NOT NULL IDENTITY,
        [GrantType] nvarchar(250) NOT NULL,
        [ClientId] int NOT NULL,
        CONSTRAINT [PK_ClientGrantTypes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClientGrantTypes_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ClientIdPRestrictions] (
        [Id] int NOT NULL IDENTITY,
        [Provider] nvarchar(200) NOT NULL,
        [ClientId] int NOT NULL,
        CONSTRAINT [PK_ClientIdPRestrictions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClientIdPRestrictions_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ClientPostLogoutRedirectUris] (
        [Id] int NOT NULL IDENTITY,
        [PostLogoutRedirectUri] nvarchar(400) NOT NULL,
        [ClientId] int NOT NULL,
        CONSTRAINT [PK_ClientPostLogoutRedirectUris] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClientPostLogoutRedirectUris_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ClientProperties] (
        [Id] int NOT NULL IDENTITY,
        [ClientId] int NOT NULL,
        [Key] nvarchar(250) NOT NULL,
        [Value] nvarchar(2000) NOT NULL,
        CONSTRAINT [PK_ClientProperties] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClientProperties_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ClientRedirectUris] (
        [Id] int NOT NULL IDENTITY,
        [RedirectUri] nvarchar(400) NOT NULL,
        [ClientId] int NOT NULL,
        CONSTRAINT [PK_ClientRedirectUris] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClientRedirectUris_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ClientScopes] (
        [Id] int NOT NULL IDENTITY,
        [Scope] nvarchar(200) NOT NULL,
        [ClientId] int NOT NULL,
        CONSTRAINT [PK_ClientScopes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClientScopes_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [ClientSecrets] (
        [Id] int NOT NULL IDENTITY,
        [ClientId] int NOT NULL,
        [Description] nvarchar(2000) NULL,
        [Value] nvarchar(4000) NOT NULL,
        [Expiration] datetime2 NULL,
        [Type] nvarchar(250) NOT NULL,
        [Created] datetime2 NOT NULL,
        CONSTRAINT [PK_ClientSecrets] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClientSecrets_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [IdentityResourceClaims] (
        [Id] int NOT NULL IDENTITY,
        [IdentityResourceId] int NOT NULL,
        [Type] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_IdentityResourceClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_IdentityResourceClaims_IdentityResources_IdentityResourceId] FOREIGN KEY ([IdentityResourceId]) REFERENCES [IdentityResources] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE TABLE [IdentityResourceProperties] (
        [Id] int NOT NULL IDENTITY,
        [IdentityResourceId] int NOT NULL,
        [Key] nvarchar(250) NOT NULL,
        [Value] nvarchar(2000) NOT NULL,
        CONSTRAINT [PK_IdentityResourceProperties] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_IdentityResourceProperties_IdentityResources_IdentityResourceId] FOREIGN KEY ([IdentityResourceId]) REFERENCES [IdentityResources] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ApiResourceClaims_ApiResourceId_Type] ON [ApiResourceClaims] ([ApiResourceId], [Type]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ApiResourceProperties_ApiResourceId_Key] ON [ApiResourceProperties] ([ApiResourceId], [Key]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ApiResources_Name] ON [ApiResources] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ApiResourceScopes_ApiResourceId_Scope] ON [ApiResourceScopes] ([ApiResourceId], [Scope]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE INDEX [IX_ApiResourceSecrets_ApiResourceId] ON [ApiResourceSecrets] ([ApiResourceId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ApiScopeClaims_ScopeId_Type] ON [ApiScopeClaims] ([ScopeId], [Type]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ApiScopeProperties_ScopeId_Key] ON [ApiScopeProperties] ([ScopeId], [Key]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ApiScopes_Name] ON [ApiScopes] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ClientClaims_ClientId_Type_Value] ON [ClientClaims] ([ClientId], [Type], [Value]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ClientCorsOrigins_ClientId_Origin] ON [ClientCorsOrigins] ([ClientId], [Origin]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ClientGrantTypes_ClientId_GrantType] ON [ClientGrantTypes] ([ClientId], [GrantType]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ClientIdPRestrictions_ClientId_Provider] ON [ClientIdPRestrictions] ([ClientId], [Provider]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ClientPostLogoutRedirectUris_ClientId_PostLogoutRedirectUri] ON [ClientPostLogoutRedirectUris] ([ClientId], [PostLogoutRedirectUri]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ClientProperties_ClientId_Key] ON [ClientProperties] ([ClientId], [Key]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ClientRedirectUris_ClientId_RedirectUri] ON [ClientRedirectUris] ([ClientId], [RedirectUri]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_Clients_ClientId] ON [Clients] ([ClientId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_ClientScopes_ClientId_Scope] ON [ClientScopes] ([ClientId], [Scope]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE INDEX [IX_ClientSecrets_ClientId] ON [ClientSecrets] ([ClientId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_IdentityProviders_Scheme] ON [IdentityProviders] ([Scheme]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_IdentityResourceClaims_IdentityResourceId_Type] ON [IdentityResourceClaims] ([IdentityResourceId], [Type]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_IdentityResourceProperties_IdentityResourceId_Key] ON [IdentityResourceProperties] ([IdentityResourceId], [Key]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    CREATE UNIQUE INDEX [IX_IdentityResources_Name] ON [IdentityResources] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220601191024_InitialIdentityServerConfigurationDbMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220601191024_InitialIdentityServerConfigurationDbMigration', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125700_ConfigurationV60')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'CoordinateLifetimeWithUserSession');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Clients] DROP COLUMN [CoordinateLifetimeWithUserSession];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125700_ConfigurationV60')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221026125700_ConfigurationV60', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125749_ConfigurationV61')
BEGIN
    ALTER TABLE [Clients] ADD [CoordinateLifetimeWithUserSession] bit NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125749_ConfigurationV61')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221026125749_ConfigurationV61', N'6.0.10');
END;
GO

COMMIT;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE TABLE [DeviceCodes] (
        [UserCode] nvarchar(200) NOT NULL,
        [DeviceCode] nvarchar(200) NOT NULL,
        [SubjectId] nvarchar(200) NULL,
        [SessionId] nvarchar(100) NULL,
        [ClientId] nvarchar(200) NOT NULL,
        [Description] nvarchar(200) NULL,
        [CreationTime] datetime2 NOT NULL,
        [Expiration] datetime2 NOT NULL,
        [Data] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_DeviceCodes] PRIMARY KEY ([UserCode])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE TABLE [Keys] (
        [Id] nvarchar(450) NOT NULL,
        [Version] int NOT NULL,
        [Created] datetime2 NOT NULL,
        [Use] nvarchar(450) NULL,
        [Algorithm] nvarchar(100) NOT NULL,
        [IsX509Certificate] bit NOT NULL,
        [DataProtected] bit NOT NULL,
        [Data] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Keys] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE TABLE [PersistedGrants] (
        [Id] bigint NOT NULL IDENTITY,
        [Key] nvarchar(200) NULL,
        [Type] nvarchar(50) NOT NULL,
        [SubjectId] nvarchar(200) NULL,
        [SessionId] nvarchar(100) NULL,
        [ClientId] nvarchar(200) NOT NULL,
        [Description] nvarchar(200) NULL,
        [CreationTime] datetime2 NOT NULL,
        [Expiration] datetime2 NULL,
        [ConsumedTime] datetime2 NULL,
        [Data] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_PersistedGrants] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE TABLE [ServerSideSessions] (
        [Id] int NOT NULL IDENTITY,
        [Key] nvarchar(100) NOT NULL,
        [Scheme] nvarchar(100) NOT NULL,
        [SubjectId] nvarchar(100) NOT NULL,
        [SessionId] nvarchar(100) NULL,
        [DisplayName] nvarchar(100) NULL,
        [Created] datetime2 NOT NULL,
        [Renewed] datetime2 NOT NULL,
        [Expires] datetime2 NULL,
        [Data] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ServerSideSessions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE UNIQUE INDEX [IX_DeviceCodes_DeviceCode] ON [DeviceCodes] ([DeviceCode]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE INDEX [IX_DeviceCodes_Expiration] ON [DeviceCodes] ([Expiration]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE INDEX [IX_Keys_Use] ON [Keys] ([Use]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE INDEX [IX_PersistedGrants_ConsumedTime] ON [PersistedGrants] ([ConsumedTime]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE INDEX [IX_PersistedGrants_Expiration] ON [PersistedGrants] ([Expiration]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_PersistedGrants_Key] ON [PersistedGrants] ([Key]) WHERE [Key] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE INDEX [IX_PersistedGrants_SubjectId_ClientId_Type] ON [PersistedGrants] ([SubjectId], [ClientId], [Type]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE INDEX [IX_PersistedGrants_SubjectId_SessionId_Type] ON [PersistedGrants] ([SubjectId], [SessionId], [Type]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE INDEX [IX_ServerSideSessions_DisplayName] ON [ServerSideSessions] ([DisplayName]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE INDEX [IX_ServerSideSessions_Expires] ON [ServerSideSessions] ([Expires]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE UNIQUE INDEX [IX_ServerSideSessions_Key] ON [ServerSideSessions] ([Key]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE INDEX [IX_ServerSideSessions_SessionId] ON [ServerSideSessions] ([SessionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    CREATE INDEX [IX_ServerSideSessions_SubjectId] ON [ServerSideSessions] ([SubjectId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_Grants')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220324152905_Grants', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_IdentityColumn')
BEGIN
    DROP INDEX [PersistedGrants_Id_index] ON [PersistedGrants];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_IdentityColumn')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PersistedGrants]') AND [c].[name] = N'Id');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [PersistedGrants] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [PersistedGrants] ALTER COLUMN [Id] bigint NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_IdentityColumn')
BEGIN
    CREATE INDEX [PersistedGrants_Id_index] ON [PersistedGrants] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220324152905_IdentityColumn')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220324152905_IdentityColumn', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125653_GrantsV60')
BEGIN
	IF OBJECT_ID(N'[ServerSideSessions]') IS NOT NULL
	BEGIN
		DROP TABLE [ServerSideSessions];
	END;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125653_GrantsV60')
BEGIN
    ALTER TABLE [PersistedGrants] DROP CONSTRAINT [PK_PersistedGrants];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125653_GrantsV60')
BEGIN
IF OBJECT_ID(N'[IX_PersistedGrants_Key]') IS NOT NULL
	BEGIN
		DROP INDEX [IX_PersistedGrants_Key] ON [PersistedGrants];
	END;
END;
GO

 

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125653_GrantsV60')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PersistedGrants]') AND [c].[name] = N'Key');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [PersistedGrants] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [PersistedGrants] ALTER COLUMN [Key] nvarchar(200) NOT NULL;
    ALTER TABLE [PersistedGrants] ADD DEFAULT N'' FOR [Key];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125653_GrantsV60')
BEGIN
    ALTER TABLE [PersistedGrants] ADD CONSTRAINT [PK_PersistedGrants] PRIMARY KEY ([Key]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125653_GrantsV60')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221026125653_GrantsV60', N'6.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125742_GrantsV61')
BEGIN
    ALTER TABLE [PersistedGrants] DROP CONSTRAINT [PK_PersistedGrants];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125742_GrantsV61')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PersistedGrants]') AND [c].[name] = N'Key');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [PersistedGrants] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [PersistedGrants] ALTER COLUMN [Key] nvarchar(200) NULL;
END;
GO

 

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125742_GrantsV61')
BEGIN
    ALTER TABLE [PersistedGrants] ADD CONSTRAINT [PK_PersistedGrants] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125742_GrantsV61')
BEGIN
    CREATE TABLE [ServerSideSessions] (
        [Id] int NOT NULL IDENTITY,
        [Key] nvarchar(100) NOT NULL,
        [Scheme] nvarchar(100) NOT NULL,
        [SubjectId] nvarchar(100) NOT NULL,
        [SessionId] nvarchar(100) NULL,
        [DisplayName] nvarchar(100) NULL,
        [Created] datetime2 NOT NULL,
        [Renewed] datetime2 NOT NULL,
        [Expires] datetime2 NULL,
        [Data] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ServerSideSessions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125742_GrantsV61')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_PersistedGrants_Key] ON [PersistedGrants] ([Key]) WHERE [Key] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125742_GrantsV61')
BEGIN
    CREATE INDEX [IX_ServerSideSessions_DisplayName] ON [ServerSideSessions] ([DisplayName]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125742_GrantsV61')
BEGIN
    CREATE INDEX [IX_ServerSideSessions_Expires] ON [ServerSideSessions] ([Expires]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125742_GrantsV61')
BEGIN
    CREATE UNIQUE INDEX [IX_ServerSideSessions_Key] ON [ServerSideSessions] ([Key]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125742_GrantsV61')
BEGIN
    CREATE INDEX [IX_ServerSideSessions_SessionId] ON [ServerSideSessions] ([SessionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125742_GrantsV61')
BEGIN
    CREATE INDEX [IX_ServerSideSessions_SubjectId] ON [ServerSideSessions] ([SubjectId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221026125742_GrantsV61')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221026125742_GrantsV61', N'6.0.10');
END;
GO

COMMIT;
GO

