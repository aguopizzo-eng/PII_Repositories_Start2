//------------------------------------------------------------------------------
// <copyright file="MoviesCatalog.cs" company="Universidad Católica del Uruguay">
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
    /// Esta clase representa un catálogo de películas.
    /// </summary>
    public class MoviesCatalog
    {
        private List<Movie> movies = new List<Movie>();

        /// <summary>
        /// Obtiene la lista de películas en el catálogo.
        /// </summary>
        public ReadOnlyCollection<Movie> Movies
        {
            get { return this.movies.AsReadOnly(); }
        }

        /// <summary>
        /// Agrega una película al catálogo.
        /// </summary>
        /// <param name="movie">La película a agregar.</param>
        public void Add(Movie movie)
        {
            if (movie != null)
            {
                this.movies.Add(movie);
            }
        }

        /// <summary>
        /// Elimina una película del catálogo.
        /// </summary>
        /// <param name="movie">La película a remover.</param>
        public void Remove(Movie movie)
        {
            this.movies.Remove(movie);
        }

        /// <summary>
        /// Busca una película en el catálogo que cumpla con un criterio
        /// específico.
        /// </summary>
        /// <param name="field">El nombre del atributo por el cual
        /// busMovie.</param>
        /// <param name="value">El valor del atributo por el cual
        /// busMovie.</param>
        /// <returns>La película encontrada que cumple el criterio especificado
        /// o null si no se encuentra ninguna.</returns>
        public Movie Find(string field, string value)
        {
            foreach (Movie movie in this.movies)
            {
                if (movie.HasValue(field, value))
                {
                    return movie;
                }
            }

            return null;
        }

        /// <summary>
        /// Convierte la catálogo de películas a una representación en formato
        /// JSON.
        /// </summary>
        /// <returns>Una representación de la base de datos en formato
        /// JSON.</returns>
        public string ConvertToJson()
        {
            return JsonSerializer.Serialize(this.movies);
        }

        /// <summary>
        /// Carga el catálogo de películas desde una representación en formato
        /// JSON.
        /// </summary>
        /// <param name="content">La representación en formato JSON desde la
        /// cual cargar el catálogo.</param>
        public void LoadFromJson(string content)
        {
                List<Movie> items = JsonSerializer.Deserialize<List<Movie>>(content);
                if (items != null)
                {
                    this.movies = items;
                }
                else
                {
                    this.movies = new List<Movie>();
                }
        }

        /// <summary>
        /// Guarda el catálogo de películas en un archivo en formato JSON.
        /// </summary>
        /// <param name="filePath">El nombre del archivo, incluyendo
        /// opcionalmente la ruta.</param>
        public void SaveToFile(string filePath)
        {
            string content = this.ConvertToJson();
            File.WriteAllText(filePath, content);
        }

        /// <summary>
        /// Carga la catálogo de películas desde un archivo en formato JSON.
        /// </summary>
        /// <param name="filePath">El nombre del archivo, incluyendo
        /// opcionalmente la ruta.</param>
        /// <returns>Retorna <c>true</c> si se cargó el catálogo y <c>false</c>
        /// en caso contrario.</returns>
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
