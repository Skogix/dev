FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UI/UI.fsproj", "UI/"]
RUN dotnet restore "UI/UI.fsproj"
COPY . .
WORKDIR "/src/UI"
RUN dotnet build "UI.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UI.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UI.dll"]
