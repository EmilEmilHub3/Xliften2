# BUILD STAGE: build the .NET app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore
COPY Xliften2.csproj ./
RUN dotnet restore

# Copy the rest of the source code
COPY . ./

# Publish in Release configuration to /app/publish
RUN dotnet publish -c Release -o /app/publish

# RUNTIME STAGE: run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Listen on port 8080 inside the container
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# The application DLL is Xliften2.dll (same name as the csproj)
ENTRYPOINT ["dotnet", "Xliften2.dll"]
