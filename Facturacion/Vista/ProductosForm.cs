using Datos;
using Entidades;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vista
{
    public partial class ProductosForm : Form
    {
        public ProductosForm()
        {
            InitializeComponent();
        }
        string operacion;
        Producto producto;
        ProductoDB productoDB = new ProductoDB();
        DataTable dt = new DataTable();

        private void NuevoButton_Click(object sender, EventArgs e)
        {
            operacion = "Nuevo";
            HabilitarControles();
        }

        private void HabilitarControles()
        {
            CodigoTextBox.Enabled = true;
            DescripcionTextBox.Enabled = true;
            ExistenciaTextBox.Enabled = true;
            PrecioTextBox.Enabled = true;
            EstaActivoCheckBox.Enabled = true;
            AdjuntarImagenButton.Enabled = true;
            GuardarButton.Enabled = true;
            CancelarButton.Enabled = true;
            ModificarButton.Enabled = false;
            EliminarButton.Enabled = false;


        }

        private void LimpiarControles()
        {
            CodigoTextBox.Clear();
            DescripcionTextBox.Clear();
            ExistenciaTextBox.Clear();
            PrecioTextBox.Clear();
            EstaActivoCheckBox.Checked = false;
            ImagenPictureBox.Image = null;
            producto = null;
        }

        private void DeshabilitarControles()
        {
            CodigoTextBox.Enabled = false;
            DescripcionTextBox.Enabled = false;
            ExistenciaTextBox.Enabled = false;
            PrecioTextBox.Enabled = false;
            EstaActivoCheckBox.Enabled = false;
            AdjuntarImagenButton.Enabled = false;
            GuardarButton.Enabled = false;
            CancelarButton.Enabled = false;
            ModificarButton.Enabled = true;
            EliminarButton.Enabled = true;
        }

        private void CancelarButton_Click(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LimpiarControles();
        }

        private void ModificarButton_Click(object sender, EventArgs e)
        {
            operacion = "Modificar";
            if (ProductosDataGridView.SelectedRows.Count > 0)
            {
                CodigoTextBox.Text = ProductosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString();
                DescripcionTextBox.Text = ProductosDataGridView.CurrentRow.Cells["Descripcion"].Value.ToString();
                ExistenciaTextBox.Text = ProductosDataGridView.CurrentRow.Cells["Existencia"].Value.ToString();
                PrecioTextBox.Text = ProductosDataGridView.CurrentRow.Cells["Precio"].Value.ToString();
                EstaActivoCheckBox.Checked = Convert.ToBoolean(ProductosDataGridView.CurrentRow.Cells["EstaActivo"].Value);

                byte[] img = productoDB.DevolverFoto(ProductosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString());

                if (img.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(img);
                    ImagenPictureBox.Image = System.Drawing.Bitmap.FromStream(ms);
                }
                HabilitarControles();
                CodigoTextBox.ReadOnly = true;
            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro");
            }
        }

        private void GuardarButton_Click(object sender, EventArgs e)
        {

            producto = new Producto();
            producto.Codigo = CodigoTextBox.Text;
            producto.Descripcion = DescripcionTextBox.Text;
            producto.Precio = Convert.ToDecimal(PrecioTextBox.Text);
            producto.Existencia = Convert.ToInt32(ExistenciaTextBox.Text);
            producto.EstaActivo = EstaActivoCheckBox.Checked;

            if (ImagenPictureBox.Image != null)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                ImagenPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                producto.Foto = ms.GetBuffer();
            }
            if (operacion == "Nuevo")
            {
                if (string.IsNullOrEmpty(CodigoTextBox.Text))
                {
                    errorProvider1.SetError(CodigoTextBox, "Ingrese un código");
                    CodigoTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(DescripcionTextBox.Text))
                {
                    errorProvider1.SetError(DescripcionTextBox, "Ingrese una descripción");
                    DescripcionTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(ExistenciaTextBox.Text))
                {
                    errorProvider1.SetError(ExistenciaTextBox, "Ingrese una existencia");
                    ExistenciaTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(PrecioTextBox.Text))
                {
                    errorProvider1.SetError(PrecioTextBox, "Ingrese un Precio");
                    PrecioTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                bool inserto = productoDB.Insertar(producto);

                if (inserto)
                {
                    DeshabilitarControles();
                    LimpiarControles();
                    TraerProductos();

                    MessageBox.Show("Registro guardado con exito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el registro", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (operacion == "Modificar")
            {
                bool modifico = productoDB.Editar(producto);
                if (modifico)
                {
                    CodigoTextBox.ReadOnly = false;
                    DeshabilitarControles();
                    LimpiarControles();
                    TraerProductos();

                    MessageBox.Show("Registro actualizado con exito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el registro", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExistenciaTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private void PrecioTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && e.KeyChar != '\b')
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void AdjuntarImagenButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult resultado = dialog.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                ImagenPictureBox.Image = Image.FromFile(dialog.FileName);
            }
        }

        private void ProductosForm_Load(object sender, EventArgs e)
        {
            TraerProductos();
        }
        private void TraerProductos()
        {
            ProductosDataGridView.DataSource = productoDB.DevolverProductos();
        }

        private void EliminarButton_Click(object sender, EventArgs e)
        {
            if (ProductosDataGridView.SelectedRows.Count > 0)
            {
                DialogResult resultado = MessageBox.Show("Esta seguro de eliminar el registro", "Advertencia", MessageBoxButtons.YesNo);
                if (resultado == DialogResult.Yes)
                {
                    bool elimino = productoDB.Eliminar(ProductosDataGridView.CurrentRow.Cells["Codigo"].Value.ToString());
                    if (elimino)
                    {
                        LimpiarControles();
                        DeshabilitarControles();
                        TraerProductos();
                        MessageBox.Show("Registro Eliminado");
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el registro");
                    }
                }
            }
        }
    }
}
