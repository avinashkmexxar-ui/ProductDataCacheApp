## Libraries and why they are used
**Microsoft.Data.SqlClient** is used for all database access. The brief forbids an ORM, so Entity Framework Core and Dapper were not used.
**AutoMapper**  maps DummyJSON JSON models to the domain `Product`, and `Product` to `ProductDetailDto`. Those types have many matching fields.
**Swashbuckle.AspNetCore** (Swagger UI) helps to call the two endpoints from a browser without a separate REST client.
**IHttpClientFactory** (`AddHttpClient`) registers the DummyJSON client.
**System.Text.Json** (`ReadFromJsonAsync`) deserializes DummyJSON responses.

## Clone and run
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
