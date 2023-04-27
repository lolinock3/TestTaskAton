using TestTask.Models;

namespace TestTask.DTO
{
    public class SearchByLoginDto
    {
        public string Name { get; set; }
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsActive { get; set; } 

        public SearchByLoginDto(User user)
        {
            Name = user.Name;
            Gender = user.Gender;
            Birthday = user.Birthday;
            IsActive = user.RevokedOn == null;
        }
    }
}
