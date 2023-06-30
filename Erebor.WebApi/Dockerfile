#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./WebAPI ./

RUN dotnet restore "Erebor.WebApi/Erebor.WebApi.csproj"

WORKDIR "/src/Erebor.WebApi"
RUN dotnet build "Erebor.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Erebor.WebApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Erebor.WebApi.dll"]
