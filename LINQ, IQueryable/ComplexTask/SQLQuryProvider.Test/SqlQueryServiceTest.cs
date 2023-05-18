using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SQLQueryProvider.Models;
using SQLQueryProvider.Services;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQLQuryProvider.Test
{
    public class SqlQueryServiceTest
    {
        [Test]
        public void Test()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
            var connectionString = configuration.GetConnectionString("Connection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Drop table if exists
                ExecuteCommand("IF OBJECT_ID('Person', 'U') IS NOT NULL DROP TABLE Person", connection);

                // Create table
                ExecuteCommand("CREATE TABLE Person(FirstName nvarchar(30), LastName nvarchar(30), Age int, PRIMARY KEY(FirstName, LastName));", connection);

                // Insert data
                ExecuteCommand("insert into Person(FirstName, LastName, Age) values ('fName1', 'lName1', 20)", connection);
                ExecuteCommand("insert into Person(FirstName, LastName, Age) values ('fName2', 'lName2', 20)", connection);
                ExecuteCommand("insert into Person(FirstName, LastName, Age) values ('fName3', 'lName3', 25)", connection);
            }

            var service = new SqlQueryService(connectionString);

            // Act
            var persons = service.Execute<Person>("select * from Person");

            // Assert
            persons.Should().BeEquivalentTo(new List<Person>
            {
                new Person { FirstName = "fName1", LastName = "lName1", Age = 20 },
                new Person { FirstName = "fName2", LastName = "lName2", Age = 20 },
                new Person { FirstName = "fName3", LastName = "lName3", Age = 25 }
            });
        }

        private void ExecuteCommand(string query, SqlConnection connection)
        {
            var command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
        }
    }
}
