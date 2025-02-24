# 📧 EmailApp

## 🌟 Overview
EmailApp is an **ASP.NET Core MVC** application that provides **email newsletter functionality**. It has two main modules:

### 👥 User Module
- 🔐 Users can register and log in.
- 📩 Users can subscribe to newsletters.

### 🛠️ Admin Module
- 📜 Admin can view registered users.
- ✉️ Admin can compose and send newsletters via email.

The application uses **MailKit** to send emails via SMTP using Google credentials (**App Passwords**) and **SQL Server** for database storage.

---

## 🚀 Features
✅ User authentication and newsletter subscription.  
✅ Admin panel to manage users and send newsletters.  
✅ Uses **EF Core Code-First** approach.  
✅ Securely stores credentials (SMTP email, password, connection string) in `appsettings.json`.  
✅ Easily configurable and extendable.  

---

## 🛠️ Installation Guide
Follow these steps to set up and run the **EmailApp** project:

### 📌 Prerequisites
- 🏗️ Install [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- 🗄️ Install [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- 🔑 Create a Google **App Password** for SMTP authentication ([Site](https://myaccount.google.com/apppasswords))

### 📝 Steps to Set Up

1️⃣ **Clone the Repository**
   ```sh
   git clone https://github.com/Neutrino79/EmailApp.NET
   cd EmailApp
   ```

2️⃣ **Update Configuration**
   - Open `appsettings.json` and update SMTP and database credentials.
   ```json
   "SmtpSettings": {
     "Email": "your-email@gmail.com",
     "Password": "your-app-password",
   },
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=EmailAppDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3️⃣ **Restore Dependencies**
   ```sh
   dotnet restore
   ```

4️⃣ **Apply Migrations**
   ```sh
   dotnet ef database update
   ```
   _If you face issues, run:_
   ```sh
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5️⃣ **Run the Application**
   ```sh
   dotnet run
   ```
   🎯 The app will start on **`https://localhost:7103/`**.

---

## 📂 Project Structure
```
EmailApp/
│   EmailApp.sln                   # Solution file
│   README.md                      # Project documentation
│
├───EmailApp.Presentation           # UI Layer (MVC Views & Controllers)
│   ├── Views/                      # Razor Views
│   ├── wwwroot/                    # Static files (CSS, JS, Images)
│   ├── appsettings.json            # Application configuration
│   ├── Program.cs                  # Main entry point
│   └── EmailApp.Presentation.csproj
│
├───EmailApp.Application            # Business logic & services
│   ├── Services/                   # Application services
│   └── EmailApp.Application.csproj
│
├───EmailApp.Contracts              # Interfaces for Dependency Injection
│   └── Contracts/                   # Service Contracts
│
├───EmailApp.Domain                 # Core domain models & interfaces
│   ├── Models/                      # Entity models
│   ├── Interfaces/                  # Repository interfaces
│   └── EmailApp.Domain.csproj
│
├───EmailApp.Infrastructure         # Database context & repositories
│   ├── Migrations/                  # EF Core migrations
│   ├── Repositories/                # Implementation of repositories
│   ├── ApplicationDbContext.cs      # Database context
│   └── EmailApp.Infrastructure.csproj
```

---

## 🎯 Usage
- **Register/Login** as a user to subscribe to newsletters.
- **Admin Panel**: View users and send newsletters.
- **Custom SMTP Configuration**: Update `appsettings.json` for custom SMTP servers.

---

## 📦 Dependencies
- 🏗️ .NET 9  
- 🖥️ ASP.NET Core MVC  
- 🗃️ Entity Framework Core  
- 🛢️ SQL Server  
- ✉️ MailKit (for sending emails)  

---

## 📜 License
This project is licensed under the **MIT License**.

---

## 📞 Contact
For any issues or contributions, create a **pull request**
