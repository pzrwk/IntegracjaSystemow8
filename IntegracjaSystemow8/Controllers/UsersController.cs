using IntegracjaSystemow8.Entities;
using IntegracjaSystemow8.Model;
using IntegracjaSystemow8.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntegracjaSystemow8.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpPost("authenticate")]
    public IActionResult Authenticate(AuthenticationRequest request)
    {
        var response = userService.Authenticate(request);

        if(response == null)
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }

        return Ok(response);
    }

    [HttpGet("getAllUsers")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult getAllUsers()
    {
        var response = userService.GetUsers().ToList();

        if(response.Count < 1)
        {
            return BadRequest(new { message = "There are no users available" });
        }

        return Ok(response);
    }

    [HttpGet("userCount")]
    [Authorize(Roles = "user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult countUsers()
    {
        var response = userService.GetUsers().ToList().Count;

        if (response < 1)
        {
            return BadRequest(new { message = "There are no users available" });
        }

        return Ok(response);
    }
}
