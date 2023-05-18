using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SQLQueryProvider.Models;
using SQLQueryProvider.QueryProvider;
using SQLQueryProvider.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLQuryProvider.Test
{
    public class SqlProviderTest
    {
        private IConfigurationRoot _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        [Test]
        public void GetAll()
        {
            // Arrange
            SetupDb();

            var set = new SqlEntitySet<Person>(new SqlQueryService(_configuration.GetConnectionString("Connection")));

            // Act
            var persons = set.ToList();

            // Assert
            persons.Should().BeEquivalentTo(new List<Person>
            {
                new Person { FirstName = "fName1", LastName = "lName1", Age = 20 },
                new Person { FirstName = "fName2", LastName = "lName2", Age = 20 },
                new Person { FirstName = "fName3", LastName = "lName3", Age = 25 }
            });
        }

        [Test]
        public void WhereClause()
        {
            // Arrange
            SetupDb();

            var set = new SqlEntitySet<Person>(new SqlQueryService(_configuration.GetConnectionString("Connection")));

            // Act
            var persons = set.Where(person => person.Age < 25).ToList();

            // Assert
            persons.Should().BeEquivalentTo(new List<Person>
            {
                new Person { FirstName = "fName1", LastName = "lName1", Age = 20 },
                new Person { FirstName = "fName2", LastName = "lName2", Age = 20 }
            });
        }

        private void SetupDb()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Connection")))
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
        }

        private void ExecuteCommand(string query, SqlConnection connection)
        {
            var command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
        }
    }
}
