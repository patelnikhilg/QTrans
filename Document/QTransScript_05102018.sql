USE [master]
GO
/****** Object:  Database [QTrans]    Script Date: 10/5/2018 2:02:41 AM ******/
CREATE DATABASE [QTrans]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QTrans', FILENAME = N'E:\Projects\SQL Database\Transportation\QTrans.mdf' , SIZE = 6144KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'QTrans_log', FILENAME = N'E:\Projects\SQL Database\Transportation\QTrans_log.ldf' , SIZE = 9216KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [QTrans] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QTrans].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QTrans] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QTrans] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QTrans] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QTrans] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QTrans] SET ARITHABORT OFF 
GO
ALTER DATABASE [QTrans] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QTrans] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [QTrans] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QTrans] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QTrans] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QTrans] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QTrans] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QTrans] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QTrans] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QTrans] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QTrans] SET  DISABLE_BROKER 
GO
ALTER DATABASE [QTrans] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QTrans] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QTrans] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QTrans] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QTrans] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QTrans] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QTrans] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QTrans] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [QTrans] SET  MULTI_USER 
GO
ALTER DATABASE [QTrans] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QTrans] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QTrans] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QTrans] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [QTrans]
GO
/****** Object:  User [QTrans]    Script Date: 10/5/2018 2:02:43 AM ******/
CREATE USER [QTrans] FOR LOGIN [QTrans] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [QTrans]
GO
/****** Object:  UserDefinedTableType [dbo].[UT_BiddingDetails]    Script Date: 10/5/2018 2:02:44 AM ******/
CREATE TYPE [dbo].[UT_BiddingDetails] AS TABLE(
	[vehicleno] [smallint] NOT NULL,
	[capacity] [smallint] NOT NULL
)
GO
/****** Object:  StoredProcedure [dbo].[GenerateSPforInsertUpdateDelete]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[GenerateSPforInsertUpdateDelete] 
 @Schemaname Sysname = 'dbo' 
,@Tablename  Sysname 
,@ProcName     Sysname = '' 
,@IdentityInsert  bit  = 0  
AS 
 
SET NOCOUNT ON 
 
/* 
Parameters 
@Schemaname            - SchemaName to which the table belongs to. Default value 'dbo'. 
@Tablename            - TableName for which the procs needs to be generated. 
@ProcName            - Procedure name. Default is blank and when blank the procedure name generated will be sp_<Tablename> 
@IdentityInsert        - Flag to say if the identity insert needs to be done to the table or not if identity column exists in the table. 
                      Default value is 0. 
*/ 
 
DECLARE @PKTable TABLE 
( 
TableQualifier SYSNAME 
,TableOwner       SYSNAME 
,TableName       SYSNAME 
,ColumnName       SYSNAME 
,KeySeq           int 
,PKName           SYSNAME 
) 
 
INSERT INTO @PKTable 
EXEC sp_pkeys @Tablename,@Schemaname 
 
--SELECT * FROM @PKTable 
 
DECLARE @columnNames              VARCHAR(MAX) 
DECLARE @columnNamesWithDatatypes VARCHAR(MAX) 
DECLARE @InsertcolumnNames          VARCHAR(MAX) 
DECLARE @UpdatecolumnNames          VARCHAR(MAX) 
DECLARE @IdentityExists              BIT 
 
SELECT @columnNames = '' 
SELECT @columnNamesWithDatatypes = '' 
SELECT @InsertcolumnNames = '' 
SELECT @UpdatecolumnNames = '' 
SELECT @IdentityExists = 0 
 
DECLARE @MaxLen INT 
 
 
 
SELECT @MaxLen =  MAX(LEN(SC.NAME)) 
  FROM sys.schemas SCH 
  JOIN sys.tables  ST 
    ON SCH.schema_id =ST.schema_id 
  JOIN sys.columns SC 
    ON ST.object_id = SC.object_id 
 WHERE SCH.name = @Schemaname 
   AND ST.name  = @Tablename  
   AND SC.is_identity = CASE 
                        WHEN @IdentityInsert = 1 THEN SC.is_identity 
                        ELSE 0 
                        END 
   AND SC.is_computed = 0 
 
 
SELECT @columnNames = @columnNames + SC.name + ',' 
      ,@columnNamesWithDatatypes = @columnNamesWithDatatypes +'@' + SC.name  
                                                             + REPLICATE(' ',@MaxLen + 5 - LEN(SC.NAME)) + STY.name  
                                                             + CASE  
                                                               WHEN STY.NAME IN ('Char','Varchar') AND SC.max_length <> -1 THEN '(' + CONVERT(VARCHAR(4),SC.max_length) + ')' 
                                                               WHEN STY.NAME IN ('Nchar','Nvarchar') AND SC.max_length <> -1 THEN '(' + CONVERT(VARCHAR(4),SC.max_length / 2 ) + ')' 
                                                               WHEN STY.NAME IN ('Char','Varchar','Nchar','Nvarchar') AND SC.max_length = -1 THEN '(Max)' 
                                                               ELSE '' 
                                                               END  
                                                               + CASE 
                                                                 WHEN NOT EXISTS(SELECT 1 FROM @PKTable WHERE ColumnName=SC.name) THEN  ' = NULL,' + CHAR(13) 
                                                                 ELSE ',' + CHAR(13) 
                                                                 END 
       ,@InsertcolumnNames = @InsertcolumnNames + '@' + SC.name + ',' 
       ,@UpdatecolumnNames = @UpdatecolumnNames  
                             + CASE 
                               WHEN NOT EXISTS(SELECT 1 FROM @PKTable WHERE ColumnName=SC.name) THEN  
                                    CASE  
                                    WHEN @UpdatecolumnNames ='' THEN '' 
                                    ELSE '       ' 
                                    END +  SC.name +  + REPLICATE(' ',@MaxLen + 5 - LEN(SC.NAME)) + '= ' + '@' + SC.name + ',' + CHAR(13) 
                               ELSE '' 
                               END  
      ,@IdentityExists  = CASE  
                          WHEN SC.is_identity = 1 OR @IdentityExists = 1 THEN 1  
                          ELSE 0 
                          END 
  FROM sys.schemas SCH 
  JOIN sys.tables  ST 
    ON SCH.schema_id =ST.schema_id 
  JOIN sys.columns SC 
    ON ST.object_id = SC.object_id 
  JOIN sys.types STY 
    ON SC.user_type_id     = STY.user_type_id 
   AND SC.system_type_id = STY.system_type_id 
 WHERE SCH.name = @Schemaname 
   AND ST.name  = @Tablename 
   AND SC.is_identity = CASE 
                        WHEN @IdentityInsert = 1 THEN SC.is_identity 
                        ELSE 0 
                        END 
   AND SC.is_computed = 0 
 
DECLARE @InsertSQL VARCHAR(MAX) 
DECLARE @UpdateSQL VARCHAR(MAX) 
DECLARE @DeleteSQL VARCHAR(MAX) 
DECLARE @PKWhereClause VARCHAR(MAX) 
 
SELECT @PKWhereClause = '' 
 
SELECT @PKWhereClause = @PKWhereClause + ColumnName + ' = ' + '@' + ColumnName + CHAR(13) + '   AND '  
  FROM @PKTable 
ORDER BY KeySeq 
 
SELECT @columnNames          = SUBSTRING(@columnNames,1,LEN(@columnNames)-1) 
SELECT @InsertcolumnNames = SUBSTRING(@InsertcolumnNames,1,LEN(@InsertcolumnNames)-1) 
SELECT @UpdatecolumnNames = SUBSTRING(@UpdatecolumnNames,1,LEN(@UpdatecolumnNames)-2) 
SELECT @PKWhereClause      = SUBSTRING(@PKWhereClause,1,LEN(@PKWhereClause)-5) 
SELECT @columnNamesWithDatatypes = SUBSTRING(@columnNamesWithDatatypes,1,LEN(@columnNamesWithDatatypes)-2) 
SELECT @columnNamesWithDatatypes = @columnNamesWithDatatypes + ',' +  CHAR(13) + '@DMLFlag     VARCHAR(1)' 
 
 
SELECT @InsertSQL = 'INSERT INTO ' + @Schemaname +'.' + @Tablename  
                                   + CHAR(13) + '(' + @columnNames + ')' +  
                                   + CHAR(13) + 'SELECT ' + @InsertcolumnNames  
 
SELECT @DeleteSQL = 'DELETE FROM ' + @Schemaname +'.' + @Tablename  
                                   + CHAR(13) + ' WHERE ' + @PKWhereClause 
 
