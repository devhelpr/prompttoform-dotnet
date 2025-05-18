# time-service
Demonstrating how to build a simple CI/CD pipeline for Azure App Service.

## run

from the root of the project:

```bash
cd Time.Api
dotnet run
```
in the browser navigate to `http://localhost:5256/time` to see the current time.

for the scalar openapi UI navigate to `http://localhost:5256/scalar/` to see the API documentation.

dotnet user-jwts create --scope "time_api" --role "admin"

use Authorization header with the token generated above to access the API : Bearer <token>