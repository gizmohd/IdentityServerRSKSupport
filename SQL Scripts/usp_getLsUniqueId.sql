--  Application:  LUMA BETA                                                        --
--  Stored Procedure Desc:  Get FID, first and last name based on  lsunique_id     --
--  Change History:                                                                --
--  06/16/2022  Padma Muthukumarasami, Landstar                                    --
--  Initial object creation                                                        --

USE [LSOpsDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE OR ALTER PROC [im].[usp_xr_getLsUniqueId]
	 @lsUniqueId INT
	,@loginIdTxt VARCHAR(100) OUTPUT
	,@roleId INT OUTPUT
	,@firstNameTxt VARCHAR(100) OUTPUT
	,@lastNameTxt VARCHAR(100) OUTPUT
	,@naturalKeyTxt VARCHAR(100) OUTPUT

	WITH EXECUTE AS CALLER
AS

SELECT 
  @loginIdTxt = login_id_txt
 ,@roleId = role_id
 ,@firstNameTxt = first_name_txt
 ,@lastNameTxt= last_name_txt
 ,@naturalKeyTxt= natural_key_txt
 FROM (SELECT 
u.login_id_txt,
u.lsunique_id,
ur.role_id ,
u.first_name_txt,
u.last_name_txt,
CASE WHEN SUBSTRING(UI.NATURAL_KEY_TXT, 1, 1) = ' ' THEN UI.NATURAL_KEY_TXT ELSE ' ' + UI.NATURAL_KEY_TXT END as natural_key_txt
 FROM im.users u WITH (nolock)
                   LEFT JOIN dbo.unique_ids ui WITH (nolock)
             ON u.lsunique_id = ui.lsunique_id   
       LEFT JOIN im.userroles ur WITH (nolock)
              ON u.lsunique_id = ur.id
                 AND ur.role_id IN ( 2, 15 ) where ur.role_id is not null   AND ui.system_id = 2    and u.lsunique_id=@lsUniqueId) t
GO