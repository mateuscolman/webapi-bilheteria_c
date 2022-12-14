FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base 
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY . .
RUN dotnet restore 
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT Staging
ADD /Cert.Staging.p12 /app/Cert.Staging.p12

WORKDIR /app
COPY --from=publish /app .
# Padrão de container ASP.NET
# ENTRYPOINT ["dotnet", "webapi-bilheteria_c.dll"]
# Opção utilizada pelo Heroku
CMD ASPNETCORE_URLS=http://*:$PORT dotnet webapi-bilheteria_c.dll