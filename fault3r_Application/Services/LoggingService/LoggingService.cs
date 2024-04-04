

using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace fault3r_Application.Services.LoggingService
{

    public class LoggingService: ILoggingService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LoggingService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task AddAccountLogAsync(string email, string title)
        {            
            string directory = Path.Combine(_webHostEnvironment.WebRootPath, "log");
            string file = Path.Combine(directory, email.Replace("@", "-") + ".txt");
            string log = "log: " + title.ToString() + "\ntime: " + DateTime.Now.ToShortDateString() +
                " | " + DateTime.Now.ToShortTimeString() + "\n-------------------------\n";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            StreamWriter logger = new StreamWriter(file, true);
            await logger.WriteAsync(log);
            logger.Close();
        }
    }   
}
