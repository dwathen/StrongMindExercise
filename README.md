# StrongMind Exercise Pizza Manager

A full‑stack application built using ASP.NET Core 8 (Web API), Blazor, and MudBlazor for the UI. The application manages pizzas and toppings using a many-to-many relationship, following Domain‑Driven Design (DDD) and Onion Architecture principles. Data is persisted using SQLite, and unit tests are implemented using XUnit and Moq.

## Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Building and Running Locally](#building-and-running-locally)
- [Running Tests](#running-tests)

## Features

- **CRUD Operations:** Create, read, update, and delete pizzas and toppings.
- **Many-to-Many Relationship:** Pizzas can have multiple toppings and toppings can belong to multiple pizzas.
- **DTO-based API:** Decoupled API contracts using Data Transfer Objects.
- **Blazor UI:** A modern, responsive UI built with Blazor and styled using MudBlazor components.
- **Unit Testing:** Comprehensive unit tests using XUnit and Moq.

## Project Structure

The solution is organized following Domain‑Driven Design and the Onion Architecture

## Prerequisites

- **.NET 8 SDK:** [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQLite:** (No additional installation required—the app uses the SQLite engine via EF Core.)
- **Git:** (for cloning the repository)
- **Visual Studio 2022/Visual Studio Code:** (or your preferred IDE)
- **Optional:** [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) for managing migrations

## Getting Started

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/dwathen/StrongMindExercise.git
   cd PizzaAppSolution

2. **Restore Dependencies**
   Run the following command from the solution directory:

   ```bash
   dotnet restore

## Building and Running Locally
Run the following command from the StrongMindExercise.API folder:

    dotnet run

The API will be available at https://localhost:7077 or http://localhost:5006

The project defaults to the production url. The appsettings.json file will need to be modified for the StrongmindExercise.WebUI project.
Navigate to that folder and change the APIUrl from "https://strongmindexerciseapi20250205121115.azurewebsites.net/api/" to point towards the port hosting the API shown above.

Run the following command from the StrongMindExercise.WebUI folder:

    dotnet run

The Web Application will be available at https://localhost:7289 or https://localhost:5294.

With both projects running, you should be able to interact with the web ui for all CRUD operations on Toppings and Pizzas

## Running Tests
The solution includes unit tests for services using XUnit and Moq.

To run the tests, execute the following commad from the solution directory:

    dotnet test
