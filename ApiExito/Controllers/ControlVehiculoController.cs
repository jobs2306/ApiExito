using ApiExito.Model;
using ApiExito.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiExito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlVehiculoController : ControllerBase
    {
        private readonly IControlVehiculoService _Service;
        private readonly IVehiculoService _Vehiculo;

        public ControlVehiculoController(IControlVehiculoService service, IVehiculoService vehiculo)
        {
            _Service = service;
            _Vehiculo = vehiculo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ControlVehiculo>>> GetAll()
        {
            return Ok(await _Service.GetAllAsync());
        }
        
        [HttpGet("taller")]
        public  async Task<ActionResult<IEnumerable<ControlVehiculo>>> GetVehiculoTaller()
        {
            return Ok(await _Service.GetVehiculosTaller());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ControlVehiculo>> GetById(int id)
        {
            var objeto = await _Service.GetByIdAsync(id);
            if (objeto == null) return NotFound();
            return Ok(objeto);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Create([FromBody] ControlVehiculo control)
        {
            //Verificar si el vehiculo ya tiene un control de entrada
            var controles = await _Service.GetVehiculoTaller(control.Vehiculoid);

            if(controles != null)
                return Conflict("El vehiculo ya se encuentra en el taller");


            var newObject = await _Service.AddAsync(control);

            var vehiculo = await _Vehiculo.GetByIdAsync(control.Vehiculoid);
            
            if(vehiculo == null)
                return Conflict("El vehiculo relacionado no existe");


            return CreatedAtAction(nameof(GetById), new { id = newObject.id }, newObject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ControlVehiculo control)
        {
            var updated = await _Service.UpdateAsync(id, control);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
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
