
using System.Threading.Tasks;

namespace ServiceDesk.Models
{
    public interface IPhoneCall
    {
        Task MakeQuickCall(string PhoneNumber);
    }
}
