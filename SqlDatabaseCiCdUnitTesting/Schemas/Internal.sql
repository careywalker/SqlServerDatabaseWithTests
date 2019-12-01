IF NOT EXISTS (SELECT name from sys.schemas WHERE name = N'Internal')
BEGIN
	EXEC('CREATE SCHEMA [internal] AUTHORIZATION [dbo]');
END
GO