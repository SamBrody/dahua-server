FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Dahua.Server/Dahua.Server.csproj", "Dahua.Server/"]
COPY ["NetSDKCS/NetSDKCS.csproj", "NetSDKCS/"]
RUN dotnet restore "Dahua.Server/Dahua.Server.csproj"
COPY . .
WORKDIR "/src/Dahua.Server"
RUN dotnet build "Dahua.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Dahua.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dahua.Server.dll"]
