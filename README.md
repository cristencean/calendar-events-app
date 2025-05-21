# C# .NET Core - Web API - SQLite - FastEndpoints - Angular Calendar App Project

Calendar app built with C# Web Api and Angular on the front end, that manages CRUD operations for events in a nice calendar UI.

* Uses C# .NET Core and FastEndpoints to create the APIs
* SQLite to set the DB
* Moq and xUnit to test the C# functionality
* Angular SPA to add/update/remove events in a calendar

## About 

This project is a full-stack Calendar app built with:

- **Backend:** .NET Core Web API (.NET 8), FastEndpoints  
- **Database:** SQLite
- **Frontend:** Angular with TypeScript and Material UI  
- **Testing:** xUnit and Moq for .NET, Karma and Jasmine for frontend

## Running locally in development mode

To get started, just clone the repository and run `dotnet restore & dotnet build & dotnet run`:

    git clone https://github.com/cristencean/calendar-events-app.git
	cd CalendarApp
	dotnet restore
	dotnet build
	cd CalendarApp.Api
	dotnet ef database update
	dotnet run
	swagger UI: https://localhost:7240/swagger/index.html
    testing example: http://localhost:5291/calendar-events
    
## Run the unit testing

To run the unit testing just run `dotnet test`:

    dotnet test

## Project structure

	CalendarApp/
	│
	├── CalendarApp.Api/            
	│   └── Endpoints/              # API endpoints
	│
	├── CalendarApp.Application/    # Services and business logic
	│   ├── Services/               # Business logic
	│   └── Validators/             # Custom validation rules
	│
	├── CalendarApp.Core/           # Interfaces and Models
	│
	├── CalendarApp.DataAccess/     # EF Core DbContext and Repository
	│
	├── CalendarApp.Tests/          # xUnit tests with Moq
	│
	├── CalendarApp.FE/             # Angular frontend (with MUI)
	│
	└── README.md