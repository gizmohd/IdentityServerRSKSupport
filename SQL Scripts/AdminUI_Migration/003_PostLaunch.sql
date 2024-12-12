USE identitydb
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


DECLARE @id int
select top 1 @id = id from Clients  where clientid = 'admin_ui'

update Clients  set enablelocallogin = 0 where id = @id
insert into [identitydb].[dbo].[ClientIdPRestrictions] (clientid, [provider]) values (@id, 'oidc-adfs')
INSERT into [dbo].[ClientProperties] (clientid, [key],[value]) values (@id, 'SkipLsolRulesCheck', 'true')

