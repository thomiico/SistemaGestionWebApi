using System.Data.SqlClient;
using System.Data;
using SistemaGestionWebApi.Models;

namespace SistemaGestionWebApi.Repository
{
    public static class ProductoRepository
    {
        public static string cadenaConexion = "Data Source=DESKTOP-6T3D4NI;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static Producto obtenerProducto(long id)
        {
            Producto producto = new Producto();
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando = new SqlCommand("SELECT * FROM Producto WHERE Id=@id", conn);

                var parameter = new SqlParameter();
                parameter.ParameterName = "Id";
                parameter.SqlDbType = SqlDbType.BigInt;
                parameter.Value = id;

                conn.Open();

                SqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    producto.Id = reader.GetInt64(0);
                    producto.Descripciones = reader.GetString(1);
                    producto.Costo = reader.GetDecimal(2);
                    producto.PrecioVenta = reader.GetDecimal(3);
                    producto.Stock = reader.GetInt32(4);
                    producto.IdUsuario = reader.GetInt64(5);

                }
            }
            return producto;
        }

        public static Producto obtenerProductoPorDescripcion(string descripcion)
        {
            Producto producto = new Producto();
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando = new SqlCommand("SELECT * FROM Producto WHERE descripciones = @Descripciones", conn);
                conn.Open();

                SqlParameter prodParam = new SqlParameter();
                prodParam.Value = descripcion;
                prodParam.SqlDbType = SqlDbType.VarChar;
                prodParam.ParameterName = "Descripciones";

                comando.Parameters.Add(prodParam);

                SqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    producto.Id = reader.GetInt64(0);
                    producto.Descripciones = reader.GetString(1);
                    producto.Costo = reader.GetDecimal(2);
                    producto.PrecioVenta = reader.GetDecimal(3);
                    producto.Stock = reader.GetInt32(4);
                    producto.IdUsuario = reader.GetInt64(5);

                    Console.WriteLine("Id = " + Convert.ToInt64(reader["Id"]));
                    Console.WriteLine("Descripciones = " + reader["Descripciones"].ToString());
                    Console.WriteLine("Costo = " + reader.GetDecimal(2));
                    Console.WriteLine("Precio Venta = " + Convert.ToDecimal(reader["PrecioVenta"]));
                    Console.WriteLine("Stock = " + Convert.ToInt32(reader["Stock"]));
                    Console.WriteLine("Id Usuario = " + Convert.ToInt64(reader["IdUsuario"]));


                }
                return producto;
            }
        }

        public static List<Producto> obtenerProductos(long id)
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando = new SqlCommand("SELECT * FROM Producto WHERE IdUsuario=@id", conn);

                comando.Parameters.AddWithValue("id", id);

                //var parameter = new SqlParameter();
                //parameter.ParameterName = "id";
                //parameter.SqlDbType = SqlDbType.BigInt;
                //parameter.Value = id;

                conn.Open();

                SqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Producto producto = new Producto();

                        producto.Id = reader.GetInt64(0);
                        producto.Descripciones = reader.GetString(1);
                        producto.Costo = reader.GetDecimal(2);
                        producto.PrecioVenta = reader.GetDecimal(3);
                        producto.Stock = reader.GetInt32(4);
                        producto.IdUsuario = reader.GetInt64(5);

                        productos.Add(producto);
                    }
                }
            }
            return productos;
        }

        public static int BorrarProducto(long id)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
                try
                {
                    SqlCommand comando = new SqlCommand("DELETE FROM Producto WHERE id=@id", conn);
                    comando.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    return comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return -1;
                }
        }

        public static int CrearProducto(Producto producto)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando = new SqlCommand("INSERT INTO Producto(Descripciones, Costo, PrecioVenta, Stock, IdUsuario)" +
                    "VALUES(@descripciones, @costo, @precioVenta, @stock, @idUsuario)", conn);
                comando.Parameters.AddWithValue("@descripciones", producto.Descripciones);
                comando.Parameters.AddWithValue("@costo", producto.Costo);
                comando.Parameters.AddWithValue("@precioVenta", producto.PrecioVenta);
                comando.Parameters.AddWithValue("@stock", producto.Stock);
                comando.Parameters.AddWithValue("@idUsuario", producto.IdUsuario);

                conn.Open();
                return comando.ExecuteNonQuery();

            }
        }

        public static int ModificarProducto(Producto producto, long id)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand comando = new SqlCommand("UPDATE Producto SET Descripciones = @descripciones, Costo = @costo, PrecioVenta = @precioVenta, Stock = @stock, IdUsuario = @idUsuario WHERE Id = @id", conn);
                    comando.Parameters.AddWithValue("@id", producto.Id);
                    comando.Parameters.AddWithValue("@descripciones", producto.Descripciones);
                    comando.Parameters.AddWithValue("@costo", producto.Costo);
                    comando.Parameters.AddWithValue("@precioVenta", producto.PrecioVenta);
                    comando.Parameters.AddWithValue("@stock", producto.Stock);
                    comando.Parameters.AddWithValue("@idUsuario", producto.IdUsuario);
                    conn.Open();
                    return comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return -1;
                }
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
                Producto productoObt = obtenerProducto(id);
                productos.Add(productoObt);
            }

            return productos;

        }


        public static Producto InsertarProducto(Producto producto)
        {

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando = new SqlCommand();

                comando.Connection = conn;
                comando.Connection.Open();

                comando.CommandText = @"INSERT INTO Producto([Descripciones],[Costo], [PrecioVenta], [Stock], [IdUsuario]) VALUES(@descripciones, @costo, @precioVenta, @stock, @idUsuario)";

                comando.Parameters.AddWithValue("@descripciones", producto.Descripciones);
                comando.Parameters.AddWithValue("@costo", producto.Costo);
                comando.Parameters.AddWithValue("@precioVenta", producto.PrecioVenta);
                comando.Parameters.AddWithValue("@stock", producto.Stock);
                comando.Parameters.AddWithValue("@idUsuario", producto.IdUsuario);

                comando.ExecuteNonQuery();

                comando.Connection.Close();
            }

            return producto;
        }

        public static int UpdateStockProducto(long id, int cantidadVendidos)
        {
            Producto producto = obtenerProducto(id);
            producto.Stock -= cantidadVendidos;
            return ModificarProducto(producto, id);
        }
    }

}
