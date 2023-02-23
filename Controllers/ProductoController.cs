using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionWebApi.Repository;
using SistemaGestionWebApi.Models;

namespace SistemaGestionWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        [HttpGet("/producto/{descripciones}")]
        public Producto ObtenerProductoPorDescripcion(string descripciones)
        {
            Producto producto = ProductoRepository.obtenerProductoPorDescripcion(descripciones);
            return producto;
        }


        [HttpPost]
        public Producto CrearProducto(Producto producto)
        {
            ProductoRepository.CrearProducto(producto);
            return producto;
        }

        [HttpPut("{id}")]
        public string ModificarProducto(Producto producto, long id)
        {
            return ProductoRepository.ModificarProducto(producto, id) == 1 ? "Modificado" : "No se pudo modificar";
        }



        [HttpDelete("{id}")]
        public string BorrarProducto(long id)
        {
            return ProductoRepository.BorrarProducto(id) == 1 ? "Eliminado" : "No se puede eliminar";
        }
    }
}
