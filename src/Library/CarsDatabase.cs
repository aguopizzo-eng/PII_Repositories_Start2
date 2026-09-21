//------------------------------------------------------------------------------
// <copyright file="CarsDatabase.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ucu.Poo.Repositories
{
    /// <summary>
    /// Esta clase representa una base de datos de autos.
    /// </summary>
    public class CarsDatabase : Repository<Car>
    {
        /// <summary>
        /// Obtiene los autos de la base de datos en modo solo lectura.
        /// </summary>
        public ReadOnlyCollection<IReadOnlyCar> Cars
        {
            get
            {
                List<IReadOnlyCar> result = new List<IReadOnlyCar>();
                foreach (IReadOnlyCar car in this.Items)
                {
                    result.Add(car);
                }

                return result.AsReadOnly();
            }
        }

        /// <summary>
        /// Busca un auto en la base de datos que cumpla con un criterio
        /// específico.
        /// </summary>
        /// <param name="field">El nombre del atributo por el cual
        /// buscar.</param>
        /// <param name="value">El valor del atributo por el cual
        /// buscar.</param>
        /// <returns>El auto encontrado, en modo solo lectura, o null si no se
        /// encuentra ninguno.</returns>
        public IReadOnlyCar FindCar(string field, string value)
        {
            return this.Find(field, value);
        }
    }
}
