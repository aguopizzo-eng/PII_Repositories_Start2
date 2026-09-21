//------------------------------------------------------------------------------
// <copyright file="MoviesCatalog.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ucu.Poo.Repositories
{
    /// <summary>
    /// Esta clase representa un catálogo de películas.
    /// </summary>
    public class MoviesCatalog : Repository<Movie>
    {
        /// <summary>
        /// Obtiene las películas del catálogo en modo solo lectura.
        /// </summary>
        public ReadOnlyCollection<IReadOnlyMovie> Movies
        {
            get
            {
                List<IReadOnlyMovie> result = new List<IReadOnlyMovie>();
                foreach (IReadOnlyMovie movie in this.Items)
                {
                    result.Add(movie);
                }

                return result.AsReadOnly();
            }
        }

        /// <summary>
        /// Busca una película en el catálogo que cumpla con un criterio
        /// específico.
        /// </summary>
        /// <param name="field">El nombre del atributo por el cual
        /// buscar.</param>
        /// <param name="value">El valor del atributo por el cual
        /// buscar.</param>
        /// <returns>La película encontrada, en modo solo lectura, o null si no
        /// se encuentra ninguna.</returns>
        public IReadOnlyMovie FindMovie(string field, string value)
        {
            return this.Find(field, value);
        }
    }
}
