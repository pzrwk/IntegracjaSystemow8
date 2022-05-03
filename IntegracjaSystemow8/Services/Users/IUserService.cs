using IntegracjaSystemow8.Entities;
using IntegracjaSystemow8.Model;

namespace IntegracjaSystemow8.Services.Users;

public interface IUserService
{
    AuthenticationResponse Authenticate(AuthenticationRequest request);
    IEnumerable<User> GetUsers();
    User GetByUsername(string username);
    User GetById(int id);
}
