IF(OBJECTPROPERTY(object_id('[internal].[AccountStatus]'), N'IsTable')  IS NULL)
BEGIN
	CREATE TABLE [internal].[AccountStatus]
	(
		[Id] SMALLINT NOT NULL
		,[StatusTitle] CHAR(20) NOT NULL
		,[CreatedDateTime] DATETIME2(7) NOT NULL DEFAULT (SYSDATETIME())
		,[CreatedBy] VARCHAR(50) NOT NULL DEFAULT (SUSER_NAME())
		,CONSTRAINT [PK_AccountStatus] PRIMARY KEY CLUSTERED
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	INSERT INTO internal.ChangeTracker (ChangeDescription) VALUES ('Created table: internal.AccountStatus');

	INSERT INTO internal.AccountStatus (Id, StatusTitle) VALUES (1, 'Pending');
	INSERT INTO internal.AccountStatus (Id, StatusTitle) VALUES (2, 'Active');
	INSERT INTO internal.AccountStatus (Id, StatusTitle) VALUES (3, 'InActive');
	INSERT INTO internal.AccountStatus (Id, StatusTitle) VALUES (4, 'Suspended');

	INSERT INTO internal.ChangeTracker (ChangeDescription) VALUES ('Reference data insert: internal.AccountStatus');

END
GO