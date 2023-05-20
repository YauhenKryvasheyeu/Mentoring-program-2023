using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        // todo: add as many test methods as you wish, but they should be enough to cover basic scenarios of the mapping generator

        [TestMethod]
        public void TestMethod1()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>((f, b) =>
            {
                b.BarProperty = f.FooProperty;
                b.Year = f.Year.ToString();
            });
            var foo = new Foo
            {
                Name = "name",
                Age = 22,
                Date = DateTime.Now,
                Amount = 345.56,
                FooProperty = "foo property",
                Year = 2023
            };

            var res = mapper.Map(foo);

            Assert.IsNotNull(res);
            Assert.AreEqual(foo.Name, res.Name);
            Assert.AreEqual(foo.Age, res.Age);
            Assert.AreEqual(foo.Date, res.Date);
            Assert.AreEqual(foo.Amount, res.Amount);
            
            // different names
            Assert.AreEqual(foo.FooProperty, res.BarProperty);

            // different types
            Assert.AreEqual(foo.Year.ToString(), res.Year);
        }
    }
}
