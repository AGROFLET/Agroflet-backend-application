FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY global.json ./
COPY Agroflet.Platform/Agroflet.Platform.csproj Agroflet.Platform/
RUN dotnet restore Agroflet.Platform/Agroflet.Platform.csproj
COPY Agroflet.Platform/ Agroflet.Platform/
RUN dotnet publish Agroflet.Platform/Agroflet.Platform.csproj -c Release --no-restore -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=10000
EXPOSE 10000
COPY --from=build /app/publish .
USER app
ENTRYPOINT ["dotnet", "Agroflet.Platform.dll"]
