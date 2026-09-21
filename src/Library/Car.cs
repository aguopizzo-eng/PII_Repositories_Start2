//------------------------------------------------------------------------------
// <copyright file="Car.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.Repositories
{
    /// <summary>
    /// Esta clase representa un auto.
    /// </summary>
    public class Car : IReadOnlyCar
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Car"/>.
        /// </summary>
        /// <param name="model">El modelo del auto.</param>
        /// <param name="maker">El fabricante del auto.</param>
        /// <param name="year">El año del auto.</param>
        public Car(string model, string maker, int year)
        {
            this.Model = model;
            this.Maker = maker;
            this.Year = year;
        }

        /// <summary>
        /// Obtiene o establece el modelo del auto.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Obtiene o establece el fabricante del auto.
        /// </summary>
        public string Maker { get; set; }

        /// <summary>
        /// Obtiene o establece el año del auto.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Determina si el auto tiene un valor específico para un atributo dado.
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
            else if ((field == "Model" && this.Model == value)
                || (field == "Maker" && this.Maker == value)
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
