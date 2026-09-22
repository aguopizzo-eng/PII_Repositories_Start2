# Mi proceso de aprendizaje en este ejercicio

Este documento reconstruye cómo avanzaste durante la conversación, de dónde
vinieron tus dudas y qué ideas tenías mal armadas. Está escrito para que lo
uses como material de estudio, no como un juicio. Al final hay un método
concreto para trabajar sin depender de que yo escriba el código.

---

## 1. La foto general

**Qué era el ejercicio.** Analizar `CarsDatabase` y `MoviesCatalog`, encontrar
el problema y resolverlo. Terminaste resolviendo **dos** problemas distintos:

1. **Código duplicado.** Las dos clases eran casi idénticas. Solución:
   `Repository<T>` (clase genérica).
2. **Encapsulación rota.** `ReadOnlyCollection` protegía la lista, pero los
   elementos (`Car`, `Movie`) tenían `set` públicos, así que se podían
   modificar desde afuera. Solución: interfaces de solo lectura
   (`IReadOnlyCar`, `IReadOnlyMovie`) para lo que sale hacia afuera.

**El segundo lo identificaste vos solo**, y bien: dijiste que el problema era
que los elementos seguían siendo mutables aunque la colección fuera de solo
lectura. Eso es exactamente lo que tenías que ver.

**Resultado final.** Un `Repository<T>` con la lógica común;
`CarsDatabase` y `MoviesCatalog` que heredan y solo agregan la vista de solo
lectura (`Cars`/`Movies`) y la búsqueda de solo lectura (`FindCar`/`FindMovie`);
`Items` y `Find` protegidos; una interfaz `ISearchable` para que el
repositorio pueda llamar a `HasValue`.

---

## 2. Cómo fuiste avanzando (etapa por etapa)

### Etapa 1: Ejecutar el programa
- Escribiste `dotnet run --PII_Repositories_Start2`. Pusiste el nombre del
  proyecto como si fuera un flag. El comando era
  `dotnet run --project src/Program`.
- **Qué mostró:** todavía no tenías el mapa de cómo se organiza una solución
  de .NET (solución → proyectos → clases).

### Etapa 2: Primera interfaz (`IReadOnly<T>`)
- Escribiste `private List<T> list = ...` dentro de una interfaz.
- **Qué mostró:** en ese momento tratabas la interfaz como si fuera una clase.
  Una interfaz no guarda datos: solo declara qué métodos y propiedades hay que
  tener.

### Etapa 3: Encontrar el problema de la consigna
- Te señalé el código duplicado. Ahí apareció tu primera gran duda: *"una
  interfaz no me ahorra código, entonces tengo que escribir todo en las dos
  clases?"*.
- Tu intuición era **correcta**: una interfaz sola no ahorra código. Lo que
  ahorra código es una **clase genérica**. La confusión estaba en mezclar los
  tres conceptos (interfaz, clase, genérico).
- Después declaraste `Repository<T>` como `interface`, con `Add`, `Remove` y
  `Find` sin código. Eso te llevó al mismo problema de nuevo.
- Se resolvió cuando la convertiste en `class` y le pusiste el código adentro.

### Etapa 3b: El problema de `Find` y `HasValue`
- Al mover `Find` a la clase genérica, `item.HasValue(...)` no compilaba.
  Tu pregunta fue muy buena: *"el método Find no usa un método que tienen
  Car y Movie, pero al ponerlo en la clase genérica ese método no existe"*.
- La causa: dentro de `Repository<T>` el compilador no sabe qué es `T`. Hay que
  decirle que `T` tiene `HasValue` con `where T : ISearchable`.
- Te ofrecí una alternativa con `Predicate<T>`; el profe dijo que **no** se usa
  en el curso. Ahí me equivoqué yo al recomendarla sin saber qué habían visto.
- Notaste algo importante: *"eso no es genérico, tengo dos interfaces
  distintas"*. Tenías razón: con `IReadOnlyCar` e `IReadOnlyMovie` no se puede
  usar una sola restricción. Hacía falta una interfaz común (`ISearchable`).

### Etapa 4: Exponer solo lectura (`Cars`, `Movies`)
- Preguntaste cómo hacer `CarsDatabase : Repository<IReadOnlyCar>`. No se
  puede: el JSON necesita **crear** objetos, y una interfaz no se puede crear.
- Tu primera versión de `Cars` devolvía `this.Cars` dentro de la propia
  propiedad `Cars` (recursión infinita).
- Aparecieron varias dudas seguidas: *¿de dónde sale `this.Items` si no está
  en `CarsDatabase`?*, *¿por qué el cast?*, *¿por qué repito código en dos
  clases si esto ahorra código?* (ver sección 3, raíces B y E).

