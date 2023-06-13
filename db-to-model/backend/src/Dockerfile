FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# copy all the layers' csproj files into respective folders
COPY ["./Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["./Data/Data.csproj", "src/Data/"]
COPY ["./Core/Core.csproj", "src/Core/"]
COPY ["./Api/Api.csproj", "src/Api/"]

# run restore over API project - this pulls restore over the dependent projects as well
RUN dotnet restore "src/Api/Api.csproj"

COPY . .

# run build over the API project
WORKDIR "/src/Api/"
RUN dotnet build -c Release -o /app/build

# run publish over the API project
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l
ENTRYPOINT [ "dotnet", "Api.dll" ]