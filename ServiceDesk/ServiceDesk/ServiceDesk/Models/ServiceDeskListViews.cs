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

    public class ServiceDesk_TaskAttachmentListView : ServiceDeskListViews
    {
        public int Task_id { get; set; }
        public int Attachment_num { get; set; }
        public string Attachment_name { get; set; }
        public byte[] Attachment_bytes { get; set; }
        public DateTime Timestamp { get; set; }
        public string User_id { get; set; }
        public string User_name { get; set; }
    }
}