### Etapa 5: `Find` también filtra objetos mutables
- Te marqué que `Find` devolvía un `Car`, y por lo tanto se podía modificar.
  Vos tomaste ese punto y propusiste la solución: `Find` protegido en
  `Repository` y, en cada subclase, un método público que devuelva la versión
  de solo lectura (`FindCar`/`FindMovie`).
- Volvió el mismo error de recursión: un método `Find` en `CarsDatabase`
  llamaba a `this.Find` (a sí mismo). Se resolvió renombrándolo a `FindCar`.

### Etapa 6: Tipo de la variable vs. tipo del objeto
- Preguntaste varias veces, con distintas palabras, lo mismo: *si `Car`
  implementa `IReadOnlyCar`, ¿por qué `Program` tiene que usar `IReadOnlyCar`?,
  ¿para qué la implementa entonces?, ¿los tipos tienen la misma referencia?*.
  Ver raíz A: es el núcleo de todo el ejercicio.

### Etapa 7: Tests
- Ajustaste los tests (`IReadOnlyMovie found = ...`).
- Preguntaste cómo probar `Find` e `Items` si son protegidos, y cómo probar la
  encapsulación. Surgieron `File.Delete`, `[SetUp]` que faltaba y qué probar
  en `Repository<T>` (ver raíz D).

### Etapa 8: Comentarios XML y cierre
- Pediste solo comentarios XML y una lista de lo que faltaba. Yo hice cambios
  de más en dos ocasiones, y eso te generó el enojo comprensible de esos
  mensajes. Lo tomé y volví atrás. Lo que quedó: comentarios XML, encabezados,
  `[SetUp]`, dos tests de `Cars` y `Movies`.

---

## 3. La raíz de tus dudas

Casi todas tus preguntas salen de **cinco confusiones** de fondo. Si las
entendés, la mayoría de las dudas desaparecen solas.

### Raíz A: confundir *el objeto* con *el tipo con el que lo mirás*

Un objeto existe **una sola vez** en memoria. Cada variable que apunta a él
tiene un **tipo declarado**, y ese tipo decide qué te deja hacer el compilador.

```csharp
Car auto = new Car("Jimny", "Suzuki", 2024);
Car a = auto;              // lo mirás como Car: hay set
IReadOnlyCar b = auto;     // MISMO objeto, mirado como IReadOnlyCar: no hay set
```

Dudas tuyas que salen de acá:
- *"Car implementa IReadOnlyCar para quedar de solo lectura"*: implementar la
  interfaz **no** hace que el objeto sea de solo lectura; solo permite
  **mirarlo** como tal.
- *"¿Por qué el `foreach` con `Car car` no protege?"*: porque al escribir `Car`
  vos elegís volver a mirarlo con el tipo completo.
- *"Cars es subtipo de IReadOnlyCar"* y *"la lista guarda autos que implementan
  IReadOnlyCar"*: son verdades, pero la lista **desde afuera** se ve por el tipo
  con el que se devuelve, no por lo que hay adentro.
- *Test con `Movie found = FindMovie(...)`*: no compila porque `FindMovie`
  devuelve `IReadOnlyMovie`, y el compilador no baja al tipo completo solo.
- *"¿Los tipos tienen la misma referencia que el objeto?"*: la referencia
  apunta al objeto; el tipo es lo que te dejan ver a través de ella.

**Lo que ya entendiste bien:** "depende de la colección donde pongas el objeto"
y "el tipo es una característica que extraés del objeto para usarla". La
idea es correcta, solo que el tipo no se "extrae": es una restricción sobre lo
que podés usar.

### Raíz B: qué es `this` y cómo funciona la herencia

`this` significa **"este objeto"**, no "lo que está escrito en esta clase". Un
objeto `CarsDatabase` incluye, por herencia, todo lo de `Repository<Car>`.

Dudas tuyas que salen de acá:
- *"¿Dónde aparece `Items` en `CarsDatabase`?"* y *"`this.Items` no tiene sentido,
  acá no hay un Items"*: `Items` está en `Repository<T>`, y `CarsDatabase` lo
  recibe al heredar.
- *"¿Repository llama a CarsDatabase?"*: al revés. `CarsDatabase` llama a
  `Repository`. La base no conoce a sus hijas.
- *"Sacar la lógica de Repository y dejarla en CarsDatabase"*: eso vuelve a
  duplicar todo; la herencia existe justamente para poner la lógica una vez.

**Regla útil:** cuando un nombre "no aparece", buscalo hacia arriba en la cadena
de herencia (`: Repository<Car>`) antes de pensar que hay algo mal.

### Raíz C: interfaz vs. clase vs. genérico

