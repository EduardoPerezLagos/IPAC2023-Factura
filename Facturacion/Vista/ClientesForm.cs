using Datos;
using Entidades;
using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class ClientesForm : Form
    {
        public ClientesForm()
        {
            InitializeComponent();

        }
        string operacion;
        Cliente cliente = new Cliente();
        ClienteDB clienteDB = new ClienteDB();


        private void HabilitarControles()
        {
            IdentidadTextBox.Enabled = true;
            NombreTextBox.Enabled = true;
            TelefonoTextBox.Enabled = true;
            CorreoTextBox.Enabled = true;
            DireccionTextBox.Enabled = true;
            FechaNacimientoDateTimePicker.Enabled = true;
            EstaActivoCheckBox.Enabled = true;
            GuardarButton.Enabled = true;
            CancelarButton.Enabled = true;
            ModificarButton.Enabled = false;
            EliminarButton.Enabled = false;


        }

        private void LimpiarControles()
        {
            IdentidadTextBox.Clear();
            NombreTextBox.Clear();
            TelefonoTextBox.Clear();
            CorreoTextBox.Clear();
            DireccionTextBox.Clear();
            EstaActivoCheckBox.Checked = false;


        }

        private void DeshabilitarControles()
        {
            IdentidadTextBox.Enabled = false;
            NombreTextBox.Enabled = false;
            TelefonoTextBox.Enabled = false;
            CorreoTextBox.Enabled = false;
            DireccionTextBox.Enabled = false;
            EstaActivoCheckBox.Enabled = false;
            FechaNacimientoDateTimePicker.Enabled = false;
            GuardarButton.Enabled = false;
            CancelarButton.Enabled = false;
            ModificarButton.Enabled = true;
            EliminarButton.Enabled = true;
        }

        private void NuevoButton_Click(object sender, EventArgs e)
        {

            operacion = "Nuevo";
            HabilitarControles();
            IdentidadTextBox.ReadOnly = false;
        }

        private void CancelarButton_Click(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LimpiarControles();
        }

        private void ModificarButton_Click(object sender, EventArgs e)
        {
            operacion = "Modificar";
            if (ClientesDataGridView.SelectedRows.Count > 0)
            {
                IdentidadTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Identidad"].Value.ToString();
                NombreTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Nombre"].Value.ToString();
                TelefonoTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Telefono"].Value.ToString();
                CorreoTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Correo"].Value.ToString();
                DireccionTextBox.Text = ClientesDataGridView.CurrentRow.Cells["Direccion"].Value.ToString();
                EstaActivoCheckBox.Checked = Convert.ToBoolean(ClientesDataGridView.CurrentRow.Cells["EstaActivo"].Value);
                FechaNacimientoDateTimePicker.Value = Convert.ToDateTime(ClientesDataGridView.CurrentRow.Cells["FechaNacimiento"].Value);
                IdentidadTextBox.ReadOnly = true;

                HabilitarControles();
            }
        }

        private void GuardarButton_Click(object sender, EventArgs e)
        {
            cliente.Identidad = IdentidadTextBox.Text;
            cliente.Nombre = NombreTextBox.Text;
            cliente.Telefono = TelefonoTextBox.Text;
            cliente.Correo = CorreoTextBox.Text;
            cliente.Direccion = DireccionTextBox.Text;
            cliente.EstaActivo = EstaActivoCheckBox.Checked;
            cliente.FechaNacimiento = FechaNacimientoDateTimePicker.Value.Date;

            if (operacion == "Nuevo")
            {
                if (string.IsNullOrEmpty(IdentidadTextBox.Text))
                {
                    errorProvider1.SetError(IdentidadTextBox, "Ingrese un código");
                    IdentidadTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(NombreTextBox.Text))
                {
                    errorProvider1.SetError(NombreTextBox, "Ingrese una descripción");
                    NombreTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                bool inserto = clienteDB.Insertar(cliente);
                if (inserto)
                {
                    DeshabilitarControles();
                    LimpiarControles();
                    TraerClientes();

                    MessageBox.Show("Registro guardado con exito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el registro", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (operacion == "Modificar")
            {
                bool modifico = clienteDB.Editar(cliente);
                if (modifico)
                {
                    IdentidadTextBox.ReadOnly = false;
                    DeshabilitarControles();
                    LimpiarControles();
                    TraerClientes();

                    MessageBox.Show("Registro actualizado con exito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el registro", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClientesForm_Load(object sender, EventArgs e)
        {
            TraerClientes();
        }
        private void TraerClientes()
        {
            ClientesDataGridView.DataSource = clienteDB.DevolverClientes();
        }

        private void EliminarButton_Click(object sender, EventArgs e)
        {
            if (ClientesDataGridView.SelectedRows.Count > 0)
            {
                DialogResult resultado = MessageBox.Show("Esta seguro de eliminar el registro", "Advertencia", MessageBoxButtons.YesNo);
                if (resultado == DialogResult.Yes)
                {
                    bool elimino = clienteDB.Eliminar(ClientesDataGridView.CurrentRow.Cells["Identidad"].Value.ToString());
                    if (elimino)
                    {
                        LimpiarControles();
                        DeshabilitarControles();
                        TraerClientes();
                        MessageBox.Show("Registro Eliminado");
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el registro");
                    }
                }
            }
        }

        private void FechaNacimientoDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            FechaNacimientoDateTimePicker.CustomFormat = "dd/MM/yyyy";
        }

        private void FechaNacimientoDateTimePicker_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Back) || (e.KeyCode == Keys.Delete))
            {
                FechaNacimientoDateTimePicker.CustomFormat = " ";
            }
        }
    }
}

