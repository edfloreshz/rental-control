# Rent Control API - Go Version

A complete rewrite of the .NET Rent Control API in Go, featuring a clean architecture with proper separation of concerns.

## Architecture

- **Framework**: Gin (HTTP web framework)
- **Database**: PostgreSQL with GORM
- **Authentication**: JWT (ready for implementation)
- **Containerization**: Docker & Docker Compose
- **API Documentation**: Swagger/OpenAPI (ready for implementation)

## Project Structure

```
├── main.go                     # Application entry point
├── internal/
│   ├── config/                 # Configuration management
│   ├── database/               # Database initialization
│   ├── dto/                    # Data Transfer Objects
│   ├── handlers/               # HTTP handlers
│   ├── middleware/             # Custom middleware
│   ├── models/                 # Database models
│   └── services/               # Business logic
├── docker-compose.yml          # Docker services configuration
├── Dockerfile.go               # Go application Dockerfile
└── .env.example               # Environment variables example
```

## Features

### Core Entities
- **Addresses**: Manage property addresses
- **Tenants**: Handle tenant information
- **Guarantors**: Manage guarantor details
- **Contracts**: Handle rental contracts with relationships

### API Endpoints

#### Addresses
- `GET /api/v1/addresses` - List all addresses
- `GET /api/v1/addresses/{id}` - Get address by ID
- `POST /api/v1/addresses` - Create new address
- `PUT /api/v1/addresses/{id}` - Update address
- `DELETE /api/v1/addresses/{id}` - Delete address

#### Tenants
- `GET /api/v1/tenants` - List all tenants
- `GET /api/v1/tenants/{id}` - Get tenant by ID
- `POST /api/v1/tenants` - Create new tenant
- `PUT /api/v1/tenants/{id}` - Update tenant
- `DELETE /api/v1/tenants/{id}` - Delete tenant

#### Guarantors
- `GET /api/v1/guarantors` - List all guarantors
- `GET /api/v1/guarantors/{id}` - Get guarantor by ID
- `POST /api/v1/guarantors` - Create new guarantor
- `PUT /api/v1/guarantors/{id}` - Update guarantor
- `DELETE /api/v1/guarantors/{id}` - Delete guarantor

#### Contracts
- `GET /api/v1/contracts` - List all contracts
- `GET /api/v1/contracts/{id}` - Get contract by ID
- `POST /api/v1/contracts` - Create new contract
- `PUT /api/v1/contracts/{id}` - Update contract
- `DELETE /api/v1/contracts/{id}` - Delete contract

## Getting Started

### Prerequisites
- Go 1.21 or higher
- PostgreSQL 15+
- Docker & Docker Compose (optional)

### Local Development

1. **Clone and navigate to the project**:
   ```bash
   cd /home/eduardo/src/rent-control/api
   ```

2. **Install dependencies**:
   ```bash
   go mod download
   ```

3. **Set up environment variables**:
   ```bash
   cp .env.example .env
   # Edit .env with your database configuration
   ```

4. **Run the application**:
   ```bash
   go run main.go
   ```

### Docker Development

1. **Start services with Docker Compose**:
   ```bash
   docker-compose up -d
   ```

2. **View logs**:
   ```bash
   docker-compose logs -f api
   ```

3. **Stop services**:
   ```bash
   docker-compose down
   ```

## Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `PORT` | Server port | `8080` |
| `DATABASE_URL` | PostgreSQL connection string | `postgres://user:password@localhost:5432/rental_control?sslmode=disable` |
| `ENVIRONMENT` | Application environment | `development` |

## Database Schema

The application uses GORM for database operations with automatic migrations. The schema includes:

- **addresses**: Property addresses
- **tenants**: Tenant information with address references
- **guarantors**: Guarantor information with address references
- **contracts**: Rental contracts linking tenants, addresses, and guarantors
- **contract_guarantors**: Many-to-many relationship between contracts and guarantors

## API Usage Examples

### Create an Address
```bash
curl -X POST http://localhost:8080/api/v1/addresses \
  -H "Content-Type: application/json" \
  -d '{
    "street": "123 Main St",
    "number": "456",
    "neighborhood": "Downtown",
    "city": "New York",
    "state": "NY",
    "zip_code": "10001",
    "country": "USA"
  }'
```

### Create a Tenant
```bash
curl -X POST http://localhost:8080/api/v1/tenants \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Doe",
    "email": "john@example.com",
    "phone": "+1234567890",
    "address_id": "uuid-of-address"
  }'
```

### Create a Contract
```bash
curl -X POST http://localhost:8080/api/v1/contracts \
  -H "Content-Type: application/json" \
  -d '{
    "tenant_id": "uuid-of-tenant",
    "address_id": "uuid-of-address",
    "deposit": 1000.00,
    "rent": 1500.00,
    "business": "Residential Rental",
    "start_date": "2024-01-01T00:00:00Z",
    "end_date": "2024-12-31T23:59:59Z",
    "status": "active",
    "type": "yearly",
    "guarantor_ids": ["uuid-of-guarantor"]
  }'
```

## Key Improvements Over .NET Version

1. **Simplified Architecture**: Clean separation of concerns without over-abstraction
2. **Better Performance**: Go's built-in concurrency and efficiency
3. **Smaller Footprint**: Minimal dependencies and lightweight runtime
4. **Cloud-Native**: Optimized for containerization and microservices
5. **Developer Experience**: Simple, readable code with excellent tooling

## Next Steps

- [ ] Add JWT authentication middleware
- [ ] Implement OpenAPI/Swagger documentation
- [ ] Add comprehensive unit tests
- [ ] Implement logging with structured output
- [ ] Add health checks and metrics
- [ ] Implement graceful shutdown
- [ ] Add rate limiting
- [ ] Implement PDF generation for contracts

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## License

This project is licensed under the MIT License.
