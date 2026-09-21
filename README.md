# Product Data Cache API

ASP.NET Core Web API that caches DummyJSON products in SQL Server.

- `GET /api/products`
- `GET /api/products/{id}`

If the data is already in the database, it is returned from SQL Server. If not, it is fetched from [DummyJSON](https://dummyjson.com/), saved, then returned.

DummyJSON is public. **No API key is required.**

## Run with Visual Studio and LocalDB

### 1. Clone the repo

```bash
git clone https://github.com/avinashkmexxar-ui/ProductDataCacheApp.git
cd ProductDataCacheApp
```

### 2. Create the database

Run this from the **repository root** (the folder that contains `schema.sql`):

```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -i schema.sql
```

### 3. Open in Visual Studio and run

1. Open `ProductDataCacheApp.slnx`.
2. Right-click **API** → **Set as Startup Project**.
3. Press **F5** (or **Ctrl+F5**).

Swagger: https://localhost:7247/swagger

- https://localhost:7247/api/products
- https://localhost:7247/api/products/1

`appsettings.json` already uses LocalDB:

```text
Server=(localdb)\MSSQLLocalDB;Database=ProductCache;Trusted_Connection=True;TrustServerCertificate=True
```

## Libraries and why they are used

**ASP.NET Core** is the required Web API host.

**Microsoft.Data.SqlClient** is used instead of an ORM. The brief requires SQL queries; a stored procedure upserts products.

**AutoMapper** maps DummyJSON `ExternalProductDto` to domain `Product`, and `Product` to `ProductDetailDto`.

**Swashbuckle** provides Swagger UI so the endpoints can be tried in a browser.

**IHttpClientFactory** registers the DummyJSON HTTP client.

There is no third-party API key to configure.
