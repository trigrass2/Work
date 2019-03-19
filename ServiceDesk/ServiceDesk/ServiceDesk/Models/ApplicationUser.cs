using System.ComponentModel;

namespace ServiceDesk.Models
{
    public class ApplicationUser : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }
        public string UserName { get; set; }
    }
}
