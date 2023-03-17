using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class FacturaDB
    {
        string cadena = "server=localhost; user=root; database=factura; password=123456;";

        public bool Guardar(Factura factura, List<DetalleFactura> detalles)
        {
            bool inserto = false;
            int idFactura = 0;
            try
            {
                StringBuilder sqlFactura = new StringBuilder();
                sqlFactura.Append(" INSERT INTO factura (Fecha, IdentidadCliente, CodigoUsuario, ISV, Descuento, SubTotal, Total) VALUES (@Fecha, @IdentidadCliente, @CodigoUsuario, @ISV, @Descuento, @SubTotal, @Total); ");
                sqlFactura.Append(" SELECT LAST_INSERT_ID(); ");

                StringBuilder sqlDetalle = new StringBuilder();
                sqlDetalle.Append(" INSERT INTO detallefactura (IdFactura, CodigoProducto, Precio, Cantidad, Total) VALUES (@IdFactura, @CodigoProducto, @Precio, @Cantidad, @Total); ");

                StringBuilder sqlExistencia = new StringBuilder();
                sqlExistencia.Append(" UPDATE producto SET Existencia = Existencia - @Cantidad WHERE Codigo = @Codigo; ");

                using (MySqlConnection con = new MySqlConnection(cadena))
                {
                    con.Open();

                    MySqlTransaction transaction = con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                    try
                    {
                        using (MySqlCommand cmd1 = new MySqlCommand(sqlFactura.ToString(), con, transaction))
                        {
                            cmd1.CommandType = System.Data.CommandType.Text;
                            cmd1.Parameters.Add("@Fecha", MySqlDbType.DateTime).Value = factura.Fecha;
                            cmd1.Parameters.Add("@IdentidadCliente", MySqlDbType.VarChar, 25).Value = factura.IdentidadCliente;
                            cmd1.Parameters.Add("@CodigoUsuario", MySqlDbType.VarChar, 50).Value = factura.CodigoUsuario;
                            cmd1.Parameters.Add("@ISV", MySqlDbType.Decimal).Value = factura.ISV;
                            cmd1.Parameters.Add("@Descuento", MySqlDbType.Decimal).Value = factura.Descuento;
                            cmd1.Parameters.Add("@SubTotal", MySqlDbType.Decimal).Value = factura.SubTotal;
                            cmd1.Parameters.Add("@Total", MySqlDbType.Decimal).Value = factura.Total;
                            idFactura = Convert.ToInt32(cmd1.ExecuteScalar());
                        }

                        foreach (DetalleFactura detalle in detalles)
                        {
                            using (MySqlCommand cmd2 = new MySqlCommand(sqlDetalle.ToString(), con, transaction))
                            {
                                cmd2.CommandType = System.Data.CommandType.Text;
                                cmd2.Parameters.Add("@IdFactura", MySqlDbType.Int32).Value = idFactura;
                                cmd2.Parameters.Add("@CodigoProducto", MySqlDbType.VarChar, 80).Value = detalle.CodigoProducto;
                                cmd2.Parameters.Add("@Precio", MySqlDbType.Decimal).Value = detalle.Precio;
                                cmd2.Parameters.Add("@Cantidad", MySqlDbType.Decimal).Value = detalle.Cantidad;
                                cmd2.Parameters.Add("@Total", MySqlDbType.Decimal).Value = detalle.Total;
                                cmd2.ExecuteNonQuery();
                            }

                            using (MySqlCommand cmd3 = new MySqlCommand(sqlExistencia.ToString(), con, transaction))
                            {
                                cmd3.CommandType = System.Data.CommandType.Text;
                                cmd3.Parameters.Add("@Cantidad", MySqlDbType.Decimal).Value = detalle.Cantidad;
                                cmd3.Parameters.Add("@Codigo", MySqlDbType.VarChar, 80).Value = detalle.CodigoProducto;
                                cmd3.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        inserto = true;
                    }
                    catch (System.Exception)
                    {
                        inserto = false;
                        transaction.Rollback();
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return inserto;
        }
    }
}
