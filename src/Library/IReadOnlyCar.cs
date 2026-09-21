//------------------------------------------------------------------------------
// <copyright file="IReadOnlyCar.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.Repositories
{
  /// <summary>
  /// Esta interfaz representa un auto del que solo se pueden leer los datos.
  /// </summary>
  public interface IReadOnlyCar : ISearchable
  {
    /// <summary>
    /// Obtiene el modelo del auto.
    /// </summary>
    string Model { get; }

    /// <summary>
    /// Obtiene el fabricante del auto.
    /// </summary>
    string Maker { get; }

    /// <summary>
    /// Obtiene el año del auto.
    /// </summary>
    int Year { get; }
  }
}
