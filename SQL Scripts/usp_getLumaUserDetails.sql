--  Application:  LUMA Beta                                                   --
--  Object Name:  usp_getLumaUserDetails                                      --
--  Object Type:  SQL Stored Procedure                                        --
--  Object Date:  08/16/2022                                                  --
--  Object Desc:  Get User details based on lsunique id                       --
--  Change History:                                                           --
--     08/16/2022  Padma Muthukumarasami, Landstar                            --
--               Initial object creation                                      -- 
--     08/29/2022  Padma Muthukumarasami, Landstar                            --
--                  Updated SQL object name that is called                    --  
USE [UsersSysXRefDB]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[usp_getLumaUserDetails]
	@lsUniqueId [int]
WITH EXECUTE AS CALLER
AS
DECLARE @loginIdTxt VARCHAR(100),
        @roleId INT,
        @firstNameTxt VARCHAR(100),
        @lastNameTxt VARCHAR(100),
        @naturalKeyTxt VARCHAR(100)
exec dbo.usp_getLumaUser @lsUniqueId = @lsUniqueId,
                     @loginIdTxt = @loginIdTxt OUTPUT,
                     @roleId = @roleId OUTPUT,
                     @firstNameTxt = @firstNameTxt OUTPUT,
                     @lastNameTxt = @lastNameTxt OUTPUT,
                     @naturalKeyTxt = @naturalKeyTxt OUTPUT
IF Object_id('TempDB..#DriverMaster') IS NOT NULL 
DROP TABLE #drivermaster;

CREATE TABLE #drivermaster(
driverId INT PRIMARY KEY CLUSTERED,
driverFirstName VARCHAR(100),
driverLastName VARCHAR(100),
driverStatus VARCHAR(10),
driverFid VARCHAR(10),
driverSsn VARCHAR(10),
driverTractor VARCHAR(10),
hazmatDate INT,
bcoId INT,
bcoName VARCHAR(100),
dispatchNum VARCHAR(10),
domcileTerminal VARCHAR(10),
licenseExpiration INT,
licenseState VARCHAR(10),
tractorYear INT,
birthDate INT,
effectiveDate INT,
driverCity VARCHAR(100),
driverState VARCHAR(10),
terminateDate INT,
orientationDate INT) 

if @roleId = 15
begin
INSERT INTO #drivermaster(
driverId,
driverFirstName,
driverLastName,
driverStatus,
driverFid,
driverSsn,
driverTractor,
hazmatDate,
bcoId,
bcoName,
dispatchNum,
domcileTerminal,
licenseExpiration,
licenseState,
tractorYear,
birthDate,
effectiveDate,
driverCity,
driverState,
terminateDate,
orientationDate)
SELECT * FROM OPENQUERY(iseries,'
    select dadrv ,
           dagnam,
           dalnam ,
           dasts ,
		   dacnt ,
		   dassn,
		   daatr#,
		   drncds,
           cix_unique_id,
           ctanam,
           tradsp,
           ctadtm ,
		   traedt,
           tralst,
           trayr,
		   dabtdt,
		   daefdt,
		   dacty,
		   dast,
		   datrdt,
		   dadtor
		   from blue.rp_db.drmst d
 LEFT JOIN blue.rp_db.drmsts ds
            ON  ds.drdrvs =d.dadrv
        LEFT JOIN blue.rp_db.ctmst c
            ON c.ctactn = d.dacnt
        LEFT JOIN blue.rp_db.tractr t
            ON t.tratno = d.daatr#
        LEFT JOIN blue.rp_db.ctuniqid ce
            ON  ce.cix_fid =d.dacnt
    WHERE dasts <> ''Z''
          and daownr = ''N''') t
       WHERE Upper(ltrim(rtrim(t.dagnam))) = @firstNameTxt
          AND Upper(ltrim(rtrim(t.dalnam))) = @lastNameTxt
          and t.dacnt = @naturalKeyTxt
end
ELSE if @roleId = 2
begin
INSERT INTO #drivermaster(
driverId,
driverFirstName,
driverLastName,
driverStatus,
driverFid,
driverSsn,
driverTractor,
hazmatDate,
bcoId,
bcoName,
dispatchNum,
domcileTerminal,
licenseExpiration,
licenseState,
tractorYear,
birthDate,
effectiveDate,
driverCity,
driverState,
terminateDate,
orientationDate)
SELECT * FROM OPENQUERY(iseries,'select dadrv ,
           dagnam,
           dalnam ,
           dasts ,
		   dacnt ,
		   dassn,
		   daatr#,
		   drncds,
           cix_unique_id,
           ctanam,
           tradsp,
           ctadtm ,
		   traedt,
           tralst,
           trayr,
		   dabtdt,
		   daefdt,
		   dacty,
		   dast,
		   datrdt,
		   dadtor
	    from blue.rp_db.drmst d
 LEFT JOIN blue.rp_db.drmsts ds
            ON  ds.drdrvs =d.dadrv
        LEFT JOIN blue.rp_db.ctmst c
            ON c.ctactn = d.dacnt
        LEFT JOIN blue.rp_db.tractr t
            ON t.tratno = d.daatr#
        LEFT JOIN blue.rp_db.ctuniqid ce
            ON  ce.cix_fid =d.dacnt
			WHERE dasts <> ''Z''
          and daownr = ''Y''') t where t.dacnt = @naturalKeyTxt
