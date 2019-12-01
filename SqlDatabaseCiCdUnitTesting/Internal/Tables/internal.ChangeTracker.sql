IF(OBJECTPROPERTY(object_id('[internal].[ChangeTracker]'), N'IsTable')  IS NULL)
BEGIN
	CREATE TABLE [internal].[ChangeTracker]
	(
		[Id] INT IDENTITY (1, 1) NOT NULL
		,[ChangeDescription] VARCHAR(1000) NOT NULL
		,[CreatedDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSDATETIME())
		,[CreatedBy] VARCHAR(50) NOT NULL DEFAULT (SUSER_NAME())
	)

	INSERT INTO internal.ChangeTracker (ChangeDescription) VALUES ('Created table: internal.ChangeTracker')

END
GO