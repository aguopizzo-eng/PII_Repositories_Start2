//------------------------------------------------------------------------------
// <copyright file="IReadOnlyMovie.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.Repositories
{
  /// <summary>
  /// Esta interfaz representa una película de la que solo se pueden leer los
  /// datos.
  /// </summary>
  public interface IReadOnlyMovie : ISearchable
  {
    /// <summary>
    /// Obtiene el nombre de la película.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Obtiene el año de la película.
    /// </summary>
    int Year { get; }
  }
}
