FROM microsoft/dotnet:runtime AS base
WORKDIR /app

FROM microsoft/dotnet:sdk AS build
WORKDIR /src
COPY [".", "Api/"]
RUN dotnet restore "Api/Api.csproj"
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Api.dll"]