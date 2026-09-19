//------------------------------------------------------------------------------
// <copyright file="CarsDatabase.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace Ucu.Poo.Repositories
{
    /// <summary>
    /// Esta clase representa una base de datos de autos.
    /// </summary>
    public class CarsDatabase
    {
        private List<Car> cars = new List<Car>();

        /// <summary>
        /// Obtiene la lista de autos en la base de datos.
        /// </summary>
        public ReadOnlyCollection<Car> Cars
        {
            get { return this.cars.AsReadOnly(); }
        }

        /// <summary>
        /// Agrega un auto a la base de datos.
        /// </summary>
        /// <param name="car">El auto a agregar.</param>
        public void Add(Car car)
        {
            if (car != null)
            {
                this.cars.Add(car);
            }
        }

        /// <summary>
        /// Elimina un auto de la base de datos.
        /// </summary>
        /// <param name="car">El auto a remover.</param>
        public void Remove(Car car)
        {
            this.cars.Remove(car);
        }

        /// <summary>
        /// Busca un auto en la base de datos que cumpla con un criterio
        /// específico.
        /// </summary>
        /// <param name="field">El nombre del atributo por el cual
        /// buscar.</param>
        /// <param name="value">El valor del atributo por el cual
        /// buscar.</param>
        /// <returns>El auto encontrado que cumple el criterio especificado o
        /// null si no se encuentra ninguno.</returns>
        public Car Find(string field, string value)
        {
            foreach (Car car in this.cars)
            {
                if (car.HasValue(field, value))
                {
                    return car;
                }
            }

            return null;
        }

        /// <summary>
        /// Convierte la base de datos de autos a una representación en formato
        /// JSON.
        /// </summary>
        /// <returns>Una representación de la base de datos en formato
        /// JSON.</returns>
        public string ConvertToJson()
        {
            return JsonSerializer.Serialize(this.cars);
        }

        /// <summary>
        /// Carga la base de datos de autos desde una representación en formato
        /// JSON.
        /// </summary>
        /// <param name="content">La representación en formato JSON desde la
        /// cual cargar la base de datos.</param>
        public void LoadFromJson(string content)
        {
                List<Car> items = JsonSerializer.Deserialize<List<Car>>(content);
                if (items != null)
                {
                    this.cars = items;
                }
                else
                {
                    this.cars = new List<Car>();
                }
        }

        /// <summary>
        /// Guarda la base de datos de autos en un archivo en formato JSON.
        /// </summary>
        /// <param name="filePath">El nombre del archivo, incluyendo
        /// opcionalmente la ruta.</param>
        public void SaveToFile(string filePath)
        {
            string content = this.ConvertToJson();
            File.WriteAllText(filePath, content);
        }

        /// <summary>
        /// Carga la base de datos de autos desde un archivo en formato JSON.
        /// </summary>
        /// <param name="filePath">El nombre del archivo, incluyendo
        /// opcionalmente la ruta.</param>
        /// <returns>Retorna <c>true</c> si se cargó la base de datos y
        /// <c>false</c> en caso contrario.</returns>
        public bool LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                this.LoadFromJson(content);
                return true;
            }

            return false;
        }
    }
}
