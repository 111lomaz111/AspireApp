FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["AspireApp.Web/AspireApp.Web.csproj", "AspireApp.Web/"]
COPY ["AspireApp.ServiceDefaults/AspireApp.ServiceDefaults.csproj", "AspireApp.ServiceDefaults/"]

RUN dotnet restore "AspireApp.Web/AspireApp.Web.csproj"

COPY . .

WORKDIR "/src/AspireApp.Web"
RUN dotnet publish "AspireApp.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AspireApp.Web.dll"]