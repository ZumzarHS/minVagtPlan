# minVagtPlan

## Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (for local development)
- (Optional) [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/)

## Clone the Repository

Clone the project to your local machine using the following command:

```bash
git clone https://github.com/ZumzarHS/minVagtPlan.git
```

## Setting Up the Development Environment

1. **Install Dependencies**:
   In Visual Studio 2022, right click Dependicies in Solution Explorer and select "Manage NuGet Packages". Make sure the following packages are installed:
   ```bash
    Microsoft.EntityFrameworkCore.SqlServer
    Microsoft.EntityFrameworkCore.Tools
   ```

2. **Set up the Database**:
   Ensure that SQL Server Express is installed and running. If your server og database name is different change it in **appsettings.json**.

   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=SQLEXPRESS;Database=myVagtPlanDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

3. **Apply Migrations**:
   Run the following commands to setup db and make initial migration:
   ```bash
   dotnet ef migrations add InitialMigration
   dotnet ef database update
   ```
   This will create the database and tables as defined in your **DbContext**.

4. **Update database**:
   Run the following commands to update changes to database and tables as defined in your **DbContext**:
   ```bash
   dotnet ef migrations add [INSERT MIGRATION NAME]
   dotnet ef database update
   ```
