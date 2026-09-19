using NUnit.Framework;

namespace Ucu.Poo.Repositories.Tests
{
    [TestFixture]
    public class CarTests
    {
        private Car car;

        [SetUp]
        public void SetUp()
        {
            this.car = new Car("Jimny", "Suzuki", 2024);
        }

        [Test]
        public void HasValue_MatchingModel_ReturnsTrue()
        {
            Assert.That(this.car.HasValue("Model", "Jimny"), Is.True);
        }

        [Test]
        public void HasValue_MatchingMaker_ReturnsTrue()
        {
            Assert.That(this.car.HasValue("Maker", "Suzuki"), Is.True);
        }

        [Test]
        public void HasValue_MatchingYear_ReturnsTrue()
        {
            Assert.That(this.car.HasValue("Year", "2024"), Is.True);
        }

        [Test]
        public void HasValue_NonMatchingValue_ReturnsFalse()
        {
            Assert.That(this.car.HasValue("Model", "Focus"), Is.False);
        }

        [Test]
        public void HasValue_UnknownField_ReturnsFalse()
        {
            Assert.That(this.car.HasValue("Color", "Red"), Is.False);
        }

        [Test]
        public void HasValue_NullField_ReturnsFalse()
        {
            Assert.That(this.car.HasValue(null, "Jimny"), Is.False);
        }

        [Test]
        public void HasValue_NullValue_ReturnsFalse()
        {
            Assert.That(this.car.HasValue("Model", null), Is.False);
        }
    }
}
