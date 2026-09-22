//------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Ucu.Poo.Repositories
{
    /// <summary>
    /// Programa principal.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Punto de entrada al programa principal.
        /// </summary>
        public static void Main()
        {
            Car jimny = new Car("Jimny", "Suzuki", 2024);
            Car focus = new Car("Focus", "Ford", 2018);
            Repository<Car, IReadOnlyCar> database = new Repository<Car, IReadOnlyCar>();
            database.Add(jimny);
            database.Add(focus);
            database.SaveToFile("cars.json");
            Console.WriteLine("Database saved:");
            foreach (IReadOnlyCar car in database.Items)
            {
                Console.WriteLine($"Model: {car.Model}, Maker: {car.Maker}, Year: {car.Year}");
            }

            Repository<Car, IReadOnlyCar> restoredDatabase = new Repository<Car, IReadOnlyCar>();
            restoredDatabase.LoadFromFile("cars.json");
            Console.WriteLine("Restored database:");
            foreach (IReadOnlyCar car in restoredDatabase.Items)
            {
                Console.WriteLine($"Model: {car.Model}, Maker: {car.Maker}, Year: {car.Year}");
            }

            Movie movie = new Movie("The Avengers", 2014);
            Movie movie2 = new Movie("Avengers Endgame", 2018);
            Repository<Movie, IReadOnlyMovie> catalog = new Repository<Movie, IReadOnlyMovie>();
            catalog.Add(movie);
            catalog.Add(movie2);
            catalog.SaveToFile("movies.json");
            Console.WriteLine("Catalog saved:");
            foreach (IReadOnlyMovie item in catalog.Items)
            {
                Console.WriteLine($"Name: {item.Name}, Year: {item.Year}");
            }
        }
    }
}