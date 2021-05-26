FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src
COPY ["./Cryptocop.Software.API/Cryptocop.Software.API.csproj", "Cryptocop.Software.API/Cryptocop.Software.API/"]
COPY ["Cryptocop.Software.API.Models/Cryptocop.Software.API.Models.csproj", "Cryptocop.Software.API/Cryptocop.Software.API.Models/"]
COPY ["Cryptocop.Software.API.Repositories/Cryptocop.Software.API.Repositories.csproj", "Cryptocop.Software.API/Cryptocop.Software.API.Repositories/"]
COPY ["Cryptocop.Software.API.Services/Cryptocop.Software.API.Services.csproj", "Cryptocop.Software.API/Cryptocop.Software.API.Services/"]


RUN dotnet restore "Cryptocop.Software.API\Cryptocop.Software.API\Cryptocop.Software.API.csproj"
COPY . .


WORKDIR "/src/Cryptocop.Software.API/Cryptocop.Software.API"


RUN dotnet build Cryptocop.Software.API.csproj -c Release -o /app
