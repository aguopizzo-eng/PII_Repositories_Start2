using System;
using System.IO;
using NUnit.Framework;

namespace Ucu.Poo.Repositories.Tests
{
    [TestFixture]
    public class MoviesCatalogTests
    {
        private MoviesCatalog catalog;

        [SetUp]
        public void SetUp()
        {
            this.catalog = new MoviesCatalog();
        }

        [Test]
        public void Add_ValidMovie_MovieIsFound()
        {
            Movie movie = new Movie("Inception", 2010);

            this.catalog.Add(movie);

            Movie found = this.catalog.Find("Name", "Inception");
            Assert.That(found, Is.SameAs(movie));
        }

        [Test]
        public void Add_NullMovie_MovieIsNotAdded()
        {
            this.catalog.Add(null);

            Movie found = this.catalog.Find("Name", "Inception");
            Assert.That(found, Is.Null);
        }

        [Test]
        public void Remove_ExistingMovie_MovieIsNoLongerFound()
        {
            Movie movie = new Movie("The Matrix", 1999);
            this.catalog.Add(movie);

            this.catalog.Remove(movie);

            Movie found = this.catalog.Find("Name", "The Matrix");
            Assert.That(found, Is.Null);
        }

        [Test]
        public void Find_MatchingCriteria_ReturnsMovie()
        {
            Movie movie = new Movie("Interstellar", 2014);
            this.catalog.Add(movie);

            Movie found = this.catalog.Find("Year", "2014");

            Assert.That(found, Is.SameAs(movie));
        }

        [Test]
        public void Find_NoMatchingCriteria_ReturnsNull()
        {
            Movie movie = new Movie("Dunkirk", 2017);
            this.catalog.Add(movie);

            Movie found = this.catalog.Find("Name", "Tenet");

            Assert.That(found, Is.Null);
        }

        [Test]
        public void Find_EmptyCatalog_ReturnsNull()
        {
            Movie found = this.catalog.Find("Name", "Inception");

            Assert.That(found, Is.Null);
        }

        [Test]
        public void ConvertToJson_CatalogWithMovie_ReturnsMovieAsJson()
        {
            this.catalog.Add(new Movie("Inception", 2010));

            string json = this.catalog.ConvertToJson();

            Assert.That(json, Is.EqualTo("[{\"Name\":\"Inception\",\"Year\":2010}]"));
        }

        [Test]
        public void LoadFromJson_ValidJson_MovieIsLoaded()
        {
            this.catalog.LoadFromJson("[{\"Name\":\"The Matrix\",\"Year\":1999}]");

            Movie found = this.catalog.Find("Name", "The Matrix");

            Assert.That(found, Is.Not.Null);
            Assert.That(found.Year, Is.EqualTo(1999));
        }

        [Test]
        public void SaveToFile_CatalogWithMovie_FileContainsCatalogAsJson()
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            this.catalog.Add(new Movie("Interstellar", 2014));

            try
            {
                this.catalog.SaveToFile(filePath);

                Assert.That(File.ReadAllText(filePath), Is.EqualTo("[{\"Name\":\"Interstellar\",\"Year\":2014}]"));
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [Test]
        public void LoadFromFile_ExistingFile_MovieIsLoaded()
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            File.WriteAllText(filePath, "[{\"Name\":\"Dunkirk\",\"Year\":2017}]");

            try
            {
                bool loaded = this.catalog.LoadFromFile(filePath);

                Assert.That(loaded, Is.True);
                Assert.That(this.catalog.Find("Name", "Dunkirk"), Is.Not.Null);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Test]
        public void LoadFromFile_NonExistingFile_ReturnsFalse()
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");

            bool loaded = this.catalog.LoadFromFile(filePath);

            Assert.That(loaded, Is.False);
        }
    }
}
