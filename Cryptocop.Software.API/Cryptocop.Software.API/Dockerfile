FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Cryptocop.Software.API/Cryptocop.Software.API/Cryptocop.Software.API.csproj", "Cryptocop.Software.API/Cryptocop.Software.API/"]
RUN dotnet restore "Cryptocop.Software.API\Cryptocop.Software.API\Cryptocop.Software.API.csproj"
COPY . .
WORKDIR "/src/Cryptocop.Software.API/Cryptocop.Software.API"
RUN dotnet build "Cryptocop.Software.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cryptocop.Software.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cryptocop.Software.API.dll"]
