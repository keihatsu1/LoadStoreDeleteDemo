/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4206)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [master]
GO
/****** Object:  Database [LoadStoreDeleteDemo]    Script Date: 8/23/2017 8:03:02 PM ******/
CREATE DATABASE [LoadStoreDeleteDemo]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LoadStoreDeleteDemo', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\LoadStoreDeleteDemo.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LoadStoreDeleteDemo_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\LoadStoreDeleteDemo_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LoadStoreDeleteDemo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET ARITHABORT OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET RECOVERY FULL 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET  MULTI_USER 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'LoadStoreDeleteDemo', N'ON'
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET QUERY_STORE = OFF
GO
USE [LoadStoreDeleteDemo]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [LoadStoreDeleteDemo]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_SqlToCSharpTypeName]    Script Date: 8/23/2017 8:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create function [dbo].[fn_SqlToCSharpTypeName] (
	@SqlType varchar(50)
) returns varchar(50) as
begin

	return
	case @SqlType
	when 'bigint' then 'Int64'
	when 'binary' then 'Byte[]'
	when 'bit' then 'bool'
	when 'char' then 'string'
	when 'date' then 'DateTime'
	when 'datetime' then 'DateTime'
	when 'datetime2' then 'DateTime'
	when 'datetimeoffset' then 'DateTimeOffset'
	when 'decimal' then 'Decimal'
	when 'float' then 'double'
	when 'geography' then 'string'
	when 'geometry' then 'string'
	when 'hierarchyid' then 'int'
	when 'image' then 'Byte[]'
	when 'int' then 'int'
	when 'money' then 'Decimal'
	when 'nchar' then 'string'
	when 'ntext' then 'string'
	when 'numeric' then 'Decimal'
	when 'nvarchar' then 'string'
	when 'real' then 'double'
	when 'smalldatetime' then 'DateTime'
	when 'smallint' then 'Int16'
	when 'smallmoney' then 'Decimal'
	when 'sql_variant' then 'object'
	when 'sysname' then 'string'
	when 'text' then 'string'
	when 'time' then 'DateTime'
	when 'timestamp' then 'Byte[]'
	when 'tinyint' then 'Byte'
	when 'uniqueidentifier' then 'Guid'
	when 'varbinary' then 'Byte[]'
	when 'varchar' then 'string'
	when 'xml' then 'string'
	end
end

GO
/****** Object:  Table [dbo].[Customer]    Script Date: 8/23/2017 8:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[AddressLine1] [varchar](50) NULL,
	[AddressLine2] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Zip] [varchar](50) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Customer_Delete]    Script Date: 8/23/2017 8:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Customer_Delete]
	@ID int
as
	delete from
		Customer
	where
		ID = @ID

GO
/****** Object:  StoredProcedure [dbo].[Customer_List]    Script Date: 8/23/2017 8:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Customer_List]
as
	select
		ID,
		Name,
		AddressLine1,
		AddressLine2,
		City,
		State,
		Zip
	from
		Customer

GO
/****** Object:  StoredProcedure [dbo].[Customer_Load]    Script Date: 8/23/2017 8:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Customer_Load]
	@ID int
as
	select
		ID,
		Name,
		AddressLine1,
		AddressLine2,
		City,
		State,
		Zip
	from
		Customer
	where
		ID = @ID

GO
/****** Object:  StoredProcedure [dbo].[Customer_Store]    Script Date: 8/23/2017 8:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[Customer_Store]
	@ID int output,
	@Name varchar(50),
	@AddressLine1 varchar(50),
	@AddressLine2 varchar(50),
	@City varchar(50),
	@State varchar(50),
	@Zip varchar(50)
as
	if exists(select 1 from Customer where ID = @ID) begin
		update Customer set
			Name = @Name,
			AddressLine1 = @AddressLine1,
			AddressLine2 = @AddressLine2,
			City = @City,
			State = @State,
			Zip = @Zip
		where
			ID = @ID
	end else begin
		insert into Customer (
			Name,
			AddressLine1,
			AddressLine2,
			City,
			State,
			Zip
		) values (
			@Name,
			@AddressLine1,
			@AddressLine2,
			@City,
			@State,
			@Zip
		)

		set @ID = scope_identity()
	end

return @ID

GO
/****** Object:  StoredProcedure [dbo].[sys_CodeForSP]    Script Date: 8/23/2017 8:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*
exec sys_CodeForSP 'ta_CustomerLoad'
*/

CREATE procedure [dbo].[sys_CodeForSP]
	@SPName varchar(100)
