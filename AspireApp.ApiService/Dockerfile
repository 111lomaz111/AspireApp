FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["AspireApp.ApiService/AspireApp.ApiService.csproj", "AspireApp.ApiService/"]
COPY ["AspireApp.ServiceDefaults/AspireApp.ServiceDefaults.csproj", "AspireApp.ServiceDefaults/"]

RUN dotnet restore "AspireApp.ApiService/AspireApp.ApiService.csproj"

COPY . .

WORKDIR "/src/AspireApp.ApiService"
RUN dotnet publish "AspireApp.ApiService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AspireApp.ApiService.dll"]