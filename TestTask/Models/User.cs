using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace TestTask.Models
{
    [Table("Пользователи")]
    public class User
    {
        [Key]
        public Guid Guid { get; set; }
        [Required(ErrorMessage = "Пароль не указан или указан некоррекнто"),RegularExpression(@"^[A-Za-z0-9]+$",
            ErrorMessage = "Имя не указано или указано некоррекнто")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Логин не указан или указан некоррекнто"), RegularExpression(@"^[A-Za-z0-9]+$",
            ErrorMessage = "Имя не указано или указано некоррекнто")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Имя не указано или указано некоррекнто"), RegularExpression(@"^[A-Za-zА-Яа-я]+$",
            ErrorMessage = "Имя не указано или указано некоррекнто")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Пол не указан или указан некоррекнто"), Range(0,2)]
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        [Required (ErrorMessage = "Приналдежность к администраторам не указана или указана некоррекнто")]
        public bool IsAdmin { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        
        public DateTime? ModifiedOn { get; set; }
        
        public string? ModifiedBy { get; set; }
        
        public DateTime? RevokedOn { get; set; }
        
        public string? RevokedBy { get; set; }

        public User()
        {
            
        }
        public User(string creatorLogin, string login, string password, string name,
            int gender, bool isAdmin, DateTime? bday = null)
        {
            Login = login;
            Password = password;
            Name = name;
            Gender = gender;
            Birthday = bday;
            IsAdmin = isAdmin;
            CreatedBy = creatorLogin;
            CreatedOn = DateTime.Now;
        }
    }
}

