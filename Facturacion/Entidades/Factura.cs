using System;

namespace Entidades
{
    public class Factura
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string IdentidadCliente { get; set; }

        public string CodigoUsuario { get; set; }

        public decimal ISV { get; set; }

        public decimal Descuento { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public Factura()
        {
        }

        public Factura(int id, DateTime fecha, string identidadCliente, string codigoUsuario, decimal iSV, decimal descuento, decimal subTotal, decimal total)
        {
            Id = id;
            Fecha = fecha;
            IdentidadCliente = identidadCliente;
            CodigoUsuario = codigoUsuario;
            ISV = iSV;
            Descuento = descuento;
            SubTotal = subTotal;
            Total = total;
        }
    }
}
