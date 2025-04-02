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

        [HttpGet("cc/{cc}")]
        public async Task<ActionResult<Cliente>> GetByCC(int cc)
        {
            var objeto = await _Service.GetByCedulaAsync(cc);
            if (objeto == null) return NotFound();
            return Ok(objeto);
        }

        [HttpGet("nit/{nit}")]
        public async Task<ActionResult<Cliente>> GetByNit(string nit)
        {
            var objeto = await _Service.GetByNitAsync(nit);
            if (objeto == null) return NotFound();
            return Ok(objeto);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Create([FromBody] Cliente cliente)
        {
            //Verificaciones
            if (string.IsNullOrEmpty(cliente.nit) && (cliente.cc == 0 || cliente.cc == null))
                return BadRequest("Debe indicar un Nit o CC");

            //Verificacion por cc
            if(cliente.cc != null && cliente.cc != 0)
            {
                var ClienteToCreate = await _Service.GetByCedulaAsync(cliente.cc);
                if (ClienteToCreate != null)
                    return Conflict("El cliente ya existe");
            }

            //Verificacion por nit
            if(!string.IsNullOrEmpty(cliente.nit))
            {
                var ClienteToCreate = await _Service.GetByNitAsync(cliente.nit);
                if (ClienteToCreate != null)
                    return Conflict("El cliente ya existe");
            }

            if (string.IsNullOrEmpty(cliente.nombre))
                return BadRequest("Debe indicar un nombre");

            if (cliente.cc != 0 && cliente.cc != null && !string.IsNullOrEmpty(cliente.nit))
                return BadRequest("No puede tener CC y NIT a la vez, debe indicar uno solo");

            var newObject = await _Service.AddAsync(cliente);
            return CreatedAtAction(nameof(GetById), new { id = newObject.id }, newObject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cliente cliente)
        {
            //Antes de actualizar verificar que exista
            var client = await _Service.GetByIdAsync(id);

            //Debido a que el cliente solo puede tener NIT o CC se verifica eso
            //Basicamente si antes tenia CC y se quiere setear un NIT entonces 
            //se elimina el CC que estaba antes para evitar la doble identificacion
            if (client != null)
            {
                if ((cliente.cc != 0 && cliente.cc != null) && (!string.IsNullOrEmpty(client.nit)))
                {
                    cliente.nit = "";
                }
                else if(!string.IsNullOrEmpty(cliente.nit) && (client.cc != 0 && client.cc != null))
                {
                    cliente.cc = 0;
                }
            }
            else
            {
                return NotFound();
            }

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
