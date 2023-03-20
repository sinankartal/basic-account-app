# Set the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-buster-slim AS base
WORKDIR /app
EXPOSE 7093

# Set the build image
FROM mcr.microsoft.com/dotnet/sdk:6.0-buster AS build
WORKDIR /src
COPY ["./src", "src/"]
RUN dotnet restore "src/API/API.csproj"
COPY ./src .
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

# Set the final image
FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
