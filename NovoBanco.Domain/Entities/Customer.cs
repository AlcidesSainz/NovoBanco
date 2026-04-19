namespace NovoBanco.Domain.Entities;

/// <summary>
/// Entidad que representa un cliente bancario, con propiedades como el nombre completo, 
/// número de documento y una colección de cuentas asociadas.
/// </summary>
public class Customer
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}