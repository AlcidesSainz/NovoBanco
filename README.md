# NovoBanco API

API REST desarrollada en .NET 8 para la gestión de clientes, cuentas bancarias y transacciones (depósitos, retiros y transferencias), siguiendo principios de arquitectura limpia.

---

## Tecnologías utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server 2022
- Swagger (OpenAPI)

---

## Arquitectura

Se implementó una arquitectura por capas:

- Domain
- Application
- Infrastructure
- API

Principios aplicados:

- Separación de responsabilidades
- Inyección de dependencias
- Manejo centralizado de errores
- Persistencia desacoplada mediante repositorios

---

## Cómo ejecutar el proyecto

### 1. Clonar repositorio

git clone <https://github.com/AlcidesSainz/NovoBanco.git>
cd NovoBanco

### 2. Configurar base de datos

Editar appsettings.json:

"ConnectionStrings": {
"DefaultConnection": "Server=.;Database=NovoBancoDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

### 3. Aplicar migraciones

Update-Database

### 4. Ejecutar la API

dotnet run

Swagger disponible en:

https://localhost:xxxx/swagger

---

## Funcionalidades

### Clientes

- Crear cliente

### Cuentas

- Crear cuenta
- Obtener cuenta
- Bloquear cuenta

### Transacciones

- Depósito
- Retiro
- Transferencia

---

## Reglas de negocio

- Saldo no puede ser negativo
- Referencia única
- No transferir a misma cuenta
- Solo cuentas activas operan

---

## Manejo de transacciones

Se usa UnitOfWork para garantizar atomicidad y rollback.

---

## Concurrencia

Uso de RowVersion para evitar conflictos.

---

## Base de datos

Incluye:

- FK
- índices
- restricciones

Archivos:

- schema.sql
- schema.md

---

## Pruebas

- Depósitos
- Retiros
- Transferencias
- Validaciones

---

## Mejoras futuras

- JWT
- Logs

---

## Conclusión

Solución consistente, segura y organizada.
