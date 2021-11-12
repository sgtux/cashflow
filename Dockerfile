FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY Api /source
WORKDIR /source/
RUN dotnet publish -c release -o /app

FROM alpine AS site
COPY Site /source
WORKDIR /source/
RUN apk add nodejs yarn && \
    yarn && \
    yarn build:prod

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
COPY --from=site /Api/wwwroot ./wwwroot
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Cashflow.Api.dll