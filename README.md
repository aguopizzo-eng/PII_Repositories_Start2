<!-- markdownlint-disable-next-line MD033 MD041 -->
<img alt="UCU" src="https://www.ucu.edu.uy/plantillas/images/logo_ucu.svg"
width="150"/>

# Universidad Católica del Uruguay

## Programación II

# Ejercicio de aplicación

## Consigna

> [!NOTE]
>
> Crea tu propio repositorio a partir de esta plantilla **en tu organización** y
> clona ese repositorio en tu equipo.

Analiza las clases [`CarsDatabase`](./src/Library/CarsDatabase.cs) y
[`MoviesCatalog`](./src/Library/MoviesCatalog.cs). ¿Qué problema encuentras?
¿Cómo podrías resolver ese problema?

Implementa tu solución y actualiza los casos de prueba.

## Uso de ![GitHub Copilot](https://img.shields.io/badge/GitHub%20Copilot-000?logo=githubcopilot&logoColor=fff)

Es posible usar GitHub Copilot en este repositorio. Consulta [cómo usar Copilot
para aprender](./COPILOT.md).


--- 
### Notas de Agustín

Antes de la solución, CarsDatabase y MoviesCatalog repetían el mismo código salvo por los tipos que se manipulan dentro de cada una de las clases. 

Además, existía el problema de que la propiedad Cars/Movies exponía la lista de autos/películas de solo lectura, pero los elementos que la lista contiene eran de tipo Car/Movie con propiedades de lectura y escritura, lo que permite modificar los atributos de cada instancia por separado "desde afuera".

Para arreglar este problema de encapsulación creé las interfaces IReadOnlyCar y IReadOnlyMovie que permiten que las clases Car y Movie que las implementan puedan ser vistas como de solo lectura. 

Además creé la interfaz ISearchable que declara que las clases que la implementen tienen un método HasValue para buscar un objeto según un criterio.

Finalmente creé la clase genérica `Repository<T>` con la restricción (where T : ISearchable) para implementar todas las propiedades y métodos que CarsDatabase y MoviesCatalog tienen en común, y para poder lograr que el método Find compile, ya que utiliza un método HasValue que el objeto genérico (Car y Movie) tiene dentro de sí. 

Dentro de `Repository<T>` la propiedad Items devuelve una lista de solo lectura con objetos tipo T (Car y Movie) que tienen sus propiedades de lectura y escritura. Para hacer efectiva la encapsulación, creé la propiedad Cars/Movies dentro de CarsDatabase y MoviesCatalog que toma la lista de solo lectura y crea otra lista de tipo IReadOnlyCar/IReadOnlyMovie que devuelve las instancias de solo lectura.

Hice lo mismo para el método Find, ya que el método dentro de `Repository<T>` me devuelve un elemento tipo T con propiedades de lectura y escritura, así que dentro de cada clase CarsDatabase y MoviesCatalog creé los métodos FindCar y FindMovie que llamen al método Find pero que devuelvan una instancia de tipo IReadOnlyCar y IReadOnlyMovie. 

Debido a esto, protegí la propiedad Items y el método Find de `Repository<T>` e hice públicas las respectivas propiedades y métodos de CarsDatabase y MoviesCatalog.

