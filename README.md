# 🌍 TourismApi
 
 A tourism web application that allows users to browse and book hotels, cars, and restaurants.  
 Users can also create full tour plans based on a set budget.
 
 The system includes roles for:
 - **Clients**: Can view services, book them, rate and review.
 - **Service Owners**: Can upload services like hotels, cars, and restaurants for admin approval.
 - **Admins**: Review and approve services before they are available.
 
 Booking management, reviews, and approval workflows are all supported within the platform.
 
 ---
 
 ## Getting Started
 
 ### 1. Clone the Repository
 
 ```bash
 git clone https://github.com/dina119/TourismApi.git
 cd TourismApi
 ```
 
 ---
 
 ### 2. Setup Environment
 #### Make sure you have:
 #### .NET SDK 8+
 #### SQL Server 
 #### A code editor ( Visual Studio or VS Code)
 
 Make sure you have:
 
 - ✅ .NET SDK 8+  
 - ✅ SQL Server  
 - ✅ A code editor (Visual Studio or VS Code)
 
 ---
 
 ### 3. Restore NuGet Packages

 
 Run the following command in the terminal:
 
 ```bash
 dotnet restore
 ```
 
 ---
 
## 🔐 Connection String Setup (Local Development)

To avoid sharing sensitive connection strings, we use a local `appsettings.Development.json` file that is **ignored by Git**.

### 🛠 Steps to set up:

1. Search for file named `appsettings.Development.json` in the root of the `TourismApi` project.
2. Add your local connection string to it. Example:
 
 ```json
 "ConnectionStrings": {
   "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=TourismDb;Trusted_Connection=True;TrustServerCertificate=True"
 }
 ```
 
 Replace `YOUR_SERVER_NAME` with your actual SQL Server name or use `localhost`.
 
 ---
 
 ### 5. Apply Database Migrations
 
 To create the database locally with the latest schema:
 
 ```bash
 dotnet ef database update
 ```
 
 > If you don’t have `dotnet ef`, install it using:
 ```bash
 dotnet tool install --global dotnet-ef
 ```
 
 ---
 
 ### 6. Run the Project
 
 Run the API locally:
 
 ```bash
 dotnet run
 ```
 
 Swagger UI will let you explore and test the API endpoints.
 
 ---
 
 ## 💡 Common Git Commands for the Team
 
 > ⚠️ Always pull before you push to avoid merge conflicts
 
 ```bash
 git pull origin main
 git add .
 git commit -m "your changes"
 git push origin main
 ```
 
 ---
 
 ## 📌 Notes
 
 - Run `dotnet ef migrations add` **only if you're making changes to the database models**.
 - All dependencies are handled by `dotnet restore`.
 - Ensure everyone is using the **same .NET SDK version** to avoid build issues.