# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG DOTNET_VERSION=9.0
# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

RUN apt-get update
RUN apt-get install -y curl
HEALTHCHECK --interval=30s --timeout=10s --start-period=10s --start-interval=2s --retries=5 CMD curl -f http://localhost:8080/health || exit 1

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
COPY ["src/Services/Gateway/WebApi/WebApi.csproj", "src/Services/Gateway/WebApi/"]
COPY ["src/Services/Common/Infrastructure/Infrastructure.csproj", "src/Services/Common/Infrastructure/"]
COPY ["src/Services/Common/Application/Application.csproj", "src/Services/Common/Application/"]
COPY ["src/Services/SharedKernel/SharedKernel.csproj", "src/Services/SharedKernel/"]
COPY ["src/Services/Common/Domain/Domain.csproj", "src/Services/Common/Domain/"]
COPY ["src/Services/Common/WebApi/WebApi.csproj", "src/Services/Common/WebApi/"]
COPY ["src/Services/Common/Presentation/Presentation.csproj", "src/Services/Common/Presentation/"]
RUN dotnet restore "./src/Services/Gateway/WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/src/Services/Gateway/WebApi"
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
ENTRYPOINT ["dotnet", "Gateway.WebApi.dll"]