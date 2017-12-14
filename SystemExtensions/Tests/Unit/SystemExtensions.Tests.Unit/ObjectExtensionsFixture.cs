using NUnit.Framework;

namespace Walls.Julian.System.Extensions.Tests.Units
{
    [TestFixture]
    internal class ObjectExtensionsFixture
    {
        [Test]
        public void IsNull_ReturnsTrue_WhenObjectIsNull()
        {
            //Arrange
            TestObject @object = null;

            //Act and Assert
            Assert.That(@object.IsNull(), Is.True);
        }

        [Test]
        public void IsNull_ReturnsFalse_WhenObjectIsNotNull()
        {
            //Arrange
            TestObject @object = new TestObject();

            //Act and Assert
            Assert.That(@object.IsNull(), Is.False);
        }

        [Test]
        public void IsNotNull_ReturnsTrue_WhenObjectIsNotNull()
        {
            //Arrange
            TestObject @object = new TestObject();

            //Act and Assert
            Assert.That(@object.IsNotNull(), Is.True);
        }

        [Test]
        public void IsNotNull_ReturnsFalse_WhenObjectIsNull()
        {
            //Arrange
            TestObject @object = null;

            //Act and Assert
            Assert.That(@object.IsNotNull(), Is.False);
        }

        private class TestObject
        {

        }
    }
}
