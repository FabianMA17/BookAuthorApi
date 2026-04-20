# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar todo el repo
COPY . .

# Restore (apuntando directamente al proyecto API)
RUN dotnet restore BookAuthorApi/BookAuthorApi.csproj

# Copiar todo el código
COPY . .

# Publicar API
WORKDIR /src/BookAuthorApi
RUN dotnet publish -c Release -o /app/publish

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
#ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "BookAuthorApi.dll"]