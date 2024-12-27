FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/FileTransfer/WebApi/WebApi.csproj", "src/Services/FileTransfer/WebApi/"]
COPY ["src/Services/FileTransfer/Application/Application.csproj", "src/Services/FileTransfer/Application/"]
COPY ["src/Services/FileTransfer/Domain/Domain.csproj", "src/Services/FileTransfer/Domain/"]
COPY ["src/Services/Common/Domain/Domain.csproj", "src/Services/Common/Domain/"]
COPY ["src/Services/FileTransfer/Infrastructure/Infrastructure.csproj", "src/Services/FileTransfer/Infrastructure/"]
COPY ["src/Services/FileTransfer/Presentation/Presentation.csproj", "src/Services/FileTransfer/Presentation/"]
COPY ["src/Services/FileTransfer/Persistence/Persistence.csproj", "src/Services/FileTransfer/Persistence/"]
RUN dotnet restore "src/Services/FileTransfer/WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/src/Services/FileTransfer/WebApi"
RUN dotnet build "WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileTransfer.WebApi.dll"]
