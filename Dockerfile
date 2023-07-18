FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./ams/ams.csproj" --disable-parallel
RUN dotnet publish "./ams/ams.csproj" -c release -o /app --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5011
ENV ASPNETCORE_URLS=http://+:5011

ENTRYPOINT ["dotnet", "ams.dll"]
