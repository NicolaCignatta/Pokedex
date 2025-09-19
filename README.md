# Pokedex API

This project implements a REST API that provides information about Pokémon, including a "fun" translated description. The API interfaces with the [PokéAPI](https://pokeapi.co/) for basic Pokémon data and with the [FunTranslations API](https://funtranslations.com) for translations.

## 1. Project Structure

The project is organized into a .NET solution with several projects, following a hexagonal architectural approach that aims to ensure a clear separation of concerns, maintainability, and testability.

The solution structure is as follows:

*   **Pokedex.API**: The main RESTful API project. It contains all endpoints, manages dependencies registration, and cache layer.
*   **Pokedex.Core**:
    *   `Pokedex.Domain`: Contains the Pokémon domain model, interfaces for all domain services, and core business logic. This layer is independent of any infrastructural technology but contains all infrastrucure interfaces in order to define the entire architecture of the solution.
*   **Pokedex.Infrastructure**: Contains the implementations of technical details and external dependencies.
    *   `Pokedex.Infrastructure.Cache.Redis`: Implements the caching strategy using Redis and Output Cache, designed to improve performance and reduce redundant calls to external APIs and rate limit mitigation.
    *   `Pokedex.Infrastructure.Http.PokemonAPI`: A specific HTTP client for interacting with the PokéAPI, responsible for retrieving basic Pokémon data.
    *   `Pokedex.Infrastructure.Translation.FunTranslation`: A dedicated HTTP client for interacting with the FunTranslations API, handling requests for description translations.
*   **Pokedex.Shared**: Contains code and contracts that can be shared across different layers of the project to avoid duplication.
    *   `Pokedex.Shared.API`: May contain DTOs (Data Transfer Objects) or common interfaces used in API contracts.
    *   `Pokedex.Shared.Infrastructure.Http`: Provides generic HTTP utilities or a reusable base HTTP client for external calls.
    *   `Pokedex.Shared.Infrastructure.Http.UnitTests`: Contains unit tests for shared HTTP utilities.
*   **Pokedex.Tests**: Projects dedicated to testing, ensuring comprehensive coverage and code robustness.
    *   `Pokedex.FunctionalTests`: Tests that verify the end-to-end behavior of API functionalities, simulating real user scenarios.
    *   `Pokedex.IntegrationTests`: Tests that verify the correct interaction between different components or services of the system, including external API clients.
    *   `Pokedex.ManualTests`: May contain instructions or use cases for manual testing.
    *   `Pokedex.UnitTests`: Unit tests to verify domain logics.

## 2. Technical and Architectural Choices and Patterns Used

*   **Language and Framework**: The application is developed in **C#** using **.NET 9**.
*   **Hexagonal Architecture**: The project strictly adheres to the principles of a hexagonal architecture, with a clear separation between Domain, Application, and Infrastructure. 
*   **RESTful API**: The API exposes resources via minimal API in order to have a vertical flow for each use case.
*   **Caching with Redis**: A caching layer has been implemented using Redis to:
    *   Reduce the number of redundant calls to external APIs, especially for frequently requested data and rate limit mitigation.
    *   Significantly improve API response times.
*   **Comprehensive Testing Strategy**: The project is accompanied by a complete suite of tests (unit, integration, and functional).
    *    **Unit Test**: validate all domain logics
    *    **Integration Test**: validate all integrations with external seervices, such as Pokemon and FunTranslation API
    *    **Functional Test**: validate E2E API flows.
 
## 3. Key Feature Development (Pull Requests)

The core functionalities of this Pokedex API were primarily developed and integrated through the following Pull Requests:

*   **Feature 1: Basic Pokemon Information**: This PR introduced the initial setup for retrieving basic Pokémon data from the PokéAPI.
    *   [https://github.com/NicolaCignatta/Pokedex/pull/1](https://github.com/NicolaCignatta/Pokedex/pull/1)
*   **Feature 2: Translated Pokemon Description**: This PR implemented the logic for translating Pokémon descriptions using the FunTranslations API, including conditional translation rules and fallback mechanisms.
    *   [https://github.com/NicolaCignatta/Pokedex/pull/2](https://github.com/NicolaCignatta/Pokedex/pull/2)

## 4. How to Run the Project

To run the Pokedex API application, follow the steps below.

### Prerequisites

*   **.NET SDK**: Ensure you have the .NET SDK installed (the latest LTS version is recommended, e.g., .NET 9). You can download it from the [official .NET website](https://dotnet.microsoft.com/download).
*   **Docker (Optional but Recommended)**: To easily launch a Redis instance locally. Alternatively, ensure you have a Redis instance running and accessible and add your instance in the appsettings file
*    **Redis Connection**: The Redis connection string is typically configured in all `appsettings.json` in `Pokedex.API` project. When using Docker Compose as described below, Redis will be accessible at `localhost:6379`.

### Steps to Run

1.  **Clone the Repository**:
    ```bash
    git clone https://github.com/NicolaCignatta/Pokedex.git
    cd Pokedex # Navigate to the solution root folder
    ```

2.  **Start Redis using Docker Compose**:
    Navigate to the `Pokedex/infra` directory, which contains the `docker-compose.yml` file for Redis:
    ```bash
    cd infra
    docker compose up -d
    ```
    This command will start a Redis instance in the background.

    **Note on Redis Availability**: If Redis is not running or is inaccessible, the application is designed to log an error regarding the cache connection and will **fallback** to bypassing the cache layer, directly fetching data from external APIs without attempting to store or retrieve from Redis. This ensures the application remains functional even without a running cache.

3.  **Navigate back to the API project**:
    ```bash
    cd ./src/Pokedex.API
    ```

4.  **Restore dependencies**:
    ```bash
    dotnet restore
    ```

5.  **Build the project**:
    ```bash
    dotnet build
    ```

6.  **Run the project**:
    ```bash
    dotnet run
    ```
     The API will be available at the address `https://localhost:7251` for the main API endpoints. You can also access the **Swagger UI** for interactive testing and documentation at:
    `https://localhost:7251/swagger/index.html`

### Available Endpoints

Once the API is running, you can test the following endpoints:

1.  **Basic Pokémon Information**:
    This endpoint returns basic information for a Pokémon.
    *   **Method**: `GET`
    *   **URL**: `/pokemon/{pokemonName}`
        ```bash
        curl -X GET https://localhost:7251/pokemon/pikachu
        ```
    *   **Example response**:
        ```json
        {
          "id": 25,
          "name": "Pikachu",
          "description": "When several of\nthese POKéMON\ngather, their\felectricity could\nbuild and cause\nlightning storms.",
          "habitat": "forest",
          "isLegendary": false
        }
        ```

2.  **Pokémon Information with Translated Description**:
    This endpoint returns basic Pokémon information with a translated description following specific rules.
    *   **Method**: `GET`
    *   **URL**: `/pokemon/translate/{pokemonName}`
        ```bash
        curl -X GET https://localhost:7251pokemon/translated/pikachu
        ```
    *   **Example response (Pikachu is not legendary, so Shakespeare translation)**:
        ```json
        {
          "id": 25,
          "name": "Pikachu",
          "description": "At which hour several of these pokémon gather,  their electricity couldst buildeth and cause lightning storms.",
          "habitat": "forest",
          "isLegendary": false
        }
        ```
 **Note on manual tests**: in Pokedex.ManualTests project, you can find two http file which allow you to try both API calls.
### Running Tests

To run all project tests and verify the correctness of the implementations:

1.  Navigate to the root directory of the Pokedex solution:
    ```bash
    cd <PATH_TO_POKEDEX_FOLDER>
    ```
    (If you are in `Pokedex.API`, use `cd ..` to go back).

2.  Run all tests:
    ```bash
    dotnet test
    ```
    This command will automatically execute all unit, integration, and functional tests .
