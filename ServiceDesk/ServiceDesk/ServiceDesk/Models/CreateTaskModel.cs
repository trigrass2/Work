using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ServiceDesk.Models
{
    /// <summary>
    /// Модель создания заявки
    /// </summary>
    public class CreateTaskModel : BaseTask
    {    
        /// <summary>
        /// Прикрепленные файлы
        /// </summary>
        public ObservableCollection<AttachmentFileModel> Attachments { get; set; }
    }    
}
