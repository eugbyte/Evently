dev:
	dotnet run --project ./src/Evently.Server/Evently.Server.csproj --launch-profile https

test:
	dotnet test ./tests/Evently.Server.Test/VisualPatron.Server.Test.csproj
	
add-migration:
	dotnet ef migrations add RemoveLogoSrc --project=src/Evently.Server --context=AppDbContext --output-dir=Common/Adapters/Data/Migrations

update-migration:
	dotnet ef database update --project=src/Evently.Server --context=AppDbContext

remove-migration:
	dotnet ef migrations remove --project=src/Evently.Server --context=AppDbContext
	
fmt:
	dotnet tool restore
	jb cleanupcode ./src/Evently.Server/**/*
	cd src/evently.client && npm run fmt
    
    
docker:
	docker build --no-cache --progress=plain --tag evently:latest ./src/Evently.Server