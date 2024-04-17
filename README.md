
# Music Store Demo: React App with .NET Core API (Clean Architecture)

This project is a demonstration of a music store application, with a React frontend interacting with a .NET Core API backend following the principles of Clean Architecture. The React app provides a user-friendly interface for browsing, searching, and purchasing music items, while the .NET Core API adheres to Clean Architecture principles for maintainability, testability, and separation of concerns.

## Planned Features

- **Browse Music**: Explore a catalog of music albums, artists, and tracks.
- **Search Functionality**: Quickly find specific albums or tracks using search functionality.
- **Shopping Cart**: Add items to a shopping cart for easy checkout.
- **Order Processing**: Complete orders for music purchases.

## Prerequisites

1. **[.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)**: The solution uses NuGet packages dependent on this version of .NET.
2. **[Microsoft Visual Studio Community 2022](https://visualstudio.microsoft.com/vs/community/)**: Not mandatory to use 2022 Community but the solution was developed using this version of Visual Studio, so the versions of MSBuild and such will be appropriate.
3. **[NodeJs v21.7.1](https://nodejs.org/en/blog/release/v21.7.1)**: The front end (and the SPA proxy) was developed using this version so it is recommended.

## Installation and Instructions

1. **Clone the Repository**:
    - `git clone https://github.com/mikec-sc/MusicStoreDemo.git`
2. **Open the Solution in Visual Studio 2022**:
    - Navigate to the solution directory and open `MusicStore.sln` in Visual Studio 2022.
3. **Set Up Debug Configuration**:
    - Ensure the React app and .NET Core API projects are both set as startup projects.
4. **Run the Solution**:
    - Simply run the solution in Debug mode from Visual Studio 2022.

**Troubleshooting**:
 - On first-time run, when prompted, accept any certificate changes.
 - The back-end is slower to start than the front-end so you will need to
   refresh your browser after the back-end is ready.
   If you get any errors around ```FSLegacyMainResolve``` or ```Cannot find package
   esbuild\package.json``` then delete
   \src\WebUI\ClientApp\node_modules and try again.

## Configuration

None needed as yet.

## Usage

- Access the React application via the provided URL in the browser.
- Browse the music catalog, add items to the shopping cart, and complete orders.
- Browse the .NET Core API endpoints by the Swagger URL.

## Clean Architecture Principles in .NET Core API

The .NET Core API follows the principles of Clean Architecture, ensuring:
- **Independence of Frameworks**: Inner layers are independent of outer layers. Changes in the UI or database shouldn't affect business logic.
- **Testability**: Business rules can be tested without UI, database, or any external element.
- **Independence of Database**: The database layer is independent of the schema. Changes in the database shouldn't affect business logic.
- **Independence of UI**: The UI layer is a plugin on the application. Changes in the UI shouldn't affect business logic.
- **Independence of External Systems**: Interfaces are defined and implemented in the outer layers. External systems are treated as plugins.

## Contributing

This is only a demo application, not really meant to be publicly viewed.

## License

This project is licensed under the [MIT License](LICENSE).

## Acknowledgments

- **React**: A JavaScript library for building user interfaces.
- **.NET Core**: A cross-platform, high-performance framework for building modern cloud-based, internet-connected applications.
- **Clean Architecture**: A software design philosophy that separates concerns in a way that promotes maintainability and testability.
- **SVG Repo**: Provides free to use SVG files.

## TODO

- Make the ReactApp wait for the .NET Core API to come online when running in dev mode
- Move the DB test data to a JSON file
- Style the FE, maybe using bootstrap
- Add validators for mediator requests
- Add more API endpoints for delete and create
- Add authorization to create and delete endpoints
- Introduce AutoFixture to make unit testing easier
- Add a Postman project to do backend integration testing
- Add more React components to support the purchasing (right now only possible via direct API calls)
- Add more React components for header/footer/purchasing/sorting
- Purchasing API is a stub - in reality it would need some more e-commerce considerations, perhaps to use Stripe or somesuch
- Add create and delete APIs
- Add an admin page with Authorization filters on the page/API endpoints
- Add filters to the existing APIs including searching by ID, sorting, pagination
- Add security to the APIs, especially any create/update/delete endpoints
- Add front-end validation
- Add more unit tests on the backend to ensure new developers do not break existing features
- Improve the front-end to group by artist/year and improve the structure
- Add styles (with bootstrap perhaps or some custom BEM + SASS)
- Support actual DBs instead of InMemory EF DB
- Send useful messages to the frontend when there are errors
