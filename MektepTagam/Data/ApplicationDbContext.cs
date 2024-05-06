using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MektepTagam.Model;
using Microsoft.EntityFrameworkCore;

namespace MektepTagam.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Token>? Tokens { get; set; }
        public DbSet<CardCode>? CardCodes { get; set; }
        public DbSet<TransactionMT>? Transactions { get; set; }
        public DbSet<Dish>? Dishes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Создаем уникальное имя базы данных для каждой инсталляции
            var databaseName = $"MektepTagam";
            options.UseSqlite($"Data Source={databaseName}.db");
        }
    }
}