| | ¿Qué es? | ¿Tiene código? | ¿Para qué sirve? |
|---|---|---|---|
| Interfaz | Un contrato | No | Decir "esto sabe hacer X" |
| Clase | Un molde con datos y código | Sí | Implementar y reutilizar |
| Genérico (`<T>`) | Una clase con un tipo a completar | Sí | Escribir el código **una vez** para muchos tipos |
| `where T : I` | Una restricción sobre `T` | — | Garantizarle al compilador que `T` cumple el contrato `I` |

Dudas tuyas que salen de acá:
- *"una puta interfaz no ahorra código"*: correcto. Ahorra la **clase genérica**.
- *Declarar `Repository<T>` como `interface`*: falta de esta distinción.
- *"Tengo dos interfaces distintas, eso no es genérico"*: correcto, y por eso
  hace falta una tercera interfaz común (`ISearchable`).

### Raíz D: qué se puede probar con un test y qué no

Un test **ejecuta código** y mira resultados. La visibilidad (`protected`,
`private`) y los tipos son reglas del **compilador**, no de ejecución.

Dudas tuyas que salen de acá:
- *"¿Cómo pruebo `Find` si es protegido?"*: no se prueba directamente; se prueba
  a través de lo público (`FindCar`).
- *"¿Cómo pruebo la encapsulación?"*: un test común no puede comprobar que algo
  **no compila**. La encapsulación la garantiza el compilador; solo con reflexión
  se puede vigilar desde un test.
- *"`File.Delete`, ¿de qué me hablás?"*: un test que escribe archivos tiene que
  limpiar después.

**Cuidado con esto.** Yo te dije cosas contradictorias sobre este punto (ver
sección 6). La versión correcta es la de este párrafo.

### Raíz E: buscar "la solución perfecta" en vez de decidir con criterio

Tenías reacciones fuertes cuando encontraste que:
- Igual se repetían `Cars`/`Movies` y `FindCar`/`FindMovie` en las dos clases.
- Igual quedaba la posibilidad de un cast a `Car`.
- Igual `Find` podía filtrar un `Car`.

En diseño casi siempre hay **compromisos**. Repetir 10 líneas por clase para
tener encapsulación es un precio razonable; lo importante es poder explicar el
compromiso. Es una habilidad más valiosa que encontrar un diseño sin ninguna
repetición.

También aparece acá otro hábito: no saber cuánto pide la consigna. Antes de
seguir diseñando, conviene preguntar qué se espera (por ejemplo, si había que
cerrar `Find` o alcanzaba con la colección).

---

## 4. En qué te estás equivocando (creencia → realidad)

| Lo que pensabas | La realidad |
|---|---|
| "Una interfaz me ahorra código" | No. Es un contrato. La clase genérica es la que ahorra código. |
| "Si `Car` implementa `IReadOnlyCar`, queda de solo lectura" | No. Implementar solo permite **verlo** como solo lectura. Quien tenga la variable de tipo `Car` lo puede modificar. |
| "`this.X` solo funciona si `X` está escrito en esta clase" | `this` es el objeto entero, con todo lo heredado. |
| "`Repository` llama a `CarsDatabase`" | Al revés: la subclase usa lo de la base. |
| "Pongo `protected` en `Find` y listo" | Nadie de afuera podría buscar. Hay que dar un método público de solo lectura (`FindCar`). |
| "`ReadOnlyCollection` ya protege todo" | Protege la **lista** (agregar/quitar), no los **elementos**. |
| "Los tests de `FindCar` prueban la encapsulación" | Prueban que **busca bien**. El tipo de retorno no lo verifica un test común. |
| "Quiero probar `Find` protegido desde el test" | No se puede llamar; se prueba por `FindCar`. |
| "`Movie found = FindMovie(...)` debería andar" | No: `FindMovie` devuelve `IReadOnlyMovie`. |
| "Una `List<Car>` es una `List<IReadOnlyCar>`" | No. Aunque `Car` sea subtipo, las listas no heredan esa relación; hay que copiar los elementos. |
| "Si repito código, el diseño está mal" | A veces es el costo de la encapsulación. Lo importante es saber por qué. |

---

## 5. Lo que hiciste bien

- Identificaste **por tu cuenta** el problema de mutabilidad de los elementos.
- Trajiste el mensaje del profe (sin predicados) y con eso corregiste el enfoque
  que yo te había propuesto.
- Notaste que dos interfaces distintas no alcanzan para una restricción
  genérica.
- Notaste que una interfaz no evitaba la duplicación, que era tu duda de fondo.
- Propusiste `FindCar` y `FindMovie` como forma de cerrar el hueco de `Find`.
- Preguntaste "por qué" en lugar de aceptar el código: tus dudas apuntaron
  casi siempre al concepto y no al detalle.

