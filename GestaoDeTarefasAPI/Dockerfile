# Est�gio 1: Build (para compilar o projeto)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copia o arquivo .csproj primeiro para permitir o cache do restore do NuGet.
# Agora, como o Dockerfile est� dentro da pasta do projeto,
# o .csproj � referenciado diretamente.
COPY GestaoDeTarefasAPI.csproj ./

# Restaura as depend�ncias do NuGet
RUN dotnet restore GestaoDeTarefasAPI.csproj

# Copia todo o restante dos arquivos do projeto para o WORKDIR.
# '.' aqui refere-se � pasta atual do contexto de build (GestaoDeTarefasAPI/).
COPY . ./

# Constr�i o projeto
RUN dotnet build GestaoDeTarefasAPI.csproj -c Debug -o /app/build

# Est�gio 2: Publica��o (para criar a vers�o "pronta para rodar")
FROM build-env AS publish

# Publica a aplica��o
RUN dotnet publish GestaoDeTarefasAPI.csproj -c Debug -o /app/publish /p:UseAppHost=false

# Est�gio 3: Runtime (a imagem final e leve que ser� executada)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia os arquivos publicados do est�gio de publica��o para a imagem final
COPY --from=publish /app/publish .

# Define as portas que a aplica��o vai expor.
# Essas portas devem corresponder aos mapeamentos no docker-compose.yml e ASPNETCORE_URLS.
EXPOSE 8080
EXPOSE 8081

# Define o ponto de entrada da aplica��o quando o cont�iner for iniciado
ENTRYPOINT ["dotnet", "GestaoDeTarefasAPI.dll"]