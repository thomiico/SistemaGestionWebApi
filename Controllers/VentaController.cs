using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionWebApi.Models;
using SistemaGestionWebApi.Repository;

namespace SistemaGestionWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        [HttpGet("{idUsuario}")]
        public void ObtenerVentas(int idUsuario)
        {
            VentaRepository.obtenerVentas(idUsuario);
        }


        [HttpPost("{idUsuario}")]
        public void InsertarVentas(List<Producto> productos, int idUsuario)
        {
            VentaRepository.CargarVenta(idUsuario, productos);
        }
    }
}
