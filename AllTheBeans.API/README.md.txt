# All The Beans - API

This is the backend API for **All The Beans**.

---

## Features
- CRUD endpoints for Beans and Orders
- Bean of the Day (ensures one BOTD per date)
- Configurable CORS policy
- EF Core with SQL Server
- Seeds beans from `AllTheBeans.json`

---

## Configuration
- **Connection String** → set in `appsettings.json`
- **CORS Allowed Origins** → set in `appsettings.json` under `Cors:AllowedOrigins`

Example:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AllTheBeans;Trusted_Connection=True;"
  },
  "Cors": {
    "AllowedOrigins": [ "http://localhost:4200" ]
  }
}
