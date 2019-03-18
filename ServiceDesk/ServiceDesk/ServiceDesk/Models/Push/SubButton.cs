
namespace ServiceDesk.Models.Push
{
    /// <summary>
    /// Tag
    /// </summary>
    public class SubButton
    {
        /// <summary>
        /// key
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// value
        /// </summary>
        public string Label { get; set; }

        public SubButton(string id, string label)
        {
            
            Id = id;
            Label = label;
        }
    }
}
