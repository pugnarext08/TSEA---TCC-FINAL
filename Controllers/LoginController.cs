using TSEA.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private readonly AppDbContext _context;

    public LoginController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest login)
    {
        var user = _context.Usuarios
            .FirstOrDefault(x =>
                x.Email == login.Email &&
                x.Senha == login.Senha);

        if (user == null)
            return Unauthorized("Login inválido");

        return Ok(new
        {
            user.Id,
            user.Nome,
            user.Email,
            user.Perfil
        });
    }
}