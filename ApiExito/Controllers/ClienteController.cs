using ApiExito.Model;
using ApiExito.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiExito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _Service;

        public ClienteController(IClienteService service)
        {
            _Service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
        {
            return Ok(await _Service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetById(int id)
        {
            var objeto = await _Service.GetByIdAsync(id);
            if (objeto == null) return NotFound();
            return Ok(objeto);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Create([FromBody] Cliente cliente)
        {
            var newObject = await _Service.AddAsync(cliente);
            return CreatedAtAction(nameof(GetById), new { id = newObject.id }, newObject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cliente cliente)
        {
            var updated = await _Service.UpdateAsync(id, cliente);
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
