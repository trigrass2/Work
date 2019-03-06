using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ServiceDesk.Models
{
    public class EditTaskCommentModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ID заявки
        /// </summary>
        public int Task_id { get; set; }

        /// <summary>
        /// Номер комментария
        /// </summary>
        public int Comment_num { get; set; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Text { get; set; }
    }
}
