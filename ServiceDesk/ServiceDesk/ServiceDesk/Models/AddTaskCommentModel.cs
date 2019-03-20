using System.ComponentModel;

namespace ServiceDesk.Models
{
    /// <summary>
    /// Комментарий для отправки
    /// </summary>
    public class AddTaskCommentModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ID заявки, к которой отправляется комментарий
        /// </summary>
        public int Task_id { get; set; }

        /// <summary>
        /// Текст комменьария
        /// </summary>
        public string Text { get; set; }
    }
}
