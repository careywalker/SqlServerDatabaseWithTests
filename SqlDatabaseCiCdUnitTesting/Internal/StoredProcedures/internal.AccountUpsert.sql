CREATE OR ALTER PROCEDURE internal.AccountUpsert
	@Id INT = 0,
	@Username VARCHAR(100),
	@FirstName VARCHAR(100),
	@LastName VARCHAR(100),
	@Email VARCHAR(100),
	@StatusId SMALLINT
AS
BEGIN
	MERGE internal.Account AS target
	USING (SELECT @Id, @Username, @FirstName, @LastName, @Email, @StatusId) AS source (Id, Username, FirstName, LastName, Email, StatusId)
	ON (target.Id = source.Id)
	WHEN MATCHED THEN
		UPDATE SET
			target.Username = source.Username,
			target.FirstName = source.FirstName,
			target.LastName = source.LastName,
			target.Email = source.Email,
			target.StatusId = source.StatusId
	WHEN NOT MATCHED THEN
		INSERT
		(
			Username,
			FirstName,
			LastName,
			Email,
			StatusId
		)
		VALUES
		(
			@Username,
			@FirstName,
			@LastName,
			@Email,
			@StatusId
		);

		SELECT @@IDENTITY AS 'Id';


END
GO

INSERT INTO internal.ChangeTracker (ChangeDescription) VALUES ('CREATE OR ALTER PROCEDURE internal.AccountUpsert')
GO