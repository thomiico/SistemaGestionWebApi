using ProyectoFinalCoderHouse.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoFinalCoderHouse.ADO.NET
{
    public static class ProductoVendidoHandler
    {
        public static string ConnectionString = "Data Source=(localdb)\\Servidor Ejercicio; Initial Catalog = SistemaGestion; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    
        public static List<ProductoVendido> GetProductosVendidos(int id)
        {
            List<ProductoVendido> listProductosVendidos = new List<ProductoVendido>();
            List<Producto> listProductos = ProductoHandler.GetProductos(id);

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {

                    
                    foreach (Producto producto in listProductos)
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Connection.Open();
                        sqlCommand.CommandText = @"select* from ProductoVendido
                                                where IdProducto = @idProducto";
                        
                        sqlCommand.Parameters.AddWithValue("@idProducto", producto.Id);
                        

                        SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        dataAdapter.SelectCommand = sqlCommand;
                        DataTable table = new DataTable();
                        dataAdapter.Fill(table); //Se ejecuta el Select
                        sqlCommand.Parameters.Clear();

                        foreach (DataRow row in table.Rows)
                        {
                            ProductoVendido productoVendido = new ProductoVendido();
                            productoVendido.Id = Convert.ToInt32(row["Id"]);
                            productoVendido.Stock = Convert.ToInt32(row["Stock"]);
                            productoVendido.IdProducto = Convert.ToInt32(row["IdProducto"]);
                            productoVendido.IdVenta = Convert.ToInt32(row["IdVenta"]);

                            listProductosVendidos.Add(productoVendido);
                        }
                        sqlCommand.Connection.Close();
                    }
                    
                }
            }
            return listProductosVendidos;
        }
    }
    
}
