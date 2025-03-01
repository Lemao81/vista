# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG DOTNET_VERSION=9.0
# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["src/Services/FileTransfer/Directory.Build.props", "src/Services/FileTransfer/"]
COPY ["Directory.Build.props", "."]
COPY ["src/Services/FileTransfer/WebApi/WebApi.csproj", "src/Services/FileTransfer/WebApi/"]
COPY ["src/Services/Common/WebApi/WebApi.csproj", "src/Services/Common/WebApi/"]
COPY ["src/Services/FileTransfer/Application/Application.csproj", "src/Services/FileTransfer/Application/"]
COPY ["src/Services/Common/Application/Application.csproj", "src/Services/Common/Application/"]
COPY ["src/Services/SharedKernel/SharedKernel.csproj", "src/Services/SharedKernel/"]
COPY ["src/Services/Common/Domain/Domain.csproj", "src/Services/Common/Domain/"]
COPY ["src/Services/FileTransfer/Domain/Domain.csproj", "src/Services/FileTransfer/Domain/"]
COPY ["src/Services/FileTransfer/Infrastructure/Infrastructure.csproj", "src/Services/FileTransfer/Infrastructure/"]
COPY ["src/Services/FileTransfer/Persistence/Persistence.csproj", "src/Services/FileTransfer/Persistence/"]
COPY ["src/Services/Common/Persistence/Persistence.csproj", "src/Services/Common/Persistence/"]
COPY ["src/Services/FileTransfer/Presentation/Presentation.csproj", "src/Services/FileTransfer/Presentation/"]
RUN dotnet restore "./src/Services/FileTransfer/WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/src/Services/FileTransfer/WebApi"
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileTransfer.WebApi.dll"]