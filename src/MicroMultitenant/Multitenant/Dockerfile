#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MicroMultitenant/Multitenant/Multitenant.csproj", "src/MicroMultitenant/Multitenant/"]
COPY ["src/SrcBase/Base.Api/Base.Api.csproj", "src/SrcBase/Base.Api/"]
COPY ["src/SrcBase/Base.Infraestructure/Base.Infraestructure.csproj", "src/SrcBase/Base.Infraestructure/"]
COPY ["src/SrcBase/Base.Application/Base.Application.csproj", "src/SrcBase/Base.Application/"]
COPY ["src/SrcBase/Base.Domain/Base.Domain.csproj", "src/SrcBase/Base.Domain/"]
COPY ["src/MicroMultitenant/Multitenant.Infraestructure/Multitenant.Infraestructure.csproj", "src/MicroMultitenant/Multitenant.Infraestructure/"]
COPY ["src/MicroMultitenant/Multitenant.Application/Multitenant.Application.csproj", "src/MicroMultitenant/Multitenant.Application/"]
COPY ["src/MicroMultitenant/Multitenant.Domain/Multitenant.Domain.csproj", "src/MicroMultitenant/Multitenant.Domain/"]
RUN dotnet restore "./src/MicroMultitenant/Multitenant/./Multitenant.csproj"
COPY . .
WORKDIR "/src/src/MicroMultitenant/Multitenant"
RUN dotnet build "./Multitenant.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Multitenant.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Multitenant.dll"]