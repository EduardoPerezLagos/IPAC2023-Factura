using Entidades;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace Datos
{
    public class ClienteDB
    {
        string cadena = "server=localhost; user=root; database=factura; password=123456;";

        public bool Insertar(Cliente cliente)
        {
            bool inserto = false;
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO cliente VALUES");
                sql.Append("(@Identidad, @Nombre, @Telefono, @Correo, @Direccion, @FechaNacimiento, @EstaActivo);");
                using (MySqlConnection _conexion = new MySqlConnection(cadena))
                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@Identidad", MySqlDbType.VarChar, 25).Value = cliente.Identidad;
                        comando.Parameters.Add("@Nombre", MySqlDbType.VarChar, 50).Value = cliente.Nombre;
                        comando.Parameters.Add("@Telefono", MySqlDbType.VarChar, 15).Value = cliente.Telefono;
                        comando.Parameters.Add("@Correo", MySqlDbType.VarChar, 45).Value = cliente.Correo;
                        comando.Parameters.Add("@Direccion", MySqlDbType.VarChar, 100).Value = cliente.Direccion;
                        comando.Parameters.Add("@FechaNacimiento", MySqlDbType.DateTime).Value = cliente.FechaNacimiento;
                        comando.Parameters.Add("@EstaActivo", MySqlDbType.Bit).Value = cliente.EstaActivo;
                        comando.ExecuteNonQuery();
                        inserto = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return inserto;
        }
        public bool Editar(Cliente cliente)
        {
            bool edito = false;
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" UPDATE cliente SET ");
                sql.Append(" Identidad = @Identidad, Nombre = @Nombre, Telefono = @Telefono, Correo = @Correo, Direccion = @Direccion, FechaNacimiento = @FechaNacimiento, EstaActivo = @EstaActivo ");
                sql.Append(" WHERE Identidad = @Identidad; ");
                using (MySqlConnection _conexion = new MySqlConnection(cadena))

                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@Identidad", MySqlDbType.VarChar, 25).Value = cliente.Identidad;
                        comando.Parameters.Add("@Nombre", MySqlDbType.VarChar, 50).Value = cliente.Nombre;
                        comando.Parameters.Add("@Telefono", MySqlDbType.VarChar, 15).Value = cliente.Telefono;
                        comando.Parameters.Add("@Correo", MySqlDbType.VarChar, 45).Value = cliente.Correo;
                        comando.Parameters.Add("@Direccion", MySqlDbType.VarChar, 100).Value = cliente.Direccion;
                        comando.Parameters.Add("@FechaNacimiento", MySqlDbType.DateTime).Value = cliente.FechaNacimiento;
                        comando.Parameters.Add("@EstaActivo", MySqlDbType.Bit).Value = cliente.EstaActivo;
                        comando.ExecuteNonQuery();
                        edito = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return edito;
        }
        public bool Eliminar(string identidad)
        {
            bool elimino = false;
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("DELETE FROM cliente ");
                sql.Append("WHERE Identidad = @Identidad; ");
                using (MySqlConnection _conexion = new MySqlConnection(cadena))

                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.Add("@Identidad", MySqlDbType.VarChar, 25).Value = identidad;
                        comando.ExecuteNonQuery();
                        elimino = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return elimino;
        }
        public DataTable DevolverClientes()
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("SELECT * FROM cliente ");
                using (MySqlConnection _conexion = new MySqlConnection(cadena))

                {
                    _conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), _conexion))
                    {
                        comando.CommandType = CommandType.Text;
                        MySqlDataReader dr = comando.ExecuteReader();
                        dt.Load(dr);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return dt;
        }
    }
}
