###Run Coverlet
```
dotnet test --collect:"XPlat Code Coverage"
```
dotnet tool install --global dotnet-reportgenerator-globaltool --version 4.8.6

reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html

/home/tux/projects/Cashflow/Tests/TestResults/45d6791a-e538-4477-a138-b029ab33a7a6

dotnet test ${{ env.CAMINHO_PROJETO_TESTES }} --verbosity minimal --collect:"XPlat Code Coverage" -r ./coverage
