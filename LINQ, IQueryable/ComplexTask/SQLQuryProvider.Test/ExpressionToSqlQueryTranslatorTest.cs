using NUnit.Framework;
using SQLQueryProvider;
using System.Linq.Expressions;
using System;
using SQLQueryProvider.Models;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace SQLQuryProvider.Test
{
    public class ExpressionToSqlQueryTranslatorTest
    {

        [TestCaseSource(nameof(PredicateTestData))]
        public void EqualsTest(Expression<Func<Person, bool>> expression, string expectedResult)
        {
            // Arrange
            var translator = new ExpressionToSqlQueryTranslator();

            // Act
            var translated = translator.Translate(expression);

            // Assert
            translated.Should().Be(expectedResult);
        }

        [TestCaseSource(nameof(WhereClauseTestData))]
        public void WhereClauseTest(Expression<Func<IQueryable<Person>, IQueryable<Person>>> expression, string expectedResult)
        {
            // Arrange
            var translator = new ExpressionToSqlQueryTranslator();

            // Act
            string translated = translator.Translate(expression);

            // Assert
            translated.Should().Be(expectedResult);
        }

        private static IEnumerable<TestCaseData> WhereClauseTestData
        {
            get
            {
                yield return Create(query => query.Where(person => person.Age == 10 && person.FirstName.CompareTo(person.LastName) > 0), 
                    "Where clause query test",
                    "select * from Person where Age = 10 and FirstName > LastName ");
            }
        }

        private static TestCaseData Create(Expression<Func<IQueryable<Person>, IQueryable<Person>>> expression, string testName, string testResult)
        {
            return new TestCaseData(expression, testResult).SetName(testName);
        }

        private static IEnumerable<TestCaseData> PredicateTestData
        {
            get
            {
                yield return Create(person => person.Age == 20, "Int Member = Constant", "Age = 20 ");
                yield return Create(person => 20 == person.Age, "Constant = Int Member", "20 = Age ");
                yield return Create(person => person.FirstName == "first name", "String Member = Constant", "FirstName = 'first name' ");
                yield return Create(person => "first name" == person.FirstName, "Constant = String Member", "'first name' = FirstName ");
                yield return Create(person => person.LastName == person.FirstName, "String Member = String Member", "LastName = FirstName ");
                yield return Create(person => person.Age > 20, "Int Member > Constant", "Age > 20 ");
                yield return Create(person => 20 > person.Age, "Constant > Int Member", "20 > Age ");
                yield return Create(person => person.FirstName.CompareTo("first name") > 0, "String Member > Constant", "FirstName > 'first name' ");
                yield return Create(person => "first name".CompareTo(person.FirstName) > 0, "Constant > String Member", "'first name' > FirstName ");
                yield return Create(person => person.FirstName.CompareTo(person.LastName) > 0, "String Member > String Member", "FirstName > LastName ");
                yield return Create(person => person.Age < 20, "Int Member < Constant", "Age < 20 ");
                yield return Create(person => 20 < person.Age, "Constant < Int Member", "20 < Age ");
                yield return Create(person => person.FirstName.CompareTo("first name") < 0, "String Member < Constant", "FirstName < 'first name' ");
                yield return Create(person => "first name".CompareTo(person.FirstName) < 0, "Constant < String Member", "'first name' < FirstName ");
                yield return Create(person => person.FirstName.CompareTo(person.LastName) < 0, "String Member < String Member", "FirstName < LastName ");
                yield return Create(
                    person => person.FirstName == "first name" && person.Age > 20 && "first name".CompareTo(person.FirstName) < 0, 
                    "String Member = Constant && Int Member > Constant && Constant < String Member",
                    "FirstName = 'first name' and Age > 20 and 'first name' < FirstName ");

            }
        }

        private static TestCaseData Create(Expression<Func<Person, bool>> expression, string testName, string testResult)
        {
            return new TestCaseData(expression, testResult).SetName(testName);
        }
    }
}