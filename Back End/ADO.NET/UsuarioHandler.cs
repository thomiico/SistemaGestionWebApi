using ProyectoFinalCoderHouse.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoFinalCoderHouse.ADO.NET
{
    public static class UsuarioHandler
    {
        public static string connectionString = "Data Source=(localdb)\\Servidor Ejercicio;Initial Catalog=SistemaGestion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static List<Usuario> GetUsuarios(DataTable table)
        {
            List<Usuario> usuarios = new List<Usuario>();
            foreach (DataRow row in table.Rows)
            {
                Usuario getUsuario = new Usuario();
                getUsuario.id = Convert.ToInt32(row["Id"]);
                getUsuario.Nombre = row["Nombre"].ToString();
                getUsuario.Apellido = row["Apellido"].ToString();
                getUsuario.NombreUsuario = row["NombreUsuario"].ToString();
                getUsuario.Contraseña = row["Contraseña"].ToString();
                getUsuario.Mail = row["Mail"].ToString();

                usuarios.Add(getUsuario);
            }
            return usuarios;
        }

        public static Usuario GetUsuarioByPassword(string nombre, string contraseña)
        {
            
            Usuario usuario = new Usuario();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.Connection.Open();

                    command.CommandText = @" SELECT * 
                                FROM Usuario 
                                WHERE NombreUsuario = @nombre
                                AND   Contraseña = @contraseña;";

                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@contraseña", contraseña);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = command;
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count < 1)
                    {
                        return new Usuario();
                    }


                    List<Usuario> usuarios = GetUsuarios(table);
                    usuario = usuarios[0];

                    command.Connection.Close();
                }
            }
            return usuario;
        }

        public static bool InsertUsuario(Usuario usuario)
        {
            bool alta = false;
            Usuario usuarioRepetido = GetUsuarioByUserName(usuario.NombreUsuario);

            if (usuario.NombreUsuario == null || 
                usuario.NombreUsuario.Trim() == "" ||
                usuario.Contraseña == null ||
                usuario.Contraseña.Trim() == "" ||
                usuario.Nombre == null ||
                usuario.Nombre.Trim() == "" ||
                usuario.Apellido == null ||
                usuario.Apellido.Trim() == "")
            {
                return alta;
                throw new Exception("Faltan datos obligatorios");
            }
            else if(usuarioRepetido.id != 0)
            {
                return alta;
                throw new Exception("El nombre de usuario ya existe");
            }
            else
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Connection.Open();
                sqlCommand.CommandText = @"INSERT INTO Usuario
                                    ([Nombre]
                                    ,[Apellido]
                                    ,[NombreUsuario]
									,[Contraseña]
									,[Mail] )
                                    VALUES
                                    (@Nombre,
                                        @Apellido,
                                        @NombreUsuario,
										@Contraseña,
										@Mail)";

                sqlCommand.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                sqlCommand.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                sqlCommand.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                sqlCommand.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                sqlCommand.Parameters.AddWithValue("@Mail", usuario.Mail);

                sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el INSERT INTO
                usuario.id = GetId.Get(sqlCommand);

                alta = usuario.id != 0 ? true : false;
                sqlCommand.Connection.Close();
                return alta;
                
            }
        }

        public static bool UpdateUsuario(Usuario usuario)
        {
            bool modificado = false;

            if (usuario.NombreUsuario == null ||
                usuario.NombreUsuario.Trim() == "" ||
                usuario.Contraseña == null ||
                usuario.Contraseña.Trim() == "" ||
                usuario.Nombre == null ||
                usuario.Nombre.Trim() == "" ||
                usuario.Apellido == null ||
                usuario.Apellido.Trim() == "")
            {
                return modificado;
                throw new Exception("Faltan datos obligatorios");
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Connection.Open();
                        sqlCommand.CommandText = @" UPDATE Usuario
                                                SET 
                                                   Nombre = @Nombre,
                                                   Apellido = @Apellido,
                                                   NombreUsuario = @NombreUsuario,
										           Contraseña = @Contraseña,
										           Mail = @Mail
                                                WHERE id = @ID";

                        sqlCommand.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        sqlCommand.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                        sqlCommand.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                        sqlCommand.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                        sqlCommand.Parameters.AddWithValue("@Mail", usuario.Mail);
                        sqlCommand.Parameters.AddWithValue("@ID", usuario.id);


                        int recordsAffected = sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente UPDATE

                        sqlCommand.Connection.Close();
                        if (recordsAffected == 0){
                            return modificado;
                            throw new Exception("El registro a modificar no existe.");
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }

        public static Usuario GetUsuarioByUserName(string nombre)
        {
            Usuario usuario = new Usuario();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.Connection.Open();

                    command.CommandText = @"SELECT * 
                                FROM Usuario 
                                WHERE NombreUsuario = @nombre;";

                    command.Parameters.AddWithValue("@nombre", nombre);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = command;
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count < 1)
                    {
                        return new Usuario();
                    }


                    List<Usuario> usuarios = GetUsuarios(table);
                    usuario = usuarios[0];

                    command.Connection.Close();
                }
            }
            return usuario;
        }

    }
}
