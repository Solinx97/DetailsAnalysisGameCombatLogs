--Close all connections for database
USE Combat_Analysis_Tests
ALTER DATABASE Combat_Analysis_Tests SET SINGLE_USER WITH ROLLBACK IMMEDIATE

--Drop database if exists
USE MASTER
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'Combat_Analysis_Tests')
DROP DATABASE Combat_Analysis_Tests