using SistemaGestionWebApi.Models;
using System.Data;
using System.Data.SqlClient;
using SistemaGestionWebApi.Models;

namespace SistemaGestionWebApi.Repository
{
    public static class VentaRepository
    {
        public static string cadenaConexion = "Data Source=DESKTOP-6T3D4NI;Initial Catalog=db_thomiico;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static List<Venta> obtenerVentas(long idUsuario)
        {
            List<Venta> ventas = new List<Venta>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando = new SqlCommand("SELECT * FROM Venta WHERE IdUsuario=@idUsuario", conn);

                comando.Parameters.AddWithValue("idUsuario", idUsuario);
                //var parameter = new SqlParameter();
                //parameter.ParameterName = "IdUsuario";
                //parameter.SqlDbType = SqlDbType.BigInt;
                //parameter.Value = idUsuario;

                conn.Open();

                SqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        Venta venta = new Venta();

                        venta.Id = reader.GetInt64(0);
                        venta.Comentarios = reader.GetString(1);
                        venta.IdUsuario = reader.GetInt64(2);

                        ventas.Add(venta);
                    }
                }
            }
            return ventas;
        }

        public static long InsertarVenta(Venta venta)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {

                SqlCommand command = new SqlCommand();

                command.Connection = conn;
                command.Connection.Open();

                command.CommandText = "INSERT INTO Venta ([Comentarios], [IdUsuario]) VALUES( @comentarios, @idUsuario)";
                command.Parameters.AddWithValue("@comentarios", venta.Comentarios);
                command.Parameters.AddWithValue("@idUsuario", venta.IdUsuario);
                command.ExecuteNonQuery();

                command.CommandText = "SELECT @@Identity";
                long IDReference = Convert.ToInt64(command.ExecuteScalar());
                command.Connection.Close();

                return IDReference;

            }
        }

        public static void CargarVenta(long idUsuario, List<Producto> productosVendidos)
        {
            Venta venta = new Venta();
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {

                SqlCommand comando = new SqlCommand();
                comando.Connection = conn;
                comando.Connection.Open();

                venta.Comentarios = "";
                venta.IdUsuario = idUsuario;
                venta.Id = InsertarVenta(venta);

                foreach (Producto producto in productosVendidos)
                {
                    ProductoVendido productoVendido = new ProductoVendido();
                    productoVendido.Stock = producto.Stock;
                    productoVendido.IdProducto = producto.Id;
                    productoVendido.IdVenta = venta.Id;

                    ProductoVendidoRepository.InsertarProductoVendido(productoVendido);

                    ProductoRepository.UpdateStockProducto(productoVendido.IdProducto, productoVendido.Stock);


                }
                // comando.Connection.Close();

            }

        }
    }
}
