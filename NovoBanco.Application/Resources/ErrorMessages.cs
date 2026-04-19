using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovoBanco.Application.Resources
{
    /// <summary>
    /// Clase estática que contiene mensajes de error comunes utilizados en la aplicación.
    /// </summary>
    public static class ErrorMessages
    {
        public const string Account_NotFound = "La cuenta no fue encontrada.";
        public const string Customer_NotFound = "El cliente no fue encontrado.";
        public const string Insufficient_Funds = "La cuenta no tiene saldo suficiente.";
        public const string Account_Inactive = "La cuenta no esta activa.";
        public const string Duplicate_Transaction = "Ya existe una transaccion con la misma referencia.";
        public const string Invalid_Amount = "El monto debe ser mayor a cero.";
        public const string Same_Account_Transfer = "La cuenta origen y destino deben ser diferentes.";
        public const string Source_Account_NotFound = "La cuenta de origen no fue encontrada.";
        public const string Destination_Account_NotFound = "La cuenta de destino no fue encontrada.";
    }
}
