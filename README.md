# Adding CI/CD for SQL Server databases and Unit Testing your database code with MS Test
This is an example of using Sql Server Data Tools and Visual Studio to build a .dacpac file. The .dacpac file allows for automated deployment via TFS or any other CI/CD tool.

The other project included is a MSTest project. The code in the test project deploys the database and runs the unit tests against the database. This allows you to build unit tests for your database code.

