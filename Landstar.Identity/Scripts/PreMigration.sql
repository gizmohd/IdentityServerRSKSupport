/*BEgin:  Only Run this script if the following migrations have not run as for some reason this column and table's migration from an earlier step was lost...:
20240419191250_dotnet8_rewrite
20240419191356_dotnet8_rewrite
20240419191511_dotnet8_rewrite
20240419201727_dotnet8_rewrite
.*/
BEGIN TRANSACTION
DECLARE @recordcnt int;
 Select @recordcnt=count(*) 
 from __EFMigrationsHistory 
 where MigrationId in ('20240419191250_dotnet8_rewrite','20240419191356_dotnet8_rewrite','20240419191511_dotnet8_rewrite','20240419201727_dotnet8_rewrite')
 
IF (@recordcnt = 0)
BEGIN
	IF EXISTS (
		SELECT 1 FROM sys.columns 
		WHERE Name = N'DisplayName' 
		AND Object_ID = Object_ID(N'dbo.AspNetClaimType', 'U')
	)
	BEGIN
	ALTER TABLE dbo.AspNetClaimTypes
		DROP COLUMN DisplayName
	END
 
 
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EnumClaimTypeAllowedValues]') AND type in (N'U'))
	BEGIN
	DROP TABLE [dbo].[EnumClaimTypeAllowedValues]
	END
END

COMMIT

/* END: Only Run this script if the above migrations have not run as for some reason this column and table's migration from an earlier step was lost...: */