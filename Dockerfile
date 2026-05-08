FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["BookHaven.csproj", "./"]
RUN dotnet restore "BookHaven.csproj"

COPY . .
RUN dotnet build "BookHaven.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookHaven.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookHaven.dll"]
