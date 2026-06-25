cd "c:\Users\VA-11 HALL-A\Documents\Lab\CarDealership"
if (Test-Path "..\docker-compose.yml") {
    Move-Item "..\docker-compose.yml" . -Force
}

git init
Set-Content .gitignore -Value "bin/`nobj/`n.vs/`n*.user`nappsettings.Development.json" -Encoding UTF8

# Configure dummy git user just in case
git config user.email "student@university.edu"
git config user.name "Student"

git checkout -b stage1
git add docker-compose.yml .gitignore
git commit -m "Stage 1: Create database in PostgreSQL (Docker)"

git checkout -b stage2
git add CarDealershipMVC.sln DealershipDomain/ DealershipInfrastructure/DealershipInfrastructure.csproj DealershipInfrastructure/DealershipContext.cs DealershipInfrastructure/appsettings.json DealershipInfrastructure/Program.cs DealershipInfrastructure/Migrations/ DealershipInfrastructure/Properties/
git commit -m "Stage 2: Create new project and connect DB"

git checkout -b stage3
git add DealershipInfrastructure/Controllers/ DealershipInfrastructure/Views/ DealershipInfrastructure/wwwroot/
git commit -m "Stage 3: Setup controllers and views (Creative Theme)"

git checkout -b stage4
git add .
git commit -m "Stage 4: Advanced validation and UI improvements"

git checkout -b main
