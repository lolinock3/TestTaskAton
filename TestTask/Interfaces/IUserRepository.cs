using TestTask.DTO;
using TestTask.Models;

namespace TestTask.Interfaces
{
    public interface IUserRepository
    {

        ICollection<User>? GetActiveUsers();
        bool IsUniqueUser(string login);
        bool IsAdmin(string login, string password);
        SearchByLoginDto GetUserByLogin(string login);
        User GetUserByLoginPassword (string login, string password);
        ICollection<User>? GetUsersByAge(int ageRestriction);
        int GetAge(DateTime? bday);
        bool CreateUser(User user);
        bool UpdateUserLogin(string changerLogin, string initialLogin, string newLogin);
        bool UpdateUserPassword(string changerLogin, string initialLogin, string newPassword);
        bool UpdateUserInfo(string changerLogin, string initialLogin, string? name, int? gender, DateTime? bday);
        bool DeleteUser(string login, string removerLogin);
        bool RecoverUser(string login);
        bool Save();
    }
}
