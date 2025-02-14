USE [master]
GO
/****** Object:  Database [EmsDB]    Script Date: 9/1/2025 4:14:02 pm ******/
CREATE DATABASE [EmsDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EMS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\EMS.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EMS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\EMS_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [EmsDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EmsDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EmsDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EmsDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EmsDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EmsDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EmsDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [EmsDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EmsDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EmsDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EmsDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EmsDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EmsDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EmsDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EmsDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EmsDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EmsDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EmsDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EmsDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EmsDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EmsDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EmsDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EmsDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EmsDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EmsDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EmsDB] SET  MULTI_USER 
GO
ALTER DATABASE [EmsDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EmsDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EmsDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EmsDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EmsDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EmsDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [EmsDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [EmsDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [EmsDB]
GO
/****** Object:  Table [dbo].[Audit]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Audit](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[timeStamp] [datetime] NOT NULL,
	[webActivity] [nvarchar](50) NOT NULL,
	[description] [nvarchar](1024) NOT NULL,
	[userId] [int] NOT NULL,
	[moduleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyType]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Display]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Display](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ApiKey] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EngineerIncidentMapper]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EngineerIncidentMapper](
	[incidentId] [int] NOT NULL,
	[engineerId] [int] NOT NULL,
 CONSTRAINT [PK_EngineerIncidentMapper] PRIMARY KEY CLUSTERED 
(
	[incidentId] ASC,
	[engineerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Incident]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Incident](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[customer] [varchar](100) NOT NULL,
	[customerPhone] [varchar](15) NOT NULL,
	[description] [varchar](255) NOT NULL,
	[workOrderNo] [varchar](255) NULL,
	[ipAddress] [varchar](45) NULL,
	[responseDateTime] [datetime] NULL,
	[subjectId] [int] NOT NULL,
	[incidentCategoryId] [int] NOT NULL,
	[incidentStatusId] [int] NOT NULL,
	[subItem] [varchar](255) NOT NULL,
	[address] [varchar](255) NOT NULL,
	[company] [varchar](255) NOT NULL,
	[companyTypeId] [int] NOT NULL,
	[requestTypeId] [int] NULL,
	[solution] [varchar](255) NULL,
	[incidentCreatedDateTime] [datetime] NOT NULL,
	[refNum]  AS (CONVERT([varchar](4),datepart(year,[incidentCreatedDateTime]))+right('00000'+CONVERT([varchar](5),[ID]),(5))) PERSISTED,
	[adminId] [int] NOT NULL,
	[completedDateTime] [datetime] NULL,
	[arrivalDateTime] [datetime] NULL,
	[signature] [varchar](max) NULL,
 CONSTRAINT [PK__IncidentId] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncidentCategory]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncidentCategory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncidentStatus]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncidentStatus](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Module]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Module](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[moduleName] [nvarchar](50) NOT NULL,
	[url] [varchar](512) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Replacement]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Replacement](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[model] [varchar](255) NOT NULL,
	[oldSerialNo] [varchar](255) NOT NULL,
	[newSerialNo] [varchar](255) NOT NULL,
	[remarks] [varchar](255) NOT NULL,
	[incidentId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestType]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleModuleMapper]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleModuleMapper](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[roleID] [int] NOT NULL,
	[moduleID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[isDeleted] [bit] NOT NULL,
	[categoryID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subject]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[loginName] [nvarchar](50) NOT NULL,
	[password] [varchar](64) NOT NULL,
	[passwordSalt] [varchar](12) NOT NULL,
	[isDeleted] [bit] NOT NULL,
	[userStatusID] [int] NULL,
	[emailAddress] [nvarchar](320) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoleMapper]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoleMapper](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[roleID] [int] NOT NULL,
	[userID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserStatus]    Script Date: 9/1/2025 4:14:03 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserStatus](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[note] [text] NOT NULL,
	[statusID] [int] NOT NULL,
	[userID] [int] NULL,
	[lastUpdated] [datetime] NOT NULL,
	[end_time] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Display] ADD  CONSTRAINT [DF_Display_ApiKey]  DEFAULT (newid()) FOR [ApiKey]
GO
ALTER TABLE [dbo].[Audit]  WITH CHECK ADD FOREIGN KEY([moduleId])
REFERENCES [dbo].[Module] ([id])
GO
ALTER TABLE [dbo].[Audit]  WITH CHECK ADD FOREIGN KEY([userId])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[EngineerIncidentMapper]  WITH CHECK ADD  CONSTRAINT [FK_EngineerIncidentMapper_Incident] FOREIGN KEY([incidentId])
REFERENCES [dbo].[Incident] ([id])
GO
ALTER TABLE [dbo].[EngineerIncidentMapper] CHECK CONSTRAINT [FK_EngineerIncidentMapper_Incident]
GO
ALTER TABLE [dbo].[EngineerIncidentMapper]  WITH CHECK ADD  CONSTRAINT [FK_EngineerIncidentMapper_User] FOREIGN KEY([engineerId])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[EngineerIncidentMapper] CHECK CONSTRAINT [FK_EngineerIncidentMapper_User]
GO
ALTER TABLE [dbo].[Incident]  WITH CHECK ADD  CONSTRAINT [FK__Incident__CompanyType] FOREIGN KEY([companyTypeId])
REFERENCES [dbo].[CompanyType] ([id])
GO
ALTER TABLE [dbo].[Incident] CHECK CONSTRAINT [FK__Incident__CompanyType]
GO
ALTER TABLE [dbo].[Incident]  WITH CHECK ADD  CONSTRAINT [FK__Incident__IncidentCategory] FOREIGN KEY([incidentCategoryId])
REFERENCES [dbo].[IncidentCategory] ([id])
GO
ALTER TABLE [dbo].[Incident] CHECK CONSTRAINT [FK__Incident__IncidentCategory]
GO
ALTER TABLE [dbo].[Incident]  WITH CHECK ADD  CONSTRAINT [FK__Incident__IncidentStatus] FOREIGN KEY([incidentStatusId])
REFERENCES [dbo].[IncidentStatus] ([id])
GO
ALTER TABLE [dbo].[Incident] CHECK CONSTRAINT [FK__Incident__IncidentStatus]
GO
ALTER TABLE [dbo].[Incident]  WITH CHECK ADD  CONSTRAINT [FK__Incident__Subject] FOREIGN KEY([subjectId])
REFERENCES [dbo].[Subject] ([id])
GO
ALTER TABLE [dbo].[Incident] CHECK CONSTRAINT [FK__Incident__Subject]
GO
ALTER TABLE [dbo].[Incident]  WITH CHECK ADD  CONSTRAINT [FK_Incident_Admin] FOREIGN KEY([adminId])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[Incident] CHECK CONSTRAINT [FK_Incident_Admin]
GO
ALTER TABLE [dbo].[Incident]  WITH CHECK ADD  CONSTRAINT [FK_Incident_RequestType] FOREIGN KEY([requestTypeId])
REFERENCES [dbo].[RequestType] ([id])
GO
ALTER TABLE [dbo].[Incident] CHECK CONSTRAINT [FK_Incident_RequestType]
GO
ALTER TABLE [dbo].[Replacement]  WITH CHECK ADD FOREIGN KEY([incidentId])
REFERENCES [dbo].[Incident] ([id])
GO
ALTER TABLE [dbo].[RoleModuleMapper]  WITH CHECK ADD  CONSTRAINT [FK_RoleModuleMapper_ModuleID] FOREIGN KEY([moduleID])
REFERENCES [dbo].[Module] ([id])
GO
ALTER TABLE [dbo].[RoleModuleMapper] CHECK CONSTRAINT [FK_RoleModuleMapper_ModuleID]
GO
ALTER TABLE [dbo].[RoleModuleMapper]  WITH CHECK ADD  CONSTRAINT [FK_RoleModuleMapper_RoleID] FOREIGN KEY([roleID])
REFERENCES [dbo].[Role] ([id])
GO
ALTER TABLE [dbo].[RoleModuleMapper] CHECK CONSTRAINT [FK_RoleModuleMapper_RoleID]
GO
ALTER TABLE [dbo].[Status]  WITH CHECK ADD  CONSTRAINT [Status_Category_FK] FOREIGN KEY([categoryID])
REFERENCES [dbo].[Category] ([id])
GO
ALTER TABLE [dbo].[Status] CHECK CONSTRAINT [Status_Category_FK]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [User_UserStatus_FK] FOREIGN KEY([userStatusID])
REFERENCES [dbo].[UserStatus] ([id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [User_UserStatus_FK]
GO
ALTER TABLE [dbo].[UserRoleMapper]  WITH CHECK ADD  CONSTRAINT [FK1] FOREIGN KEY([roleID])
REFERENCES [dbo].[Role] ([id])
GO
ALTER TABLE [dbo].[UserRoleMapper] CHECK CONSTRAINT [FK1]
GO
ALTER TABLE [dbo].[UserRoleMapper]  WITH CHECK ADD  CONSTRAINT [FK2] FOREIGN KEY([userID])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[UserRoleMapper] CHECK CONSTRAINT [FK2]
GO
ALTER TABLE [dbo].[UserStatus]  WITH CHECK ADD  CONSTRAINT [FK_userID] FOREIGN KEY([userID])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[UserStatus] CHECK CONSTRAINT [FK_userID]
GO
ALTER TABLE [dbo].[UserStatus]  WITH CHECK ADD  CONSTRAINT [UserStatus_Status_FK] FOREIGN KEY([statusID])
REFERENCES [dbo].[Status] ([id])
GO
ALTER TABLE [dbo].[UserStatus] CHECK CONSTRAINT [UserStatus_Status_FK]
GO
USE [master]
GO
ALTER DATABASE [EmsDB] SET  READ_WRITE 
GO
