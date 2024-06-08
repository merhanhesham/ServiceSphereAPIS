# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy all .csproj files and the solution file
COPY ["ServiceSphere.APIs.sln", "./"]
COPY ["ServiceSphere.core/ServiceSphere.core.csproj", "ServiceSphere.core/"]
COPY ["ServiceSphere.repository/ServiceSphere.repository.csproj", "ServiceSphere.repository/"]
COPY ["ServiceSphere.services/ServiceSphere.services.csproj", "ServiceSphere.services/"]
COPY ["ServiceSphere.APIs/ServiceSphere.APIs.csproj", "ServiceSphere.APIs/"]

# Restore dependencies
RUN dotnet restore "ServiceSphere.APIs.sln"

# Copy the rest of the files and build the application
COPY . .
WORKDIR "/src/ServiceSphere.APIs"
RUN dotnet build "ServiceSphere.APIs.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ServiceSphere.APIs.csproj" -c Release -o /app/publish

# Use the official ASP.NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServiceSphere.APIs.dll"]
