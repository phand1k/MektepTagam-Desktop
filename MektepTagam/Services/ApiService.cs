using MektepTagam.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MektepTagam.Services
{
    public class ApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<string> CreateTransactionAsync(TransactionData transaction)
        {
            string url = "https://mekteptagam-001-site1.gtempurl.com/api/cashbox/createtransaction";
            string json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode(); // Проверка на успешный ответ сервера
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                // Обработка возможных ошибок при запросе
                return $"Error: {e.Message}";
            }
        }
    }
}
