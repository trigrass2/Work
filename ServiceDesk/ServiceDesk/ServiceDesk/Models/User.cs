using System.ComponentModel;

namespace ServiceDesk.Models
{
    /// <summary>
    /// Учетные данные для входа
    /// </summary>
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Login { get; set; }
        public string Password { get; set; }
    }
}
