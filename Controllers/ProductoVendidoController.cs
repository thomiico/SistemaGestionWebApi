using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionWebApi.Models;
using SistemaGestionWebApi.Repository;

namespace SistemaGestionWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoVendidoController : ControllerBase
    {
        [HttpGet("{idUsuario}")]
        public List<Producto> ObtenerProductoVendidoPorUsuarioId(long idUsuario)
        {

            return ProductoVendidoRepository.ObtenerProductoVendido(idUsuario);
        }
    }
}
