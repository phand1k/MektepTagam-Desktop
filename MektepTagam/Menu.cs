using MektepTagam.Data;
using MektepTagam.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace MektepTagam
{
    public partial class Menu : Form
    {
        private readonly ApplicationDbContext context;
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _apiBaseUrl = "https://mekteptagam-001-site1.gtempurl.com/api/CashBox";
        public Menu()
        {
            InitializeComponent();
            this.Load += Menu_Load;
            this.codeTextBox.KeyPress += CodeTextBox_KeyPress;
            context = new ApplicationDbContext();
        }
        private void CodeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверка на окончание ввода (обычно сканеры настраиваются для отправки Enter после сканирования)
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Предотвращение звукового сигнала
                e.Handled = true;

                // Нажатие кнопки
                enterTokenButton.PerformClick();
            }
        }
        private void Menu_Load(object sender, EventArgs e)
        {
            // Установка фокуса на textBox1 при загрузке формы
            codeTextBox.Focus();
        }
        private async void enterTokenButton_Click(object sender, EventArgs e)
        {
            var cardCode = await context.CardCodes.Where(x => x.AspNetUserId == codeTextBox.Text || x.Code == codeTextBox.Text).FirstOrDefaultAsync(c => c.IsDeleted == false);
            if (cardCode == null)
            {
                MessageBox.Show("Карта не найдена или удалена.");
                return;
            }
            if (context.Transactions == null)
            {
                throw new InvalidOperationException("context.Transactions is null.");
            }
            double? summ = context.Transactions.Include(x => x.CardCode)
                .Where(x => x.IsDeleted == false && (x.CardCode.Code == codeTextBox.Text || x.CardCode.AspNetUserId == codeTextBox.Text))
                .Sum(x => (double?)x.Amount) ?? 0.0;


            var dishes = await context.Dishes.Where(x => x.Price <= summ).ToListAsync();

            DishSelectionForm selectionForm = new DishSelectionForm(dishes, (double)summ, cardCode.Id);
            if (selectionForm.ShowDialog() == DialogResult.OK)
            {
                var selectedDishes = selectionForm.SelectedDishes;
                double totalSum = (double)selectedDishes.Sum(d => d.Price);

                // Здесь код для отправки выбранных блюд на сервер
                MessageBox.Show($"Выбрано блюд на сумму: {totalSum:C2}");

                // Пример кода для отправки данных на сервер (псевдокод)
                // await SendSelectedDishesToServer(selectedDishes);
            }
            codeTextBox.Clear();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Показываем MessageBox с кнопками "Да" и "Нет"
                var result = MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Если пользователь нажимает "Нет", отменяем закрытие формы
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private async void refreshDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Показываем сообщение пользователю о начале процесса обновления
                var token = await context.Tokens.FirstOrDefaultAsync(x => x.IsDeleted == false);
                MessageBox.Show("Начинаем обновление данных...");

                // Вызов метода синхронизации данных для Gender и City
                await SyncData.SyncEntityDataFromApiAsyncGuid<CardCode>(context, "GetAllCardCodes");
                await SyncData.SyncEntityDataFromApiAsyncGuid<TransactionMT>(context, "GetAllTransactions");
                await SyncData.SyncEntityDataFromApiAsyncGuid<Dish>(context, "GetAllDishes");
                await SendData.ResendUnsentTransactions(context);

                await context.SaveChangesAsync();
                // Сохраняем изменения в локальной БД

                // Показываем сообщение пользователю об успешном завершении процесса обновления
                MessageBox.Show("Обновление данных завершено успешно.");
            }
            catch (Exception ex)
            {
                // Логгирование ошибки (в вашем случае показываем MessageBox)
                MessageBox.Show($"Произошла ошибка при обновлении данных: {ex.Message}");
            }
        }

        private void cashRegisterReportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Menu_Load_1(object sender, EventArgs e)
        {

        }

        public static async Task<List<T>> GetDataFromApiAsync<T>(string url)
        {
            try
            {
                // Выполняем HTTP GET запрос к указанному URL
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Генерирует исключение, если HTTP-ответ не был успешным

                // Читаем содержимое ответа как строку
                var content = await response.Content.ReadAsStringAsync();

                // Десериализуем JSON в список объектов типа T
                var result = JsonConvert.DeserializeObject<List<T>>(content);

                return result ?? new List<T>(); // Возвращаем результат или новый пустой список, если результат null
            }
            catch (HttpRequestException e)
            {
                // Обработка ошибок HTTP запроса
                Console.WriteLine($"Ошибка запроса: {e.Message}");
                throw;
            }
            catch (JsonException e)
            {
                // Обработка ошибок десериализации JSON
                Console.WriteLine($"Ошибка десериализации JSON: {e.Message}");
                throw;
            }
        }
    }
}
