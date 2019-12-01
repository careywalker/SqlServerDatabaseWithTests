IF(OBJECTPROPERTY(object_id('[internal].[Account]'), N'IsTable')  IS NULL)
BEGIN
	CREATE TABLE [internal].[Account]
	(
		[Id] INT IDENTITY (1, 1) NOT NULL
		,[Username] VARCHAR(1000) NOT NULL
		,[FirstName] VARCHAR(100) NOT NULL
		,[LastName] VARCHAR(100) NOT NULL
		,[Email] VARCHAR(100) NOT NULL
		,[StatusId] SMALLINT NOT NULL
		,[CreatedDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSDATETIME())
		,[CreatedBy] VARCHAR(50) NOT NULL DEFAULT (SUSER_NAME())
	)

	ALTER TABLE [internal].[Account] WITH CHECK ADD CONSTRAINT [FK_Account_AccountStatus] FOREIGN KEY ([StatusId])
	REFERENCES [internal].[AccountStatus] ([Id])

	ALTER TABLE [internal].[Account] CHECK CONSTRAINT [FK_Account_AccountStatus]

	INSERT INTO internal.ChangeTracker (ChangeDescription) VALUES ('Created table: internal.Account')

END
GO