--Close all connections for database
USE [Combat_Analysis.Chat]
ALTER DATABASE [Combat_Analysis.Chat] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO

USE [master]
GO

--Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'Combat_Analysis.Chat')
DROP DATABASE [Combat_Analysis.Chat]
GO