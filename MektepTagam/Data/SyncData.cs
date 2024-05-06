using MektepTagam.Interfaces;
using MektepTagam.Model;
using MektepTagam.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MektepTagam.Data
{
    public static class SyncData
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _apiBaseUrl = "https://mekteptagam-001-site1.gtempurl.com/api/CashBox";
        //приемка штрих кодов с сервера
        public static async Task SyncEntityDataFromApiAsyncInt<T>(ApplicationDbContext context, string endpoint) where T : class, IEntity, new()
        {
            var jwtToken = await context.Tokens.Where(x=>x.IsDeleted == false).Select(x=>x.Value).FirstOrDefaultAsync();
            // Получаем данные из API
            var itemsFromApi = await GetDataFromApiAsyncAuth<T>($"{_apiBaseUrl}/{endpoint}", jwtToken);


            // Получаем ссылку на соответствующий DbSet в контексте
            var dbSet = context.Set<T>();

            // Проходимся по каждому элементу, полученному из API
            foreach (var itemFromApi in itemsFromApi)
            {
                // Ищем существующий объект в базе данных по ID
                var existingItem = await dbSet.FindAsync(itemFromApi.Id);

                if (existingItem != null)
                {
                    // Если объект найден, обновляем его поля данными из API
                    context.Entry(existingItem).CurrentValues.SetValues(itemFromApi);
                }
                else
                {
                    // Если объект не найден, добавляем новый объект в базу данных
                    dbSet.Add(itemFromApi);
                }
            }

            // Сохраняем изменения в базе данных
            await context.SaveChangesAsync();
        }
        //приемка транзакций с сервера
        public static async Task SyncEntityDataFromApiAsyncGuid<T>(ApplicationDbContext context, string endpoint) where T : class, IEntityString, new()
        {
            var jwtToken = await context.Tokens.Where(x=>x.IsDeleted == false).Select(x=>x.Value).FirstOrDefaultAsync();
            // Получаем данные из API
            var itemsFromApi = await GetDataFromApiAsyncAuth<T>($"{_apiBaseUrl}/{endpoint}", jwtToken);


            // Получаем ссылку на соответствующий DbSet в контексте
            var dbSet = context.Set<T>();

            // Проходимся по каждому элементу, полученному из API
            foreach (var itemFromApi in itemsFromApi)
            {
                if (itemFromApi is TransactionMT transaction)
                {
                    transaction.IsSent = true;
                }
                // Ищем существующий объект в базе данных по ID
                var existingItem = await dbSet.FindAsync(itemFromApi.Id);

                if (existingItem != null)
                {
                    // Если объект найден, обновляем его поля данными из API
                    context.Entry(existingItem).CurrentValues.SetValues(itemFromApi);
                }
                else
                {
                    // Если объект не найден, добавляем новый объект в базу данных
                    dbSet.Add(itemFromApi);
                }
            }

            // Сохраняем изменения в базе данных
            await context.SaveChangesAsync();
        }
        public static async Task<List<T>> GetDataFromApiAsyncAuth<T>(string endpoint, string jwtToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            // Добавляем параметр запроса к URL
            var builder = new UriBuilder($"{endpoint}");
            var query = HttpUtility.ParseQueryString(builder.Query);
            builder.Query = query.ToString();
            string urlWithParams = builder.ToString();

            var response = await _httpClient.GetAsync(urlWithParams);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                var data = JsonConvert.DeserializeObject<List<T>>(json);
                return data ?? new List<T>();
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
                throw;
            }
        }

    }
}
