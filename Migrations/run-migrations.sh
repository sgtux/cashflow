#!/bin/bash
DATABASE_URL="Host=172.30.30.10;Port=5432;Pooling=true;User Id=postgres;Password=Postgres123" dotnet run --project Migrations/Migrations.csproj