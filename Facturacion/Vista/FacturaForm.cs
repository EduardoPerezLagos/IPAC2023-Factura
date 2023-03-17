using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vista
{
    public partial class FacturaForm : Form
    {
        public FacturaForm()
        {
            InitializeComponent();
        }

        Cliente miCliente = null;
        ClienteDB clienteDB = new ClienteDB();
        Producto miProducto = null;
        ProductoDB productoDB = new ProductoDB();
        List<DetalleFactura> listaDetalles = new List<DetalleFactura>();
        FacturaDB facturaDB = new FacturaDB();
        decimal subTotal = 0;
        decimal isv = 0;
        decimal totalAPagar = 0;
        decimal descuento = 0;

        private void IdentidadTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(IdentidadTextBox.Text))
            {
                miCliente = new Cliente();
                miCliente = clienteDB.DevolverClientePorIdentidad(IdentidadTextBox.Text);
                NombreClienteTextBox.Text = miCliente.Nombre;
            }
            else
            {
                miCliente = null;
                NombreClienteTextBox.Clear();
            }
        }

        private void BuscarClienteButton_Click(object sender, System.EventArgs e)
        {
            BuscarClienteForm form = new BuscarClienteForm();
            form.ShowDialog();
            miCliente = new Cliente();
            miCliente = form.cliente;
            IdentidadTextBox.Text = miCliente.Identidad;
            NombreClienteTextBox.Text = miCliente.Nombre;
        }

        private void FacturaForm_Load(object sender, System.EventArgs e)
        {
            UsuarioTextBox.Text = System.Threading.Thread.CurrentPrincipal.Identity.Name;
        }

        private void CodigoProductoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(CodigoProductoTextBox.Text))
            {
                miProducto = new Producto();
                miProducto = productoDB.DevolverProductoPorCodigo(CodigoProductoTextBox.Text);
                DescripcionProductoTextBox.Text = miProducto.Descripcion;
                ExistenciaTextBox.Text = miProducto.Existencia.ToString();
            }
            else
            {
                miProducto = null;
                DescripcionProductoTextBox.Clear();
                ExistenciaTextBox.Clear();
            }
        }

        private void BuscarProductoButton_Click(object sender, System.EventArgs e)
        {
            BuscarProductoForm form = new BuscarProductoForm();
            form.ShowDialog();
            miProducto = new Producto();
            miProducto = form.producto;
            CodigoProductoTextBox.Text = miProducto.Codigo;
            DescripcionProductoTextBox.Text = miProducto.Descripcion;
            ExistenciaTextBox.Text = miProducto.Existencia.ToString();
        }

        private void CantidadTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(CantidadTextBox.Text))
            {
                DetalleFactura detalle = new DetalleFactura();
                detalle.CodigoProducto = miProducto.Codigo;
                detalle.Cantidad = Convert.ToInt32(CantidadTextBox.Text);
                detalle.Precio = Convert.ToDecimal(miProducto.Precio);
                detalle.Total = Convert.ToInt32(CantidadTextBox.Text) * miProducto.Precio;
                detalle.Descripcion = miProducto.Descripcion;

                subTotal += detalle.Total;
                isv = subTotal * 0.15M;
                totalAPagar = subTotal + isv - descuento;

                listaDetalles.Add(detalle);
                DetalleDataGridView.DataSource = null;
                DetalleDataGridView.DataSource = listaDetalles;

                SubTotalTextBox.Text = subTotal.ToString();
                ISVTextBox.Text = isv.ToString();
                TotalTextBox.Text = totalAPagar.ToString();

                miProducto = null;
                CodigoProductoTextBox.Clear();
                DescripcionProductoTextBox.Clear();
                ExistenciaTextBox.Clear();
                CantidadTextBox.Clear();
                CodigoProductoTextBox.Focus();
            }
        }

        private void GuardarButton_Click(object sender, EventArgs e)
        {
            Factura miFactura = new Factura();
            miFactura.Fecha = FechaDateTimePicker.Value;
            miFactura.CodigoUsuario = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            miFactura.IdentidadCliente = miCliente.Identidad;
            miFactura.SubTotal = subTotal;
            miFactura.ISV = isv;
            miFactura.Descuento = descuento;
            miFactura.Total = totalAPagar;

            bool inserto = facturaDB.Guardar(miFactura, listaDetalles);

            if (inserto)
            {
                LimpiarControles();
                IdentidadTextBox.Focus();
                MessageBox.Show("Factura registrada exitosamente");
            }
            else
                MessageBox.Show("No se pudo registrar la factura");
        }
        private void LimpiarControles()
        {
            miCliente = null;
            miProducto = null;
            listaDetalles = null;
            FechaDateTimePicker.Value = DateTime.Now;
            IdentidadTextBox.Clear();
            NombreClienteTextBox.Clear();
            CodigoProductoTextBox.Clear();
            DescripcionProductoTextBox.Clear();
            ExistenciaTextBox.Clear();
            CantidadTextBox.Clear();
            DetalleDataGridView.DataSource = null;
            subTotal = 0;
            SubTotalTextBox.Clear();
            isv = 0;
            ISVTextBox.Clear();
            descuento = 0;
            DescuentoTextBox.Clear();
            totalAPagar = 0;
            TotalTextBox.Clear();
        }

        private void CancelarButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
