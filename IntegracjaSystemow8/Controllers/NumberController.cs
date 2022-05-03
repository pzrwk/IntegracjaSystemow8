using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntegracjaSystemow8.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NumberController : ControllerBase
{
    [HttpGet("getNumber")]
    [Authorize(Roles = "number", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult getNumber()
    {
        int[] numbers = { 2, 3, 5, 7, 11, 13 };
        Random rand = new Random();
        int n = rand.Next(6);

        var response = numbers[n];

        return Ok(response);
    }
}
