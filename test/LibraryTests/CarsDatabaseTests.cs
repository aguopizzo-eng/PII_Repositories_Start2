using System;
using System.IO;
using NUnit.Framework;

namespace Ucu.Poo.Repositories.Tests
{
    [TestFixture]
    public class CarsDatabaseTests
    {
        private CarsDatabase database;

        [SetUp]
        public void SetUp()
        {
            this.database = new CarsDatabase();
        }

        [Test]
        public void AddCar_ValidCar_CarIsFound()
        {
            Car car = new Car("Jimny", "Suzuki", 2024);

            this.database.Add(car);

            IReadOnlyCar found = this.database.FindCar("Model", "Jimny");
            Assert.That(found, Is.SameAs(car));
        }

        [Test]
        public void Cars_AfterAddingCar_ContainsCar()
        {
            Car car = new Car("Jimny", "Suzuki", 2024);
            this.database.Add(car);

            Assert.That(this.database.Cars, Contains.Item(car));
        }

        [Test]
        public void AddCar_NullCar_CarIsNotAdded()
        {
            this.database.Add(null);

            IReadOnlyCar found = this.database.FindCar("Model", "Jimny");
            Assert.That(found, Is.Null);
        }

        [Test]
        public void RemoveCar_ExistingCar_CarIsNoLongerFound()
        {
            Car car = new Car("Focus", "Ford", 2018);
            this.database.Add(car);

            this.database.Remove(car);

            IReadOnlyCar found = this.database.FindCar("Maker", "Ford");
            Assert.That(found, Is.Null);
        }

        [Test]
        public void FindCar_MatchingCriteria_ReturnsCar()
        {
            Car car = new Car("Onix", "Chevrolet", 2022);
            this.database.Add(car);

            IReadOnlyCar found = this.database.FindCar("Year", "2022");

            Assert.That(found, Is.SameAs(car));
        }

        [Test]
        public void FindCar_NoMatchingCriteria_ReturnsNull()
        {
            Car car = new Car("Sandero", "Renault", 2015);
            this.database.Add(car);

            IReadOnlyCar found = this.database.FindCar("Maker", "Ford");

            Assert.That(found, Is.Null);
        }

        [Test]
        public void FindCar_EmptyDatabase_ReturnsNull()
        {
            IReadOnlyCar found = this.database.FindCar("Model", "Jimny");

            Assert.That(found, Is.Null);
        }

        [Test]
        public void ConvertToJson_DatabaseWithCar_ReturnsCarAsJson()
        {
            this.database.Add(new Car("Jimny", "Suzuki", 2024));

            string json = this.database.ConvertToJson();

            Assert.That(json, Is.EqualTo("[{\"Model\":\"Jimny\",\"Maker\":\"Suzuki\",\"Year\":2024}]"));
        }

        [Test]
        public void LoadFromJson_ValidJson_CarIsLoaded()
        {
            this.database.LoadFromJson("[{\"Model\":\"Focus\",\"Maker\":\"Ford\",\"Year\":2018}]");

            IReadOnlyCar found = this.database.FindCar("Model", "Focus");

            Assert.That(found, Is.Not.Null);
            Assert.That(found.Maker, Is.EqualTo("Ford"));
        }

        [Test]
        public void SaveToFile_DatabaseWithCar_FileContainsDatabaseAsJson()
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            this.database.Add(new Car("Onix", "Chevrolet", 2022));

            try
            {
                this.database.SaveToFile(filePath);

                Assert.That(File.ReadAllText(filePath), Is.EqualTo("[{\"Model\":\"Onix\",\"Maker\":\"Chevrolet\",\"Year\":2022}]"));
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
        public void LoadFromFile_ExistingFile_CarIsLoaded()
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            File.WriteAllText(filePath, "[{\"Model\":\"Sandero\",\"Maker\":\"Renault\",\"Year\":2015}]");

            try
            {
                bool loaded = this.database.LoadFromFile(filePath);

                Assert.That(loaded, Is.True);
                Assert.That(this.database.FindCar("Model", "Sandero"), Is.Not.Null);
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

            bool loaded = this.database.LoadFromFile(filePath);

            Assert.That(loaded, Is.False);
        }
    }
}
