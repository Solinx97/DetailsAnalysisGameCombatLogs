--Close all connections for database
USE [Combat_Analysis.Community]
ALTER DATABASE [Combat_Analysis.Community] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO

USE [master]
GO

--Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'Combat_Analysis.Community')
DROP DATABASE [Combat_Analysis.Community]
GO