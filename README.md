# 🍰 CupcakeShop.API - Backend .NET 8

API REST completa para gerenciamento de loja de cupcakes personalizados, com autenticação JWT, integração MySQL e CRUD completo de pedidos.

---

## 🚀 Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **Entity Framework Core 8.0** - ORM para MySQL
- **Pomelo.EntityFrameworkCore.MySql 8.0.0** - Provider MySQL
- **BCrypt.Net-Next 4.0.3** - Hash de senhas
- **JWT Bearer Authentication** - Autenticação segura
- **Swagger/OpenAPI** - Documentação automática

---

## 📁 Estrutura do Projeto

```
CupcakeShop.API/
├── Controllers/
│   ├── AuthController.cs         # Login e Registro
│   ├── CustomizationController.cs # Produtos (massas, coberturas, recheios)
│   └── OrdersController.cs        # Gerenciamento de pedidos
├── Models/
│   ├── User.cs                    # Usuário
│   ├── DoughType.cs               # Tipo de massa
│   ├── Frosting.cs                # Cobertura
│   ├── Filling.cs                 # Recheio
│   ├── Order.cs                   # Pedido
│   └── OrderItem.cs               # Item do pedido
├── DTOs/
│   └── Dtos.cs                    # Data Transfer Objects
├── Data/
│   └── CupcakeDbContext.cs        # Contexto do Entity Framework
├── Services/
│   ├── IAuthService.cs            # Interface do serviço de autenticação
│   └── AuthService.cs             # Implementação (JWT + BCrypt)
├── appsettings.json               # Configurações
└── Program.cs                     # Ponto de entrada e configuração
```

---

## 🗄️ Banco de Dados

### Schema MySQL

O banco possui 6 tabelas principais:

- **users** - Usuários do sistema
- **dough_types** - Tipos de massa disponíveis
- **frostings** - Coberturas disponíveis
- **fillings** - Recheios disponíveis
- **orders** - Pedidos dos clientes
- **order_items** - Itens de cada pedido

### Modelo do banco

Localização: EER MySQL

## ⚙️ Configuração

### 1. Instalar Dependências

```bash
dotnet restore
```

### 2. Configurar String de Conexão

Edite `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=cupcake_shop;User=root;Password=SUA_SENHA;"
  }
}
```

### 3. Configurar JWT (Opcional)

```json
{
  "JwtSettings": {
    "SecretKey": "sua-chave-secreta-minimo-32-caracteres",
    "Issuer": "CupcakeShop.API",
    "Audience": "CupcakeShop.Angular",
    "ExpirationHours": 24
  }
}
```

---

## ▶️ Executar a API

### Via .NET CLI

```bash
dotnet run
```

### Via Visual Studio

1. Abra `CupcakeShop.API.sln`
2. Pressione `F5`

### Acessar Swagger

```
https://localhost:7000/swagger
```

---

## 📡 Endpoints

### 🔓 Autenticação (Público)

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "joao@example.com",
  "password": "123456"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "userId": 1,
    "name": "João Silva",
    "email": "joao@example.com",
    "phone": "11987654321"
  }
}
```

#### Registro
```http
POST /api/auth/register
Content-Type: application/json

{
  "name": "Novo Usuario",
  "email": "novo@example.com",
  "password": "senha123",
  "phone": "11999999999"
}
```

---

### 🎨 Customização (Autenticado)

#### Listar Massas
```http
GET /api/customization/doughs
Authorization: Bearer {token}
```

**Resposta:**
```json
[
  {
    "doughId": 1,
    "name": "Massa de Baunilha",
    "description": "Massa tradicional com essência de baunilha",
    "price": 3.50,
    "isAvailable": true
  }
]
```

#### Listar Coberturas
```http
GET /api/customization/frostings
Authorization: Bearer {token}
```

#### Listar Recheios
```http
GET /api/customization/fillings
Authorization: Bearer {token}
```

---

### 📦 Pedidos (Autenticado)

#### Criar Pedido
```http
POST /api/orders
Authorization: Bearer {token}
Content-Type: application/json

