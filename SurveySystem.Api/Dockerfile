## See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
## This stage is used when running from VS in fast mode (Default for Debug configuration)
#FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#USER $APP_UID
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#
## This stage is used to build the service project
#FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["SurveySystem.Api/SurveySystem.Api.csproj", "SurveySystem.Api/"]
#RUN dotnet restore "./SurveySystem.Api/SurveySystem.Api.csproj"
#COPY . .
#WORKDIR "/src/SurveySystem.Api"
#RUN dotnet build "./SurveySystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## This stage is used to publish the service project to be copied to the final stage
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./SurveySystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "SurveySystem.Api.dll"]


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/SurveySystem.Api/SurveySystem.Api.csproj", "src/SurveySystem.Api/"]
COPY ["src/SurveySystem.Application/SurveySystem.Application.csproj", "src/SurveySystem.Application/"]
COPY ["src/SurveySystem.Contracts/SurveySystem.Contracts.csproj", "src/SurveySystem.Contracts/"]
COPY ["src/SurveySystem.Domain/SurveySystem.Domain.csproj", "src/SurveySystem.Domain/"]
COPY ["src/SurveySystem.Infrastructure/SurveySystem.Infrastructure.csproj", "src/SurveySystem.Infrastructure/"]

COPY ["SurveySystem.sln", "."]
RUN dotnet restore "SurveySystem.sln"

COPY . .
WORKDIR "/src/src/SurveySystem.Api"
RUN dotnet build "SurveySystem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SurveySystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SurveySystem.Api.dll"]