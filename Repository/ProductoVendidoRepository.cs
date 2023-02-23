using System.Data.SqlClient;
using SistemaGestionWebApi.Models;

namespace SistemaGestionWebApi.Repository
{
    public static class ProductoVendidoRepository
    {
        public static string cadenaConexion = "Data Source=DESKTOP-6T3D4NI;Initial Catalog=db_thomiico;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static List<ProductoVendido> obtenerProductosVendidos(long idUsuario)
        {
            List<ProductoVendido> productosVendidos = new List<ProductoVendido>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando = new SqlCommand("SELECT IdProducto FROM Venta INNER JOIN ProductoVendido ON Venta.Id = ProductoVendido.IdVenta WHERE Venta.IdUsuario = @idUsuario", conn);

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
                        ProductoVendido productoVendido = new ProductoVendido();

                        productoVendido.Id = reader.GetInt64(0);

                        productosVendidos.Add(productoVendido);
                    }
                }
            }
            return productosVendidos;
        }

        public static Producto ObtenerProducto(long id)
        {
            Producto producto = new Producto();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando2 = new SqlCommand("SELECT * FROM Producto WHERE id=@Id", conn);
                comando2.Parameters.AddWithValue("@Id", id);

                conn.Open();

                SqlDataReader reader = comando2.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    Producto productoTemporal = new Producto();

                    producto.Id = reader.GetInt64(0);
                    producto.Descripciones = reader.GetString(1);
                    producto.Costo = reader.GetDecimal(2);
                    producto.PrecioVenta = reader.GetDecimal(3);
                    producto.Stock = reader.GetInt32(4);
                    producto.IdUsuario = reader.GetInt64(5);

                }
                return producto;
            }
        }

        public static List<Producto> ObtenerProductoVendido(long idUsuario)
        {
            List<long> ProductosID = new List<long>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando3 = new SqlCommand("SELECT IdProducto FROM Venta INNER JOIN ProductoVendido  ON venta.id = ProductoVendido.IdVenta WHERE IdUsuario = @idUsuario", conn);

                comando3.Parameters.AddWithValue("@idUsuario", idUsuario);

                conn.Open();

                SqlDataReader reader = comando3.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductosID.Add(reader.GetInt64(0));
                    }
                }
            }
            List<Producto> productos = new List<Producto>();
            foreach (var id in ProductosID)
            {
                Producto productoObt = ObtenerProducto(id);
                productos.Add(productoObt);
            }

            return productos;


        }

        public static void InsertarProductoVendido(ProductoVendido productoVendido)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {

                SqlCommand comando = new SqlCommand();

                comando.Connection = conn;
                comando.Connection.Open();

                comando.CommandText = @"INSERT INTO ProductoVendido ([Stock], [IdProducto],[IdVenta] ) VALUES( @stock, @idProducto, @idVenta)";

                comando.Parameters.AddWithValue("@stock", productoVendido.Stock);
                comando.Parameters.AddWithValue("@idProducto", productoVendido.IdProducto);
                comando.Parameters.AddWithValue("@idVenta", productoVendido.IdVenta);

                comando.ExecuteNonQuery();

                comando.Connection.Close();

            }

        }
    }
}
