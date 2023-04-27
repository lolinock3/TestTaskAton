using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TestTask.Interfaces;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserUpdateContrroller: Controller
    {
        private readonly IUserRepository _userRepository;

        public UserUpdateContrroller(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPut("ChangeLogin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UserLoginUpdate ([Required(ErrorMessage = "Требуется ввести логин")] string initiatorLogin,
            [Required(ErrorMessage = "Требуется ввести пароль")] string initiatorPassword,
            [Required(ErrorMessage = "Требуется ввести новый логин"), RegularExpression (@"^[A-Za-z0-9]+$",
            ErrorMessage = "В пароле можно использовать только латинские буквы и цифры")] string loginToChange,
            [Required(ErrorMessage = "Требуется ввести логин, который вы хотите изменить"), RegularExpression (@"^[A-Za-z0-9]+$",
            ErrorMessage = "В пароле можно использовать только латинские буквы и цифры")] string newLogin)
        {
            var uniquenessCheck = _userRepository.IsUniqueUser(newLogin);
            var user = _userRepository.GetUserByLoginPassword(initiatorLogin, initiatorPassword);
            if (user == null)
                {
                return StatusCode(401, "Вы не являетесь пользователем");
            }
            if (!uniquenessCheck)
            {
                return StatusCode(406, "Пользователь с таким именем уже существует");
            }
            var adminCheck = user.IsAdmin;
            if (adminCheck == false && initiatorLogin != loginToChange)
            {
                return StatusCode(403, "Вы не имеете прав для выполнения этого действия");
            }
            if (!ModelState.IsValid)
                return BadRequest("Некорректно введены данные");
            if (!_userRepository.UpdateUserLogin(initiatorLogin, loginToChange, newLogin))
            {
                return BadRequest("Данный пользователь не найден или что-то пошло не так во время сохранения данных");
            }
            return Ok("Логин пользователя успешно изменен");

        }

        [HttpPut("ChangePassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UserPasswordUpdate([Required(ErrorMessage = "Требуется ввести логин")] string initiatorLogin,
            [Required(ErrorMessage = "Требуется ввести пароль")] string initiatorPassword,
            [Required(ErrorMessage = "Требуется ввести новый логин")] string login,
            [Required(ErrorMessage = "Требуется вsвести логин, который вы хотите изменить"), RegularExpression (@"^[A-Za-z0-9]+$",
            ErrorMessage = "В пароле можно использовать только латинские буквы и цифры")] string newPassword)
        {
            var user = _userRepository.GetUserByLoginPassword(initiatorLogin, initiatorPassword);
            if (user == null)
            {
                return StatusCode(401, "Вы не являетесь пользователем");
            }

            if (!ModelState.IsValid)
                return BadRequest("Некорректно введены данные");

            var adminCheck = user.IsAdmin;
            if (adminCheck == false && initiatorLogin != login)
            {
                return StatusCode(403, "Вы не имеете прав для выполнения этого действия");
            }

            if (!_userRepository.UpdateUserPassword(initiatorLogin, login, newPassword))
            {
                return BadRequest("Данный пользователь не найден или что-то пошло не так во время сохранения данных");
            }
            return Ok("Пароль пользователя успешно изменен");
        }

        [HttpPut("ChangeInfo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult ChangeUserInfo ([Required(ErrorMessage = "Требуется ввести логин")] string initiatorLogin,
            [Required(ErrorMessage = "Требуется ввести пароль")] string initiatorPassword,
            [Required(ErrorMessage = "Требуется ввести логин")] string userLogin,
            [RegularExpression (@"^[A-Za-z0-9]+$")] string? name,
            [Range(0,2)] int? gender,
            DateTime? bday)
        {
            if (name == null && gender == null && bday == null)
            {
                return StatusCode(400, "Вы не ввелии информацию, которую требуется изменить");
            }

            if (!ModelState.IsValid)
                return BadRequest("Некорректно введены данные");

            var user = _userRepository.GetUserByLoginPassword(initiatorLogin, initiatorPassword);
            if (user == null)
            {
                return StatusCode(401, "Вы не являетесь пользователем");
            }

            var adminCheck = user.IsAdmin;
            if (adminCheck == false && initiatorLogin != userLogin)
            {
                return StatusCode(403, "Вы не имеете прав для выполнения этого действия");
            }

            if (!_userRepository.UpdateUserInfo(initiatorLogin, userLogin, name, gender, bday))
            {
                return BadRequest("Данный пользователь не найден или что-то пошло не так во время сохранения данных");
            }
                return Ok("Данные пользователя успешно обновлены");
        }

        [HttpPut("RecoverUser")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult RecoverUser ([Required(ErrorMessage = "Требуется ввести логин")] string login,
            [Required(ErrorMessage = "Требуется ввести пароль")] string password,
            [Required(ErrorMessage = "Требуется ввести логин")] string userToRecover)
        {
            var adminCheck = _userRepository.IsAdmin(login, password);
            if(!adminCheck)
            {
                return StatusCode(403, "Вы не являетесь администратором");
            }

            if (!ModelState.IsValid)
                return BadRequest("Некорректно введены данные");

            if (!_userRepository.RecoverUser(userToRecover))
            {
                return StatusCode (404, "Данный пользователь не был удален/никогда не существовал");
            }
            return Ok("Пользователь успешно восстановлен");
        }
    }
}
