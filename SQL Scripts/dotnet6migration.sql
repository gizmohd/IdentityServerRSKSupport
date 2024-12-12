

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


USE [identitydb];



GO
PRINT N'Altering Table [dbo].[Clients]...';


GO
ALTER TABLE [dbo].[Clients]
    ADD [CoordinateLifetimeWithUserSession] BIT NULL;


GO
PRINT N'Altering Table [dbo].[IdentityProviders]...';


GO
ALTER TABLE [dbo].[IdentityProviders] ALTER COLUMN [LastAccessed] DATETIME2 (7) NULL;
GO
ALTER TABLE [dbo].[IdentityProviders] ALTER COLUMN [Updated] DATETIME2 (7) NULL;
GO
ALTER TABLE [dbo].PersistedGrants ADD [Id] bigint identity;
GO
create index PersistedGrants_Id_index
    on PersistedGrants (Id)
go

GO
INSERT into __EFMigrationsHistory ( [MigrationId],[ProductVersion]) VALUES('20220601191024_InitialIdentityServerConfigurationDbMigration',	'6.0.5')
GO
INSERT into __EFMigrationsHistory ( [MigrationId],[ProductVersion]) VALUES('20220324152905_Grants',	'6.0.5')
GO
INSERT into __EFMigrationsHistory ( [MigrationId],[ProductVersion]) VALUES('20220607151147_updateSaml',	'6.0.5')

GO
PRINT N'Update complete.';


GO


