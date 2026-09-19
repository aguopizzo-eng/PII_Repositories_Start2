//------------------------------------------------------------------------------
// <copyright file="Movie.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.Repositories
{
    /// <summary>
    /// Esta clase representa una película.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Movie"/>.
        /// </summary>
        /// <param name="name">El nombre de la película.</param>
        /// <param name="year">El año de la película.</param>
        public Movie(string name, int year)
        {
            this.Name = name;
            this.Year = year;
        }

        /// <summary>
        /// El nombre de la película.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// El año de la película.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Determina si la película tiene un valor específico para un atributo
        /// dado.
        /// </summary>
        /// <param name="field">El nombre del atributo.</param>
        /// <param name="value">El valor del atributo.</param>
        /// <returns>Retorna <c>true</c> si el objeto tiene ese valor y
        /// <c>false</c> en caso contrario.</returns>
        public bool HasValue(string field, string value)
        {
            if (field == null || value == null)
            {
                return false;
            }
            else if ((field == "Name" && this.Name == value)
                || (field == "Year" && this.Year.ToString() == value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
