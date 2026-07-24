#!/bin/bash
docker exec -it cashflow-db-1 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Mssql123' -C -Q "IF DB_ID('cashflow') IS NULL BEGIN CREATE DATABASE [cashflow]; END"
DATABASE_CONNECTION_STRING="Server=172.31.31.10,1433;Database=cashflow;Pooling=true;user=sa;Password=Mssql123;TrustServerCertificate=True;" dotnet run --project Migrations/Migrations.csproj