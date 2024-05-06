using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MektepTagam.Model;
using MektepTagam.Interfaces;
using MektepTagam.Services;
using System.Web;

namespace MektepTagam.Data
{
    public static class SendData
    {
        private static readonly string _apiBaseUrl = "https://mekteptagam-001-site1.gtempurl.com/api/CashBox";
        private static readonly HttpClient _httpClient = new HttpClient();
        public static async Task ResendUnsentTransactions(ApplicationDbContext context)
        {
            // Получаем список всех пациентов, которые не были отправлены на сервер
            var unsentTransactions = context.Transactions.Where(p => p.IsSent == false).ToList();
            var jwtToken = await context.Tokens.Where(x => x.IsDeleted == false).FirstOrDefaultAsync();
            // Перебираем каждого пациента из списка
            foreach (var transaction in unsentTransactions)
            {
                try
                {
                    // Пытаемся отправить данные пациента на сервер
                    bool success = await SendTransactionDataToServer(transaction, jwtToken.Value);
                    if (success)
                    {
                        // Если данные успешно отправлены, обновляем статус в локальной базе данных
                        transaction.IsSent = true;
                        context.Entry(transaction).State = EntityState.Modified;
                    }
                }
                catch (Exception ex)
                {
                    // Логгирование ошибки
                    MessageBox.Show($"Ошибка при отправке данных о транзакции: {transaction.Id}: {ex.Message}");
                }
            }

            // Сохраняем изменения в базе данных
            await context.SaveChangesAsync();
        }

        public static async Task SyncEntityDataFromApiAsyncInt<T>(ApplicationDbContext context, string endpoint) where T : class, IEntity, new()
        {
            var jwtToken = await context.Tokens.Where(x => x.IsDeleted == false).Select(x => x.Value).FirstOrDefaultAsync();
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
            var jwtToken = await context.Tokens.Where(x => x.IsDeleted == false).Select(x => x.Value).FirstOrDefaultAsync();
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
        public static async Task<List<T>> GetDataFromApiAsyncAuth<T>(string endpoint, string jwtToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            // Добавляем параметр запроса к URL
            var builder = new UriBuilder($"{_apiBaseUrl}/{endpoint}");
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
        public static async Task<bool> SendTransactionDataToServer(TransactionMT transaction, string jwtToken)
        {
            TransactionMT transactionToSend = new TransactionMT
            {
                Id = transaction.Id,
                CardCodeId = transaction.CardCodeId,
                Amount = transaction.Amount,
                DishId = transaction.DishId
            };

            string json = JsonConvert.SerializeObject(transactionToSend);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_apiBaseUrl}/CreateTransaction"))
            {
                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    // Возвращаем true, если статус-код успеха
                    return true;
                }
                else
                {
                    // Логируем ошибку и читаем тело ответа
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Не удалось отправить данные о транзакции: {responseBody}");
                    return false;
                }
            }
        }
    }
}
