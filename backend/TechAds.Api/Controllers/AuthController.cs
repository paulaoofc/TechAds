using Microsoft.AspNetCore.Mvc;
using MediatR;
using TechAds.Application.Commands;
using TechAds.Api.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechAds.Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace TechAds.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(IMediator mediator, IConfiguration configuration, UserManager<IdentityUser> userManager)
    {
        _mediator = mediator;
        _configuration = configuration;
        _userManager = userManager;
    }

    [HttpPost("register-simple")]
    public async Task<IActionResult> RegisterSimple([FromBody] RegisterRequest request)
    {
        try
        {
            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
            }

            return Ok(new { Message = "User registered successfully", UserId = user.Id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(new UserDto 
            { 
                Id = Guid.Parse(user.Id), 
                Email = user.Email!, 
                DisplayName = user.UserName!,
                Role = TechAds.Domain.Enums.Role.Candidate // Default role
            });

            return Ok(new { Token = token, User = new UserDto 
            { 
                Id = Guid.Parse(user.Id), 
                Email = user.Email!, 
                DisplayName = user.UserName!,
                Role = TechAds.Domain.Enums.Role.Candidate
            }});
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new { Message = "API is working!" });
    }

    private string GenerateJwtToken(UserDto user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}