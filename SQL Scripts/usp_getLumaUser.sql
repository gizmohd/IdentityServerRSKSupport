---  Application:  LUMA Beta                                                  --
--  Object Name:  usp_getLumaUser                                             --
--  Object Type:  SQL Stored Procedure                                        --
--  Object Date:  08/29/2022                                                  --
--  Object Desc:  Get User details from lsOps based on lsunique id            --
--  Change History:                                                           --
--     08/29/2022  Padma Muthukumarasami, Landstar                            --
--               Initial object creation                                      -- 
 
USE [DBA]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[usp_getLumaUser]
	@lsUniqueId [int],
	@loginIdTxt [varchar](100) OUTPUT,
	@roleId [int] OUTPUT,
	@firstNameTxt [varchar](100) OUTPUT,
	@lastNameTxt [varchar](100) OUTPUT,
	@naturalKeyTxt [varchar](100) OUTPUT
WITH EXECUTE AS CALLER
AS
SELECT 
  @loginIdTxt = login_id_txt
 ,@roleId = role_id
 ,@firstNameTxt = first_name_txt
 ,@lastNameTxt= last_name_txt
 ,@naturalKeyTxt= natural_key_txt
 FROM (SELECT * FROM OPENQUERY (DATACENTERSQL,'SELECT 
u.login_id_txt,
u.lsunique_id,
ur.role_id ,
upper(ltrim(rtrim(u.first_name_txt))) as first_name_txt,
upper(ltrim(rtrim(u.last_name_txt))) as last_name_txt,
CASE WHEN SUBSTRING(UI.NATURAL_KEY_TXT, 1, 1) = '' '' THEN UI.NATURAL_KEY_TXT ELSE '' '' + UI.NATURAL_KEY_TXT END as natural_key_txt
 FROM lsopsdb.im.users u WITH (nolock)
                   LEFT JOIN lsopsdb.dbo.unique_ids ui WITH (nolock)
             ON u.lsunique_id = ui.lsunique_id   
       LEFT JOIN lsopsdb.im.userroles ur WITH (nolock)
              ON u.lsunique_id = ur.id
                 AND ur.role_id IN ( 2, 15 ) where ur.role_id is not null   AND ui.system_id = 2') t where t.lsunique_id=@lsUniqueId) u
GO