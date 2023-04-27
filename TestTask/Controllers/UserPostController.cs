using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TestTask.Interfaces;
using TestTask.Models;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPostController: Controller
    {
        private readonly IUserRepository _userRepository;

        public UserPostController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("CreateUser")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateNewUser(string initiatorLogin, string initiatorPassword,
             string userLogin, string userPassword, string userName,
            int userGender, bool isAdmin, DateTime? userBday = null)
        {
            
            var adminCheck = _userRepository.IsAdmin(initiatorLogin, initiatorPassword);
            if (!adminCheck)
            {
                return StatusCode(403, "Вы не являетесь администратором");
            }
            if (!_userRepository.IsUniqueUser(userLogin))
            {
                return StatusCode(422, "Пользователь уже существует");
            }
            var user = new User(initiatorLogin, userLogin, userPassword, userName,
                userGender, isAdmin, userBday);
            if (!TryValidateModel(user))
                return BadRequest("Некорректно введены данные");
            if (!_userRepository.CreateUser(user))
            {
                return StatusCode(500, "Что-то пошло не так во время сохранения данных");
            }
            if (!ModelState.IsValid)
                return BadRequest("Некорректно введены данные");
            return Ok("Пользователь успешно создан");
        }
    }
}
