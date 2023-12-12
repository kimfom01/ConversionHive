#  Conversion Hive

---

## Introduction
- This is an api which users can use to send mails to their contacts
- The application is tailored to simplify the process of sending custom mails to one or multiple contacts.

## Installation

### Prerequisites
- Ensure [.NET SDK/Runtime](https://dotnet.microsoft.com/download) (version 8.0 is installed on your machine.
- Ensure you have `postgres` installed on your machine or you can connect to remote db.
- Install [Visual Studio](https://visualstudio.microsoft.com/) or Rider (not free).

### Getting the Project
- Clone the repository: `git clone https://github.com/kimfom01/ConversionHive.git`
- Alternatively, download and extract the project ZIP file.

### Configuration

- Update `appsettings.json` with your typical postgres [connection string](https://www.connectionstrings.com/postgresql/).

### Building the Project
- Navigate to the project's root directory in the terminal.
- Run `dotnet build` to compile the project.

### Running the Application
- Execute `dotnet run` within the project directory.
- Access the application via the provided local server URL for web projects.

### Publishing (For Deployment)
- Run `dotnet publish -c Release -o ./publish` to package the application for deployment.
- Deploy the contents of the `./publish` directory to your hosting environment.

---

## Features
- Send custom mails
- Add multiple contacts using csv
- Have many groups of contacts called companies
- Companies have individual mail configuration
