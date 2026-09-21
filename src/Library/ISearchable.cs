//------------------------------------------------------------------------------
// <copyright file="ISearchable.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.Repositories
{
  /// <summary>
  /// Esta interfaz representa un objeto que se puede buscar por el valor de
  /// alguno de sus atributos.
  /// </summary>
  public interface ISearchable
  {
    /// <summary>
    /// Determina si el objeto tiene un valor específico para un atributo
    /// dado.
    /// </summary>
    /// <param name="field">El nombre del atributo.</param>
    /// <param name="value">El valor del atributo.</param>
    /// <returns>Retorna <c>true</c> si el objeto tiene ese valor y
    /// <c>false</c> en caso contrario.</returns>
    bool HasValue(string field, string value);
  }
}
