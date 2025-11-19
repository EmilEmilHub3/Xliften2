# BUILD-STAGE: bygger din .NET app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiér projektfil og restore
COPY Xliften2.csproj ./
RUN dotnet restore

# Kopiér resten af koden
COPY . ./

# Publish i Release til /app/publish
RUN dotnet publish -c Release -o /app/publish

# RUNTIME-STAGE: kørsel af appen
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Kopiér den publish'ede app
COPY --from=build /app/publish .

# Lyt på port 8080 inde i containeren
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Din dll hedder Xliften2.dll (samme navn som csproj)
ENTRYPOINT ["dotnet", "Xliften2.dll"]
