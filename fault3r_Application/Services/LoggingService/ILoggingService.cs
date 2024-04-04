using System.Threading.Tasks;

namespace fault3r_Application.Services.LoggingService
{
    public interface ILoggingService
    {
        Task AddAccountLogAsync(string email, string title);
    }
}
