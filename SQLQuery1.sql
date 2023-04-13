IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Dentist')
BEGIN
    CREATE database Dentist
	PRINT 'Database Dentist successfully created.'
END
ELSE
	PRINT 'You already have database Dentist created.'
GO

USE [Dentist]
GO

:setvar path "C:\Users\jesse\OneDrive\Tiedostot\Object oriented programming\OopDentist\OopDentist"

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
           WHERE TABLE_NAME = N'Dentist')
BEGIN
	:r $(path)\CreateDentist.sql
	PRINT 'Table Dentist successfully created.'
END
ELSE
	PRINT 'You already have table Dentist created.'
GO

IF NOT EXISTS (SELECT * FROM Dentist)
BEGIN
	:r $(path)\InsertDentist.sql
	PRINT 'Information added successfully to table Dentist.'
END
ELSE
	PRINT 'You already have the Dentist information added.'
GO