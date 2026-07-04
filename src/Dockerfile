FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia todos os csproj
COPY ["AutoReparaAPI/AutoReparaAPI.csproj", "AutoReparaAPI/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infra/Infra.csproj", "Infra/"]
COPY ["IOC/IOC.csproj", "IOC/"]

# Copia também a solução
COPY ["GestaoAutoRepara.slnx", "./"]

# Restaura usando a solução
RUN dotnet restore "AutoReparaAPI/AutoReparaAPI.csproj"

# Copia o restante do código
COPY . .
WORKDIR /src/AutoReparaAPI
RUN dotnet publish "AutoReparaAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "AutoReparaAPI.dll"]
