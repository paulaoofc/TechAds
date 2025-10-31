# ACCEPTANCE_STEP_1.md

## Checklist

- [x] Created directory structure: src/, frontend/, .github/workflows/
- [x] Created .NET solution TechAds.sln with projects: TechAds.Domain, TechAds.Application, TechAds.Infrastructure, TechAds.Api, TechAds.Tests
- [x] Added NuGet packages: MediatR, FluentValidation, Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Design, Microsoft.EntityFrameworkCore.InMemory, Microsoft.AspNetCore.Authentication.JwtBearer, Microsoft.AspNetCore.Identity.EntityFrameworkCore, Swashbuckle.AspNetCore
- [x] Created MIT LICENSE file
- [x] Created CI workflow .github/workflows/ci.yml
- [x] Verified dotnet build succeeds

## Run Locally

1. Navigate to the project root: `cd d:\TechAds`
2. Build the solution: `dotnet build src/TechAds.sln`
3. Run tests: `dotnet test src/TechAds.sln`
