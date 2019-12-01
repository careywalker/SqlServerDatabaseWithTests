CREATE OR ALTER PROCEDURE internal.AccountSelect
	@Id INT = NULL
AS
BEGIN
	SELECT
		[ia].[Id],
		[ia].[Username],
		[ia].[FirstName],
		[ia].[LastName],
		[ia].[Email],
		[ias].[StatusTitle]
	FROM internal.Account ia INNER JOIN internal.AccountStatus ias ON ia.StatusId = ias.Id
	WHERE ia.Id = CASE WHEN @Id IS NULL THEN ia.Id ELSE @Id END
END
GO

INSERT INTO internal.ChangeTracker (ChangeDescription) VALUES ('CREATE OR ALTER PROCEDURE internal.AccountSelect')
GO