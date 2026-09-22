using System;
using System.IO;
using NUnit.Framework;

namespace Ucu.Poo.Repositories.Tests
{
  public class RepositoryTests
  {
    private Repository<Car, IReadOnlyCar> repository;

    [SetUp]
    public void SetUp()
    {
        this.repository = new Repository<Car, IReadOnlyCar>();
    }

    [Test]
    public void Find_ExistingModel_ReturnsThatCarAsReadOnly()
    {
      Car car = new Car("Jimny", "Suzuki", 2024);
      this.repository.Add(car);

      IReadOnlyCar found = this.repository.Find("Model", "Jimny");
      Assert.That(found, Is.SameAs(car));
    }

    [Test]
    public void FindCar_NoMatchingCriteria_ReturnsNull()
    {
        Car car = new Car("Sandero", "Renault", 2015);
        this.repository.Add(car);

        IReadOnlyCar found = this.repository.Find("Maker", "Ford");

        Assert.That(found, Is.Null);
    }

    [Test]
    public void Add_Null_DoesNotAdd()
    {
        this.repository.Add(null);

        Assert.That(this.repository.ConvertToJson(), Is.EqualTo("[]"));
    }

    [Test]
    public void Remove_ExistingCar_RemovesIt()
    {
        Car car = new Car("Jimny", "Suzuki", 2024);
        this.repository.Add(car);

        this.repository.Remove(car);

        Assert.That(this.repository.ConvertToJson(), Is.EqualTo("[]"));
    }

    [Test]
    public void SaveToFile_ThenLoadFromFile_RestoresSameData()
    {
        this.repository.Add(new Car("Jimny", "Suzuki", 2024));
        string path = "test_cars.json";
        this.repository.SaveToFile(path);

        Repository<Car, IReadOnlyCar> restored = new Repository<Car, IReadOnlyCar>();
        bool loaded = restored.LoadFromFile(path);

        Assert.That(loaded, Is.True);
        Assert.That(restored.ConvertToJson(), Is.EqualTo(this.repository.ConvertToJson()));
        File.Delete(path);
    }

    [Test]
    public void Movies_SaveToFile_ThenLoadFromFile_RestoresSameData()
    {
      Repository<Movie, IReadOnlyMovie> movies = new Repository<Movie, IReadOnlyMovie>();
      movies.Add(new Movie("Inception", 2010));
      string path = "test_movies.json";
      movies.SaveToFile(path);

      Repository<Movie, IReadOnlyMovie> restored = new Repository<Movie, IReadOnlyMovie>();
      bool loaded = restored.LoadFromFile(path);

      Assert.That(loaded, Is.True);
      Assert.That(restored.ConvertToJson(), Is.EqualTo(movies.ConvertToJson()));
      File.Delete(path);
    }
  }
}