SELECT @UpdateSQL = 'UPDATE '  + @Schemaname +'.' + @Tablename  
                               + CHAR (13) + '   SET ' + @UpdatecolumnNames  
                               + CHAR (13) + ' WHERE ' + @PKWhereClause 
 
 IF LTRIM(RTRIM(@ProcName)) = ''  
    SELECT @ProcName = 'SP_' + @Tablename 
     
 PRINT 'IF OBJECT_ID(''' + @ProcName + ''',''P'') IS NOT NULL' 
 PRINT 'DROP PROC ' + @ProcName 
 PRINT 'GO' 
 PRINT 'CREATE PROCEDURE ' + @ProcName +  CHAR (13) +  @columnNamesWithDatatypes +  CHAR (13) + 'AS' +  CHAR (13) 
 PRINT 'IF @DMLFlag = ''I''' 
 PRINT 'BEGIN' 
 IF @IdentityExists = 1 AND @IdentityInsert = 1  
 PRINT 'SET IDENTITY_INSERT ' + @Schemaname + '.' + @Tablename + ' ON ' 
 PRINT '' 
 PRINT @InsertSQL 
 PRINT '' 
 IF @IdentityExists = 1 AND @IdentityInsert = 1  
 PRINT 'SET IDENTITY_INSERT ' + @Schemaname + '.' + @Tablename + ' OFF ' 
 PRINT 'END' 
 PRINT '' 
 PRINT 'IF @DMLFlag = ''U''' 
 PRINT 'BEGIN' 
 PRINT '' 
 PRINT @UpdateSQL 
 PRINT '' 
 PRINT 'END' 
  
 PRINT '' 
 PRINT 'IF @DMLFlag = ''D''' 
 PRINT 'BEGIN' 
 PRINT '' 
 PRINT @DeleteSQL 
 PRINT '' 
 PRINT 'END' 
 PRINT 'GO' 
  
SET NOCOUNT OFF 
 
 

