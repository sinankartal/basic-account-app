# basic-account-app


# Account App
This application provides a user-friendly web API and user interface for managing customer accounts. Users who are responsible for managing customers' accounts can use this application to perform the following tasks:

- View a list of existing customers
 - Open a new account for a customer with an initial amount
 - Add transactions to a customer's account after opening it
 - View the customer's account information, including their name, surname, balance, and transaction history
 - This application consists of two main components that work together to provide a seamless user experience: a backend solution and a user interface.
        -- The backend solution, built on .NET 6, processes API operations and manages transactions. There is two project on the backend. The backend service(Transaction-Service) manages account transactions , ensuring that all transactions are completed successfully and accurately. This service runs in the background, allowing users to perform various tasks without interruption. Secondly, the web API provides a set of endpoints that allow users to interact with the application programmatically. Users can create a new account, view customer information, and add transactions to a customer's account using these endpoints. The API uses RESTful architecture style and follows industry-standard best practices, making it easy for developers to integrate the API into their applications.
        -- The user interface, built using the Angular framework, provides a visually way for users to manage customer accounts. Users can create a new account, view customer information, and add transactions to a customer's account with ease. The interface also allows users to view the customer's open accounts and their transaction and balance information, making it easy to keep track of their accounts.

## Configuration
SDK: The latest version of .NET 6 should be installed to run the application for the backend. NodeJS should be installed to run the UI application. 
IDE: The application is developed using Visual Studio Code and Rider. You can use any of these IDEs or Visual Studio to open and run the application.
Database: The application uses an in-memory database, no additional setup is required to run the application.
Logging: The application uses serilog for logging, the configuration for serilog is already done.
Environment variables: Backend appolication does not use any environment variables. However, UI app keeps environment data in environment.ts file.
Additional setup: The application does not require any additional setup.
Dependencies: The application depends on the latest version of the .NET 6 SDK and it will automatically install the required packages when you build the solution.
Configuration files: The application uses an appsettings.json configuration file to store settings such as database and Azure connection strings and other settings. This file is already included in the solution and no additional setup is required. If you need to make changes to the configuration, you can modify the appsettings.json file.

Message queue: The application use Azure Service bus to make to communication between api project and transaction service.

## Assumptions

Customers are already registered in the system.
Each customer can have one or more accounts.
Users can only manage customers' account if they are authenticated to do so.

## Controller Endpoints

AccountController:
POST '/Account/create': This endpoint creates a new customer account. It accepts a JSON object in the format of CreateAccountRequest, which includes a UserId and an Initial Amount. The response will include the newly created AccountId.
POST '/Account/add-transaction': This endpoint creates a new money transaction for an account. It accepts a JSON object in the format of AddTransactionRequest, which includes an AccountId and an Amount. 

TokenController:
POST '/token/login': This endpoint allows a user to authenticate and receive a JWT token. It accepts a JSON object in the request body, including a username and password. If the credentials are valid, it returns a JWT token. If the credentials are invalid, it returns an Unauthorized response with the message "Invalid Credentials". To receive the token, use the username "admin" and password "admin".

UsersController:
GET '/user/all': This endpoint allows a user to view a list of all registered users in the system. The response will include a list of UserDTO objects.
GET '/user/{userId}/accounts': This endpoint allows a user to view the list of accounts and transactions for a specific user. It accepts a userId parameter in the URL path, and the response will include a GetUserAccountsResponse object, which contains a UserDTO and a list of AccountDTO objects.


## Execution
Clone the github repo that is provided.
Navigate to the "Backend" folder and open the solution in an IDE.
Navigate to the "API" folder inside the "src" folder and run the following commands:
    - dotnet build
    - dotnet run.
Navigate to the "Transaction-Service" folder inside the "src" folder and run the following commands: 
    - dotnet build
    - dotnet run.
Navigate to the "Frontend" folder and open the "Account-UI" folder.
Run the following commands:
    - npm install
    - npm start
The API will be hosted on http://localhost:7093 by default, where 7093 is the port number specified in the launchSettings.json file.
The UI application will be hosted on http://localhost:4200.
Open a web browser to see the UI and go to http://localhost:4200.
Go to http://localhost:7093 to view the available endpoints and test the API.

