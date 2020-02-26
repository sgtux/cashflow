###Run Coverlet
```
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover Tests/Cashflow.Tests.csproj
```

psql -U cashflow -h 172.17.0.2 --set ON_ERROR_STOP=on cashflow < Migrations/202002220330.CreateTables.sql  
