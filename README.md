Get list of migrations:
```
dotnet ef migrations list -p Infrastructure -c DataContext --configuration Release
```

Create SQL script to migrate from one migration to another:
```
dotnet ef migrations script 20231217065233_Appointment 20240105065006_CompanyData -p Infrastructure -c DataContext --configuration Release
```


## Systemmd Services

### Backend

```
[Unit]
Description=Curacaru .NET Backend

[Service]
WorkingDirectory=/var/net
ExecStart=/root/.dotnet/dotnet /var/net/Curacaru.dll
Restart=always
RestartSec=10
SyslogIdentifier=curacaru_backend
User=root
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DB_CONNECTION=Host=localhost;Port=5432;Username={{ POSTGRE USER }};Password={{ POSTGRE PASSWORD }};Database=Curacaru;IncludeErrorDetail=true
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

### Budget Service
```
[Unit]
Description=Curacaru Service - Budget Replenisher
After=network.target

[Service]
WorkingDirectory=/var/net/services/BudgetReplenisher/
ExecStart=/root/.dotnet/dotnet /var/net/services/BudgetReplenisher/BudgetReplenisher.dll
Restart=always
RestartSec=10
Environment=ASPNETCORE_ENVIRONMENT=Production
nvironment=DB_CONNECTION=Host=localhost;Port=5432;Username={{ POSTGRE USER }};Password={{ POSTGRE PASSWORD }};Database=Curacaru;IncludeErrorDetail=true

[Install]
WantedBy=multi-user.target
```

systemmd commands

```
sudo systemctl daemon-reload
sudo systemctl enable your-service.service
sudo systemctl start your-service.service
journalctl -u your-service.service
```
