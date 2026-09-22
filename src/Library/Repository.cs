//------------------------------------------------------------------------------
// <copyright file="Repository.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace Ucu.Poo.Repositories
{
  /// <summary>
  /// Esta clase representa un repositorio genérico de elementos.
  /// </summary>
  /// <typeparam name="T">El tipo de los elementos almacenados.</typeparam>
  /// <typeparam name="TReadOnly">El tipo de solo lectura que se muestra hacia
  /// afuera para cada elemento.</typeparam>
  public class Repository<T, TReadOnly>
  where T : TReadOnly
  where TReadOnly : ISearchable
  {
    private List<T> lst = new List<T>();

    /// <summary>
    /// Obtiene los elementos del repositorio en modo solo lectura.
    /// </summary>
    public ReadOnlyCollection<TReadOnly> Items
    {
        get
        {
            List<TReadOnly> list = new List<TReadOnly>();
            foreach (TReadOnly element in this.lst)
            {
                list.Add(element);
            }

            return list.AsReadOnly();
        }
    }

    /// <summary>
    /// Agrega un elemento al repositorio.
    /// </summary>
    /// <param name="item">El elemento a agregar.</param>
    public void Add(T item)
    {
        if (item != null)
        {
            this.lst.Add(item);
        }
    }

    /// <summary>
    /// Elimina un elemento del repositorio.
    /// </summary>
    /// <param name="item">El elemento a remover.</param>
    public void Remove(T item)
    {
        this.lst.Remove(item);
    }

    /// <summary>
    /// Busca un elemento en el repositorio que cumpla con un criterio
    /// específico.
    /// </summary>
    /// <param name="field">El nombre del atributo por el cual
    /// buscar.</param>
    /// <param name="value">El valor del atributo por el cual
    /// buscar.</param>
    /// <returns>El elemento encontrado que cumple el criterio especificado o
    /// null si no se encuentra ninguno.</returns>
    public TReadOnly Find(string field, string value)
    {
        foreach (TReadOnly item in this.lst)
        {
            if (item.HasValue(field, value))
            {
                return item;
            }
        }

        return default(TReadOnly);
    }

    /// <summary>
    /// Convierte el repositorio a una representación en formato JSON.
    /// </summary>
    /// <returns>Una representación del repositorio en formato
    /// JSON.</returns>
    public string ConvertToJson()
    {
        return JsonSerializer.Serialize(this.lst);
    }

    /// <summary>
    /// Carga el repositorio desde una representación en formato JSON.
    /// </summary>
    /// <param name="content">La representación en formato JSON desde la
    /// cual cargar el repositorio.</param>
    public void LoadFromJson(string content)
    {
      List<T> items = JsonSerializer.Deserialize<List<T>>(content);
      if (items != null)
      {
          this.lst = items;
      }
      else
      {
          this.lst = new List<T>();
      }
    }

    /// <summary>
    /// Guarda el repositorio en un archivo en formato JSON.
    /// </summary>
    /// <param name="filePath">El nombre del archivo, incluyendo
    /// opcionalmente la ruta.</param>
    public void SaveToFile(string filePath)
    {
        string content = this.ConvertToJson();
        File.WriteAllText(filePath, content);
    }

    /// <summary>
    /// Carga el repositorio desde un archivo en formato JSON.
    /// </summary>
    /// <param name="filePath">El nombre del archivo, incluyendo
    /// opcionalmente la ruta.</param>
    /// <returns>Retorna <c>true</c> si se cargó el repositorio y
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
