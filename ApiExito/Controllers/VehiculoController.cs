using ApiExito.Model;
using ApiExito.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiExito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoService _Service;

        public VehiculoController(IVehiculoService service)
        {
            _Service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> GetAll()
        {
            return Ok(await _Service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehiculo>> GetById(int id)
        {
            var objeto = await _Service.GetByIdAsync(id);
            if (objeto == null) return NotFound();
            return Ok(objeto);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Create([FromBody] Vehiculo vehiculo)
        {
            var newObject = await _Service.AddAsync(vehiculo);
            return CreatedAtAction(nameof(GetById), new { id = newObject.id }, newObject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Vehiculo vehiculo)
        {
            var updated = await _Service.UpdateAsync(id, vehiculo);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_Service.Verify(id)) return BadRequest("No se puede eliminar porque está en uso.");

            var deleted = await _Service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}
