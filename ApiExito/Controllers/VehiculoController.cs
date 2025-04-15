using ApiExito.Model;
using ApiExito.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiExito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoService _Service; 
        private readonly IClienteService _ClienteService;

        public VehiculoController(IVehiculoService service, IClienteService cliente)
        {
            _Service = service;
            _ClienteService = cliente;
        }

        [HttpGet]
        [Authorize]
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

        [HttpGet("placa/{placa}")]
        [Authorize]
        public async Task<ActionResult<Vehiculo>> GetByPlaca(string placa)
        {
            var objeto = await _Service.GetByPlacaAsync(placa);
            if (objeto == null) return NotFound();
            return Ok(objeto);
        }

        [HttpPost]
        [Authorize(Roles = "AdminRole,GeneralRole")]
        public async Task<ActionResult<Cliente>> Create([FromBody] Vehiculo vehiculo)
        {
            if (string.IsNullOrEmpty(vehiculo.placa))
                return BadRequest("Debe indicar la placa del vehiculo");

            var vehiculoToCreate = await _Service.GetByPlacaAsync(vehiculo.placa);
            if (vehiculoToCreate != null)
                return Conflict("El vehiculo ya se ha creado");

            if (string.IsNullOrEmpty(vehiculo.marca))
                return BadRequest("Debe indicar la marca del vehiculo");

            if (string.IsNullOrEmpty(vehiculo.diesel_gasolina))
                return BadRequest("Debe indicar si es Diesel o Gasolina");

            if (vehiculo.diesel_gasolina.ToLower() != "diesel" && vehiculo.diesel_gasolina.ToLower() != "gasolina")
                return BadRequest("Debe indicar si es Diesel o Gasolina");

            if (string.IsNullOrEmpty(vehiculo.modelo))
                return BadRequest("Debe indicar el modelo del vehiculo");

            var cliente = await _ClienteService.GetByIdAsync(vehiculo.Clienteid);

            if(cliente == null)
                return BadRequest("El cliente escogido no existe");

            var newObject = await _Service.AddAsync(vehiculo);
            return CreatedAtAction(nameof(GetById), new { id = newObject.id }, newObject);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "AdminRole,GeneralRole")]
        public async Task<IActionResult> Update(int id, [FromBody] Vehiculo vehiculo)
        {
            var updated = await _Service.UpdateAsync(id, vehiculo);
            if (!updated) return NotFound();
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "AdminRole,GeneralRole")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_Service.Verify(id)) return BadRequest("No se puede eliminar porque está en uso.");

            var deleted = await _Service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
