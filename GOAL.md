Our main objective is to implement this technical test described by this email:

¡Hola!

¡Felicidades! Has avanzado a la siguiente etapa de nuestro proceso de selección para el puesto de Programador web.
Prueba Técnica

Nos interesa evaluar tu capacidad para implementar interfaces basadas en un diseño, estructurar una solución con buenas prácticas y entregar un resultado funcional. Para esta prueba, deberás desarrollar una aplicación web funcional que replique lo más fielmente posible las pantallas y flujos mostrados en el diseño de Figma.

Enlace a Figma: https://www.figma.com/design/QeuyC4joVTjoRVrKU9OiRh/Prueba-ASP.Net--OFICIAL-?node-id=7174-99647&t=rcoHFOWbos72oD9Z-1
Tecnologías requeridas:

      Framework: ASP.NET Core

      Front-end: Bootstrap

      JavaScript o TypeScript: Implementar al menos una interacción o funcionalidad del lado del cliente

      Base de datos: SQL Server

      Lógica: Validación de usuario, control de errores, flujo de mensajes

      Despliegue: Todo puede ser trabajado y ejecutado de forma local (no es necesario subirlo a internet)

Instrucciones:

      El desarrollo debe replicar lo más fielmente posible el diseño proporcionado en Figma. En caso de que alguna fuente, ícono o recurso gráfico no esté disponible, puedes utilizar una alternativa similar, procurando mantener la apariencia, estructura y distribución visual del diseño original.

      Crea una base de datos simple con una tabla de usuarios para validar el inicio de sesión.

      Al final, deberás mostrar el resultado grabando tu pantalla, explica tu enfoque, lógica y decisiones técnicas.

      Puedes usar cualquier herramienta de grabación que prefieras.

      Explicación breve del desarrollo: Incluye una descripción corta del trabajo realizado (¿Qué hiciste, cómo lo abordaste y qué herramientas utilizaste?)

Entrega:

      Sube todo tu entrega en el siguinte formulario: https://forms.gle/GrpUmBACMv1aSvzT6

Fecha y Hora de Entrega:
Completa el formulario con tu entrega antes del miércoles 8 a las 8:00 pm hora Venezuela.

Quedamos atentos a tu evaluación.

¡Éxitos en esta etapa!

El equipo de AHVA Global

I already created the solution skeleton, and you have it already available. Respect it as ure is based on some sort of personal Onion Architecture.
I need to add an PropertyGroup to every .csproj with two values "AssemblyName" and "RootNa Ahva.Ceplan.[ProjectName] (where ProjectName might be WebApi, WebApi.Client, WebApi.Tests and so for). You might need to adjust the default namespace of the project at initial code files. This is to have a cohesive solution with standard namespacing. Format well the xml code in the csproj.  
 Also I need to move out the IdentityDbContext from the Portal project to the Data project. As that's the place where it will reside on.  
 I need to configure a docker container of SQL Server in the aspire app host. Also, in this computer you have podman available, not docker so you might need to tweak that a little bit, but just for this computer.

Shared.ApiResponses project is for holding standard API responses wrappers like SingleResponse, LimtedResponse, ErrorResponse and so on. Basically it adds a standard response JSONwrapper with an ok property (boolean, generally true for most responses except errors), the data property (generic property, either a value, object or array), other metadata properties related to list, paging
and limited queries, and finally error metadata (in an errors array).

Contracts project is for holding DTO models between the WebApi, Domain services, and cliennly would be referenced in almost every project that might need this models. DTO willgenerally be classied as Input models (used to represent user payload), Output models (used for server results), Filter models (query params models) and any other kind you might need. Naming convention would
be UserInput, UserOutput, UserFilter.

Data project is for holding the EF Core dbcontext and direct entity models. Used primarily is the responsible to hold migrations in a standard way. The additional user propertiesyou might find, should be defined in a new table separated from IdentityDbContext AspNetUser entity, but still gracefully related.

Domains project is for logic, classes in the sort of services with the final logic to direct use in APIs. It consumes the contracts defined. For a simple CRUD service, there would be a class like UserCrudService with four methods: CreateOne (accepting an Input model and returns output as parameter, and the input model, and returns output model), GetOne (with the ID asparameter and returns output model), and DeleteOne (with the ID as parameter and returns output model). For an advanced listing logic, there would be a class like UserListingService with a few public methods
like for querying by paging, counting, and stuff, also have a private method for filteringto parse Filter model into a EF Core Query. Usually the mapping logic between entitymodels and DTO models would be defined in static class extensions in the Mappers namespace, classified by main entity. Just write mapping logic by hand, don't use filthy libraries. You might need to create
very specific services for some cases, like hard logic, validation and stuff, they don't nyou might try to keep them cohesive. You don't need to implement an interface for everyservice class, unless polymorphism is actually justified for a good reason. Add a ServiceCollectionExtensions static class that injects these services in a method AddCeplanDomainServices.

WebApi project is the API representation of the distributed app. Consumes Domains and exposes that logic into a RESTful web service. Use minimal APIs, classify into static classes with extensions where you
think needs separation and map all endpoints in the program files. Wrap responses accordindefined in the Shared.ApiResponses. The API should have an exception middleware thatgracefully returns exceptions into a well formatted ErrorResponse. This API should hold the Identity logic, portal should depend on the API. Add options for Bearer tokens through OAuth 2.0. Activate swagger
and organize endpoints good enough.

WebApi.Client project is a Refit client implementation, easy to use in other .NET projects

Portal is the Razor Pages web UI, final point to use the app. Only use bootstrap and jquerlibrary aside. As request from the technical test, you need to implement some logic inJavaScript.  
 Don't use very specific libraries I haven't explicitly allowed you to use, unless it is something official from Microsoft and considered to be an standard practice. You can ask me if you might need some, I'll check it out and confirm you later. Never used MediatR or Automapper or anything that you C# code. Don't use Async suffix for async methods, I'm ok with working with async methods by default always.

AppHost and ServiceDefaults are for the Aspire distributed application.

Domains.Tests is for unit tests for logic, WebApi.Tests is for integration tests, and Portal.Tests is for End2End tests. You might not need to implement many tests for now, just a couple for demonstration. I'd be more interested in end-to-end tests that showcases the features we tryna make it work.

Please, let me know if you need me to pass you screenshots of the screens available in theng as I don't want you to do ahead work that might not be useful.
