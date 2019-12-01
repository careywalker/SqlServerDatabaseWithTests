# Adding CI/CD for SQL Server databases and Unit Testing your database code with MS Test
This is an example of using Sql Server Data Tools and Visual Studio to build a .dacpac file. The .dacpac file allows for automated deployment via TFS or any other CI/CD tool.

The other project included is a MSTest project. The code in the test project deploys the database and runs the unit tests against the database. This allows you to build unit tests for your database code.

To build the solution, make sure you have the latest version of Visual Studio (I have 16.3.10) and you have the Data Storage and Processing workload installed.