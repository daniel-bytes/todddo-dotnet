FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY Todddo/Todddo.csproj Todddo/
RUN dotnet restore "Todddo/Todddo.csproj"
COPY . .
WORKDIR "/src/Todddo"
RUN dotnet build "Todddo.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Todddo.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Todddo.dll"]
