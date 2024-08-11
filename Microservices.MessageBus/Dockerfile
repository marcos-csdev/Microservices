FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5088

ENV ASPNETCORE_URLS=http://+:5088

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Microservices.MessageBus/Microservices.MessageBus.csproj", "Microservices.MessageBus/"]
RUN dotnet restore "Microservices.MessageBus/Microservices.MessageBus.csproj"
COPY . .
WORKDIR "/src/Microservices.MessageBus"
RUN dotnet build "Microservices.MessageBus.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Microservices.MessageBus.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.MessageBus.dll"]
