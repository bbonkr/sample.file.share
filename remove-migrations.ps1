cd ./src/Sample.Data

dotnet ef migrations remove --context DefaultDbContext --startup-project ../Sample.App --project ../Sample.Data.SqlServer --json

cd ../../