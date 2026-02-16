# Event Manager - Backend

.NET 8 Web API com Entity Framework Core, SQL Server e JWT Authentication.

## Estrutura

```
src/
├── API/                    # Controllers
├── Application/            # Services, DTOs
├── Domain/                 # Entities, Interfaces
├── Infrastructure/         # Database, Repositories
├── Migrations/             # EF Core Migrations
├── Tests/                  # Testes unitários
└── Program.cs             # Configuração
```

## Setup

1. Restaurar dependências:
```bash
dotnet restore
```

2. Configurar `appsettings.json`:

⚠️ **IMPORTANTE**: O arquivo `appsettings.json` não está no repositório por segurança.

Copie o arquivo de exemplo:
```bash
cp appsettings.example.json appsettings.json
```

Edite `appsettings.json` com suas credenciais:
```json
{
  "AzureAd": {
    "ClientId": "SEU_AZURE_CLIENT_ID",
    "ClientSecret": "SEU_AZURE_CLIENT_SECRET",
    "TenantId": "SEU_AZURE_TENANT_ID"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=EventManagerDb;Trusted_Connection=true;"
  },
  "Jwt": {
    "Secret": "sua-chave-secreta-com-pelo-menos-32-caracteres"
  }
}
```

3. Aplicar migrations:
```bash
dotnet ef database update
```

4. Iniciar a aplicação:
```bash
dotnet run
```

## Endpoints

### Autenticação
- `POST /api/auth/login` - Login

### Usuários
- `GET /api/users` - Listar todos
- `GET /api/users/{id}` - Por ID
- `GET /api/users/{id}/events` - Com eventos

### Eventos
- `GET /api/events/user/{userId}` - Por usuário
- `GET /api/events/{id}` - Por ID

### Sincronização
- `POST /api/sync/users` - Sincronizar usuários
- `POST /api/sync/events` - Sincronizar eventos
- `POST /api/sync/schedule` - Agendar jobs

## Testes

```bash
dotnet test
```

## Documentação

Swagger disponível em: `https://localhost:56057/swagger`

## Recursos

- ✅ JWT Bearer Authentication
- ✅ Entity Framework Core 8
- ✅ SQL Server
- ✅ Microsoft Graph Integration
- ✅ Hangfire Background Jobs
- ✅ Testes unitários xUnit
- ✅ Swagger/OpenAPI
