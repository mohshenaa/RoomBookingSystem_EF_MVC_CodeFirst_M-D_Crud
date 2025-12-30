# Room Booking System (EF MVC Code-First with CRUD)

A simple **Room Booking Management System** built using **ASP.NET MVC**, **Entity Framework (Code-First)**, and **SQL Server**.

This project implements full **CRUD** operations for managing room bookings — creating, reading, updating, and deleting booking data.

---

## 🧠 Overview

This is a web application that allows users/admins to manage room booking information.  
It uses the **Entity Framework Code-First** approach to generate the database from C# models.  

The MVC pattern separates the app into:

- **Models** — data classes and EF entities  
- **Views** — UI pages (.cshtml)  
- **Controllers** — logic for handling user requests and CRUD operations

---

## 🚀 Features

✔ Add new room bookings  
✔ View all records  
✔ Edit booking details  
✔ Delete bookings  
✔ Database generated using EF Core Code-First

---

## 📦 Files & Structure

- `RoomBooking.sln` — Visual Studio solution  
- `SQLQuery1.sql` — SQL script (optional)  
- `Controllers/` — MVC controllers  
- `Models/` — Entity classes  
- `Views/` — UI pages  
- `Data/` — EF DbContext  
- `README.md` — Project documentation

---

## 🛠 Technologies

- ASP.NET MVC (Model-View-Controller)  
- Entity Framework (Code-First)  
- C#  
- Microsoft SQL Server  
- HTML, CSS, JavaScript

---

## 🚧 Requirements

Before running the application locally, make sure you have:

✔ Visual Studio (2019 or newer)  
✔ .NET Framework or .NET Core installed  
✔ SQL Server (LocalDB or full instance)

---

## 📌 Installation & Setup

1. **Clone the repository**
   git clone https://github.com/mohshenaa/RoomBookingSystem_EF_MVC_CodeFirst_M-D_Crud.git

2. Open the solution in Visual Studio
RoomBooking.sln

3. Update connection string
Open Web.config and set your SQL Server connection:

<connectionStrings>
  <add name="DefaultConnection" connectionString="Server=YOUR_SERVER;Database=RoomBookingDB;Trusted_Connection=True;" providerName="System.Data.SqlClient" />
</connectionStrings>


4. Apply migrations
In the Package Manager Console:(Tools -> Nuget package Manager ->Package Manager Console)

update-database


5. Run the application
Press F5 or click Run in Visual Studio.

📬 Contact

Created by Mohshena Akter Meem —
Email: mohshenaa@gmail.com
GitHub: https://github.com/mohshenaa
