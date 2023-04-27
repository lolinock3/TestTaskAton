using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TestTask.DTO;
using TestTask.Interfaces;
using TestTask.Models;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGetController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserGetController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetActiveUsers([Required]string login,
           [Required] string password)
        {
            var check = _userRepository.IsAdmin(login, password);
            if (!check)
            {
                return StatusCode(403, "Вы не являетесь администратором");
            }
            var users = _userRepository.GetActiveUsers();
            if (!ModelState.IsValid)
                return BadRequest("Данные введены некорректно");
            if (users == null)
            {
                return StatusCode(404, "Не найдено активных пользователей");
            }    
            return Ok(users);
        }
        [HttpGet ("Authentification")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetYourself ([Required] string login, [Required] string password)
        {
            if (!ModelState.IsValid)
                return BadRequest("Некорректно введены данные");
            var user = _userRepository.GetUserByLoginPassword(login, password);
            if (user == null)
            {
                return StatusCode(404, "Пользователя с таким логином и паролем не существует");
            }
            return Ok(user);

        }

        /* Реализовано так, что возраст пользователя,
         у котрого Дата рождения равна null, считается равным 0 */
        [HttpGet ("SearchByAge")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(400)]
        public IActionResult GetUsersByAge ([Required] string login,
            [Required] string password, [Required] int years)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Данные введены некорректно");
            }

            var adminCheck = _userRepository.IsAdmin(login, password);
            if (!adminCheck)
            {
                return StatusCode(403, "Вы не являеетесь администратором");
            }

            var users = _userRepository.GetUsersByAge(years);
            if (users == null)
            {
                return StatusCode(404, "Пользователей старше заданного возраста не существует");
            }
            return Ok(users);
        }

        [HttpGet ("SearchByLogin")]
        [ProducesResponseType (200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUserByLogin ([Required] string initiatorLogin,
            [Required] string initiatorPassword, [Required] string userLogin)
        {
            var adminCheck = _userRepository.IsAdmin(initiatorLogin, initiatorPassword);
            if (!adminCheck)
            {
                return StatusCode(403, "Вы не являетесь администратором");
            }
            var result = _userRepository.GetUserByLogin(userLogin);
            if (result == null)
            {
                return StatusCode(404, "Пользователь не найден");
            }
            if (!ModelState.IsValid)
                return BadRequest("Некоррекно введены данные");
            return Ok(result);
        }
    }
}