---

## 6. Dónde fallé yo (para que sepas qué desconfiar)

- **Te expliqué en el orden equivocado.** Usé `this.Items` sin decirte antes que
  viene de la herencia. Eso te hizo perder tiempo.
- **Te recomendé `Predicate<T>`** sin saber que el curso no lo usaba.
- **Te dije cosas contradictorias sobre los tests de encapsulación.** Primero
  reflexión, después "no se prueba", después "queda cubierta por `FindCar`", y
  finalmente que no. La versión final es la de la raíz D.
- **Te di código antes de tiempo** cuando me pedías que te guiara. Me pediste
  varias veces "no lo resuelvas".
- **Hice cambios que no pedías** (tests, refactors de indentación, orden de
  métodos) cuando me habías pedido solo comentarios XML.
- **Te respondí con más opciones de las necesarias** cuando querías una
  respuesta simple.

**Lo práctico:** cuando una explicación mía no cierre con lo que ves en el
compilador, **el compilador tiene la razón**. Probá, mirá el error y pedime que
te explique **ese error**.

---

## 7. Cómo trabajar para no depender de copiar

Este es el método que te propongo. Tiene pasos cortos.

1. **Antes de preguntar, escribí tu hipótesis.** Una línea: "creo que el error
   es X porque Y". Aunque esté mal, te obliga a pensar.
2. **Pedime pistas, no código.** Frase concreta: *"Dame una pista, no el
   código."* Si después necesitás el código, pedilo explícitamente.
3. **Compilá seguido.** Después de cada cambio chico, `dotnet build`. Un error
   de compilación es la mejor explicación de qué no entendés.
4. **Leé el mensaje de error completo** y tratá de decir con tus palabras qué
   pide. Recién después preguntá.
5. **Escribí el código vos y pedime que lo revise.** En vez de "escribime
   `FindCar`", "escribí yo `FindCar`; ¿qué está mal?".
6. **Explicá de vuelta.** Después de cada cosa que aprendas, explicala en
   voz alta o en un párrafo, como si se lo contaras a un compañero. Si
   no podés, no la entendiste.
7. **Antes de tocar código, dibujá.** Las tarjetas CRC y el UML de
   `docs/CRC_y_UML.md` sirven para eso: si sabés quién colabora con quién,
   el código sale más solo.
8. **Llevá un registro de dudas.** Anotá cada duda con una etiqueta (por
   ejemplo, "tipo vs objeto", "this", "test"). Cuando una etiqueta se
   repite, encontraste una raíz que te falta estudiar.
9. **Definí qué se espera antes de diseñar.** Preguntale al profe cuánto se pide
   (por ejemplo, si hay que cerrar `Find`).
10. **Pedí que no toque nada** cuando solo querés entender. Decilo explícito:
    *"No modifiques archivos."*

### Mi plan (completalo vos)

Elegí las dos raíces (A a E) que más te cuestan y escribí qué vas a hacer:

Raíz 1: ______________

Qué voy a hacer para practicarla:

______________________________________________________________________

______________________________________________________________________

Raíz 2: ______________

Qué voy a hacer para practicarla:

______________________________________________________________________

______________________________________________________________________

---

## 8. Ejercicio de autoevaluación

Respondé **sin mirar el código ni este documento**. Después comprobá.

1. Escribí un ejemplo (3 líneas) donde el mismo objeto se vea como `Car` y como
   `IReadOnlyCar`, y decí cuál permite modificarlo.

   ______________________________________________________________________

   ______________________________________________________________________

2. ¿Por qué `CarsDatabase` puede escribir `this.Items` sin haberlo declarado?

   ______________________________________________________________________

   ______________________________________________________________________

3. ¿Qué garantiza `where T : ISearchable` y qué pasa si lo sacás?

   ______________________________________________________________________

   ______________________________________________________________________

4. ¿Qué diferencia hay entre proteger la lista y proteger los elementos?

   ______________________________________________________________________

   ______________________________________________________________________

5. ¿Por qué un test no puede comprobar que `Items` es protegido, salvo con
   reflexión?

   ______________________________________________________________________

   ______________________________________________________________________

6. Si mañana agregás la clase `Book`, ¿qué archivos tendrías que crear y cuáles
   no tendrías que tocar?

   ______________________________________________________________________

   ______________________________________________________________________

   ______________________________________________________________________

7. Con tus palabras, en tres frases: ¿cuál fue el problema de la consigna y
   cómo lo resolviste?

   ______________________________________________________________________

   ______________________________________________________________________

   ______________________________________________________________________
