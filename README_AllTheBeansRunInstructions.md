# AllTheBeans – Full Stack Code Development for "AllTheBeans” Business  

## Overview  
**AllTheBeans** is a full stack application built with **ASP.NET Core 8 (Web API)**, **Entity Framework Core**, **SQL Server**, and **Angular**.  
It provides features such as:  
- Browsing available beans (with images, descriptions, cost, etc.)  
- Searching beans (real-time, case-insensitive)  
- Viewing a **Bean of the Day** (randomly chosen, persisted daily)  
- Placing and managing orders with validation  
- Clean UI styled with Angular and CSS  

The solution has been developed with principles of **security, performance, readability, testability, scalability, and simplicity** in mind.  

---

## 1. Prerequisites  

Install the following before running the solution:  

- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- [Node.js (LTS) & npm](https://nodejs.org/en/)  
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or LocalDB)  
- [Angular CLI](https://angular.io/cli)  
- IDE (Visual Studio 2022, VS Code)  

---

## 2. Backend (ASP.NET Core API)  

### Navigate to the backend  
```powershell
cd C:\dev\AllTheBeans\AllTheBeans.API
```

### Details
* API will start on **[http://localhost:5000](http://localhost:5000)** (or configured port).
* Seeds database from **AllTheBeans.json** (if DB is empty).


### Database setup  
- The API uses **Entity Framework Core** with SQL Server.  
- `Bean.Id` and `Order.Id` are **primary keys with auto-increment (IDENTITY)**.  
- Ensure your SQL Server connection string is correctly configured in **appsettings.json** (e.g.,  
  `Server=.;Database=AllTheBeansDb;Trusted_Connection=True;TrustServerCertificate=True;`).

- Run migrations to set up schema:  
```powershell
dotnet ef database update
```

### Run the API  
```powershell
dotnet run
```

### Endpoints  
- Swagger: `http://localhost:5000/swagger`  
- Beans API: `http://localhost:5000/api/beans`  
- Orders API: `http://localhost:5000/api/orders`  

---

## 3. Frontend (Angular)  

### Navigate to frontend  
```powershell
cd C:\dev\AllTheBeans\AllTheBeans.UI
```

### Install dependencies  
```powershell
npm install
```

### Configure API endpoint  
Ensure `src/environments/environment.ts` has the correct backend URL:  
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

### Run Angular app  
```powershell
ng serve
```

### Open in browser  
```
http://localhost:4200
```

---

## 4. Tests (xUnit + EF Core InMemory)  

### Navigate to tests  
```powershell
cd C:\dev\AllTheBeans\AllTheBeans.Tests
```

### Restore and run tests  
```powershell
dotnet restore
dotnet test
```

### Notes  
- Uses **EF Core InMemory provider** → no DB required.  
- Covers:  
  - `BeansController`  
  - `OrdersController`  
  - `BeanService`  
  - `SeedData`  
- Each test runs with **seeded data** for **repeatability**.  

---

## 5. Key Implementation Details  

- **Primary Keys**: Auto-incrementing IDs (`IDENTITY`) for Beans and Orders.  
- **Validation**: Orders require valid `BeanId`, `CustomerName`, `Address`, and `Quantity > 0`.  
- **Search**: Case-insensitive search using EF Core `LIKE` across `Name`, `Country`.  
- **Bean of the Day**: Persisted daily in DB, ensures no repeat of yesterday’s selection.  
- **Serialization**: `[JsonIgnore]` prevents circular references on navigation properties.  
- **Separation of Concerns**:  
  - Angular → UI and client logic  
  - ASP.NET Core API → business logic and validation  
  - EF Core → persistence  

---

## 6. Running the Complete Solution  

1. Start backend API:  
   ```powershell
   cd AllTheBeans.API
   dotnet run
   ```
2. Open `http://localhost:5000`

3. Start frontend Angular UI:  
   ```powershell
   cd AllTheBeans.UI
   ng serve
   ```
4. Open: `http://localhost:4200`  

### Features Available:  
- Browse all beans (grid layout, collapsible details)  
- Search beans (real-time, case-insensitive, backend-powered)  
- View **Bean of the Day** (with image, description, and cost)  
- Place orders (validated, saved in DB)  

---

With this setup, the solution is ready to run end-to-end, with a clean separation between backend, frontend, and tests.  
