/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
:r .\Schemas\Internal.sql
:r .\Internal\Tables\internal.ChangeTracker.sql
:r .\Internal\Tables\internal.AccountStatus.sql
:r .\Internal\Tables\internal.Account.sql
:r .\Internal\StoredProcedures\internal.AccountSelect.sql
:r .\Internal\StoredProcedures\internal.AccountUpsert.sql