#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["devfestcertapi.csproj", ""]
RUN dotnet restore "./devfestcertapi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "devfestcertapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "devfestcertapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "devfestcertapi.dll"]