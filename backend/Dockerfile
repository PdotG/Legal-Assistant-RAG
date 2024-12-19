FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /app

# Copy file project dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the source code
COPY . ./

# Build the app using dotnet
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set working directory
WORKDIR /app

# We copy the build to the container
COPY --from=build /app/out .

# Expose the port the application runs on
EXPOSE 3000

# We launch the app
ENTRYPOINT ["dotnet", "backend.dll"]