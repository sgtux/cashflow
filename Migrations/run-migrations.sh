#!/bin/bash
DATABASE_CONNECTION_STRING="Server=172.31.31.10,1433;Database=cashflow;Pooling=true;user=sa;Password=Mssql123" dotnet run --project Migrations/Migrations.csproj