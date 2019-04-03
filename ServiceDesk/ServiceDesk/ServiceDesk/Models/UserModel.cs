
namespace ServiceDesk.Models
{
    /// <summary>
    /// Данные пользователя
    /// </summary>
    public class UserModel : BaseUser
    {
        public string Email { get; set; }        

        public string FullName { get; set; }
    }
}
