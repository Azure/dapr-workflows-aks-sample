# Use Microsoft's official build .NET image.
# https://hub.docker.com/_/microsoft-dotnet-core-sdk/
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /app

# Install production dependencies.
# Copy csproj and restore as distinct layers.
COPY *.csproj ./
RUN dotnet restore

# Copy local code to the container image.
COPY . ./
WORKDIR /app

# Build a release artifact.
RUN dotnet publish -c Release -o out

# Use Microsoft's official runtime .NET image.
# https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine-amd64 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Make sure the app binds to port 5064
ENV ASPNETCORE_URLS http://*:5064

# Run the web service on container startup.
ENTRYPOINT ["dotnet", "WorkflowSample.dll"]