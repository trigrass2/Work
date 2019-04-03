
namespace ServiceDesk.Models
{
    /// <summary>
    /// Учетные данные для входа
    /// </summary>
    public class User : BaseUser
    {        
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
