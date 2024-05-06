using MektepTagam.Data;
using Microsoft.EntityFrameworkCore;
using MektepTagam.Model;
namespace MektepTagam
{
    public static class Program
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _apiBaseUrl = "https://avtomigapi-001-site1.itempurl.com/api/Admin";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            using (var context = new ApplicationDbContext())
            {
                context.Database.EnsureCreated();

                // Загрузка и синхронизация данных

                var tokenExists = context.Tokens.Where(x => x.IsDeleted == false).AsNoTracking().Any();
                if (tokenExists)
                {
                    Application.Run(new Menu());
                }
                else
                {
                    Application.Run(new loginForm());
                }
            }
        }
    }
}