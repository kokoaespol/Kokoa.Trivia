FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY Kokoa.Trivia.Api.csproj .
RUN dotnet restore Kokoa.Trivia.Api.csproj

COPY . .
RUN dotnet build Kokoa.Trivia.Api.csproj -c Release -o /app/build

FROM build AS publish
WORKDIR /src
RUN dotnet publish Kokoa.Trivia.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

ENV PORT=5000
EXPOSE ${PORT}

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Kokoa.Trivia.Api.dll"]
