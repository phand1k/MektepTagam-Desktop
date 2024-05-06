using MektepTagam.Data;
using MektepTagam.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MektepTagam
{
    public partial class DishSelectionForm : Form
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private Dictionary<Dish, int> selectedDishes = new Dictionary<Dish, int>();
        private double maxSum;
        private readonly ApplicationDbContext context;
        private List<Dish> availableDishes; // Поле должно быть объявлено здесь
        private Guid cardCodeId;
        public List<Dish> SelectedDishes { get; private set; }
        private TextBox searchTextBox;
        private void DishSelectionForm_Load(object sender, EventArgs e)
        {
        }
        public DishSelectionForm(List<Dish> dishes, double maxSum, Guid cardCodeId)
        {
            InitializeComponent();
            dataGridViewDishes.CellClick += DataGridViewDishes_CellClick;
            context = new ApplicationDbContext();
            this.availableDishes = dishes;
            this.maxSum = maxSum;
            this.cardCodeId = cardCodeId;

            InitializeDataGridView();
            searchTextBox = new TextBox();
            searchTextBox.Dock = DockStyle.Top;
            searchTextBox.PlaceholderText = "Поиск блюда по наименованию...";
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            this.Controls.Add(searchTextBox); // Добавьте searchTextBox на форму
            var balance = context.Transactions.Where(x => x.CardCodeId == cardCodeId).Sum(x => x.Amount);
            balanceInfoLabel.Text = $"Текущий баланс: {balance}";
        }
        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            // Получение текста для поиска
            string searchText = searchTextBox.Text.ToLower();

            // Фильтрация списка блюд по тексту поиска
            var filteredDishes = availableDishes
                .Where(d => d.Name.ToLower().Contains(searchText))
                .ToList();

            // Обновление DataGridView
            UpdateDataGridView(filteredDishes);
        }
        private void UpdateDataGridView(List<Dish> dishes)
        {
            dataGridViewDishes.Rows.Clear();

            foreach (var dish in dishes)
            {
                dataGridViewDishes.Rows.Add(dish.Name, dish.Price.ToString("F2"), 0);
            }
        }

        private void UpdateTotalSumDisplay()
        {
            double totalSum = 0;
            foreach (DataGridViewRow row in dataGridViewDishes.Rows)
            {
                double price = Convert.ToDouble(row.Cells["Price"].Value);
                int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                totalSum += price * quantity;
            }
            labelTotalSum.Text = $"Текущая сумма: {totalSum:C2}";
        }






        private async Task<bool> SaveTransactionToLocalDb(TransactionData transactionData)
        {
            var transaction = new TransactionMT
            {
                Id = Guid.NewGuid(), // Генерируем новый GUID для транзакции
                Amount = transactionData.Amount,
                CardCodeId = transactionData.CardCodeId,
                DishId = transactionData.DishId,
                DateOfCreatedTransaction = DateTime.Now,
                IsSent = false // Устанавливаем флаг отправки в false
            };

            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

            transactionData.Id = transaction.Id; // Обновляем ID в исходном объекте

            return true;
        }

        private async void buttonConfirm_Click(object sender, EventArgs e)
        {

            buttonConfirm.Enabled = false;
            var token = await context.Tokens.Where(x => x.IsDeleted == false).FirstOrDefaultAsync();
            if (selectedDishes.Count > 0)
            {

                string jwtToken = token.Value; // Получите ваш токен

                foreach (var entry in selectedDishes)
                {
                    Dish dish = entry.Key;
                    int quantity = entry.Value;

                    for (int i = 0; i < quantity; i++)
                    {
                        TransactionData transaction = new TransactionData
                        {
                            Amount = -dish.Price,
                            CardCodeId = cardCodeId,
                            DishId = dish.Id
                        };

                        bool dbResult = await SaveTransactionToLocalDb(transaction); // Сохраняем в локальную БД
                        if (dbResult)
                        {
                            bool result = await SendTransactionAsync(transaction, jwtToken);
                            UpdateTransactionStatus(dish, result);
                        }
                    }
                }
                buttonConfirm.Enabled = true;
                this.Hide();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }

            this.Close();
        }




        public static async Task<bool> SendTransactionAsync(TransactionData transaction, string jwtToken)
        {
            string url = "https://mekteptagam-001-site1.gtempurl.com/api/cashbox/createtransaction";
            string json = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void InitializeDataGridView()
        {
            dataGridViewDishes.Columns.Clear();
            dataGridViewDishes.Rows.Clear();

            dataGridViewDishes.Columns.Add("Name", "Название блюда");
            dataGridViewDishes.Columns.Add("Price", "Цена");
            dataGridViewDishes.Columns.Add("Quantity", "Количество");
            var colAdd = new DataGridViewButtonColumn() { Name = "Add", HeaderText = "Добавить", Text = "+", UseColumnTextForButtonValue = true };
            var colRemove = new DataGridViewButtonColumn() { Name = "Remove", HeaderText = "Удалить", Text = "-", UseColumnTextForButtonValue = true };
            dataGridViewDishes.Columns.AddRange(new DataGridViewColumn[] { colAdd, colRemove });

            foreach (var dish in availableDishes)
            {
                dataGridViewDishes.Rows.Add(dish.Name, dish.Price.ToString("F2"), 0);
            }
        }

        private void UpdateSelectedDishesFromGrid()
        {
            selectedDishes.Clear();
            foreach (DataGridViewRow row in dataGridViewDishes.Rows)
            {
                if (row.Cells["Name"].Value != null)
                {
                    var dishName = row.Cells["Name"].Value.ToString();
                    var dish = availableDishes.FirstOrDefault(d => d.Name == dishName);
                    if (dish != null)
                    {
                        int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                        if (quantity > 0)
                        {
                            selectedDishes[dish] = quantity;
                        }
                    }
                }
            }
        }



        private void DataGridViewDishes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string quantityStr = dataGridViewDishes.Rows[e.RowIndex].Cells["Quantity"].Value?.ToString() ?? "0";
                if (int.TryParse(quantityStr, out int quantity))
                {
                    double price = Convert.ToDouble(dataGridViewDishes.Rows[e.RowIndex].Cells["Price"].Value.ToString());
                    double newTotalSum = GetCurrentTotalSum() + price;

                    if (e.ColumnIndex == dataGridViewDishes.Columns["Add"].Index)
                    {
                        if (newTotalSum <= maxSum) // Проверка, не превышает ли сумма максимально допустимый баланс
                        {
                            dataGridViewDishes.Rows[e.RowIndex].Cells["Quantity"].Value = ++quantity;
                        }
                        else
                        {
                            MessageBox.Show("Вы не можете выбрать блюда на сумму больше текущего баланса.");
                        }
                    }
                    else if (e.ColumnIndex == dataGridViewDishes.Columns["Remove"].Index && quantity > 0)
                    {
                        dataGridViewDishes.Rows[e.RowIndex].Cells["Quantity"].Value = --quantity;
                    }

                    UpdateTotalSumDisplay();
                    UpdateSelectedDishesFromGrid();
                }
                else
                {
                    MessageBox.Show("Некорректное значение в столбце 'Количество'.");
                }
            }
        }
        private double GetCurrentTotalSum()
        {
            double totalSum = 0;
            foreach (DataGridViewRow row in dataGridViewDishes.Rows)
            {
                if (row.Cells["Price"].Value != null && row.Cells["Quantity"].Value != null)
                {
                    double price = Convert.ToDouble(row.Cells["Price"].Value.ToString());
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                    totalSum += price * quantity;
                }
            }
            return totalSum;
        }









        private void UpdateTransactionStatus(Dish dish, bool isSuccess)
        {
            var transaction = context.Transactions.FirstOrDefault(t => t.DishId == dish.Id && t.IsSent == false);
            if (transaction != null)
            {
                transaction.IsSent = isSuccess;
                context.SaveChanges();
            }
        }
    }
}
