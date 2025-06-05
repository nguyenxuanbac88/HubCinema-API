
# 🎬 Cinema Ticket Booking API

This is a RESTful API built for managing an online cinema ticket booking system. It supports core features such as movie listings, showtimes, seat management, ticket and food purchases, and invoice tracking.

## 📌 Technologies Used

- ASP.NET Core (C#)
- SQL Server
- Entity Framework Core
- JWT Authentication
- REST API

## 🚦 Main Features

- User registration and login
- View cinema clusters, rooms, movies, and showtimes
- Select seats and book tickets
- Add food items to an invoice
- Generate and view invoices with total payment
- User spending tracking

## 🗃️ Database Overview

This system uses a relational database design. Some key tables include:

- **Cinemas** – Stores information about cinema clusters
- **Cinema_rooms** – Manages rooms within each cinema
- **Movies** – Holds details about movies
- **Showtimes** – Schedules for each movie per cinema
- **SeatTypes & ShowtimeSeats** – Defines seat categories and availability
- **Invoice / InvoiceTickets / InvoiceFood** – Tracks all ticket and food purchases
- **Food** – Available snacks and drinks
- **User** – Manages user information, roles, and spending

## 🚀 Getting Started

### 1. Clone the Project
```bash
git clone https://github.com/nguyenxuanbac88/API_ProjectCinema
cd API_ProjectCinema
```

### 2. Set up Database Connection
Edit the `appsettings.json` file to configure your connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=cinema;User Id=sa;Password=YOUR_PASSWORD;"
}
```

### 3. Run Database Migration
```bash
dotnet ef database update
```

### 4. Start the API Server
```bash
dotnet run
```

### 5. Access API Endpoints
The API will be available at:
```
http://localhost:5000/api/
```

## 🧪 Sample Endpoints

| Method | Endpoint                     | Description                     |
|--------|------------------------------|---------------------------------|
| GET    | /api/cinemas                 | List all cinemas                |
| GET    | /api/movies                  | List all movies                 |
| POST   | /api/auth/register           | Register new user               |
| POST   | /api/invoice                 | Create ticket + food invoice    |
| GET    | /api/showtimes/{movieId}     | Get showtimes for a movie       |

## 🔐 Authentication

This project uses **JWT** for securing API routes.  
Use the following header format:
```
Authorization: Bearer <your_token>
```

## 📖 API Documentation

If Swagger is enabled, access documentation at:
```
http://localhost:5000/swagger
```

## 👨‍🎓 Developer

**Nguyễn Xuân Bắc**  
Student ID: `23DH110293` 

**Lâm Tấn Thành**  
Student ID: `23DH113200` 

**Trần Duy Khoa**  
Student ID: `23DH114398` 

**Nguyễn Đàm Khá**  
Student ID: `23DH111567` 

**Ho Chi Minh City University of Foreign Languages and Information Technology (HUFLIT)**

