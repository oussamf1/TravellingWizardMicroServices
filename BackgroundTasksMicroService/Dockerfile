# Use the .NET 7.0 SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the project files and restore dependencies
COPY ["BackgroundTasksMicroService/BackgroundTasksMicroService.csproj", "BackgroundTasksMicroService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "BackgroundTasksMicroService/BackgroundTasksMicroService.csproj"



# Copy the source code
COPY . .

# Build the application in Release mode
WORKDIR "/src/BackgroundTasksMicroService"
RUN dotnet build "BackgroundTasksMicroService.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "BackgroundTasksMicroService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the .NET 7.0 runtime image as the final image
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS final
WORKDIR /app


# Copy the published application from the publish stage
COPY --from=publish /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "BackgroundTasksMicroService.dll"]
