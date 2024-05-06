using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using MektepTagam.Data;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using MektepTagam.Model;

namespace MektepTagam
{
    public partial class loginForm : Form
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext context;
        public loginForm()
        {
            InitializeComponent();
            versionLabel.Text = "v1.0 Mektep Tagam";
            context = new ApplicationDbContext();
            context.Database.EnsureCreated();
            _httpClient = new HttpClient();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {

        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            var loginRequest = new
            {
                phoneNumber = loginTextBox.Text, // Предполагается, что textBox1 содержит номер телефона
                password = passwordTextBox.Text
            };
            var apiUrl = "https://mekteptagam-001-site1.gtempurl.com/api/Authenticate/login"; // Используем правильный протокол (http, если API не поддерживает https)
            var json = JsonConvert.SerializeObject(loginRequest);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Используем PostAsync для отправки POST-запроса
                var response = await _httpClient.PostAsync(apiUrl, data);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var tokenData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                    if (tokenData != null && tokenData.ContainsKey("token"))
                    {
                        string token = tokenData["token"];
                        var tokenClaims = ParseJwtToken(token);

                        await SaveToken(token);
                        var handler = new JwtSecurityTokenHandler();
                        var jwtToken = handler.ReadJwtToken(token);
                        var organizationIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "OrganizationId")?.Value;
                        // Проверка на наличие данных для текущей организации
                        if (!context.CardCodes.Any())
                        {
                            var cardCodes = await UpdateData.GetDataFromApiAsyncAuth<CardCode>("GetAllCardCodes", Convert.ToInt32(organizationIdClaim), token);
                            await context.CardCodes.AddRangeAsync(cardCodes);
                            await context.SaveChangesAsync();
                        }
                        if (!context.Dishes.Any())
                        {
                            var dishes = await UpdateData.GetDataFromApiAsyncAuth<Dish>("GetAllDishes", Convert.ToInt32(organizationIdClaim), token);
                            await context.Dishes.AddRangeAsync(dishes);
                            await context.SaveChangesAsync();
                        }
                        if (!context.Transactions.Any())
                        {
                            var transactions = await UpdateData.GetDataFromApiAsyncAuth<TransactionMT>("GetAllTransactions", Convert.ToInt32(organizationIdClaim), token);
                            foreach (var transaction in transactions)
                            {
                                transaction.IsSent = true;
                            }
                            await context.Transactions.AddRangeAsync(transactions);
                            await context.SaveChangesAsync();
                        }
                        this.Hide();
                        MessageBox.Show("Успешный вход");
                        Menu menu = new Menu();
                        menu.FormClosed += (s, args) => this.Close();
                        menu.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Not Success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private async Task SaveToken(string tokenValue)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenValue);
            var expiryUnixTimeSeconds = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            var organizationIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "OrganizationId")?.Value;
            var aspNetUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            // Преобразование OrganizationId из строки в int?
            int? organizationId = organizationIdClaim != null ? int.Parse(organizationIdClaim) : null;
            // Проверка на наличие значения истечения срока действия
            if (expiryUnixTimeSeconds != null)
            {
                var expiryDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiryUnixTimeSeconds)).DateTime;

                var token = new Token
                {
                    Value = tokenValue,
                    AspNetUserId = aspNetUserId,
                    DateOfEndToken = expiryDateTime,
                    OrganizationId = organizationId,

                    // Теперь используем expiryDateTime
                    // Остальные поля...
                };

                await context.Tokens.AddAsync(token);
                await context.SaveChangesAsync();
            }
        }
        private ClaimsPrincipal ParseJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var principal = new ClaimsPrincipal(new ClaimsIdentity(jsonToken.Claims));
            return principal;
        }
    }
}