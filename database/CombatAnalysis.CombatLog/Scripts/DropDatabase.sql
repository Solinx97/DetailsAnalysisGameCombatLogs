--Close all connections for database
USE [Combat_Analysis.Combat_Logs]
ALTER DATABASE [Combat_Analysis.Combat_Logs] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO

USE [master]
GO

--Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'Combat_Analysis.Combat_Logs')
DROP DATABASE [Combat_Analysis.Combat_Logs]
GO