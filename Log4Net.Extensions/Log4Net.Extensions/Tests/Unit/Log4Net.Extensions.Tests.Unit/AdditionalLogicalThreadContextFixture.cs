using NUnit.Framework;
using System.Collections.Generic;
using Walls.Julian.Log4Net.Extensions;

namespace Walls.Julian.Log4Net.Extensions.Tests.Unit
{
    [TestFixture]
    internal class AdditionalLogicalThreadContextFixture
    {
        [Test]
        public void TestAdditionalLogicalThreadContextAddsKeyValuesToLogicalThreadContext_And_RemovesOnDispose()
        {
            //Arrange
            IDictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Testing", "123");
            properties.Add("Bob", "the builder");

            //Act and Assert
            using (var context = new AdditionalLogicalThreadContext(properties))
            {
                Assert.Multiple(() =>
                {
                    Assert.That(log4net.LogicalThreadContext.Properties["Testing"], Is.EqualTo("123"),"Property not added");
                    Assert.That(log4net.LogicalThreadContext.Properties["Bob"], Is.EqualTo("the builder"), "Property not added");
                });              
            }

            Assert.Multiple(() =>
            {
                Assert.That(log4net.LogicalThreadContext.Properties["Testing"], Is.Null, "Property not removed");
                Assert.That(log4net.LogicalThreadContext.Properties["Bob"], Is.Null, "Property not removed");
            });
        }
    }
}
