# Driving License Management System

A Windows Forms application for managing driving license services, applications, drivers, people, and international licenses.

## Project Overview

The Driving License Management System (DVLD) is built with C# and .NET Framework 4.8. It provides a centralized interface for managing applicants, driver records, driving license applications, local licenses, and international licenses.

The application uses a layered architecture that separates the user interface, business logic, and data access responsibilities.

## Technologies Used

- C#
- .NET Framework 4.8
- Windows Forms
- SQL Server
- ADO.NET
- Visual Studio
- Object-oriented programming
- Layered architecture

## Main Features

### User Authentication

- User login screen
- User account validation
- User information management
- User-based application creation and tracking

### People Management

- Add, update, and delete people
- Search for people
- View complete person information
- Manage contact details and personal data

### Driving Applications

- Create new driving license applications
- Track application status
- Update application statuses
- Store application fees
- Associate applications with applicants and users

### Local Driving Licenses

- Manage local driving licenses
- Search for license records
- View driver and license information
- Issue, renew, replace, and manage licenses
- Track license expiration dates and activation status

### International Licenses

- Issue international driving licenses
- List international license records
- Search international licenses
- Display the associated person and driver information
- Track issue dates, expiration dates, and active status

### Driver Management

- Manage driver records
- Associate drivers with people
- View the driver's license history
- Check whether a driver has an active license or application

## Project Structure

The project is organized into separate layers:

## Architecture

The project follows a layered design:

### Presentation Layer

Contains Windows Forms screens and UI event handlers. This layer is responsible for displaying information and receiving user input.

### Business Layer

Contains classes such as:

- `clsPerson`
- `clsDriver`
- `clsLicense`
- `clsApplication`
- `clsInternationalLicense`
- `clsUser`

These classes contain business rules and coordinate operations between the presentation and data access layers.

### Data Access Layer

Contains classes responsible for database communication, including:

- Reading records
- Inserting new records
- Updating existing records
- Deleting records
- Executing stored procedures and SQL queries

## Requirements

Before running the project, install the following:

- Visual Studio with .NET desktop development tools
- .NET Framework 4.8
- SQL Server
- SQL Server Management Studio, recommended
- The project's database and connection configuration

## Configuration

Configure the database connection string in the application's configuration file before running the project.

Verify that:

1. SQL Server is running.
2. The required database has been created.
3. The connection string points to the correct SQL Server instance.
4. The database user has the required permissions.

## Running the Application

1. Clone the repository:

2. Open the solution in Visual Studio.

3. Restore or configure the database.

4. Update the database connection string.

5. Build the solution.

6. Run the application.

7. Sign in using a valid user account configured in the database.

## Database Operations

The application uses the database to persist:

- User accounts
- People
- Drivers
- Applications
- Application types
- Local licenses
- International licenses
- License history

Database access is handled separately from the user interface to improve maintainability and organization.

## Error Handling

The application validates user input and checks whether requested records exist before displaying or modifying them. Database operations return success or failure results that are used by the business layer and user interface.