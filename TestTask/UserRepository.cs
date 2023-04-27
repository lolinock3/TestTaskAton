using Microsoft.EntityFrameworkCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using TestTask.DTO;
using TestTask.Interfaces;
using TestTask.Models;

namespace TestTask
{
    public class UserRepository: IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<User>? GetActiveUsers()
        {

            var users = _context.Users
                .Where(u => u.RevokedOn == null)
                .OrderBy(u => u.CreatedOn);
            return users?.ToList();
        }

        public bool IsUniqueUser(string initiaorLogin)
        {
            var a = _context.Users.FirstOrDefault(u => u.Login == initiaorLogin);
            if (a != null)
                    return false;
            return true;
        }
        public bool IsAdmin(string initiaorLogin, string initiatorPassword)
        {
            var a = _context.Users.FirstOrDefault(u => u.Login == initiaorLogin);
            if (a != null)
            {
                if (a.Password == initiatorPassword && a.IsAdmin == true)
                    return true;
            }
            return false;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0;
        }

        

        public SearchByLoginDto GetUserByLogin(string login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);
            if (user == null)
            {
                return null;
            }
            return new SearchByLoginDto(user);
        }

        public User GetUserByLoginPassword(string login, string password)
        {
            var user = _context.Users
                .Where(u=> u.RevokedOn == null)
                .FirstOrDefault(u => u.Login == login);
            if (user == null)
                return null;
            if (user.Password != password)
                return null;
            return user;
        }

        public int GetAge(DateTime? birthDay)
        {
            if (birthDay == null)
                return 0;
            DateTime now = DateTime.Now;
            DateTime bday = (DateTime)birthDay;
            int age = now.Year - bday.Year;
            if (bday > now.AddYears(-age))
            {
                age--;
            }
            return age;
        }
        /* Реализовано так, что возраст пользователя,
         у котрого Дата рождения равна null, считается равным 0 */
        public ICollection<User>? GetUsersByAge(int ageRestriction)
        {
            var users = _context.Users
                .AsEnumerable()
                .Where(u => GetAge(u.Birthday) >= ageRestriction)
                .ToList();
            return users;
        }

        public bool UpdateUserLogin(string changerLogin, string initialLogin, string newLogin)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == initialLogin);
            if (user == null)
            {
                return false;
            }
            user.Login = newLogin;
            user.ModifiedBy = changerLogin;
            user.ModifiedOn = DateTime.Now;
            return Save();
        }

        public bool UpdateUserPassword(string changerLogin, string login, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);
            if (user == null)
            {
                return false;
            }
            user.ModifiedBy = changerLogin;
            user.ModifiedOn = DateTime.Now;
            user.Password = newPassword;
            return Save();
        }

        public bool UpdateUserInfo(string changerLogin, string login, string? name, int? gender, DateTime? bday)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);
            if (user == null)
            {
                return false;
            }
            if (name != null)
            {
                user.Name = name;
            }
            if (gender != null)
            {
                user.Gender = (int)gender;
            }
            if (bday != null)
            {
                user.Birthday = bday;
            }
            user.ModifiedBy = changerLogin;
            user.ModifiedOn = DateTime.Now;
            return Save();
        }

        public bool DeleteUser(string login, string removerLogin)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login && u.RevokedOn == null);
            if (user == null)
            {
                return false;
            }
            user.RevokedOn = DateTime.Now;
            user.RevokedBy = removerLogin;
            return Save();
        }

        public bool RecoverUser(string login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login && u.RevokedOn != null);
            if (user == null)
            {
                return false;
            }
            user.RevokedOn = null;
            user.RevokedBy = null;
            return Save();
        }
    }
}
