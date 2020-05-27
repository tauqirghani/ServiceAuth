FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY ["APIProject/APIProject.csproj", "APIProject/"]

RUN dotnet restore "APIProject/APIProject.csproj"
COPY . .
WORKDIR "/src/APIProject"
RUN dotnet build "APIProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "APIProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIProject.dll"]