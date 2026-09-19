using NUnit.Framework;

namespace Ucu.Poo.Repositories.Tests
{
    [TestFixture]
    public class MovieTests
    {
        private Movie movie;

        [SetUp]
        public void SetUp()
        {
            this.movie = new Movie("Inception", 2010);
        }

        [Test]
        public void HasValue_MatchingName_ReturnsTrue()
        {
            Assert.That(this.movie.HasValue("Name", "Inception"), Is.True);
        }

        [Test]
        public void HasValue_MatchingYear_ReturnsTrue()
        {
            Assert.That(this.movie.HasValue("Year", "2010"), Is.True);
        }

        [Test]
        public void HasValue_NonMatchingValue_ReturnsFalse()
        {
            Assert.That(this.movie.HasValue("Name", "Tenet"), Is.False);
        }

        [Test]
        public void HasValue_UnknownField_ReturnsFalse()
        {
            Assert.That(this.movie.HasValue("Director", "Nolan"), Is.False);
        }

        [Test]
        public void HasValue_NullField_ReturnsFalse()
        {
            Assert.That(this.movie.HasValue(null, "Inception"), Is.False);
        }

        [Test]
        public void HasValue_NullValue_ReturnsFalse()
        {
            Assert.That(this.movie.HasValue("Name", null), Is.False);
        }
    }
}
