# Estágio 1: Build da aplicação
# Usamos a imagem oficial do SDK do .NET 8 para compilar o projeto
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia o arquivo .csproj e restaura as dependências primeiro para otimizar o cache
COPY ["SnakeUtilities.csproj", "./"]
RUN dotnet restore "./SnakeUtilities.csproj"

# Copia o resto do código fonte
COPY . .

# Publica a aplicação em modo Release
RUN dotnet publish "SnakeUtilities.csproj" -c Release -o /app/publish

# Estágio 2: Imagem final para rodar a aplicação
# Usamos a imagem menor do ASP.NET runtime, que é otimizada para produção
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Define o ponto de entrada para rodar a aplicação quando o contêiner iniciar
ENTRYPOINT ["dotnet", "SnakeUtilities.dll"]