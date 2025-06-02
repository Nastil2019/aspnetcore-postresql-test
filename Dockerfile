# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем только *.csproj для кэширования restore
COPY *.csproj ./
RUN dotnet restore

# Копируем остальные файлы и собираем
COPY . ./
RUN dotnet publish dockerapi.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
RUN mkdir -p /app/wwwroot
WORKDIR /app

# Добавляем утилиты для дебага
RUN apt update && \
    apt install -y --no-install-recommends \
        procps \
        iproute2 \
        net-tools \
        curl \
        bash \
        netcat-openbsd\
        telnet \
        dnsutils\
        postgresql-client\
        mc && \
    rm -rf /var/lib/apt/lists/*

# Копируем опубликованные файлы
COPY --from=build /app/publish .

# Запуск приложения с ожиданием БД
#ENTRYPOINT ["/app/wait-for-it.sh", "postgres_image", "5432", "60", "--", "dotnet", "dockerapi.dll"]
ENTRYPOINT ["dotnet", "dockerapi.dll"]