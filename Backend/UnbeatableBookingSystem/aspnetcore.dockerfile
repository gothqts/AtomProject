FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN mkdir -p /https
RUN chmod -R 755 /https

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UnbeatableBookingSystem/UnbeatableBookingSystem.csproj", "UnbeatableBookingSystem/"]
COPY ["Booking.Application/Booking.Application.csproj", "Booking.Application/"]
COPY ["Booking.Core/Booking.Core.csproj", "Booking.Core/"]
COPY ["Booking.Infrastructure/Booking.Infrastructure.csproj", "Booking.Infrastructure/"]
RUN dotnet restore "UnbeatableBookingSystem/UnbeatableBookingSystem.csproj"
COPY . .
WORKDIR "/src/UnbeatableBookingSystem"
RUN dotnet build "UnbeatableBookingSystem.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UnbeatableBookingSystem.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UnbeatableBookingSystem.dll"]
