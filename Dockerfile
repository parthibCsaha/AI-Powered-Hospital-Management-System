# ───────────────────────────  BUILD  ───────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first (layer caching for restore)
COPY HMS.sln ./
COPY HMS-Backend/HMS-Backend.csproj HMS-Backend/

RUN dotnet restore

# Copy everything else and publish
COPY . .
RUN dotnet publish HMS-Backend/HMS-Backend.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ───────────────────────────  RUNTIME  ────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Default ASP.NET 8 container port
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "HMS-Backend.dll"]
