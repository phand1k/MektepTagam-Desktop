using MektepTagam.Data;
using MektepTagam.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MektepTagam.Interfaces;

namespace MektepTagam.Model
{
    public static class UpdateData
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _apiBaseUrl = "https://mekteptagam-001-site1.gtempurl.com/api/CashBox";
        private static readonly string _doctorApiBaseUrl = "https://mekteptagam-001-site1.gtempurl.com/api/CashBox";
        private static readonly ApplicationDbContext context;
        static UpdateData()
        {
            context = new ApplicationDbContext();
        }
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
                    MessageBox.Show($"Ошибка при отправке данных транзакции {transaction.Amount}: {ex.Message}");
                }
            }

            // Сохраняем изменения в базе данных
            await context.SaveChangesAsync();
        }

        private static async Task<bool> SendTransactionDataToServer(TransactionMT transaction, string jwtToken)
        {
            TransactionMT patientToSend = new TransactionMT
            {
                // Копируем все свойства...
                CardCodeId = transaction.CardCodeId,
                Amount = transaction.Amount
            };

            string json = JsonConvert.SerializeObject(patientToSend);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_doctorApiBaseUrl}/CreateTransaction"))
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
                    MessageBox.Show($"Не удалось отправить данные пациента: {responseBody}");
                    return false;
                }
            }
        }



        public static async Task SyncEntityDataFromApiAsync<T>(ApplicationDbContext context, string endpoint) where T : class, IEntity, new()
        {
            // Получаем данные из API
            var itemsFromApi = await GetDataFromApiAsync<T>($"{_doctorApiBaseUrl}/{endpoint}");


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

        public static async Task SyncEntityDataFromApiAsyncString<T>(ApplicationDbContext context, string endpoint) where T : class, IEntityString, new()
        {
            // Получаем данные из API
            var itemsFromApi = await GetDataFromApiAsync<T>($"{_apiBaseUrl}/{endpoint}");


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
        public static async Task SyncDataAsync(ApplicationDbContext context)
        {
            if (!context.CardCodes.Any())
            {
                var codes = await GetDataFromApiAsync<CardCode>("CardCodes");
                context.CardCodes.AddRange(codes);
                await context.SaveChangesAsync();
            }
            if (!context.Transactions.Any())
            {
                var transactions = await GetDataFromApiAsync<TransactionMT>("GetTransactions/?code=123");
                context.Transactions.AddRange(transactions);
                await context.SaveChangesAsync();
            }
        }
        public static async Task<List<T>> GetDataFromApiAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            // Попытка десериализации JSON напрямую в List<T>
            try
            {
                var data = JsonConvert.DeserializeObject<List<T>>(json);
                return data ?? new List<T>();
            }
            catch (JsonSerializationException ex)
            {
                // Логгирование или другая обработка ошибок
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
                throw;
            }
        }
        public static async Task<List<T>> GetDataFromApiAsyncAuth<T>(string endpoint, int organizationId, string jwtToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            // Добавляем параметр запроса к URL
            var builder = new UriBuilder($"{_apiBaseUrl}/{endpoint}");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["OrganizationId"] = organizationId.ToString();
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
