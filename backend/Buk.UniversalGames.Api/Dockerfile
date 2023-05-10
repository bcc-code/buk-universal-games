FROM mcr.microsoft.com/dotnet/aspnet:7.0-jammy AS base
WORKDIR /app
EXPOSE 5125

ENV ASPNETCORE_URLS=http://+:5125

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build
WORKDIR /src
COPY ["backend/Buk.UniversalGames.Api/Buk.UniversalGames.Api.csproj", "backend/Buk.UniversalGames.Api/"]
RUN dotnet restore "backend/Buk.UniversalGames.Api/Buk.UniversalGames.Api.csproj"
COPY . .
WORKDIR "/src/backend/Buk.UniversalGames.Api"
RUN dotnet build "Buk.UniversalGames.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Buk.UniversalGames.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
USER 0
# Install the agent
# RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
# && echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
# && wget https://download.newrelic.com/548C16BF.gpg \
# && apt-key add 548C16BF.gpg \
# && apt-get update \
# && apt-get install -y newrelic-dotnet-agent

# # Enable the agent
# ENV CORECLR_ENABLE_PROFILING=1 \
# CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
# CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
# CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
# NEW_RELIC_LICENSE_KEY=<new_relic_licence_key> \
# NEW_RELIC_APP_NAME=buk-universal-games-local

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Buk.UniversalGames.Api.dll"]
# ENTRYPOINT [ "dotnet", "watch", "run", "--no-restore", "--urls", "http://*:5125" ]