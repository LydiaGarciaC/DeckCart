# Deck Assignment Notes

For this assignment, I developed a .NET 8 Core App called DeckCart that manages a user's shopping cart.

## Structure of the solution's architecture

* **Separated in multiple projects:**
   * **DeckCart.App:** Presentation layer with the executable Program.cs, Controllers, Facade DTOs
   * **DeckCart.Business:** Logic with handlers, interfaces and business models
   * **DeckCart.Data:** Entities, config file with their relationships, EF DbContext, migrations, repositories
   * **DeckCart.Testing:** Moq XUnit tests

* **Reference structure:** App references Business and Data, Business references Data, Data has no dependencies
* **Dependency management:** AutoMapper implemented to avoid Business depending on App

## Technical Implementation

* **Database:** I used Entity Framework's CodeFirst approach for automated migration generation. The connection string is in an environment variable called DECKCART_CONNECTION_STRING to avoid duplication in multiple appsettings.json files, since both App and Data require database access.
* **Security:** A middleware has been set up requiring an ApiKey (You can find it in appsettings.json) for all endpoints except Swagger pages.
* **Logging:** Serilog implementation for color-coded console output and text-file logging.

## Setup

To get this running locally:
1. Set up local SQL database
2. Create that DECKCART_CONNECTION_STRING environment variable
3. Run `dotnet ef database update --project DeckCart.Data` to get schema and seed data
4. Start DeckCart.App

## Future improvements

Some of the enhancements that could be done in the solution:
* Moving the initial seed data out of the DbContext into an external .json file
* Creating custom exception classes instead of throwing generic InvalidOperationExceptions
* Adding more cart features like item quantities, or making a CRUD of users/items
* Enhancing the history tracking beyond the 'deletedOn' column
* Move ApiKey to a more secure place
* Expand Unit Test coverage