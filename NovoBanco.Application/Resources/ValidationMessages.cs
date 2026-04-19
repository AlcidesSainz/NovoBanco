using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovoBanco.Application.Resources
{
    public static class ValidationMessages
    {
        public const string FullName_Required = "El nombre completo es obligatorio.";
        public const string DocumentNumber_Required = "El número de documento es obligatorio.";
        public const string Existing_Customer = "Ya existe un cliente con el mismo número de documento.";
        public const string AccountNumber_Required = "El número de cuenta es obligatorio.";
        public const string Amount_Must_Be_Greater_Than_Zero = "El monto debe ser mayor a cero.";
        public const string Transaction_Same_Reference = "Ya existe una transacción con la misma referencia.";
        public const string Same_Account_Transfer = "La cuenta origen y destino deben ser diferentes.";
    }
}
