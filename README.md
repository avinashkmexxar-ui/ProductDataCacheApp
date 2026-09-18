## Libraries and why they are used
**Microsoft.Data.SqlClient** is used for all database access. The brief forbids an ORM, so Entity Framework Core and Dapper were not used.
**AutoMapper**  maps DummyJSON JSON models to the domain `Product`, and `Product` to `ProductDetailDto`. Those types have many matching fields.
**Swashbuckle.AspNetCore** (Swagger UI) helps to call the two endpoints from a browser without a separate REST client.
**IHttpClientFactory** (`AddHttpClient`) registers the DummyJSON client.
**System.Text.Json** (`ReadFromJsonAsync`) deserializes DummyJSON responses.

## Clone and run
