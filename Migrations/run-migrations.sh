#!/bin/bash
DATABASE_URL="Host=172.31.31.10;Database=cashflow;Port=3306;Pooling=true;user=root;Password=Mysql123" dotnet run --project Migrations/Migrations.csproj