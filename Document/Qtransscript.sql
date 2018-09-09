USE [QTrans]
GO
/****** Object:  UserDefinedTableType [dbo].[UT_BiddingDetails]    Script Date: 8/24/2018 12:10:48 AM ******/
CREATE TYPE [dbo].[UT_BiddingDetails] AS TABLE(
	[vehicleno] [smallint] NOT NULL,
	[capacity] [smallint] NOT NULL
)
GO
/****** Object:  StoredProcedure [dbo].[GenerateSPforInsertUpdateDelete]    Script Date: 8/24/2018 12:10:48 AM ******/
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
/****** Object:  StoredProcedure [dbo].[Usp_ForgotUserLoginDetail]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_ForgotUserLoginDetail](@MobileNumber as varchar(15),@emailaddres as varchar(50),@Message as varchar(100) output)ASBEGINSelect * From [dbo].[tblmstusers] where ([mobilenumber] = @MobileNumber OR [emailaddress] = @emailaddres )END
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetBiddingDetailsById]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[Usp_GetBiddingDetailsById](@biddingId as bigint,@Message as varchar(10) output)ASBEGINSELECT [biddingid]
      ,[dtlpostingid]
      ,[userid]
      ,[amount]
      ,[biddercomment]
      ,[status]
      ,[servicecharges]
      ,[paymentmethod]
      ,[rating]
      ,[cancellationreson]
      ,[createddate]
      ,[modifydate]
  FROM [dbo].[tblmstbidding] where  [biddingid]=@biddingIdEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetCompanyDetailsById]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[Usp_GetCompanyDetailsById](@CompanyId as bigint,@Message as varchar(10) output)ASBEGINSELECT [companyid]
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
  FROM [dbo].[tblmstcompany] where  [companyid]=@CompanyIdEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetPostingById]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[Usp_GetPostingById](@postingId as bigint,@Message as varchar(10) output)ASBEGINSELECT [postingid]
      ,[posttype]
      ,[userid]
      ,[soureaddress]
      ,[destinationadress]
      ,[materialtypeid]
      ,[description]
      ,[packagetypeid]
      ,[packagetypedesc]
      ,[createddate]
      ,[modifydate]
  FROM [dbo].[tblmstposting] where  [postingid]=@postingIdEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetPostingDetailsById]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[Usp_GetPostingDetailsById](@postingId as bigint,@Message as varchar(10) output)ASBEGINSELECT [dtlpostingid]
      ,[postingid]
      ,[materialweight]
      ,[materialphotos]
      ,[packingdimension]
      ,[numberpckets]
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
/****** Object:  StoredProcedure [dbo].[Usp_GetUserDetailsById]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[Usp_GetUserDetailsById](@UserId as bigint,@Message as varchar(10) output)ASBEGINSELECT [userid]
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
       from [dbo].[tblmstusers] where  userid=@UserIdEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_GetUserDetailsByUserLogin]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[Usp_GetUserDetailsByUserLogin](@MobileNumber as varchar(15),@emailaddres as varchar(50),@password as varchar(10),@Message as varchar(10) output)ASBEGINSELECT [userid]
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
       from [dbo].[tblmstusers] where  ([mobilenumber] = @MobileNumber OR  [emailaddress] = @emailaddres ) and [password] = @passwordEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_InsertBidding]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_InsertBidding](@dtlpostingid as bigint,@userid as bigint,@amount as decimal,@biddercomment as varchar(500) = Null,@status as smallint,@servicecharges as decimal,@paymentmethod as smallint = Null,@rating as smallint = Null,@cancellationreson as varchar(200) = Null,@createddate as datetime,@modifydate as datetime = Null,@biddingDetails [UT_BiddingDetails] readonly,@Message as varchar(100) output)AS-- Author: Auto-- Created: 16 Aug 2018-- Function: Inserts a dbo.tblmstbidding table record-- Modifications:begin transactionbegin tryDeclare @identity bigint-- insertinsert [dbo].[tblmstbidding] (dtlpostingid,userid,amount,biddercomment,status,servicecharges,paymentmethod,rating,cancellationreson,createddate,modifydate)values (@dtlpostingid,@userid,@amount,@biddercomment,@status,@servicecharges,@paymentmethod,@rating,@cancellationreson,@createddate,@modifydate)-- Return the new IDset @identity= SCOPE_IDENTITY();INSERT INTO [dbo].[tbldtlbidding]
           ([vehicleno]
           ,[capacity]
           ,[biddingid])
   Select vehicleno,capacity,@identity from @biddingDetailscommit transactionend trybegin catch declare @ErrorMessage NVARCHAR(4000); declare @ErrorSeverity INT; declare @ErrorState INT; select @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE(); raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState); rollback transactionend catch;
GO
/****** Object:  StoredProcedure [dbo].[Usp_InsertUpdateCompany]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Usp_InsertUpdateCompany]@companyid			   Bigint,@companyname           varchar(100),@address               varchar(20),@telenumber            varchar(20),@alternettelnumber     varchar(20) = NULL,@country               varchar(15),@state                 varchar(20),@city                  varchar(20),@userid                bigint,@companytype            int,@identity				Bigint output,@Message as varchar(100) outputASBEGIN
IF Exists(Select 1 from [dbo].[tblmstcompany] where companyid = @companyid and userid=@UserID)
BEGIN


UPDATE dbo.tblmstcompany   SET companyname           = @companyname,       address               = @address,       telenumber            = @telenumber,       alternettelnumber     = @alternettelnumber,       country               = @country,       state                 = @state,       city                  = @city,              companytype            = @companytype,       modifydate            = getdate() WHERE companyid = @companyid

 END
 ELSE
 BEGIN

INSERT INTO dbo.tblmstcompany(companyname,address,telenumber,alternettelnumber,country,state,city,userid,companytype)SELECT @companyname,@address,@telenumber,@alternettelnumber,@country,@state,@city,@userid,@companytype
 
 set @identity= SCOPE_IDENTITY();
END
 
 
END

GO
/****** Object:  StoredProcedure [dbo].[usp_InsertUpdateMstPosting]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_InsertUpdateMstPosting](@postingid bigint,@posttype as varchar(20),@userid as bigint,@soureaddress as varchar(200),@destinationadress as varchar(200),@materialtypeid as smallint,@description as varchar(max) = Null,@packagetypeid as smallint,@packagetypedesc as varchar(200) = Null,@identity as bigint output,@Message as varchar(100) output)AS-- Author: Auto-- Created: 16 Aug 2018-- Function: Inserts a dbo.tblmstposting table record-- Modifications:begin transactionbegin tryIF Exists(Select 1 from [dbo].[tblmstposting] where postingid=@postingid and userid=@UserID)
BEGINUPDATE [dbo].[tblmstposting]
   SET [posttype] = @posttype      
      ,[soureaddress] = @soureaddress
      ,[destinationadress] = @destinationadress
      ,[materialtypeid] = @materialtypeid
      ,[description] = @description
      ,[packagetypeid] = @packagetypeid
      ,[packagetypedesc] = @packagetypedesc      
      ,[modifydate] = getdate() where postingid=@postingid and userid=@UserIDENDELSEBEGIN-- insertinsert [dbo].[tblmstposting] (posttype,userid,soureaddress,destinationadress,materialtypeid,description,packagetypeid,packagetypedesc)values (@posttype,@userid,@soureaddress,@destinationadress,@materialtypeid,@description,@packagetypeid,@packagetypedesc)END-- Return the new IDset @identity= SCOPE_IDENTITY();commit transactionend trybegin catch declare @ErrorMessage NVARCHAR(4000); declare @ErrorSeverity INT; declare @ErrorState INT; select @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE(); raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState); rollback transactionend catch;
GO
/****** Object:  StoredProcedure [dbo].[Usp_InsertUpdatePostingDetails]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Usp_InsertUpdatePostingDetails]@dtlpostingid				bigint,@postingid                   bigint ,@materialweight              int,@materialphotos              varchar(100) = NULL,@packingdimension            varchar(50) = NULL,@numberpckets                int,@vehicletype                 smallint,@novehicle                   smallint,@deliveryperiodday           smallint = NULL,@pickupdatetime              datetime ,@postamount                  decimal = NULL,@onpickuppercentage          smallint ,@onunloadingpercentage       smallint ,@creditday                   int ,@contractstartdatetime       datetime = NULL,@contractenddatetime         datetime = NULL,@ordertype                   datetime,@biddingactivatedatetime     datetime,@biddingclosedatetime        datetime,@poststatus                  smallint,@gprs                        bit,@menpowerloading             bit,@menpowerunloading           bit,@transporterinsurance        bit,@tolltaxinclude              bit,@remark                      varchar(200) = NULL,@loadingtype                 bit,@Message as varchar(100) outputASBeginIF Exists(Select 1 from [dbo].[tbldtlposting] where dtlpostingid = @dtlpostingid and postingid = @postingid)
	BEGIN
 
UPDATE dbo.tbldtlposting   SET        materialweight              = @materialweight,       materialphotos              = @materialphotos,       packingdimension            = @packingdimension,       numberpckets                = @numberpckets,       vehicletype                 = @vehicletype,       novehicle                   = @novehicle,       deliveryperiodday           = @deliveryperiodday,       pickupdatetime              = @pickupdatetime,       postamount                  = @postamount,       onpickuppercentage          = @onpickuppercentage,       onunloadingpercentage       = @onunloadingpercentage,       creditday                   = @creditday,       contractstartdatetime       = @contractstartdatetime,       contractenddatetime         = @contractenddatetime,       ordertype                   = @ordertype,       biddingactivatedatetime     = @biddingactivatedatetime,       biddingclosedatetime        = @biddingclosedatetime,       poststatus                  = @poststatus,       gprs                        = @gprs,       menpowerloading             = @menpowerloading,       menpowerunloading           = @menpowerunloading,       transporterinsurance        = @transporterinsurance,       tolltaxinclude              = @tolltaxinclude,       remark                      = @remark,       loadingtype                 = @loadingtype,       modifydate                  = getdate() WHERE dtlpostingid = @dtlpostingid

 END
 ELSE
 BEGIN

INSERT INTO dbo.tbldtlposting(postingid,materialweight,materialphotos,packingdimension,numberpckets,vehicletype,novehicle,deliveryperiodday,pickupdatetime,postamount,onpickuppercentage,onunloadingpercentage,creditday,contractstartdatetime,contractenddatetime,ordertype,biddingactivatedatetime,biddingclosedatetime,poststatus,gprs,menpowerloading,menpowerunloading,transporterinsurance,tolltaxinclude,remark,loadingtype)SELECT @postingid,@materialweight,@materialphotos,@packingdimension,@numberpckets,@vehicletype,@novehicle,@deliveryperiodday,@pickupdatetime,@postamount,@onpickuppercentage,@onunloadingpercentage,@creditday,@contractstartdatetime,@contractenddatetime,@ordertype,@biddingactivatedatetime,@biddingclosedatetime,@poststatus,@gprs,@menpowerloading,@menpowerunloading,@transporterinsurance,@tolltaxinclude,@remark,@loadingtype
 
END
 

 END

GO
/****** Object:  StoredProcedure [dbo].[Usp_InsertUpdateUser]    Script Date: 8/24/2018 12:10:48 AM ******/
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
@addressline2 varchar(200),
@pincode int,
@photo varchar(100),
@country varchar(15),
@state varchar(20),
@district varchar(20),
@city varchar(20),
@area varchar(25),
@mobileverification bit,
@emailverification bit,
@pan varchar(10),
@gst varchar(16),
@aadhaarno bigint,
@OTP int,
@Token varchar(50),
@Identity bigint out,
@Message as varchar(100) output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF Exists(Select 1 from [dbo].[tblmstusers] where userid=@UserID or [mobilenumber]=@mobilenumber)
	BEGIN
		UPDATE [dbo].[tblmstusers]
	   SET [emailaddress] = @emailaddress
		  ,[firstname] = @firstname
		  ,[middlename] = @middlename
		  ,[lastname] = @lastname		  
		  ,[landlinenumber] = @landlinenumber
		  ,[dob] = @dob
		  ,[addressline1] = @addressline1
		  ,[addressline2] = @addressline2
		  ,[pincode] = @pincode
		  ,[photo] = @photo
		  ,[country] = @country
		  ,[state] = @state
		  ,[district] = @district
		  ,[city] = @city
		  ,[area] = @area
		  ,[mobileverification] = @mobileverification
		  ,[emailverification] = @emailverification
		  ,[pan] = @pan
		  ,[gst] = @gst
		  ,[aadhaarno] = @aadhaarno	
		  ,[Token] = @Token		  	  
		  ,[modifydate] = getdate()
	 WHERE userid=@UserID or [mobilenumber]=@mobilenumber
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

	END
