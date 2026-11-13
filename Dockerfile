FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY global.json ./
COPY Agroflet.Platform/Agroflet.Platform.csproj Agroflet.Platform/
RUN dotnet restore Agroflet.Platform/Agroflet.Platform.csproj
COPY Agroflet.Platform/ Agroflet.Platform/
RUN dotnet publish Agroflet.Platform/Agroflet.Platform.csproj -c Release --no-restore -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
# Render inyecta PORT (10000 por defecto); se usa como puerto HTTP de Kestrel.
ENV PORT=10000
EXPOSE 10000
COPY --from=build /app/publish .
USER app
ENTRYPOINT ["sh", "-c", "export ASPNETCORE_HTTP_PORTS=\"${PORT:-10000}\" && exec dotnet Agroflet.Platform.dll"]
