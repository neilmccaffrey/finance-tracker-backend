# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# Set the working directory
WORKDIR /app

#Install curl
RUN apt-get update && apt-get install -y curl

# Copy the built app from the build stage
COPY --from=build /app/out .

# Expose the port the app will run on
# EXPOSE 10000

# Start the application
ENTRYPOINT ["dotnet", "finance-tracker-backend.dll"]