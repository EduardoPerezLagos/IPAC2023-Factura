using Datos;
using Entidades;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vista
{
    public partial class UsuariosForm : Form
    {
        public UsuariosForm()
        {
            InitializeComponent();
        }
        string tipoOperacion;

        DataTable dt = new DataTable();
        UsuarioDB UsuarioDB = new UsuarioDB();
        Usuario user = new Usuario();

        private void HabilitarControles()
        {
            CodigoTextBox.Enabled = true;
            NombreTextBox.Enabled = true;
            ContraseñaTextBox.Enabled = true;
            CorreoTextBox.Enabled = true;
            RolComboBox.Enabled = true;
            EstaActivoCheckBox.Enabled = true;
            AdjuntarFotoButton.Enabled = true;
            GuardarButton.Enabled = true;
            CancelarButton.Enabled = true;
            ModificarButton.Enabled = false;
            EliminarButton.Enabled = false;

        }
        private void DeshabilitarControles()
        {
            CodigoTextBox.Enabled = false;
            NombreTextBox.Enabled = false;
            ContraseñaTextBox.Enabled = false;
            CorreoTextBox.Enabled = false;
            RolComboBox.Enabled = false;
            EstaActivoCheckBox.Enabled = false;
            AdjuntarFotoButton.Enabled = false;
            GuardarButton.Enabled = false;
            CancelarButton.Enabled = false;
            ModificarButton.Enabled = true;
            EliminarButton.Enabled = true;

        }

        private void LimpiarControles()
        {
            CodigoTextBox.Clear();
            NombreTextBox.Clear();
            ContraseñaTextBox.Clear();
            CorreoTextBox.Clear();
            RolComboBox.Text = string.Empty;
            EstaActivoCheckBox.Checked = false;
            FotoPictureBox.Image = null;

        }
        private void NuevoButton_Click(object sender, System.EventArgs e)
        {
            CodigoTextBox.Focus();
            HabilitarControles();
            tipoOperacion = "Nuevo";
        }

        private void CancelarButton_Click(object sender, System.EventArgs e)
        {
            DeshabilitarControles();
            LimpiarControles();

        }

        private void GuardarButton_Click(object sender, System.EventArgs e)
        {

            if (tipoOperacion == "Nuevo")
            {
                if (string.IsNullOrEmpty(CodigoTextBox.Text))
                {
                    errorProvider1.SetError(CodigoTextBox, "Ingrese un código");
                    CodigoTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(NombreTextBox.Text))
                {
                    errorProvider1.SetError(NombreTextBox, "Ingrese un nombre");
                    NombreTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(ContraseñaTextBox.Text))
                {
                    errorProvider1.SetError(ContraseñaTextBox, "Ingrese una contraseña");
                    ContraseñaTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(RolComboBox.Text))
                {
                    errorProvider1.SetError(RolComboBox, "Seleccione un rol");
                    RolComboBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                user.CodigoUsuario = CodigoTextBox.Text;
                user.Nombre = NombreTextBox.Text;
                user.Contraseña = ContraseñaTextBox.Text;
                user.Rol = RolComboBox.Text;
                user.Correo = CorreoTextBox.Text;
                user.EstaActivo = EstaActivoCheckBox.Checked;

                if (FotoPictureBox.Image != null)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();

                    FotoPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    user.Foto = ms.GetBuffer();
                }
                //Insertar en la base de datos
                bool inserto = UsuarioDB.Insertar(user);
                if (inserto)
                {
                    LimpiarControles();
                    DeshabilitarControles();
                    TraerUsuarios();
                    MessageBox.Show("Registro Guardado");

                }
                else
                {
                    MessageBox.Show("No se pudo Guardar el Registro");
                }
            }
            else if (tipoOperacion == "Modificar")
            {
                user.CodigoUsuario = CodigoTextBox.Text;
                user.Nombre = NombreTextBox.Text;
                user.Contraseña = ContraseñaTextBox.Text;
                user.Rol = RolComboBox.Text;
                user.Correo = CorreoTextBox.Text;
                user.EstaActivo = EstaActivoCheckBox.Checked;

                if (FotoPictureBox.Image != null)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();

                    FotoPictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    user.Foto = ms.GetBuffer();
                }
                bool modifico = UsuarioDB.Editar(user);
                if (modifico)
                {
                    CodigoTextBox.ReadOnly = false;
                    LimpiarControles();
                    DeshabilitarControles();
                    TraerUsuarios();
                    MessageBox.Show("Registro Actualizado Correctamente");
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el registro");
                }
            }
        }

        private void ModificarButton_Click(object sender, System.EventArgs e)
        {
            tipoOperacion = "Modificar";

            if (UsuariosDataGridView.SelectedRows.Count > 0)
            {
                CodigoTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["CodigoUsuario"].Value.ToString();
                NombreTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Nombre"].Value.ToString();
                ContraseñaTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Contrasena"].Value.ToString();
                CorreoTextBox.Text = UsuariosDataGridView.CurrentRow.Cells["Correo"].Value.ToString();
                RolComboBox.Text = UsuariosDataGridView.CurrentRow.Cells["Rol"].Value.ToString();
                EstaActivoCheckBox.Checked = Convert.ToBoolean(UsuariosDataGridView.CurrentRow.Cells["EstaActivo"].Value);

                byte[] miFoto = UsuarioDB.DevolverFoto(UsuariosDataGridView.CurrentRow.Cells["CodigoUsuario"].Value.ToString());
                if (miFoto.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(miFoto);
                    FotoPictureBox.Image = System.Drawing.Bitmap.FromStream(ms);
                }
                HabilitarControles();
                CodigoTextBox.ReadOnly = true;
            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro");
            }
        }

        private void AdjuntarFotoButton_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult resultado = dialog.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                FotoPictureBox.Image = Image.FromFile(dialog.FileName);
            }
        }

        private void UsuariosForm_Load(object sender, System.EventArgs e)
        {
            TraerUsuarios();
        }

        private void TraerUsuarios()
        {
            dt = UsuarioDB.DevolverUsuario();
            UsuariosDataGridView.DataSource = dt;
        }

        private void EliminarButton_Click(object sender, EventArgs e)
        {
            if (UsuariosDataGridView.SelectedRows.Count > 0)
            {
                DialogResult resultado = MessageBox.Show("Esta seguro de Eliminar el registro", "Advertencia", MessageBoxButtons.YesNo);

                if (resultado == DialogResult.Yes)
                {
                    bool elimino = UsuarioDB.Eliminar(UsuariosDataGridView.CurrentRow.Cells["CodigoUsuario"].Value.ToString());
                    if (elimino)
                    {
                        LimpiarControles();
                        DeshabilitarControles();
                        TraerUsuarios();
                        MessageBox.Show("Registro Eliminado");
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el registro");
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe Seleccionar un registro");
            }
        }
    }
}
