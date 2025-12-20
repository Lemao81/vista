# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG DOTNET_VERSION=9.0
# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

RUN apt-get update
RUN apt-get install -y curl
HEALTHCHECK --interval=30s --timeout=10s --start-period=20s --start-interval=2s --retries=10 CMD curl -f http://localhost:8080/health || exit 1

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["src/Directory.Build.props", "src/"]
COPY ["Directory.Build.props", "."]
COPY ["src/Services/Maintenance/WebApi/WebApi.csproj", "src/Services/Maintenance/WebApi/"]
COPY ["src/Services/Common/Azure/Azure.csproj", "src/Services/Common/Azure/"]
COPY ["src/Services/Common/Application/Application.csproj", "src/Services/Common/Application/"]
COPY ["src/Services/SharedKernel/SharedKernel.csproj", "src/Services/SharedKernel/"]
COPY ["src/Services/Common/Domain/Domain.csproj", "src/Services/Common/Domain/"]
COPY ["src/Services/Common/Persistence/Persistence.csproj", "src/Services/Common/Persistence/"]
COPY ["src/Services/Common/WebApi/WebApi.csproj", "src/Services/Common/WebApi/"]
RUN dotnet restore "./src/Services/Maintenance/WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/src/Services/Maintenance/WebApi"
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG COMPILER_CONSTANTS=NONE
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false /p:ExtraDefineConstants=$COMPILER_CONSTANTS

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Maintenance.WebApi.dll"]