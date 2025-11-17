# .NET 8 Microservices E?Commerce 

> Soluci車n de comercio electr車nico basada en **microservicios** construida con **.NET 8**, **ASP.NET Core MVC**, **Entity Framework Core**, **.NET Identity**, **Ocelot API Gateway** y **Azure Service Bus**.

---

## Descripci車n general

Este repositorio contiene una **aplicaci車n de e?commerce basada en microservicios**, dise?ada para practicar y demostrar patrones modernos de arquitectura con **.NET 8**.

La soluci車n est芍 compuesta por varios microservicios desplegables de forma independiente, una **aplicaci車n cliente MVC**, un **API Gateway** y mensajer赤a **asincr車nica con Azure Service Bus**.

Los objetivos principales del proyecto son:

- Aplicar una **arquitectura de microservicios limpia**, con l赤mites bien definidos.
- Implementar **autenticaci車n y autorizaci車n** con **.NET Identity** y **control de acceso basado en roles**.
- Demostrar **comunicaci車n s赤ncrona (HTTP)** y **asincr車nica (eventos/mensajes)** entre servicios.
- Utilizar **Azure Service Bus (Queues & Topics)** para flujos desacoplados y dirigidos por eventos.
- Implementar **arquitectura en capas (N?Layer)** con **Repository Pattern** y **Entity Framework Core**.
- Exponer APIs REST usando **Swagger / OpenAPI**.
- Construir un front?end **ASP.NET Core MVC** con **Bootstrap 5** como capa de UI.

---

## Diagrama de infraestructura
![Infraestructura](./infra.png)

---

## Microservicios

### 1. Identity Microservice (`MS.Identity`)
- Maneja **registro**, **inicio de sesi車n** y **gesti車n de usuarios**.
- Implementa **.NET Identity** para autenticaci車n y autorizaci車n.
- Emite **tokens JWT** consumidos por la aplicaci車n MVC y otros microservicios a trav谷s del gateway.
- Soporta **autorizaci車n basada en roles** (por ejemplo, `Admin`, `Customer`).

### 2. Product Microservice (`MS.Product`)
- Administra el **cat芍logo de productos**.
- Operaciones CRUD (crear, actualizar, listar, eliminar).
- Usa **Entity Framework Core** con **SQL Server**.
- Expone APIs REST consumidas por el carrito de compras y la aplicaci車n MVC.

### 3. Coupon Microservice (`MS.Coupon`)
- Gestiona **cupones / c車digos de descuento**.
- Valida cupones y calcula descuentos.
- Utiliza una base de datos independiente en **SQL Server**.
- Es consumido por el microservicio de **Shopping Cart** para aplicar descuentos.

### 4. Shopping Cart Microservice (`MS.ShoppingCart`)
- Gestiona el **carrito de compras** de cada usuario.
- Se integra con:
  - **Product Service** para obtener detalles y precios de productos.
  - **Coupon Service** para validar y aplicar cupones.
- Persiste informaci車n usando **EF Core + SQL Server**.
- Publica **eventos de pedido** en **Azure Service Bus** cuando se realiza el checkout.

### 5. Order Microservice (`MS.Order`)
- Se encarga de la **creaci車n y gesti車n de pedidos**.
- Recibe peticiones desde el **Shopping Cart** (v赤a Gateway) o escucha eventos.
- Almacena pedidos en su propia base de datos **SQL Server**.
- Publica eventos en **Azure Service Bus** para que **Email** y **Rewards** procesen acciones de forma asincr車nica.

### 6. Email Microservice (`MS.Email`)
- Suscrito a mensajes de **Azure Service Bus** (por ejemplo, eventos de ※Order Created§).
- Env赤a **correos de confirmaci車n de pedido** a los clientes.
- Totalmente desacoplado del microservicio de pedidos.

### 7. Rewards Microservice (`MS.Rewards`)
- Escucha eventos de pedidos en Azure Service Bus.
- Calcula y aplica **puntos de recompensa / fidelidad** al usuario.
- Mantiene su propia base de datos **SQL Server**.

---

## API Gateway y aplicaci車n cliente

### Ocelot API Gateway
- Punto de entrada central para todas las **peticiones del cliente**.
- Encargado de enrutar las solicitudes HTTP al microservicio correspondiente.
- Se ocupa de:
  - Routing
  - Agregaci車n de respuestas
  - Encadenar y reenviar tokens de autenticaci車n
- Simplifica el cliente MVC al ocultar la complejidad de los microservicios tras una sola URL.

### ASP.NET Core MVC Application
- Construida con **ASP.NET Core MVC (.NET 8)** y **Bootstrap 5**.
- Implementa la UI de e?commerce (cat芍logo, carrito, checkout, login, etc.).
- Se comunica **迆nicamente** con el **Ocelot Gateway**.
- Utiliza **Cookies + JWT** e **Identity** para escenarios autenticados.

---

## Seguridad

- **.NET Identity** para gesti車n de usuarios y roles.
- **Autenticaci車n basada en JWT** para llamadas seguras a las APIs.
- **Autorizaci車n basada en roles** a nivel de controlador y acci車n.
- Operaciones sensibles (gesti車n de productos, listado de todos los pedidos, etc.) restringidas a usuarios administradores.

---

## Patrones de comunicaci車n

### Comunicaci車n s赤ncrona
- Basada en **HTTP/REST** a trav谷s del **Ocelot Gateway** para:
  - Consultas de productos
  - Operaciones del carrito
  - Validaci車n de cupones
  - Consultas de pedidos

### Comunicaci車n asincr車nica
- Basada en **Azure Service Bus (Queues & Topics)** para:
  - Eventos de creaci車n de pedidos
  - Notificaciones por correo
  - Procesamiento de puntos/recompensas

Beneficios:
- Microservicios desacoplados.
- Mensajer赤a confiable con reintentos y dead?letter.
- Mayor resiliencia y escalabilidad.

---

## Datos y persistencia

Cada microservicio es due?o de su **propia base de datos**, siguiendo el patr車n **Database?per?Microservice**:

- **SQL Server** para todos los microservicios.
- **Entity Framework Core** como ORM.
- **Migrations** para evolucionar el esquema de cada servicio de forma independiente.
- No se comparten bases de datos entre microservicios, evitando acoplamientos innecesarios.

---

## Tecnolog赤as y herramientas

- **Backend**
  - .NET 8
  - ASP.NET Core MVC
  - ASP.NET Core Web API
  - .NET Identity
  - Entity Framework Core
  - Ocelot API Gateway
  - Swagger / OpenAPI

- **Mensajer赤a**
  - Azure Service Bus (Queues & Topics)

- **Base de datos**
  - SQL Server
  - EF Core Migrations

- **Frontend**
  - ASP.NET Core MVC Views
  - Bootstrap 5

- **Arquitectura y patrones**
  - Arquitectura de microservicios
  - Database?per?Microservice
  - Arquitectura en capas (N?Layer)
  - Repository Pattern
  - Comunicaci車n dirigida por eventos

---

## Roadmap / Posibles mejoras

- Implementar **logging centralizado** (Serilog + ELK / Seq).
- A?adir **versionado de APIs** y **rate limiting**.
- Incorporar **OpenTelemetry** para trazabilidad distribuida.
- Unit Test.

---

## Sobre el autor

Este proyecto forma parte de un proceso pr芍ctico para dominar **microservicios con .NET 8**, poniendo 谷nfasis en:

- Patrones reales de microservicios en producci車n.
- Integraciones listas para la nube con Azure.
- Buenas pr芍cticas de arquitectura orientadas a entornos empresariales.

---

## Licencia

 MIT

