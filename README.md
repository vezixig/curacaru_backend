Get list of migrations:
```
dotnet ef migrations list -p Infrastructure -c DataContext --configuration Release
```

Create SQL script to migrate from one migration to another:
```
dotnet ef migrations script 20231217065233_Appointment 20240105065006_CompanyData -p Infrastructure -c DataContext --configuration Release
```


## Systemmd Service


```
[Unit]
Description=Curacaru .NET Backend

[Service]
WorkingDirectory=/var/net
ExecStart=/root/.dotnet/dotnet /var/net/Curacaru.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes
RestartSec=10
SyslogIdentifier=curacaru_backend
User=root
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DB_CONNECTION=Host=localhost;Port=5432;Username={{ POSTGRES USER }};Password={{ POSTGRES PASSWORD }};Database=Curacaru;IncludeE>
Environment=EMAIL_PASSWORD={{SMTP PASSWORD}}
Environment=EMAIL_SMTP=smtp.strato.de
Environment=EMAIL_PORT=587
Environment=EMAIL_USER={{ EMAIL USER }}
Environment=IDENTITY_AUTHORITY={{ AUTH0 AUTHORITY }}
Environment=IDENTITY_AUDIENCE={{ AUTH0 AUDIENCE }}
Environment=IDENTITY_SECRET={{ AUTH0 SECRET }}
Environment=IDENTITY_CLIENTID={{ AUTH0 CLIENTID }}

[Install]
WantedBy=multi-user.target
```
