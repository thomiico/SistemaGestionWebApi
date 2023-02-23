using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionWebApi.Models;
using SistemaGestionWebApi.Repository;

namespace SistemaGestionWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpGet("{nombreUsuario}/{contraseña}")]
        public Usuario ObtenerUsuarioPorNombreUsuario(string nombreUsuario, string contraseña)
        {
            var usuario = UsuarioRepository.Login(nombreUsuario, contraseña);
            return usuario == null ? new Usuario() : usuario;
        }

        [HttpGet("{nombreUsuario}")]
        public Usuario ObtenerUsuarioPorNombre(string nombreUsuario)
        {
            var usuario = UsuarioRepository.devolverUsuarioPorNombre(nombreUsuario);
            return usuario == null ? new Usuario() : usuario;
        }

        [HttpPut]
        public void ModificarUsuario(Usuario usuario)
        {
            UsuarioRepository.ModificarUsuario(usuario);
        }

        [HttpPost]
        public void InsertarUsuario(Usuario usuario)
        {
            UsuarioRepository.InsertarUsuario(usuario);
        }


        [HttpDelete("{id}")]
        public void BorrarUsuario(long id)
        {
            UsuarioRepository.EliminarUsuario(id);
        }
    }
}