END

GO
/****** Object:  StoredProcedure [dbo].[Usp_InsertUserCompanyMapping]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[Usp_InsertUserCompanyMapping](@UserId as bigint,@CompanyId as bigint,@TransportTypeId as int,@Message as varchar(10) output)ASBEGIN-- insertinsert [dbo].[TblDtlUsers] (userid,companyid,transporttypeid)values (@UserId,@CompanyId,@TransportTypeId)END
GO
/****** Object:  StoredProcedure [dbo].[Usp_UpdatePassword]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[Usp_UpdatePassword](@MobileNumber as varchar(15),@emailaddres as varchar(50),@password as varchar(10),@Message as varchar(10) output)ASBEGINif(len(@MobileNumber) > 9)BEGINUpdate [dbo].[tblmstusers]  set [password] = @password where  [mobilenumber] = @MobileNumber ENDELSEBEGINUpdate [dbo].[tblmstusers]  set [password] = @password where  [emailaddress] = @emailaddres ENDEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_UpdateVerification]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Usp_UpdateVerification](@MobileNumber as varchar(15),@emailaddres as varchar(50),@OTP int,@IsMobile as bit,@Message as varchar(100) output)ASBEGINif Exists (Select 1 From [dbo].[tblmstusers] where ([mobilenumber] = @MobileNumber OR [emailaddress] = @emailaddres ) AND OTP=@OTP)BEGIN	if(@IsMobile = 1)	BEGIN	Update [dbo].[tblmstusers]  set [mobileverification] = 1 where  [mobilenumber] = @MobileNumber 	END	ELSE	BEGIN	Update [dbo].[tblmstusers]  set [emailverification] = 1 where   [emailaddress] = @emailaddres 	END	SET @Message= 'Success'ENDELSEBEGIN SET @Message= 'Fail'ENDEND
GO
/****** Object:  StoredProcedure [dbo].[Usp_UpdateVerificationById]    Script Date: 8/24/2018 12:10:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[Usp_UpdateVerificationById](@UserId as BIGINT,@IsMobile as bit,@Message as varchar(100) output)ASBEGINif(@IsMobile = 1)BEGINUpdate [dbo].[tblmstusers]  set [mobileverification] = 1 where  [userid] = @UserId ENDELSEBEGINUpdate [dbo].[tblmstusers]  set [emailverification] = 1 where   [userid] = @UserIdENDEND
GO
/****** Object:  UserDefinedFunction [dbo].[createInsertSP]    Script Date: 8/24/2018 12:10:48 AM ******/
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