end

CREATE NONCLUSTERED INDEX #drivermaster_firstName on #drivermaster (driverFirstName)
CREATE NONCLUSTERED INDEX #drivermaster_lastName on #drivermaster (driverLastName)
CREATE NONCLUSTERED INDEX #drivermaster_fid on #drivermaster (driverFid)
CREATE NONCLUSTERED INDEX #drivermaster_ssn on #drivermaster (driverSsn)
CREATE NONCLUSTERED INDEX #drivermaster_tractor on #drivermaster (driverTractor)

BEGIN
  WITH hc
    AS (SELECT hcontr#,
               htract#,
               hstatus,
               heffdat,
               hexpdat,
               Row_number() OVER (partition BY hcontr# ORDER BY hexpdat DESC) row_num
        FROM iseries.blue.rp_db.oahistpf oa JOIN #drivermaster ON oa.hcontr# = #drivermaster.driverFid
        WHERE CONVERT(DATE, Cast(heffdat AS VARCHAR(8)), 112) <= Getdate()
        
       ),
         drr
    AS (SELECT drrmss#,
               drrmdta,
               Row_number() OVER (partition BY drrmss#
                                  ORDER BY CONVERT(DATE, Cast(drrmdta AS VARCHAR(8)), 112) DESC
                                 ) AS row_num
        FROM iseries.blue.rp_db.drrmtn dr JOIN #drivermaster ON dr.drrmss#=#drivermaster.driverSsn
        WHERE drrmcde IN ( 'CABS', 'CABSREQ', 'CABS3YR', 'CABSWAV' )
              AND drrmcpy = 40
              AND drrmdta <> 0 
       ),
         dor
    AS (SELECT drrmss#,
               drrmdta,
               Row_number() OVER (partition BY drrmss#
                                  ORDER BY CONVERT(DATE, Cast(drrmdta AS VARCHAR(8)), 112) DESC
                                 ) AS row_num
        FROM iseries.blue.rp_db.drrmtn dr JOIN #drivermaster ON dr.drrmss#=#drivermaster.driverSsn
        WHERE drrmcde IN ( 'ORIENBD', 'ORIENSC' )
              AND drrmcpy = 40
              AND drrmdta <> 0 
       ),
         dcon
    as (SELECT drrmss#,
               drrmcom,
               Row_number() OVER (partition BY drrmss#
                                  ORDER BY CONVERT(DATE, Cast(drrmdta AS VARCHAR(8)), 112) DESC
                                 ) AS row_num
        FROM iseries.blue.rp_db.drrmtn dr JOIN #drivermaster ON dr.drrmss#=#drivermaster.driverSsn
        WHERE drrmcde = 'CONTACT'
              AND drrmcpy = 40
              AND drrmdta <> 0 
       ) select @loginIdTxt 'User Name',
           @lsUniqueId 'Landstar Online ID',
           d.driverId AS 'Operator ID',
          ltrim(rtrim( d.driverFirstName)) AS 'Operator First Name',
          ltrim(rtrim( d.driverLastName)) AS 'Operator Last Name',
		   CASE
               WHEN d.birthDate = 0 THEN
                   NULL
               ELSE
                   CONVERT(DATE, Cast(d.birthDate AS VARCHAR(8)), 112)
           END AS 'Operator Birth Date',
           CASE
               WHEN d.effectiveDate = 0 THEN
                   NULL
               ELSE
                   CONVERT(DATE, Cast(d.effectiveDate AS VARCHAR(8)), 112)
           END AS 'Operator Effective Start Date',
           ltrim(rtrim(d.driverCity)) AS 'Operator City',
           d.driverState AS 'Operator State',
           CASE
               WHEN d.terminateDate = 0 THEN
                   NULL
               ELSE
                   CONVERT(DATE, Cast(d.terminateDate AS VARCHAR(8)), 112)
           END AS 'Operator Terminated Date',
		    dcon.drrmcom as 'Operator Email Address',
         CASE
               WHEN d.hazmatDate=0 THEN
                   NULL
               ELSE
                   CONVERT(DATE, Cast(d.hazmatDate AS VARCHAR(8)), 112)
           END AS  'Hazmat Expiration Date',
           d.bcoId AS 'BCO ID',
           ltrim(rtrim(d.bcoName)) AS 'BCO Name',
           d.dispatchNum AS 'Dispatch Number',
           d.domcileTerminal AS 'Domicile Terminal Number',
           CASE
               WHEN hexpdat = 0 THEN
                   NULL
               ELSE
                   CONVERT(DATE, Cast(hexpdat AS VARCHAR(8)), 112)
           END AS 'Contractor Contract Renewal Date',
           hstatus AS 'Contractor Contract Status',
           CASE
               WHEN drr.drrmdta = 0 THEN
                   NULL
               ELSE
                   CONVERT(DATE, Cast(drr.drrmdta AS VARCHAR(8)), 112)
           END AS 'Last CABS Attended',
		   d.driverTractor AS 'Assigned Tractor Number',
           CASE
               WHEN d.licenseExpiration = 0 THEN
                   NULL
               ELSE
                   CONVERT(DATE, Cast(d.licenseExpiration AS VARCHAR(8)), 112)
           END  AS 'Tractor License Expiration',
           d.licenseState AS 'Tractor License State',
           d.tractorYear AS 'Tractor Year',
		   CASE
               WHEN
               (
                   SELECT Max(v) FROM (VALUES (d.orientationDate), (dor.drrmdta)) value (v)
               ) = 0 THEN
                   NULL
               ELSE
                   CONVERT(
                              DATE,
                              Cast(
                              (
                                  SELECT Max(v) FROM (VALUES (d.orientationDate), (dor.drrmdta)) value (v)
                              ) AS VARCHAR(8)),
                              112
                          )
           END AS 'Last Orientation Attended'
       from  #drivermaster d 
	   LEFT JOIN hc
            ON hc.hcontr# = d.driverFid
               AND hc.row_num = 1
        LEFT JOIN drr
            ON drr.drrmss# = d.driverSsn
               AND drr.row_num = 1
        LEFT JOIN dor
            ON  dor.drrmss# =d.driverSsn
               AND dor.row_num = 1
        LEFT JOIN dcon
            ON dcon.drrmss# =d.driverSsn
               AND dcon.row_num = 1
       END
	   drop table #drivermaster
GO


