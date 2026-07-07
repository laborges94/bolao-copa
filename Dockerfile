# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Image for building the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Bolao/Bolao.csproj", "src/Bolao/"]
RUN dotnet restore "src/Bolao/Bolao.csproj"
COPY . .
WORKDIR "/src/src/Bolao"
RUN dotnet build "Bolao.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "Bolao.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bolao.dll"]
