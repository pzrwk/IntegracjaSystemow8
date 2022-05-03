using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IntegracjaSystemow8.Entities;
using IntegracjaSystemow8.Model;
using Microsoft.IdentityModel.Tokens;

namespace IntegracjaSystemow8.Services.Users;

public class UserServiceImpl : IUserService
{
    private static List<User> _users = new List<User>
    {
        new User
        {
            Id = 1,
            Username = "Andrzej",
            Password = "Andrzej",
            Roles = new List<Role>
            {
                new Role
                {
                    Role_ = "admin"
                },
                new Role
                {
                    Role_ = "user"
                }
            }
        },
        new User
        {
            Id = 2,
            Username = "Piotrek",
            Password = "Piotrek",
            Roles = new List<Role>
            {
                new Role
                {
                    Role_ = "number"
                },
                new Role
                {
                    Role_ = "user"
                }
            }
        },
        new User
        {
            Id = 3,
            Username = "Ania",
            Password = "Ania",
            Roles = new List<Role>
            {
                new Role
                {
                    Role_ = "user"
                }
            }
        }
    };
    private IConfiguration _configuration;
    public UserServiceImpl(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthenticationResponse Authenticate(AuthenticationRequest request)
    {
        User user = GetByUsername(request.Username);

        if(user == null || user.Password != request.Password)
        {
            return null;
        }

        string token = generateJwtToken(user);
        return new AuthenticationResponse(user, token);
    }

    public User GetById(int id)
    {
        return _users.FirstOrDefault(x => x.Id == id);
    }

    public User GetByUsername(string username)
    {
        return _users.FirstOrDefault(x => x.Username == username);
    }

    public IEnumerable<User> GetUsers()
    {
        return _users;
    }

    private string generateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
        var claims = new List<Claim>();

        claims.Add(new Claim("id", user.Id.ToString()));

        foreach(var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Role_));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
                )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
