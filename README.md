# 🎬 HubCinema API – Movie Ticket Booking Management System  

## 📖 Overview  
**HubCinema API** is the backend system for an online movie ticket booking platform.  
It provides RESTful APIs that manage users, movies, showtimes, seat reservations, payments, and reporting for cinema administrators.  
The system is designed to optimize ticket booking, prevent double bookings, and enhance user experience through real-time seat management and secure online payment integration.  

---

## 🚀 Features  
- 🔐 **JWT Authentication** – secure login and role-based access control (Customer, Staff, Admin).  
- 🎟️ **Online Ticket Booking** – users can select movies, seats, and combos and pay online via VNPay.  
- 🪑 **Temporary Seat Reservation using Redis** – prevents double booking and reduces database load.  
- 💳 **VNPay Integration** – handles payment gateway and callback verification.  
- 🏢 **Admin Dashboard APIs** – manage movies, theaters, rooms, promotions, and reports.  
- 📊 **Statistics & Reports** – revenue tracking, daily/weekly performance reports.  
- ⚙️ **CI/CD with GitHub Actions** – automatic build and deployment pipelines.  
- ☁️ **Cloud Deployment** – hosted on cloud environment for scalability and reliability.  

---

## 🏗️ System Architecture  

The project follows a **Multi-Layer Architecture** combined with **MVC**:  
- **Presentation Layer (View):** Web interfaces using ASP.NET Razor.  
- **Business Logic Layer (Controller/Services):** Handles business rules and data flow.  
- **Data Access Layer (Models/Repositories):** Uses Entity Framework Core and LINQ for database operations.  

🧩 **Tech Stack:**  
- **Language:** C#  
- **Framework:** ASP.NET Core MVC  
- **Database:** SQL Server  
- **Cache:** Redis  
- **Authentication:** JWT  
- **API Docs:** Swagger (Swashbuckle)  
- **Version Control:** Git & GitHub  
- **DevOps:** GitHub Actions (CI/CD)  

---

## 📦 Installation & Setup  

### 1️⃣ Clone the repository
```bash
git clone https://github.com/nguyenxuanbac88/HubCinema-API.git
cd HubCinema-API