GO
/****** Object:  StoredProcedure [dbo].[Usp_DeleteAreaPreference]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Usp_DeleteAreaPreference]@UserId          bigint,@CityId              intASBEGIN
	Delete from [dbo].[TblAreaPreference] where [UserId]=@UserId and [CityId]=@CityId
END

GO
/****** Object:  StoredProcedure [dbo].[Usp_ForgotUserLoginDetail]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_ForgotUserLoginDetail](@MobileNumber as varchar(15),@emailaddres as varchar(50),@Message as varchar(100) output)ASBEGINIf EXISTS (Select 1 From [dbo].[tblmstusers] where ([mobilenumber] = @MobileNumber OR [emailaddress] = @emailaddres ))BEGIN	Declare @num varchar(5)	 SELECT @num= cast(FLOOR(RAND()*(100000-5+1)+5) as varchar)	Update [dbo].[tblmstusers]  set password = 'QTrans.' + @num where ([mobilenumber] = @MobileNumber OR [emailaddress] = @emailaddres)	SET @Message='Update password successfully'	Select * From [dbo].[tblmstusers] where ([mobilenumber] = @MobileNumber OR [emailaddress] = @emailaddres )ENDELSEBEGIN	SET @Message='Records not found'ENDENDSELECT RAND(6);SELECT RAND()*(10-5)+5;SELECT FLOOR(RAND()*(100000-5+1)+5);
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetAreaPreferenceByUserId]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Usp_GetAreaPreferenceByUserId]@UserId          bigintASBEGIN
	Select preferenceId,
	UserId,
	CityName,
	ap.CityId,
	State,
	CreateDate from [dbo].[TblAreaPreference] ap inner join 
	[dbo].[TblMstCity] mc on ap.CityId=mc.CityId inner join
	[dbo].[TblMstState] ms on mc.StateId=ms.StateId
	where [UserId]=@UserId
END
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetBiddingDetailsByDtlPostingId]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[Usp_GetBiddingDetailsByDtlPostingId](@DtlpostingId as bigint)ASBEGINSELECT 	tmp.[postingid]
	,[posttype]
	--,[userid]
	,[soureaddress]
	,[destinationadress]
	,tmp.materialtypeid
	,[materialtype]
	,tmp.[description]
	,tmp.packagetypeid
	,[packagetype]
	,[packagetypedesc]	
	,[Src_State]
	,[Src_pincode]
	,[Src_city]
	,[Src_landMark]
	,[Dst_State]
	,[Dst_pincode]
	,[Dst_city]
	,[Dst_landMark]
	  ,dp.[dtlpostingid]      
      ,dp.[materialweight]
      ,dp.[materialphotos]
      ,dp.[packingdimension]
      ,dp.[numberpackets]
      ,dp.[vehicletype]
      ,dp.[novehicle]
      ,dp.[deliveryperiodday]
      ,dp.[pickupdatetime]
      ,dp.[postamount]
      ,dp.[onpickuppercentage]
      ,dp.[onunloadingpercentage]
      ,dp.[creditday]
      ,dp.[contractstartdatetime]
      ,dp.[contractenddatetime]
      ,dp.[ordertype]
      ,dp.[biddingactivatedatetime]
      ,dp.[biddingclosedatetime]
      ,dp.[poststatus]
      ,dp.[gprs]
      ,dp.[menpowerloading]
      ,dp.[menpowerunloading]
      ,dp.[transporterinsurance]
      ,dp.[tolltaxinclude]
      ,dp.[remark]
      ,dp.[loadingtype]
      ,dp.[createddate]
      ,dp.[modifydate]
FROM [dbo].[tblmstposting] tmp inner join [dbo].[TblMstMaterialtype] tmmt on tmp.materialtypeid = tmmt.materialtypeid inner join [dbo].[TblMstPackageType] tmpt on tmp.packagetypeid = tmpt.packagetypeid  inner join [dbo].[tbldtlposting] dp on tmp.postingid = dp.postingid  where  [dtlpostingid]=@DtlpostingIdEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetBiddingDetailsById]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_GetBiddingDetailsById](@biddingId as bigint)ASBEGINSELECT [biddingid]
      ,[dtlpostingid]
      ,[userid]
      ,[amount]
      ,[biddercomment]
      ,[status]
      ,[servicecharges]
      ,[paymentmethod]
      ,[rating]
      ,[cancellationreason]
      ,[createddate]
      ,[modifydate]
  FROM [dbo].[tblmstbidding] where  [biddingid]=@biddingIdEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetBiddingDetailsByUserId]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_GetBiddingDetailsByUserId](@userId as bigint)ASBEGINSELECT 	tmp.[postingid]
	,[posttype]
	--,[userid]
	,[soureaddress]
	,[destinationadress]
	,tmp.materialtypeid
	,[materialtype]
	,tmp.[description]
	,tmp.packagetypeid
	,[packagetype]
	,[packagetypedesc]	
	,[Src_State]
	,[Src_pincode]
	,[Src_city]
	,[Src_landMark]
	,[Dst_State]
	,[Dst_pincode]
	,[Dst_city]
	,[Dst_landMark]
	 ,dp.[amount]
      ,dp.[biddercomment]
      ,dp.[status]
      ,dp.[servicecharges]
      ,dp.[paymentmethod]
      ,dp.[rating]
      ,dp.[cancellationreason]
      ,dp.[createddate]
      ,dp.[modifydate]
FROM [dbo].[tblmstposting] tmp inner join [dbo].[TblMstMaterialtype] tmmt on tmp.materialtypeid = tmmt.materialtypeid inner join [dbo].[TblMstPackageType] tmpt on tmp.packagetypeid = tmpt.packagetypeid  inner join [dbo].[tblmstbidding] dp on tmp.postingid = dp.dtlpostingidwhere  dp.[UserId]=@userIdEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetCity]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[Usp_GetCity]ASBEGIN


SELECT [CityId]
      ,[StateId]
      ,[CityName]
  FROM [dbo].[TblMstCity]

END




GO
/****** Object:  StoredProcedure [dbo].[Usp_GetCompanyDetailsById]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_GetCompanyDetailsById](@CompanyId as bigint,@userId as bigint,@Message as varchar(100) output)ASBEGINSELECT [companyid]
      ,[companyname]
      ,[address]
      ,[telenumber]
      ,[alternettelnumber]
      ,[country]
      ,[state]
      ,[city]
      ,[userid]
      ,[companytype]
      ,[createddate]
      ,[modifydate]
  FROM [dbo].[tblmstcompany] where  [companyid]=@CompanyId or UserId=@userIdEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetListPostingByUserId]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_GetListPostingByUserId](@userId as bigint,@isPast as bit,@Message as varchar(100) output)ASBEGINIF(@isPast = 1)BEGINSELECT
	tmp.[postingid]
	,tdp.dtlpostingid
	,[posttype]
	,tmp.[userid]
	,[materialtype]
	,[packagetype]	
	,[Src_State]
	,[Src_city]
	,[soureaddress]
	,[Src_landMark]
	,[Src_pincode]
	,[destinationadress]
	,[Dst_State]
	,[Dst_city]
	,[Dst_landMark]
	,[Dst_pincode]
	,[materialweight]     
      ,vt.[vehicletype]
      ,[novehicle]
      ,[pickupdatetime]
      ,[postamount]
      ,[ordertype]
      ,[biddingactivatedatetime]
      ,[biddingclosedatetime]
      ,[poststatus]
	  ,Case when [LoadingType] = 1 then 'FullLoad' else 'PartialLoad' end as [LoadingType]
  FROM [dbo].[tblmstposting] tmp inner join [dbo].[TblMstMaterialtype] tmmt on tmp.materialtypeid = tmmt.materialtypeid inner join [dbo].[TblMstPackageType] tmpt on tmp.packagetypeid = tmpt.packagetypeid inner join [dbo].[tbldtlposting] tdp on tmp.postingid=tdp.postingidinner join [dbo].[TblMstVehicleType] vt on vt.vehicletypeid=tdp.vehicletypewhere  [userid]=@userId and [biddingclosedatetime] < getdate() ENDELSEBEGINSELECT
	tmp.[postingid]
	,tdp.dtlpostingid
	,[posttype]
	,[userid]
	,[materialtype]
	,[packagetype]	
	,[Src_State]
	,[Src_city]
	,[soureaddress]
	,[Src_landMark]
	,[Src_pincode]
	,[destinationadress]
	,[Dst_State]
	,[Dst_city]
	,[Dst_landMark]
	,[Dst_pincode]
	,[materialweight]
      ,vt.[vehicletype]
      ,[novehicle]
      ,[pickupdatetime]
      ,[postamount]
      ,[ordertype]
      ,[biddingactivatedatetime]
      ,[biddingclosedatetime]
      ,[poststatus]
	  ,Case when [LoadingType] = 1 then 'FullLoad' else 'PartialLoad' end as [LoadingType]
  FROM [dbo].[tblmstposting] tmp inner join [dbo].[TblMstMaterialtype] tmmt on tmp.materialtypeid = tmmt.materialtypeid inner join [dbo].[TblMstPackageType] tmpt on tmp.packagetypeid = tmpt.packagetypeid inner join [dbo].[tbldtlposting] tdp on tmp.postingid=tdp.postingidinner join [dbo].[TblMstVehicleType] vt on vt.vehicletypeid=tdp.vehicletypewhere  [userid]=@userId and [biddingclosedatetime] > getdate() ENDEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetMaterialType]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[Usp_GetMaterialType]ASBEGINSELECT [materialtypeid]
      ,[materialtype]
      ,[description]
  FROM [dbo].[TblMstMaterialtype]END
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetPackageType]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[Usp_GetPackageType]ASBEGINSELECT [packagetypeid]
      ,[packagetype]
      ,[description]
  FROM [dbo].[TblMstPackageType]END
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetPincode]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[Usp_GetPincode]ASBEGIN

SELECT [PincodeId]
      ,[CityId]
      ,[Pincode]
  FROM [dbo].[TblMstPincode]

END



GO
/****** Object:  StoredProcedure [dbo].[Usp_GetPostingById]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_GetPostingById](@postingId as bigint,@Message as varchar(10) output)ASBEGINSELECT 	[postingid]
	,[posttype]
	--,[userid]
	,[soureaddress]
	,[destinationadress]
	,[materialtype]
	,tmp.[description]
	,[packagetype]
	,[packagetypedesc]
	,[createddate]
	,[modifydate]
	,[Src_State]
	,[Src_pincode]
	,[Src_city]
	,[Src_landMark]
	,[Dst_State]
	,[Dst_pincode]
	,[Dst_city]
	,[Dst_landMark]
  FROM [dbo].[tblmstposting] tmp inner join [dbo].[TblMstMaterialtype] tmmt on tmp.materialtypeid = tmmt.materialtypeid   inner join [dbo].[TblMstPackageType] tmpt on tmp.packagetypeid = tmpt.packagetypeid     where  [postingid]=@postingIdEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetPostingDetailsById]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_GetPostingDetailsById](@postingId as bigint,@Message as varchar(10) output)ASBEGINSELECT 	[postingid]
	,[posttype]
	--,[userid]
	,[soureaddress]
	,[destinationadress]
	,tmp.materialtypeid
	,[materialtype]
	,tmp.[description]
	,tmp.packagetypeid
	,[packagetype]
	,[packagetypedesc]
	,[createddate]
	,[modifydate]
	,[Src_State]
	,[Src_pincode]
	,[Src_city]
	,[Src_landMark]
	,[Dst_State]
	,[Dst_pincode]
	,[Dst_city]
	,[Dst_landMark]
FROM [dbo].[tblmstposting] tmp inner join [dbo].[TblMstMaterialtype] tmmt on tmp.materialtypeid = tmmt.materialtypeid inner join [dbo].[TblMstPackageType] tmpt on tmp.packagetypeid = tmpt.packagetypeid  where  [postingid]=@postingIdSELECT [dtlpostingid]
      ,[postingid]
      ,[materialweight]
      ,[materialphotos]
      ,[packingdimension]
      ,[numberpackets]
      ,[vehicletype]
      ,[novehicle]
      ,[deliveryperiodday]
      ,[pickupdatetime]
      ,[postamount]
      ,[onpickuppercentage]
      ,[onunloadingpercentage]
      ,[creditday]
      ,[contractstartdatetime]
      ,[contractenddatetime]
      ,[ordertype]
      ,[biddingactivatedatetime]
      ,[biddingclosedatetime]
      ,[poststatus]
      ,[gprs]
      ,[menpowerloading]
      ,[menpowerunloading]
      ,[transporterinsurance]
      ,[tolltaxinclude]
      ,[remark]
      ,[loadingtype]
      ,[createddate]
      ,[modifydate]
  FROM [dbo].[tbldtlposting] where  [postingid]=@postingIdEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetPostingList]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_GetPostingList](@UserId as Bigint,@isPast as bit)ASBEGINIF(@isPast = 1)BEGINSELECT
	tmp.[postingid]
	,tdp.dtlpostingid
	,[posttype]
	,[materialtype]
	,[packagetype]	
	,[Src_State]
	,[Src_city]
	,[soureaddress]
	,[Src_landMark]
	,[Src_pincode]
	,[destinationadress]
	,[Dst_State]
	,[Dst_city]
	,[Dst_landMark]
	,[Dst_pincode]
	,[materialweight]
	 ,Case when [LoadingType] = 1 then 'FullLoad' else 'PartialLoad' end as [LoadingType]     
      ,vt.[vehicletype]
      ,[novehicle]
      ,[pickupdatetime]
      ,[postamount]
      ,[ordertype]
      ,[biddingactivatedatetime]
      ,[biddingclosedatetime]
      ,[poststatus]
	  ,[status]
	  ,[rating]
  FROM  [dbo].[tblmstposting] tmp inner join [dbo].[TblMstMaterialtype] tmmt on tmp.materialtypeid = tmmt.materialtypeid inner join [dbo].[TblMstPackageType] tmpt on tmp.packagetypeid = tmpt.packagetypeid inner join [dbo].[tbldtlposting] tdp on tmp.postingid=tdp.postingidinner join [dbo].[TblMstVehicleType] vt on vt.vehicletypeid=tdp.vehicletypeinner join [dbo].[tblmstbidding] mb on tdp.dtlpostingid=mb.dtlpostingidwhere  [biddingclosedatetime] < getdate() and poststatus=2 and mb.userid=@UserIdENDELSEBEGINSELECT
	tmp.[postingid]
	,tdp.dtlpostingid
	,[posttype]
	,[materialtype]
	,[packagetype]	
	,[Src_State]
	,[Src_city]
	,[soureaddress]
	,[Src_landMark]
	,[Src_pincode]
	,[destinationadress]
	,[Dst_State]
	,[Dst_city]
	,[Dst_landMark]
	,[Dst_pincode]
	,[materialweight]
	 ,Case when [LoadingType] = 1 then 'FullLoad' else 'PartialLoad' end as [LoadingType]
      ,vt.[vehicletype]
      ,[novehicle]
      ,[pickupdatetime]
      ,[postamount]
      ,[ordertype]
      ,[biddingactivatedatetime]
      ,[biddingclosedatetime]
      ,[poststatus]
	  ,[status]
	  ,[rating]
  FROM [dbo].[tblmstposting] tmp inner join [dbo].[TblMstMaterialtype] tmmt on tmp.materialtypeid = tmmt.materialtypeid inner join [dbo].[TblMstPackageType] tmpt on tmp.packagetypeid = tmpt.packagetypeid inner join [dbo].[tbldtlposting] tdp on tmp.postingid=tdp.postingidinner join [dbo].[TblMstVehicleType] vt on vt.vehicletypeid=tdp.vehicletypeLeft  join [dbo].[tblmstbidding] mb on tdp.dtlpostingid=mb.dtlpostingid and mb.userid=@UserIdwhere  [biddingclosedatetime] > getdate() and poststatus=1ENDEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetState]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_GetState]ASBEGINSELECT [StateId]
      ,[State]
  FROM [dbo].[TblMstState]END
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetTransportType]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[Usp_GetTransportType]ASBEGINSELECT [transporttypeid]
      ,[transporttype]
      ,[description]
      ,[Usertype]
  FROM [dbo].[tblmsttransporttype]END
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetTransportTypeByuserId]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_GetTransportTypeByuserId](@UserId bigint,@Message varchar(100) out)ASBEGINSELECT mtt.[transporttypeid]
      ,mtt.[transporttype]
      ,mtt.[description]
      ,mtt.[Usertype]
	  ,case when modifydate is null then dt.createddate else dt.modifydate end modifydate
  FROM [dbo].[TblDtlUsers] as dt inner join [dbo].[tblmsttransporttype] as mtt  on dt.[transporttypeid]= mtt.[transporttypeid]  where dt.userid=@UserId  Set @Message = 'OK'END
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetUserDetailsById]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_GetUserDetailsById](@UserId as bigint,@Message as varchar(10) output)ASBEGINSELECT [userid]
      ,[emailaddress]
      ,[firstname]
      ,[middlename]
      ,[lastname]
      ,[mobilenumber]
      ,[landlinenumber]
      ,[dob]
      ,[addressline1]
      ,[addressline2]
      ,[pincode]
      ,[photo]
      ,[country]
      ,[state]
      ,[district]
      ,[city]
      ,[area]
      ,[mobileverification]
      ,[emailverification]
      ,[pan]
      ,[gst]
      ,[aadhaarno]
      ,[createddate]
      ,[modifydate]
	  ,Token
       from [dbo].[tblmstusers] where  userid=@UserIdEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetUserDetailsBytoken]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[Usp_GetUserDetailsBytoken](@token as varchar(100),@Message as varchar(10) output)ASBEGINSELECT [userid]
      ,[emailaddress]
      ,[firstname]
      ,[middlename]
      ,[lastname]
      ,[mobilenumber]
      ,[landlinenumber]
      ,[dob]
      ,[addressline1]
      ,[addressline2]
      ,[pincode]
      ,[photo]
      ,[country]
      ,[state]
      ,[district]
      ,[city]
      ,[area]
      ,[mobileverification]
      ,[emailverification]
      ,[pan]
      ,[gst]
      ,[aadhaarno]
      ,[createddate]
      ,[modifydate]
	  ,Token
       from [dbo].[tblmstusers] where  token=@tokenEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetUserDetailsByUserLogin]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_GetUserDetailsByUserLogin](@username as varchar(50),@password as varchar(10),@Message as varchar(10) output)ASBEGINSELECT [userid]
      ,[emailaddress]
      ,[firstname]
      ,[middlename]
      ,[lastname]
      ,[mobilenumber]
      ,[landlinenumber]
      ,[dob]
      ,[addressline1]
      ,[addressline2]
      ,[pincode]
      ,[photo]
      ,[country]
      ,[state]
      ,[district]
      ,[city]
      ,[area]
      ,[mobileverification]
      ,[emailverification]
      ,[pan]
      ,[gst]
      ,[aadhaarno]
      ,[createddate]
      ,[modifydate]
       from [dbo].[tblmstusers] where  ([mobilenumber] = @username OR  [emailaddress] = @username ) and [password] = @passwordEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetVehicleType]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[Usp_GetVehicleType]ASBEGINSELECT [vehicletypeid]
      ,[vehicletype]
      ,[description]
  FROM [dbo].[TblMstVehicleType]END
GO
/****** Object:  StoredProcedure [dbo].[Usp_InsertAreaPreference]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Usp_InsertAreaPreference]@UserId          bigint,@CityId              intASBEGIN
INSERT INTO [dbo].[TblAreaPreference]
           ([UserId]
           ,[CityId]
           )
     VALUES
           (@UserId, 
           @CityId)
END

GO
/****** Object:  StoredProcedure [dbo].[Usp_InsertBidding]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_InsertBidding](@dtlpostingid as bigint,@userid as bigint,@amount as decimal,@biddercomment as varchar(500) = Null,@status as smallint,@servicecharges as decimal,@paymentmethod as smallint = Null,@rating as smallint = Null,@cancellationreason as varchar(200) = Null,@createddate as datetime,@modifydate as datetime = Null,@biddingDetails [UT_BiddingDetails] readonly,@Message as varchar(100) output)ASBEGIN	-- Author: Auto	-- Created: 16 Aug 2018	-- Function: Inserts a dbo.tblmstbidding table record	-- Modifications:	begin transaction	begin try	Declare @identity bigint	-- insert	insert [dbo].[tblmstbidding] (dtlpostingid,userid,amount,biddercomment,status,servicecharges,paymentmethod,rating,cancellationreason,createddate,modifydate)	values (@dtlpostingid,@userid,@amount,@biddercomment,@status,@servicecharges,@paymentmethod,@rating,@cancellationreason,@createddate,@modifydate)	-- Return the new ID	set @identity= SCOPE_IDENTITY();	INSERT INTO [dbo].[tbldtlbidding]
			   ([vehicleno]
			   ,[capacity]
			   ,[biddingid])
	   Select vehicleno,capacity,@identity from @biddingDetails	commit transaction	end try	begin catch	 declare @ErrorMessage NVARCHAR(4000);	 declare @ErrorSeverity INT;	 declare @ErrorState INT;	 select @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();	 raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);	 rollback transaction	end catch;END
GO
/****** Object:  StoredProcedure [dbo].[Usp_InsertContact]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Usp_InsertContact]@Name           varchar(30),@MobileNo              varchar(12),@EmailAddress            varchar(50),@State     varchar(20),@city                  varchar(20),@Message as varchar(500) ASBEGIN
INSERT INTO [dbo].[TblContact]
           ([Name]
           ,[MobileNo]
           ,[EmailAddress]
           ,[State]
           ,[City]
           ,[Message]
           )
     VALUES
           (@Name
           ,@MobileNo
           ,@EmailAddress
           ,@State
           ,@city
           ,@Message)

END
 
 

GO
/****** Object:  StoredProcedure [dbo].[Usp_InsertUpdateCompany]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Usp_InsertUpdateCompany]@companyid			   Bigint,@companyname           varchar(100),@address               varchar(20),@telenumber            varchar(20),@alternettelnumber     varchar(20) = NULL,@country               varchar(15),@state                 varchar(20),@city                  varchar(20),@userid                bigint,@companytype            int,@identity				Bigint output,@Message as varchar(100) outputASBEGIN
IF Exists(Select 1 from [dbo].[tblmstcompany] where companyid = @companyid and userid=@UserID)
BEGIN


UPDATE dbo.tblmstcompany   SET companyname           = @companyname,       address               = @address,       telenumber            = @telenumber,       alternettelnumber     = @alternettelnumber,       country               = @country,       state                 = @state,       city                  = @city,              companytype            =case when @companyname != 'NA' then 1 else 0 end,       modifydate            = getdate() WHERE companyid = @companyid
 SET @Message = 'Successfully Updated'
 END
 ELSE
 BEGIN

INSERT INTO dbo.tblmstcompany(companyname,address,telenumber,alternettelnumber,country,state,city,userid,companytype)SELECT @companyname,@address,@telenumber,@alternettelnumber,@country,@state,@city,@userid,case when @companyname != 'NA' then 1 else 0 end
 
 set @identity= SCOPE_IDENTITY();
 SET @Message = 'Successfully Inserted'
END
 
 
END

GO
/****** Object:  StoredProcedure [dbo].[usp_InsertUpdatePosting]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_InsertUpdatePosting](@postingid bigint,@posttype as varchar(20),@userid as bigint,@soureaddress as varchar(200),@destinationadress as varchar(200),@materialtypeid as smallint,@description as varchar(max) = Null,@packagetypeid as smallint,@packagetypedesc as varchar(200) = Null,@src_pincode int,
@src_state varchar(20),
@src_city varchar(20),
@src_landmark varchar(30),@dst_pincode int,
@dst_state varchar(20),
@dst_city varchar(20),
@dst_landmark varchar(30),@identity as bigint output,@Message as varchar(100) output)ASBEGIN	IF Exists(Select 1 from [dbo].[tblmstposting] where postingid=@postingid and userid=@UserID)
	BEGIN	UPDATE [dbo].[tblmstposting]
	   SET [posttype] = @posttype      
		  ,[soureaddress] = @soureaddress
		  ,[destinationadress] = @destinationadress
		  ,[materialtypeid] = @materialtypeid
		  ,[description] = @description
		  ,[packagetypeid] = @packagetypeid
		  ,[packagetypedesc] = @packagetypedesc  
		  ,[src_state]=@src_state
		  ,[src_city]=@src_city
		  ,[src_pincode]=@src_pincode
		  ,[src_landmark]=@src_landmark  
		  ,[dst_state]=@dst_state
		  ,[dst_city]=@dst_city
		  ,[dst_pincode]=@dst_pincode
		  ,[dst_landmark]=@dst_landmark    
		  ,[modifydate] = getdate() where postingid=@postingid and userid=@UserID		  set @identity= @postingid;	END	ELSE	BEGIN	-- insert	insert [dbo].[tblmstposting] (posttype,userid,soureaddress,destinationadress,materialtypeid,description,packagetypeid,packagetypedesc	,[src_state],[src_city],[src_pincode],[src_landmark],[dst_state],[dst_city],[dst_pincode],[dst_landmark])	values (@posttype,@userid,@soureaddress,@destinationadress,@materialtypeid,@description,@packagetypeid,@packagetypedesc	,@src_state,@src_city,@src_pincode,@src_landmark  ,@dst_state,@dst_city,@dst_pincode,@dst_landmark )	-- Return the new ID	set @identity= SCOPE_IDENTITY();	END	END
GO
/****** Object:  StoredProcedure [dbo].[Usp_InsertUpdatePostingDetails]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Usp_InsertUpdatePostingDetails]@dtlpostingid				bigint,@postingid                   bigint ,@materialweight              int,@materialphotos              varchar(100) = NULL,@packingdimension            varchar(50) = NULL,@numberpackets                int,@vehicletype                 smallint,@novehicle                   smallint,@deliveryperiodday           smallint = NULL,@pickupdatetime              datetime ,@postamount                  decimal = NULL,@onpickuppercentage          smallint ,@onunloadingpercentage       smallint ,@creditday                   int ,@contractstartdatetime       datetime = NULL,@contractenddatetime         datetime = NULL,@ordertype                   smallint,@biddingactivatedatetime     datetime,@biddingclosedatetime        datetime,@poststatus                  smallint,@gprs                        bit,@menpowerloading             bit,@menpowerunloading           bit,@transporterinsurance        bit,@tolltaxinclude              bit,@remark                      varchar(200) = NULL,@loadingtype                 bit,@identity as bigint output,@Message as varchar(100) outputASBeginIF Exists(Select 1 from [dbo].[tbldtlposting] where dtlpostingid = @dtlpostingid and postingid = @postingid)
	BEGIN
 
UPDATE dbo.tbldtlposting   SET        materialweight              = @materialweight,       materialphotos              = @materialphotos,       packingdimension            = @packingdimension,       numberpackets                = @numberpackets,       vehicletype                 = @vehicletype,       novehicle                   = @novehicle,       deliveryperiodday           = @deliveryperiodday,       pickupdatetime              = @pickupdatetime,       postamount                  = @postamount,       onpickuppercentage          = @onpickuppercentage,       onunloadingpercentage       = @onunloadingpercentage,       creditday                   = @creditday,       contractstartdatetime       = @contractstartdatetime,       contractenddatetime         = @contractenddatetime,       ordertype                   = @ordertype,       biddingactivatedatetime     = @biddingactivatedatetime,       biddingclosedatetime        = @biddingclosedatetime,       poststatus                  = @poststatus,       gprs                        = @gprs,       menpowerloading             = @menpowerloading,       menpowerunloading           = @menpowerunloading,       transporterinsurance        = @transporterinsurance,       tolltaxinclude              = @tolltaxinclude,       remark                      = @remark,       loadingtype                 = @loadingtype,       modifydate                  = getdate() WHERE dtlpostingid = @dtlpostingid
 SET @identity = @dtlpostingid
 END
 ELSE
 BEGIN

INSERT INTO dbo.tbldtlposting(postingid,materialweight,materialphotos,packingdimension,numberpackets,vehicletype,novehicle,deliveryperiodday,pickupdatetime,postamount,onpickuppercentage,onunloadingpercentage,creditday,contractstartdatetime,contractenddatetime,ordertype,biddingactivatedatetime,biddingclosedatetime,poststatus,gprs,menpowerloading,menpowerunloading,transporterinsurance,tolltaxinclude,remark,loadingtype)SELECT @postingid,@materialweight,@materialphotos,@packingdimension,@numberpackets,@vehicletype,@novehicle,@deliveryperiodday,@pickupdatetime,@postamount,@onpickuppercentage,@onunloadingpercentage,@creditday,@contractstartdatetime,@contractenddatetime,@ordertype,@biddingactivatedatetime,@biddingclosedatetime,@poststatus,@gprs,@menpowerloading,@menpowerunloading,@transporterinsurance,@tolltaxinclude,@remark,@loadingtype
 
 set @identity= SCOPE_IDENTITY();
END
 

 END

GO
/****** Object:  StoredProcedure [dbo].[Usp_InsertUpdateUser]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Usp_InsertUpdateUser] 
	-- Add the parameters for the stored procedure here
@UserID bigint,
@emailaddress varchar(50),
@firstname varchar(50),
@middlename varchar(50),
@lastname varchar(50),
@password varchar(10),
@mobilenumber varchar(15),
@landlinenumber varchar(20),
@dob datetime,
@addressline1 varchar(200),
@addressline2 varchar(200)=null,
@pincode int=null,
@photo varchar(100)=null,
@country varchar(15),
@state varchar(20),
@district varchar(20),
@city varchar(20),
@area varchar(25)=null,
@pan varchar(10)=null,
@gst varchar(16)=null,
@aadhaarno bigint=null,
@OTP int,
--@Token varchar(100),
@Identity bigint out,
@Message as varchar(100) output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

	IF Exists(Select 1 from [dbo].[tblmstusers] where userid=@UserID or [mobilenumber]=@mobilenumber OR [emailaddress] = case when @emailaddress != 'NA' then @emailaddress else '!@#$%^&' end)
	BEGIN
		UPDATE [dbo].[tblmstusers]
	   SET [emailaddress] = case when @emailaddress = 'NA' then [emailaddress] else @emailaddress end
		  ,[firstname] = case when @firstname = 'NA' then [firstname] else @firstname end
		  ,[middlename] = case when @middlename= 'NA' then [middlename] else @middlename end
		  ,[lastname] = case when @lastname	= 'NA' then [lastname] else @lastname end	  
		  ,[landlinenumber] = case when @landlinenumber= 'NA' then [landlinenumber] else @landlinenumber end
		  ,[dob] = @dob
		  ,[addressline1] = case when @addressline1= 'NA' then [addressline1] else @addressline1 end
		  ,[addressline2] = case when @addressline2= 'NA' then [addressline2] else @addressline2 end
		  ,[pincode] = @pincode
		  ,[photo] = @photo
		  ,[country] = case when @country= 'NA' then [country] else @country end
		  ,[state] = case when @state= 'NA' then [state] else @state end
		  ,[district] = case when @district= 'NA' then [district] else @district end
		  ,[city] = case when @city= 'NA' then [city] else @city end
		  ,[area] = case when @area= 'NA' then [area] else @area end
		  ,[pan] = case when @pan= 'NA' then [pan] else @pan end
		  ,[gst] = case when @gst= 'NA' then [gst] else @gst end
		  ,[aadhaarno] =  @aadhaarno 
		  --,[Token] = @Token		  	  
		  ,[OTP] = @OTP	
		  ,[modifydate] = getdate()
	 WHERE userid=@UserID or [mobilenumber]=@mobilenumber or [emailaddress] = case when @emailaddress != 'NA' then @emailaddress else '!@#$%^&' end

	 set @identity= (SELECT TOP(1) USERID FROM TblMstUsers where userid=@UserID or [mobilenumber]=@mobilenumber or [emailaddress] = case when @emailaddress != 'NA' then @emailaddress else '!@#$%^&' end);--satyendra
	  SET @Message = 'Successfully Updated'


	END
	ELSE
	BEGIN


	   INSERT INTO [dbo].[tblmstusers]
			   ([emailaddress]
			   ,[firstname]
			   ,[middlename]
			   ,[lastname]
			   ,[password]
			   ,[mobilenumber]
			   ,[landlinenumber]
			   ,[dob]
			   ,[addressline1]
			   ,[addressline2]
			   ,[pincode]
			   ,[photo]
			   ,[country]
			   ,[state]
			   ,[district]
			   ,[city]
			   ,[area]
			   ,[mobileverification]
			   ,[emailverification]
			   ,[pan]
			   ,[gst]
			   ,[aadhaarno]
			   ,[OTP])
		 VALUES
			   (@emailaddress
			   ,@firstname
			   ,@middlename
			   ,@lastname
			   ,@password
			   ,@mobilenumber
			   ,@landlinenumber
			   ,@dob
			   ,@addressline1
			   ,@addressline2
			   ,@pincode
			   ,@photo
			   ,@country
			   ,@state
			   ,@district
			   ,@city
			   ,@area
			   ,0
			   ,0
			   ,@pan
			   ,@gst
			   ,@aadhaarno
			   ,@OTP)

			   set @identity= SCOPE_IDENTITY();
 SET @Message = 'Successfully Inserted'
	END

	
END

GO
/****** Object:  StoredProcedure [dbo].[Usp_InsertUserCompanyMapping]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_InsertUserCompanyMapping](@UserId as bigint,@CompanyId as bigint=0,@TransportTypeId as int,@Message as varchar(100) output)ASBEGINDeclare @CID bigintIF Exists (Select 1 from [dbo].[TblDtlUsers] where Userid=@UserId and transporttypeid=@TransportTypeId)BEGIN	SET @Message = 'User Transport type already exists.'ENDELSEBEGINIF(@CompanyId is null or @CompanyId = 0)BEGIN SELECT @CID= CompanyId from tblmstcompany where Userid=@UserIdENDELSEBEGIN	SET @CID= @CompanyId	END	-- insert	insert [dbo].[TblDtlUsers] (userid,companyid,transporttypeid)	values (@UserId,@CID,@TransportTypeId)	SET @Message = 'Successfully Inserted'ENDEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_UpdatePassword]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[Usp_UpdatePassword](@MobileNumber as varchar(15),@emailaddres as varchar(50),@password as varchar(10),@Message as varchar(10) output)ASBEGINif(len(@MobileNumber) > 9)BEGINUpdate [dbo].[tblmstusers]  set [password] = @password where  [mobilenumber] = @MobileNumber ENDELSEBEGINUpdate [dbo].[tblmstusers]  set [password] = @password where  [emailaddress] = @emailaddres ENDEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_UpdateTokenById]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_UpdateTokenById](@UserId as BIGINT,@Token as varchar(100),@Message as varchar(100) output)ASBEGINUpdate [dbo].[tblmstusers]  set [Token] = @Token where   [userid] = @UserIdSet @Message = 'Token updated successfully' END
GO
/****** Object:  StoredProcedure [dbo].[Usp_UpdateTokenByNoEmail]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_UpdateTokenByNoEmail](@mobileno as varchar(20),@emailaddress as varchar(50),@Token as varchar(100),@UserId as bigint output,@Message as varchar(100) output)ASBEGINSelect @UserId=userid from TblMstUsers where mobilenumber= @mobileno or emailaddress=@emailaddressUpdate [dbo].[tblmstusers]  set [Token] = @Token where userid=@UserIdSet @Message = 'Token updated successfully' END
GO
/****** Object:  StoredProcedure [dbo].[Usp_UpdateUserPhoto]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[Usp_UpdateUserPhoto]
@UserID bigint,
@photo varchar(100)=null,
@Message as varchar(100) output
AS
BEGIN
			SET NOCOUNT ON;

			IF(@UserID > 0 and len(LTrim(RTrim(@photo))) > 0)
			Begin
					Update TblMstUsers
					Set Photo =@photo
					where UserID = @UserID
					SET @Message = 'Successfully Updated'

			End
			Else
				  SET @Message = 'Insufficient Information'

END
GO
/****** Object:  StoredProcedure [dbo].[Usp_UpdateVerification]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_UpdateVerification](@MobileNumber as varchar(15),@emailaddres as varchar(50),@OTP int,@IsMobile as bit,@Message as varchar(100) output)ASBEGINif Exists (Select 1 From [dbo].[tblmstusers] where ([mobilenumber] = @MobileNumber OR [emailaddress] = @emailaddres ) AND OTP=@OTP)BEGIN	if(@IsMobile = 1)	BEGIN	Update [dbo].[tblmstusers]  set [mobileverification] = 1 where  [mobilenumber] = @MobileNumber 	END	ELSE	BEGIN	Update [dbo].[tblmstusers]  set [emailverification] = 1 where   [emailaddress] = @emailaddres 	END	SET @Message= 'Successfully updated'ENDELSEBEGIN SET @Message= 'Records not found'ENDEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_UpdateVerificationById]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_UpdateVerificationById](@UserId as BIGINT,@OTP as int,@IsMobile as bit,@Message as varchar(100) output)ASBEGINIF exists (Select 1 from [dbo].[tblmstusers] where [userid] = @UserId and OTP=@OTP)BEGIN	if(@IsMobile = 1)	BEGIN	Update [dbo].[tblmstusers]  set [mobileverification] = 1 where  [userid] = @UserId and OTP=@OTP	END	ELSE	BEGIN	Update [dbo].[tblmstusers]  set [emailverification] = 1 where   [userid] = @UserId and OTP=@OTP	END	SET @Message = 'Successfully Updated'ENDELSEBEGINSET @Message = 'OTP is not match'ENDEND
GO
/****** Object:  UserDefinedFunction [dbo].[createInsertSP]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[createInsertSP]
(
 @spSchema varchar(200), -- desired schema
 @spTable varchar(200) -- desired table
)
RETURNS varchar(max)
AS
BEGIN

 declare @SQL_DROP varchar(max)
 declare @SQL varchar(max)
 declare @COLUMNS varchar(max)
 declare @PK_COLUMN varchar(200)

 set @SQL = ''
 set @SQL_DROP = ''
 set @COLUMNS = ''

 -- step 1: generate the drop statement and then the create statement
 set @SQL_DROP = @SQL_DROP + 'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[' + @spSchema + '].[insert' + @spTable + ']'') AND type in (N''P'', N''PC''))' + char(13)
 set @SQL_DROP = @SQL_DROP + 'DROP PROCEDURE [' + @spSchema + '].[insert' + @spTable + ']'

 set @SQL = @SQL + 'CREATE PROC [' + @spSchema + '].[insert' + @spTable + ']' + char(13)
 set @SQL = @SQL + '(' + char(13)

 -- step 2: ascertain what the primary key column for the table is
 set @PK_COLUMN = 
 (
 select c.column_name
 from information_schema.table_constraints pk 
 inner join information_schema.key_column_usage c 
 on c.table_name = pk.table_name 
 and c.constraint_name = pk.constraint_name
 where pk.TABLE_SCHEMA = @spSchema
 and pk.TABLE_NAME = @spTable
 and pk.constraint_type = 'primary key'
 and c.column_name in
 (
 select COLUMN_NAME
 from INFORMATION_SCHEMA.COLUMNS
 where columnproperty(object_id(quotename(@spSchema) + '.' + 
 quotename(@spTable)), COLUMN_NAME, 'IsIdentity') = 1 -- ensure the primary key is an identity column
 group by COLUMN_NAME
 )
 group by column_name
 having COUNT(column_name) = 1 -- ensure there is only one primary key
 )

 -- step 3: now put all the table columns in bar the primary key (as this is an insert and it is an identity column)
 select @COLUMNS = @COLUMNS + '@' + COLUMN_NAME 
 + ' as ' 
 + (case DATA_TYPE when 'numeric' then DATA_TYPE + '(' + convert(varchar(10), NUMERIC_PRECISION) + ',' + convert(varchar(10), NUMERIC_SCALE) + ')' else DATA_TYPE end)
 + (case when CHARACTER_MAXIMUM_LENGTH is not null then '(' + case when CONVERT(varchar(10), CHARACTER_MAXIMUM_LENGTH) = '-1' then 'max' else CONVERT(varchar(10), CHARACTER_MAXIMUM_LENGTH) end + ')' else '' end)
 + (case 
 when IS_NULLABLE = 'YES'
 then
 case when COLUMN_DEFAULT is null
 then ' = Null'
 else ''
 end
 else
 case when COLUMN_DEFAULT is null
 then ''
 else
 case when COLUMN_NAME = @PK_COLUMN
 then ''
 else ' = ' + replace(replace(COLUMN_DEFAULT, '(', ''), ')', '')
 end
 end
 end)
 + ',' + char(13) 
 from INFORMATION_SCHEMA.COLUMNS
 where TABLE_SCHEMA = @spSchema 
 and TABLE_NAME = @spTable
 and COLUMN_NAME <> @PK_COLUMN
 order by ORDINAL_POSITION

 set @SQL = @SQL + left(@COLUMNS, len(@COLUMNS) - 2) + char(13)

 set @SQL = @SQL + ')' + char(13)
 set @SQL = @SQL + 'AS' + char(13)
 set @SQL = @SQL + '' + char(13)

 -- step 4: add a modifications section
 set @SQL = @SQL + '-- Author: Auto' + char(13)
 set @SQL = @SQL + '-- Created: ' + convert(varchar(11), getdate(), 106) + char(13)
 set @SQL = @SQL + '-- Function: Inserts a ' + @spSchema + '.' + @spTable + ' table record' + char(13)
 set @SQL = @SQL + '' + char(13)
 set @SQL = @SQL + '-- Modifications:' + char(13)
 set @SQL = @SQL + '' + char(13)

 -- body here

 -- step 5: begins a transaction
 set @SQL = @SQL + 'begin transaction' + char(13) + char(13)

 -- step 6: begin a try
 set @SQL = @SQL + 'begin try' + char(13) + char(13) 

 set @SQL = @SQL + '-- insert' + char(13)

 -- step 7: code the insert
 set @COLUMNS = ''

 select @COLUMNS = @COLUMNS + '@' + COLUMN_NAME + ','
 from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @spTable
 and COLUMN_NAME <> @PK_COLUMN
 order by ORDINAL_POSITION

 set @COLUMNS = left(@COLUMNS, len(@COLUMNS) -1) -- trim off the last comma

 set @SQL = @SQL + 'insert [' + @spSchema + '].[' + @spTable + '] (' + replace(@COLUMNS, '@', '') + ')' + char(13)
 set @SQL = @SQL + 'values (' + @COLUMNS + ')' + char(13)
 set @SQL = @SQL + char(13) + char(13)
 set @SQL = @SQL + '-- Return the new ID' + char(13)
 set @SQL = @SQL + 'select SCOPE_IDENTITY();' + char(13) + char(13)

 -- step 8: commit the transaction
 set @SQL = @SQL + 'commit transaction' + char(13) + char(13)

 -- step 9: end the try
 set @SQL = @SQL + 'end try' + char(13) + char(13)

 -- step 10: begin a catch
 set @SQL = @SQL + 'begin catch' + char(13) + char(13) 

 -- step 11: raise the error
 set @SQL = @SQL + ' declare @ErrorMessage NVARCHAR(4000);' + char(13)
 set @SQL = @SQL + ' declare @ErrorSeverity INT;' + char(13)
 set @SQL = @SQL + ' declare @ErrorState INT;' + char(13) + char(13)
 set @SQL = @SQL + ' select @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();' + char(13) + char(13)
 set @SQL = @SQL + ' raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);' + char(13) + char(13)
 set @SQL = @SQL + ' rollback transaction' + char(13) + char(13)

 -- step 11: end the catch
 set @SQL = @SQL + 'end catch;' + char(13) + char(13)

 -- step 12: return both the drop and create statements
 RETURN @SQL_DROP + '||' + @SQL

END

GO
/****** Object:  Table [dbo].[locations]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[locations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[pincode] [varchar](50) NOT NULL,
	[city_id] [int] NULL,
	[city_name] [varchar](50) NOT NULL,
	[state_id] [int] NULL,
	[state_name] [varchar](50) NOT NULL,
	[country_id] [int] NULL,
	[country_name] [varchar](50) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblAreaPreference]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TblAreaPreference](
	[preferenceId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[CityId] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TblAreaPreference_preferenceId] PRIMARY KEY CLUSTERED 
(
	[preferenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TblBiddingBlockList]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblBiddingBlockList](
	[BlockListId] [bigint] IDENTITY(1,1) NOT NULL,
	[PostUserId] [bigint] NOT NULL,
	[BiddingUserId] [bigint] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NOT NULL,
	[Enable] [bit] NOT NULL,
	[Reason] [varchar](200) NOT NULL,
 CONSTRAINT [PK_TblBiddingBlockList_useraccessId] PRIMARY KEY CLUSTERED 
(
	[BlockListId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblContact]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblContact](
	[ContactId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[MobileNo] [varchar](15) NOT NULL,
	[EmailAddress] [varchar](50) NOT NULL,
	[State] [varchar](50) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[Message] [varchar](500) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TblContact] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblDtlBidding]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TblDtlBidding](
	[dtlbiddingid] [bigint] IDENTITY(1,1) NOT NULL,
	[vehicleno] [smallint] NOT NULL,
	[capacity] [smallint] NOT NULL,
	[biddingid] [bigint] NULL,
 CONSTRAINT [PK_tbldtlbidding_dtlbiddingid] PRIMARY KEY CLUSTERED 
(
	[dtlbiddingid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TblDtlInsurance]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblDtlInsurance](
	[insuranceid] [bigint] IDENTITY(1,1) NOT NULL,
	[insurancename] [varchar](100) NOT NULL,
	[insurancenumber] [varchar](30) NULL,
	[companyname] [varchar](100) NOT NULL,
	[description] [varchar](200) NULL,
	[insurancedate] [datetime] NOT NULL,
	[expiredate] [datetime] NULL,
	[vehicleid] [bigint] NOT NULL,
	[createddate] [datetime] NOT NULL,
	[modifydate] [datetime] NULL,
 CONSTRAINT [PK_tbldtlinsurance_insuranceid] PRIMARY KEY CLUSTERED 
(
	[insuranceid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblDtlPosting]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblDtlPosting](
	[dtlpostingid] [bigint] IDENTITY(1,1) NOT NULL,
	[postingid] [bigint] NOT NULL,
	[materialweight] [int] NOT NULL,
	[materialphotos] [varchar](100) NULL,
	[packingdimension] [varchar](50) NULL,
	[numberpackets] [int] NOT NULL,
	[vehicletype] [smallint] NOT NULL,
	[novehicle] [smallint] NOT NULL,
	[deliveryperiodday] [smallint] NULL,
	[pickupdatetime] [datetime] NOT NULL,
	[postamount] [decimal](18, 0) NULL,
	[onpickuppercentage] [smallint] NOT NULL,
	[onunloadingpercentage] [smallint] NOT NULL,
	[creditday] [int] NOT NULL,
	[contractstartdatetime] [datetime] NULL,
	[contractenddatetime] [datetime] NULL,
	[biddingactivatedatetime] [datetime] NOT NULL,
	[biddingclosedatetime] [datetime] NOT NULL,
	[poststatus] [smallint] NOT NULL,
	[gprs] [bit] NOT NULL,
	[menpowerloading] [bit] NOT NULL,
	[menpowerunloading] [bit] NOT NULL,
	[transporterinsurance] [bit] NOT NULL,
	[tolltaxinclude] [bit] NOT NULL,
	[remark] [varchar](200) NULL,
	[loadingtype] [bit] NOT NULL,
	[createddate] [datetime] NULL,
	[modifydate] [datetime] NULL,
	[ordertype] [smallint] NULL,
 CONSTRAINT [PK_tbldtlposting_dtlpostingid] PRIMARY KEY CLUSTERED 
(
	[dtlpostingid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblDtlUsers]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TblDtlUsers](
	[dtluserid] [bigint] IDENTITY(1,1) NOT NULL,
	[userid] [bigint] NOT NULL,
	[companyid] [bigint] NOT NULL,
	[transporttypeid] [smallint] NOT NULL,
	[createddate] [datetime] NOT NULL,
	[modifydate] [datetime] NULL,
 CONSTRAINT [PK_tbldtlusers_dtluserid] PRIMARY KEY CLUSTERED 
(
	[dtluserid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TblMstBidding]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMstBidding](
	[biddingid] [bigint] IDENTITY(1,1) NOT NULL,
	[dtlpostingid] [bigint] NOT NULL,
	[userid] [bigint] NOT NULL,
	[amount] [decimal](18, 0) NOT NULL,
	[biddercomment] [varchar](500) NULL,
	[status] [smallint] NOT NULL,
	[servicecharges] [decimal](18, 0) NOT NULL,
	[paymentmethod] [smallint] NULL,
	[rating] [smallint] NULL,
	[cancellationreason] [varchar](200) NULL,
	[createddate] [datetime] NULL,
	[modifydate] [datetime] NULL,
 CONSTRAINT [PK_tblmstbidding_biddingid] PRIMARY KEY CLUSTERED 
(
	[biddingid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblMstCity]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMstCity](
	[CityId] [int] IDENTITY(1,1) NOT NULL,
	[StateId] [int] NOT NULL,
	[CityName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TblMstCity] PRIMARY KEY CLUSTERED 
(
	[CityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblMstCompany]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMstCompany](
	[companyid] [bigint] IDENTITY(1,1) NOT NULL,
	[companyname] [varchar](100) NOT NULL,
	[address] [varchar](20) NOT NULL,
	[telenumber] [varchar](20) NOT NULL,
	[alternettelnumber] [varchar](20) NULL,
	[country] [varchar](15) NOT NULL,
	[state] [varchar](20) NOT NULL,
	[city] [varchar](20) NOT NULL,
	[userid] [bigint] NOT NULL,
	[companytype] [int] NOT NULL,
	[createddate] [datetime] NOT NULL,
	[modifydate] [datetime] NULL,
 CONSTRAINT [PK_tblmstcompany_companyid] PRIMARY KEY CLUSTERED 
(
	[companyid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblMstMaterialtype]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMstMaterialtype](
	[materialtypeid] [smallint] IDENTITY(1,1) NOT NULL,
	[materialtype] [varchar](50) NOT NULL,
	[description] [varchar](100) NULL,
 CONSTRAINT [PK_tblmstmaterialtype_materialtypeid] PRIMARY KEY CLUSTERED 
(
	[materialtypeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblMstPackageType]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMstPackageType](
	[packagetypeid] [smallint] IDENTITY(1,1) NOT NULL,
	[packagetype] [varchar](50) NOT NULL,
	[description] [varchar](100) NULL,
 CONSTRAINT [PK_tblmstpackagetype_packagetypeid] PRIMARY KEY CLUSTERED 
(
	[packagetypeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblMstPincode]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMstPincode](
	[PincodeId] [int] IDENTITY(1,1) NOT NULL,
	[CityId] [int] NOT NULL,
	[Pincode] [varchar](10) NULL,
 CONSTRAINT [PK_TblMstPincode] PRIMARY KEY CLUSTERED 
(
	[PincodeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblMstPosting]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMstPosting](
	[postingid] [bigint] IDENTITY(1,1) NOT NULL,
	[posttype] [varchar](20) NOT NULL,
	[userid] [bigint] NOT NULL,
	[soureaddress] [varchar](200) NOT NULL,
	[destinationadress] [varchar](200) NOT NULL,
	[materialtypeid] [smallint] NOT NULL,
	[description] [varchar](max) NULL,
	[packagetypeid] [smallint] NOT NULL,
	[packagetypedesc] [varchar](200) NULL,
	[createddate] [datetime] NOT NULL,
	[modifydate] [datetime] NULL,
	[Src_State] [varchar](20) NULL,
	[Src_pincode] [int] NULL,
	[Src_city] [varchar](20) NULL,
	[Src_landMark] [varchar](30) NULL,
	[Dst_State] [varchar](20) NULL,
	[Dst_pincode] [int] NULL,
	[Dst_city] [varchar](20) NULL,
	[Dst_landMark] [varchar](30) NULL,
 CONSTRAINT [PK_tblmstposting_postingid] PRIMARY KEY CLUSTERED 
(
	[postingid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblMstState]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMstState](
	[StateId] [int] IDENTITY(1,1) NOT NULL,
	[State] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TblMstState] PRIMARY KEY CLUSTERED 
(
	[StateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblMstTransportType]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMstTransportType](
	[transporttypeid] [smallint] IDENTITY(1,1) NOT NULL,
	[transporttype] [varchar](20) NOT NULL,
	[description] [varchar](200) NULL,
	[Usertype] [char](1) NULL,
 CONSTRAINT [PK_tblmsttransporttype_transporttypeid] PRIMARY KEY CLUSTERED 
(
	[transporttypeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblMstUsers]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMstUsers](
	[userid] [bigint] IDENTITY(1,1) NOT NULL,
	[emailaddress] [varchar](50) NULL,
	[firstname] [varchar](50) NOT NULL,
	[middlename] [varchar](50) NULL,
	[lastname] [varchar](50) NOT NULL,
	[mobilenumber] [varchar](15) NOT NULL,
	[landlinenumber] [varchar](20) NULL,
	[dob] [datetime] NULL,
	[addressline1] [varchar](200) NOT NULL,
	[addressline2] [varchar](200) NULL,
	[pincode] [int] NOT NULL,
	[photo] [varchar](100) NULL,
	[country] [varchar](15) NOT NULL,
	[state] [varchar](20) NOT NULL,
	[district] [varchar](20) NOT NULL,
	[city] [varchar](20) NOT NULL,
	[area] [varchar](25) NULL,
	[mobileverification] [bit] NOT NULL,
	[emailverification] [bit] NOT NULL,
	[pan] [varchar](10) NULL,
	[gst] [varchar](16) NULL,
	[aadhaarno] [bigint] NULL,
	[createddate] [datetime] NOT NULL,
	[modifydate] [datetime] NULL,
	[password] [varchar](10) NULL,
	[OTP] [int] NULL,
	[Token] [varchar](100) NULL,
 CONSTRAINT [PK_tblmstusers_userid] PRIMARY KEY CLUSTERED 
(
	[userid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblMstVehicle]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMstVehicle](
	[vehicleid] [bigint] IDENTITY(1,1) NOT NULL,
	[vehicletype] [varchar](20) NOT NULL,
	[manufacturername] [varchar](20) NOT NULL,
	[descrition] [varchar](200) NULL,
	[manufactureryear] [date] NOT NULL,
	[totalwheel] [smallint] NOT NULL,
	[weightcapacity] [int] NOT NULL,
	[rcbookcopypath] [varchar](100) NULL,
	[rtoregistrationnumber] [varchar](50) NOT NULL,
	[companyid] [bigint] NOT NULL,
	[registrationdate] [datetime] NOT NULL,
	[createddate] [datetime] NOT NULL,
	[modifydate] [datetime] NULL,
 CONSTRAINT [PK_tblmstvehicle_vehicleid] PRIMARY KEY CLUSTERED 
(
	[vehicleid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblMstVehicleType]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TblMstVehicleType](
	[vehicletypeid] [smallint] IDENTITY(1,1) NOT NULL,
	[vehicletype] [varchar](50) NOT NULL,
	[description] [varchar](100) NULL,
 CONSTRAINT [PK_tblmstvehicletype_vehicletypeid] PRIMARY KEY CLUSTERED 
(
	[vehicletypeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TblUserAccess]    Script Date: 10/5/2018 2:02:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TblUserAccess](
	[useraccessId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NOT NULL,
	[NoBidding] [int] NOT NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_tbluseraccess_useraccessId] PRIMARY KEY CLUSTERED 
(
	[useraccessId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[TblAreaPreference] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[TblContact] ADD  CONSTRAINT [DF_TblContact_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[TblDtlInsurance] ADD  DEFAULT (getdate()) FOR [createddate]
GO
ALTER TABLE [dbo].[TblDtlPosting] ADD  DEFAULT (getdate()) FOR [createddate]
GO
ALTER TABLE [dbo].[TblDtlUsers] ADD  DEFAULT (getdate()) FOR [createddate]
GO
ALTER TABLE [dbo].[TblMstBidding] ADD  DEFAULT (getdate()) FOR [createddate]
GO
ALTER TABLE [dbo].[TblMstCompany] ADD  DEFAULT (getdate()) FOR [createddate]
GO
ALTER TABLE [dbo].[TblMstPosting] ADD  DEFAULT (getdate()) FOR [createddate]
GO
ALTER TABLE [dbo].[TblMstUsers] ADD  DEFAULT ((0)) FOR [mobileverification]
GO
ALTER TABLE [dbo].[TblMstUsers] ADD  DEFAULT ((0)) FOR [emailverification]
GO
ALTER TABLE [dbo].[TblMstUsers] ADD  DEFAULT (getdate()) FOR [createddate]
GO
ALTER TABLE [dbo].[TblMstUsers] ADD  DEFAULT ((0)) FOR [OTP]
GO
ALTER TABLE [dbo].[TblMstVehicle] ADD  DEFAULT (getdate()) FOR [createddate]
GO
ALTER TABLE [dbo].[TblAreaPreference]  WITH CHECK ADD  CONSTRAINT [FK_TblAreaPreference_TblMstCity] FOREIGN KEY([CityId])
REFERENCES [dbo].[TblMstCity] ([CityId])
GO
ALTER TABLE [dbo].[TblAreaPreference] CHECK CONSTRAINT [FK_TblAreaPreference_TblMstCity]
GO
ALTER TABLE [dbo].[TblAreaPreference]  WITH CHECK ADD  CONSTRAINT [FK_TblAreaPreference_tblmstuser] FOREIGN KEY([UserId])
REFERENCES [dbo].[TblMstUsers] ([userid])
GO
ALTER TABLE [dbo].[TblAreaPreference] CHECK CONSTRAINT [FK_TblAreaPreference_tblmstuser]
GO
ALTER TABLE [dbo].[TblBiddingBlockList]  WITH CHECK ADD  CONSTRAINT [FK_TblBiddingBlockList_tblmstuser] FOREIGN KEY([PostUserId])
REFERENCES [dbo].[TblMstUsers] ([userid])
GO
ALTER TABLE [dbo].[TblBiddingBlockList] CHECK CONSTRAINT [FK_TblBiddingBlockList_tblmstuser]
GO
ALTER TABLE [dbo].[TblDtlBidding]  WITH CHECK ADD  CONSTRAINT [FK_tbldtlbidding_tblmstbidding] FOREIGN KEY([biddingid])
REFERENCES [dbo].[TblMstBidding] ([biddingid])
GO
ALTER TABLE [dbo].[TblDtlBidding] CHECK CONSTRAINT [FK_tbldtlbidding_tblmstbidding]
GO
ALTER TABLE [dbo].[TblDtlInsurance]  WITH CHECK ADD  CONSTRAINT [FK_tbldtlinsurance_tblmstvehicle] FOREIGN KEY([vehicleid])
REFERENCES [dbo].[TblMstVehicle] ([vehicleid])
GO
ALTER TABLE [dbo].[TblDtlInsurance] CHECK CONSTRAINT [FK_tbldtlinsurance_tblmstvehicle]
GO
ALTER TABLE [dbo].[TblDtlPosting]  WITH CHECK ADD  CONSTRAINT [FK_tbldtlposting_tblmstposting] FOREIGN KEY([postingid])
REFERENCES [dbo].[TblMstPosting] ([postingid])
GO
ALTER TABLE [dbo].[TblDtlPosting] CHECK CONSTRAINT [FK_tbldtlposting_tblmstposting]
GO
ALTER TABLE [dbo].[TblDtlUsers]  WITH CHECK ADD  CONSTRAINT [FK_tbldtlusers_MstTransportType] FOREIGN KEY([transporttypeid])
REFERENCES [dbo].[TblMstTransportType] ([transporttypeid])
GO
ALTER TABLE [dbo].[TblDtlUsers] CHECK CONSTRAINT [FK_tbldtlusers_MstTransportType]
GO
ALTER TABLE [dbo].[TblDtlUsers]  WITH CHECK ADD  CONSTRAINT [FK_tbldtlusers_tblmstcompany] FOREIGN KEY([companyid])
REFERENCES [dbo].[TblMstCompany] ([companyid])
GO
ALTER TABLE [dbo].[TblDtlUsers] CHECK CONSTRAINT [FK_tbldtlusers_tblmstcompany]
GO
ALTER TABLE [dbo].[TblDtlUsers]  WITH CHECK ADD  CONSTRAINT [FK_tbldtlusers_tblmstuser] FOREIGN KEY([userid])
REFERENCES [dbo].[TblMstUsers] ([userid])
GO
ALTER TABLE [dbo].[TblDtlUsers] CHECK CONSTRAINT [FK_tbldtlusers_tblmstuser]
GO
ALTER TABLE [dbo].[TblMstBidding]  WITH CHECK ADD  CONSTRAINT [FK_tbldtlposting_tbldtlposting] FOREIGN KEY([dtlpostingid])
REFERENCES [dbo].[TblDtlPosting] ([dtlpostingid])
GO
ALTER TABLE [dbo].[TblMstBidding] CHECK CONSTRAINT [FK_tbldtlposting_tbldtlposting]
GO
ALTER TABLE [dbo].[TblMstBidding]  WITH CHECK ADD  CONSTRAINT [FK_tblmstbidding_tblmstuser] FOREIGN KEY([userid])
REFERENCES [dbo].[TblMstUsers] ([userid])
GO
ALTER TABLE [dbo].[TblMstBidding] CHECK CONSTRAINT [FK_tblmstbidding_tblmstuser]
GO
ALTER TABLE [dbo].[TblMstCompany]  WITH CHECK ADD  CONSTRAINT [FK_tblmstcompany_tblmstuser] FOREIGN KEY([userid])
REFERENCES [dbo].[TblMstUsers] ([userid])
GO
ALTER TABLE [dbo].[TblMstCompany] CHECK CONSTRAINT [FK_tblmstcompany_tblmstuser]
GO
ALTER TABLE [dbo].[TblMstPosting]  WITH CHECK ADD  CONSTRAINT [FK_tblmstposting_tblmstmaterialtype] FOREIGN KEY([materialtypeid])
REFERENCES [dbo].[TblMstMaterialtype] ([materialtypeid])
GO
ALTER TABLE [dbo].[TblMstPosting] CHECK CONSTRAINT [FK_tblmstposting_tblmstmaterialtype]
GO
ALTER TABLE [dbo].[TblMstPosting]  WITH CHECK ADD  CONSTRAINT [FK_tblmstposting_tblmstpackagetype] FOREIGN KEY([packagetypeid])
REFERENCES [dbo].[TblMstPackageType] ([packagetypeid])
GO
ALTER TABLE [dbo].[TblMstPosting] CHECK CONSTRAINT [FK_tblmstposting_tblmstpackagetype]
GO
ALTER TABLE [dbo].[TblMstPosting]  WITH CHECK ADD  CONSTRAINT [FK_tblmstposting_tblmstuser] FOREIGN KEY([userid])
REFERENCES [dbo].[TblMstUsers] ([userid])
GO
ALTER TABLE [dbo].[TblMstPosting] CHECK CONSTRAINT [FK_tblmstposting_tblmstuser]
GO
ALTER TABLE [dbo].[TblUserAccess]  WITH CHECK ADD  CONSTRAINT [FK_tbluseraccess_tblmstuser] FOREIGN KEY([UserId])
REFERENCES [dbo].[TblMstUsers] ([userid])
GO
ALTER TABLE [dbo].[TblUserAccess] CHECK CONSTRAINT [FK_tbluseraccess_tblmstuser]
GO
USE [master]
GO
ALTER DATABASE [QTrans] SET  READ_WRITE 
GO