with execute as owner
as

	select
		CSharp = 'public ' + dbo.fn_SqlToCSharpTypeName(t.Name) + ' ' + substring(c.Name, 2, len(c.Name)) + ' { get; set; }',
		ParmList = c.Name + ' '
			+ case t.Name when 'varchar' then 'varchar(' + convert(varchar(5), case when c.Length = -1 then 'max' else CONVERT(varchar(5), c.Length) end) + ')'
				when 'nvarchar' then 'nvarchar(' + convert(varchar(5), c.Length) + ')'
				when 'char' then 'char(' + convert(varchar(5), c.Length) + ')'
				when 'nchar' then 'nchar(' + convert(varchar(5), c.Length) + ')'
				when 'datetime2' then 'datetime2(7)'
				when 'datetimeoffset' then 'datetimeoffset(7)'
				when 'varbinary' then 'varbinary(' + convert(varchar(5), case when c.Length = -1 then 'max' else CONVERT(varchar(5), c.Length) end) + ')'
				else t.Name end
			+ ',',
		FieldList = substring(c.Name, 2, len(c.Name)) + ',',
		ValueList = c.Name + ',',
		UpdateList = substring(c.Name, 2, len(c.Name)) + ' = ' + c.Name + ',',
		o.Name As TableName,
		c.Name as ColumnName,
		t.Name As Type,
		c.Length,
		c.xprec,
		c.IsNullable,
		AutoIncrement = case when t.name = 'timestamp' then 1
						when c.status = 128 then 1 else 0 end
	from
		SysColumns c
		join SysObjects o on o.id = c.id and o.xtype = 'P'
		join SysTypes t on c.xtype = t.xtype and c.xusertype = t.xusertype
    where
       o.Name = @SPName
       --and IsOutParam = 1
    order by
		c.ColOrder

GO
/****** Object:  StoredProcedure [dbo].[sys_CodeForTable]    Script Date: 8/23/2017 8:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*
exec sys_CodeForTable 'Customer'
*/

CREATE procedure [dbo].[sys_CodeForTable]
	@TableName varchar(100)
with execute as owner
as

	select
		CSharp = 'public ' + dbo.fn_SqlToCSharpTypeName(t.Name) + ' ' + c.Name + ' { get; set; }',
		SqlParms = '@' + c.Name + ' '
			+ case t.Name when 'varchar' then 'varchar(' + convert(varchar(5), case when c.Length = -1 then 'max' else CONVERT(varchar(5), c.Length) end) + ')'
				when 'nvarchar' then 'nvarchar(' + convert(varchar(5), c.Length) + ')'
				when 'char' then 'char(' + convert(varchar(5), c.Length) + ')'
				when 'nchar' then 'nchar(' + convert(varchar(5), c.Length) + ')'
				when 'datetime2' then 'datetime2(7)'
				when 'datetimeoffset' then 'datetimeoffset(7)'
				when 'varbinary' then 'varbinary(' + convert(varchar(5), case when c.Length = -1 then 'max' else CONVERT(varchar(5), c.Length) end) + ')'
				else t.Name end
			+ ',',
		FieldList = c.Name + ',',
		ParmList = '@' + c.Name + ',',
		UpdateList = c.Name + ' = @' + c.Name + ',',
		o.Name As TableName,
		c.Name as ColumnName,
		t.Name As Type,
		c.Length,
		c.xprec,
		c.IsNullable,
		AutoIncrement = case when t.name = 'timestamp' then 1
						when c.status = 128 then 1 else 0 end,
		*
	from
		SysColumns c
		join SysObjects o on o.id = c.id and o.xtype = 'U'
		join SysTypes t on c.xtype = t.xtype and c.xusertype = t.xusertype
    where
       o.Name = @TableName
       --and IsOutParam = 1
    order by c.ColOrder

GO
/****** Object:  StoredProcedure [dbo].[sys_ExtendedProperty]    Script Date: 8/23/2017 8:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*
exec sys_ExtendedProperty 'AllowsDirectSql'
*/

CREATE procedure [dbo].[sys_ExtendedProperty]
	@PropertyName varchar(100)
with execute as owner
as

	select value from sys.extended_properties where name = @PropertyName

GO
/****** Object:  StoredProcedure [dbo].[sys_ParametersForSP]    Script Date: 8/23/2017 8:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[sys_ParametersForSP]
	@SPName varchar(100)
with execute as owner
as

Select
	SPName = o.Name,
	Name = SUBSTRING(c.Name, 2, LEN(c.Name)),
	Type = t.Name,
	c.IsOutParam,
	c.Length,
	c.xprec,
	c.IsNullable,
	AutoIncrement = case when t.name = 'timestamp' then 1
					when c.status = 128 then 1 else 0 end
From
	SysColumns c
	join SysObjects o on o.id = c.id and o.xtype = 'P'
	join SysTypes t on c.xtype = t.xtype and c.xusertype = t.xusertype
Where
   o.Name = @SPName
   --and IsOutParam = 1
Order By
	c.ColOrder

GO
/****** Object:  StoredProcedure [dbo].[sys_StructureForTable]    Script Date: 8/23/2017 8:03:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*
exec sys_StructureForTable 'Customer'
*/

CREATE procedure [dbo].[sys_StructureForTable]
	@TableName varchar(100)
with execute as owner
as

	Select
		TableName = o.Name,
		c.Name,
		Type = t.Name,
		NETType = dbo.fn_SqlToCSharpTypeName(t.Name),
		c.Length,
		c.xprec,
		c.IsNullable,
		AutoIncrement = case when t.name = 'timestamp' then 1
						when c.status = 128 then 1 else 0 end
	From
		SysColumns c
		join SysObjects o on o.id = c.id and o.xtype = 'U'
		join SysTypes t on c.xtype = t.xtype and c.xusertype = t.xusertype
    Where
       o.Name = @TableName
       --and IsOutParam = 1
    Order By c.ColOrder

GO
USE [master]
GO
ALTER DATABASE [LoadStoreDeleteDemo] SET  READ_WRITE 
GO
