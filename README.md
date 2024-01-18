Get list of migrations:
```
dotnet ef migrations list -p Infrastructure -c DataContext --configuration Release
```

Create SQL script to migrate from one migration to another:
```
dotnet ef migrations script 20231217065233_Appointment 20240105065006_CompanyData -p Infrastructure -c DataContext --configuration Release
```
