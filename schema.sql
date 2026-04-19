Esquema de Base de Datos

- 1. SGBD seleccionado

**Motor:** SQL Server 2022  
**Version usada:** SQL Server compatible con EF Core 8

## 2. Justificacion tecnica

Se eligio SQL Server 2022 por:

1. Soporte ACID para transacciones seguras  
2. Manejo de concurrencia con rowversion  
3. Modelo relacional adecuado  
4. Optimizacion con indices  

## 3. Modelo de datos

### Tablas

- Clientes  
- Cuentas  
- Transacciones  

### Clientes

| Columna | Tipo | Restricciones |
|--------|------|---------------|
| Id | uniqueidentifier | PK |
| NombreCompleto | nvarchar(200) | NOT NULL |
| NumeroDocumento | nvarchar(50) | UNIQUE |

### Cuentas

| Columna | Tipo | Restricciones |
|--------|------|---------------|
| Id | uniqueidentifier | PK |
| ClienteId | uniqueidentifier | FK |
| NumeroCuenta | nvarchar(20) | UNIQUE |
| Tipo | int | NOT NULL |
| Moneda | nvarchar(3) | NOT NULL |
| Saldo | decimal(18,2) | >= 0 |
| Estado | int | NOT NULL |
| FechaCreacion | datetime2 | NOT NULL |
| RowVersion | rowversion | concurrency |

### Transacciones

| Columna | Tipo | Restricciones |
|--------|------|---------------|
| Id | uniqueidentifier | PK |
| CuentaId | uniqueidentifier | FK |
| CuentaDestinoId | uniqueidentifier | NULL |
| Monto | decimal(18,2) | NOT NULL |
| Tipo | int | NOT NULL |
| Referencia | nvarchar(50) | UNIQUE |
| Estado | int | NOT NULL |
| FechaCreacion | datetime2 | NOT NULL |


## 4. Relaciones

- Cliente 1:N Cuentas  
- Cuenta 1:N Transacciones  

---

## 5. Decisiones

### 5.1 Normalizacion
Evita duplicacion y mantiene integridad.

### 5.2 Saldo
Se guarda en Cuentas para eficiencia.

### 5.3 Restriccion

```sql
CHECK (Saldo >= 0)
```

### 5.4 Indices

- Documento unico  
- Cuenta unica  
- Referencia unica  
- Cuenta + Fecha  

### 5.5 Historial

(CuentaId, FechaCreacion DESC)

### 5.6 Concurrencia

Se usa RowVersion.

## 6. Scripts DDL

```sql
CREATE TABLE Clientes (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    NombreCompleto NVARCHAR(200) NOT NULL,
    NumeroDocumento NVARCHAR(50) UNIQUE
);

CREATE TABLE Cuentas (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ClienteId UNIQUEIDENTIFIER,
    NumeroCuenta NVARCHAR(20) UNIQUE,
    Tipo INT,
    Moneda NVARCHAR(3),
    Saldo DECIMAL(18,2),
    Estado INT,
    FechaCreacion DATETIME2,
    RowVersion ROWVERSION,
    CONSTRAINT FK_Cuentas FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
    CONSTRAINT CK_Saldo CHECK (Saldo >= 0)
);

CREATE TABLE Transacciones (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    CuentaId UNIQUEIDENTIFIER,
    CuentaDestinoId UNIQUEIDENTIFIER,
    Monto DECIMAL(18,2),
    Tipo INT,
    Referencia NVARCHAR(50) UNIQUE,
    Estado INT,
    FechaCreacion DATETIME2,
    CONSTRAINT FK_Transacciones FOREIGN KEY (CuentaId) REFERENCES Cuentas(Id)
);

CREATE INDEX IX_Cuentas_ClienteId ON Cuentas(ClienteId);
CREATE INDEX IX_Transacciones_Cuenta_Fecha ON Transacciones(CuentaId, FechaCreacion DESC);
```

## 7. Enums

Cuentas.Tipo  
1 Savings  
2 Checking  

Cuentas.Estado  
1 Active  
2 Blocked  
3 Closed  

Transacciones.Tipo  
1 Deposit  
2 Withdrawal  
3 Transfer  

## 8. Consultas

Saldo:

```sql
SELECT Saldo FROM Cuentas WHERE Id = @Id;
```

Movimientos:

```sql
SELECT TOP 20 * 
FROM Transacciones 
WHERE CuentaId = @Id 
ORDER BY FechaCreacion DESC;
```

Referencia:

```sql
SELECT 1 FROM Transacciones WHERE Referencia = @Ref;
```

## 9. Futuro

- Particionamiento de datos  
- Optimizacion de indices  
- Archivado de historial  

## 10. Conclusion

El modelo garantiza integridad, consistencia y buen rendimiento para operaciones bancarias.
