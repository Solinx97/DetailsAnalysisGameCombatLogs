--Close all connections for database
USE [Combat_Analysis.Identity]
ALTER DATABASE [Combat_Analysis.Identity] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO

USE [master]
GO

--Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'Combat_Analysis.Identity')
DROP DATABASE [Combat_Analysis.Identity]
GO