# AllTheBeans

Full Stack Code Development for "AllTheBeans” Business

---

## Features
- **Bean Catalog**
  - List all beans
  - Search/filter by name, country, or cost
- **Bean of the Day**
  - Daily featured bean
  - Enforced at DB level: one BOTD per date (unique index)
- **Orders API (Full CRUD)**
  - `GET /api/orders` → list orders
  - `GET /api/orders/{id}` → fetch single order
  - `POST /api/orders` → place a new order
  - `PUT /api/orders/{id}` → update existing order
  - `DELETE /api/orders/{id}` → cancel an order
- **CORS**
  - Configured via `appsettings.json`, no hardcoding
- **Database**
  - Indexes on `Name`, `Country`, `Cost`, and `(Country, Name)` for fast search
  - Seed data loaded from `AllTheBeans.json`
- **UI**
  - Angular frontend
  - Search box, Bean of the Day, Order form
  - Currency configurable in `environment.ts`

---

