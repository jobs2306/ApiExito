using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiExito.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "AdminRole")]
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto model)
        {
            if (string.IsNullOrEmpty(model.RoleName))
            {
                return BadRequest("El nombre del rol es obligatorio.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (roleExists)
            {
                return BadRequest("El rol ya existe.");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
            if (result.Succeeded)
            {
                return Ok($"Rol '{model.RoleName}' creado exitosamente.");
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "AdminRole")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            // Verificar si ya existe un usuario con ese email
            var existente = await _userManager.FindByEmailAsync(model.Email);
            if (existente != null)
            {
                return BadRequest("El usuario ya existe");
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            // Verificar si el rol existe y asignarlo
            if (!string.IsNullOrEmpty(model.Role))
            {
                var roleExists = await _userManager.IsInRoleAsync(user, model.Role);
                if (!roleExists)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
            }

            return Ok("Usuario registrado exitosamente con rol: " + model.Role);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized("Usuario o contraseña incorrectos");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized("Usuario o contraseña incorrectos");

            // Generar token JWT
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }.Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize]
        [HttpGet("test-token")]
        public IActionResult TestToken()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }

        [Authorize]
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        [Authorize]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }

    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RoleDto
    {
        public string RoleName { get; set; }
    }
}