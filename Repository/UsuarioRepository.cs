using System.Data.SqlClient;
using System.Data;
using SistemaGestionWebApi.Models;
using SistemaGestionWebApi.Models;

namespace SistemaGestionWebApi.Repository
{
    public static class UsuarioRepository
    {
        public static string cadenaConexion = "Data Source=DESKTOP-6T3D4NI;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static List<Usuario> obtenerUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Usuario", conn);

                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        Usuario usuario = new Usuario();

                        usuario.Id = reader.GetInt64(0);
                        usuario.Nombre = reader.GetString(1);
                        usuario.Apellido = reader.GetString(2);
                        usuario.NombreUsuario = reader.GetString(3);
                        usuario.Contraseña = reader.GetString(4);
                        usuario.Mail = reader.GetString(5);

                        usuarios.Add(usuario);
                    }
                }
            }
            return usuarios;
        }

        public static Usuario devolverUsuarioPorNombre(string Name)
        {
            Usuario usuario = new Usuario();
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando = new SqlCommand("SELECT * FROM Usuario WHERE NombreUsuario=@Name", conn);
                comando.Parameters.AddWithValue("Name", Name);

                //var parameter = new SqlParameter();
                //parameter.ParameterName = "id";
                //parameter.SqlDbType = SqlDbType.Int;
                //parameter.Value = id;

                /*SqlCommand comando = new SqlCommand("SELECT * FROM Usuario WHERE Id=1", conn);*/
                conn.Open();

                SqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    usuario.Id = reader.GetInt64(0);
                    usuario.Nombre = reader.GetString(1);
                    usuario.Apellido = reader.GetString(2);
                    usuario.NombreUsuario = reader.GetString(3);
                    usuario.Contraseña = reader.GetString(4);
                    usuario.Mail = reader.GetString(5);

                }

            }
            return usuario;
        }

        public static int InsertarUsuario(Usuario usuario)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Usuario(Nombre, Apellido, NombreUsuario, Contraseña, Mail) VALUES (@nombre, @apellido, @nombreUsuario, @contrasena, @mail)", conn);
                SqlParameter nombreParam = new SqlParameter();
                nombreParam.ParameterName = "nombre";
                nombreParam.SqlDbType = SqlDbType.VarChar;
                nombreParam.Value = usuario.Nombre;

                SqlParameter apellidoParam = new SqlParameter("apellido", usuario.Apellido);
                SqlParameter nombreUsuParam = new SqlParameter("nombreUsuario", usuario.NombreUsuario);
                SqlParameter passwParam = new SqlParameter("contrasena", usuario.Contraseña);
                SqlParameter mailParam = new SqlParameter("mail", usuario.Mail);

                cmd.Parameters.Add(nombreParam);
                cmd.Parameters.Add(apellidoParam);
                cmd.Parameters.Add(nombreUsuParam);
                cmd.Parameters.Add(passwParam);
                cmd.Parameters.Add(mailParam);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static Usuario Login(string mail, string passw)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Usuario WHERE Mail = @mail AND Contraseña = @passw", conn);

                //Se utiliza SQL Parameter para reemplazar los @ de la consulta
                SqlParameter parameterMail = new SqlParameter();
                parameterMail.ParameterName = "mail";
                parameterMail.SqlValue = SqlDbType.VarChar;
                parameterMail.Value = mail;

                SqlParameter parameterContrasena = new SqlParameter();
                parameterContrasena.ParameterName = "passw";
                parameterContrasena.SqlValue = SqlDbType.VarChar;
                parameterContrasena.Value = passw;

                //Se aplica los parámetros al comando
                command.Parameters.Add(parameterMail);
                command.Parameters.Add(parameterContrasena);
                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Usuario usuarioEncontrado = new Usuario();
                        reader.Read();
                        usuarioEncontrado.Nombre = reader.GetString(1);
                        usuarioEncontrado.Apellido = reader.GetString(2);
                        usuarioEncontrado.NombreUsuario = reader.GetString(3);
                        usuarioEncontrado.Mail = reader.GetString(5);
                        return usuarioEncontrado;
                    }
                }
                //En caso de que la consulta este vacía retornara un Usuario vacio
                return null;
            }
        }

        public static int ModificarUsuario(Usuario usuario)
        {

            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand comando = new SqlCommand("UPDATE Usuario SET Nombre = @nombre, Apellido = @apellido, NombreUsuario = @nombreUsuario, Contraseña = @contraseña, Mail = @mail WHERE Id=@Id", conn);
                    comando.Parameters.AddWithValue("@Id", usuario.Id);
                    comando.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    comando.Parameters.AddWithValue("@apellido", usuario.Apellido);
                    comando.Parameters.AddWithValue("@nombreUsuario", usuario.NombreUsuario);
                    comando.Parameters.AddWithValue("@contraseña", usuario.Contraseña);
                    comando.Parameters.AddWithValue("@mail", usuario.Mail);
                    conn.Open();
                    return comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }

        public static long EliminarUsuario(long id)
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                SqlCommand comando = new SqlCommand();

                comando.Connection = conn;
                comando.Connection.Open();
                comando.CommandText = @"DELETE [Usuario] WHERE [Id]=@ID";

                comando.Parameters.AddWithValue("@ID", id);
                comando.ExecuteNonQuery();

                comando.Connection.Close();
            }

            return id;
        }
    }
}
