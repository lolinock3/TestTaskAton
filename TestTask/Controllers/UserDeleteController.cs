using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TestTask.Interfaces;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDeleteController:Controller
    {
        private readonly IUserRepository _userRepository;

        public UserDeleteController(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult DeleteUser ([Required(ErrorMessage = "Требуется ввести логин")] string login,
            [Required(ErrorMessage = "Требуется ввести пароль")] string password,
            [Required(ErrorMessage = "Требуется ввести логин")] string userToDelete)
        {
            if (!ModelState.IsValid)
                return BadRequest("Некорректно введены данные");

            var adminCheck = _userRepository.IsAdmin(login, password);
            if (!adminCheck)
            {
                return StatusCode(403, "Вы не имеете прав для выполнения этого действия");
            }

            if (!_userRepository.DeleteUser(userToDelete, login))
                return StatusCode(404,"Данный пользователь не найден");

            return Ok("Пользователь успешно удален");
        }
    }
}
