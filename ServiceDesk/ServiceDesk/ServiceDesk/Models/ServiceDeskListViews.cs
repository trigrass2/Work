using System;
using System.ComponentModel;

namespace ServiceDesk.Models
{
    public class ServiceDeskListViews : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ServiceDesk_StatusListView : ServiceDeskListViews
    {
        public int Status_id { get; set; }
        public string Status_name { get; set; }
    }

    public class ServiceDesk_TaskAttachmentListView : AttachmentFileModel
    {
        
    }

    public class ServiceDesk_TypeListView : ServiceDeskListViews
    {        
        /// <summary>
        /// ID типа заявки
        /// </summary>
        public int Type_id { get; set; }

        /// <summary>
        /// Название типа заявки
        /// </summary>
        public string Type_name { get; set; }
    }
}