{
  "deliveryAddress": "Rua Exemplo, 123, São Paulo - SP",
  "paymentMethod": "Cartão de Crédito",
  "items": [
    {
      "doughId": 1,
      "frostingId": 2,
      "fillingId": 3,
      "quantity": 6
    }
  ]
}
```

**Resposta:**
```json
{
  "orderId": 4,
  "message": "Pedido criado com sucesso"
}
```

#### Listar Meus Pedidos
```http
GET /api/orders
Authorization: Bearer {token}
```

**Resposta:**
```json
[
  {
    "orderId": 1,
    "userId": 1,
    "orderDate": "2024-01-15T10:30:00Z",
    "totalAmount": 42.00,
    "status": "Pendente",
    "deliveryAddress": "Rua A, 123",
    "paymentMethod": "Cartão de Crédito",
    "orderItems": [
      {
        "orderItemId": 1,
        "doughType": { "name": "Massa de Baunilha" },
        "frosting": { "name": "Cobertura de Chocolate" },
        "filling": { "name": "Doce de Leite" },
        "quantity": 6,
        "unitPrice": 7.00,
        "subtotal": 42.00
      }
    ]
  }
]
```

#### Detalhes do Pedido
```http
GET /api/orders/{id}
Authorization: Bearer {token}
```

#### Atualizar Status do Pedido
```http
PATCH /api/orders/{id}/status
Authorization: Bearer {token}
Content-Type: application/json

{
  "status": "Em produção"
}
```

**Status Possíveis:**
- `Pendente`
- `Em produção`
- `Pronto para retirada`
- `Saiu para entrega`
- `Entregue`
- `Cancelado`

---

## 🔒 Segurança

### Autenticação JWT

- **Token válido por:** 24 horas
- **Algoritmo:** HS256
- **Claims incluídas:**
  - `sub` (Subject): User ID
  - `email`: Email do usuário
  - `jti` (JWT ID): Identificador único do token
  - `iat` (Issued At): Data de emissão

### Senhas

- **Hash:** BCrypt
- **Work Factor:** 10 rounds
- **Salt:** Gerado automaticamente

### CORS

Configurado para aceitar apenas:
- **Origem:** `http://localhost:4200`
- **Métodos:** Todos
- **Headers:** Todos
- **Credenciais:** Permitidas

---

## 🧪 Testar com Swagger

1. Execute a API: `dotnet run`
2. Acesse: `https://localhost:7000/swagger`
3. Faça login em `/api/auth/login`
4. Copie o token retornado
5. Clique em "Authorize" (cadeado no topo)
6. Cole: `Bearer {seu-token}`
7. Teste os endpoints protegidos

---

## 📊 Dados de Teste

### Usuários (senha: 123456)

```
joao@example.com
maria@example.com
carlos@example.com
```

### Produtos Disponíveis

**Massas:** Baunilha, Chocolate, Red Velvet, Cenoura, Limão  
**Coberturas:** Chocolate, Morango, Baunilha, Cream Cheese, Caramelo  
**Recheios:** Doce de Leite, Brigadeiro, Ganache, Geleia de Morango, Creme de Limão

---

## 🛠️ Comandos Úteis

### Compilar
```bash
dotnet build
```

### Executar
```bash
dotnet run
```

### Limpar
```bash
dotnet clean
```

### Restaurar Pacotes
```bash
dotnet restore
```

### Migrations (EF Core)
```bash
# Criar migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations
dotnet ef database update

# Reverter migration
dotnet ef database update PreviousMigrationName
```

---

## 🐛 Troubleshooting

### Erro de Conexão MySQL

```
Unable to connect to any of the specified MySQL hosts
```

**Solução:**
1. Verifique se o MySQL está rodando
2. Confirme usuário e senha no `appsettings.json`
3. Teste a conexão: `mysql -u root -p`

### Erro de CORS

```
Access to XMLHttpRequest has been blocked by CORS policy
```

**Solução:**
1. Verifique se o Angular está em `http://localhost:4200`
2. Confirme a configuração no `Program.cs`
3. Reinicie a API

### Token JWT Inválido

```
401 Unauthorized
```

**Solução:**
1. Faça login novamente
2. Verifique se o token está no header: `Authorization: Bearer {token}`
3. Confirme que a SecretKey é igual em login e validação

---

## 📝 Licença

Projeto educacional - MIT License

---

## 

