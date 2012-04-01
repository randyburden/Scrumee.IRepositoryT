Scrumee.IRepositoryT
--------------------

###About###

Scrumee is a very simple SCRUM-inspired project management solution. This project is by no means meant to be used as a real solution but meant to demonstrate .NET software components and frameworks working together for educational purposes only.

###Technologies###

Scrumee uses ASP.NET MVC 3 with a SQLite database backend bridged together using NHibernate.

Frameworks and libraries used:
  
  - ASP.NET MVC 3 with Razor Views
  - NHibernate v3.2 ( ORM )
  - StructureMap v2.6.1 ( Dependency Injection)
  - System.Data.SQLite v1.0.66.0 ( ADO.NET adapter for SQLite )

###Implementation###

This implementation of Scrumee uses an IRepository< T > pattern and dependency injection for the separation of concerns benefits.

The NHibernate session management is handled via StructureMap where the SessionFactory is registered as a Singleton for the life of the application and any requests for a new ISession are stored within the current HttpContext thereby making the NHibernate session reusable multiple times if need be during a single request.

A simple yet functional IRepository< T > implementation is used allowing the application to query the database directly from the Controller yet still adhearing to the separation of concern principle because all of the NHibernate-specific implementation is abstracted away allowing the application to query the database using LINQ. *Note: No Unit-of-Work wrapper has been created around the usage of the IRepository as most simple web applications probably wouldn't need one. In a more robust and enterprise environment where transaction scopes can be much larger, a Unit-of-Work implementation would be highly recommended.*

This implementation of NHibernate also utilizes the standard .HBM XML mapping files and the Loquacious ( NHibernate v3.0+ ) configuration API.

###Note###

This project is one in a series of projects utilizing Scrumee as the base application to demonstrate different software libraries and frameworks. All of the projects can be found here on Github.com: [https://github.com/randyburden](https://github.com/randyburden)
