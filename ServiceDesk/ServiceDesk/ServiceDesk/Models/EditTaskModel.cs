
namespace ServiceDesk.Models
{
    public class EditTaskModel : BaseTask
    {        
        /// <summary>
        /// ID заявки
        /// </summary>
        public int? Task_id { get; set; }
       
        /// <summary>
        /// ID статуса заявки
        /// </summary>
        public int? Status_id { get; set; }
    }
}
