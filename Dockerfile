# Estágio 1: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# CORREÇÃO: Copia o .csproj da subpasta para uma subpasta correspondente no contêiner
COPY ["SnakeUtilities/SnakeUtilities.csproj", "SnakeUtilities/"]
RUN dotnet restore "SnakeUtilities/SnakeUtilities.csproj"

# Copia o resto do código fonte para a raiz do WORKDIR (/src)
COPY . .

# Entra na pasta do projeto antes de fazer o build
WORKDIR "/src/SnakeUtilities"
RUN dotnet build "SnakeUtilities.csproj" -c Release -o /app/build

# Estágio 2: Publica a aplicação
FROM build AS publish
RUN dotnet publish "SnakeUtilities.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 3: Imagem final para rodar a aplicação
# Usamos a imagem menor do ASP.NET runtime, que é otimizada para produção
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SnakeUtilities.dll"]