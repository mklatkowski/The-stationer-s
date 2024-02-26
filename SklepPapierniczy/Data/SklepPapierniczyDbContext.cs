using Microsoft.EntityFrameworkCore;
using SklepPapierniczy.Models;
using System.Collections.Generic;

namespace SklepPapierniczy.Data
{
    public class SklepPapierniczyDbContext : DbContext
    {
        public SklepPapierniczyDbContext(DbContextOptions<SklepPapierniczyDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ClientDelivery> ClientDeliveries { get; set; }
        public DbSet<ClientDeliveryArticle> ClientDeliveryArticles { get; set; }
        public DbSet<ShopDelivery> ShopDeliveries { get; set; }
        public DbSet<ShopDeliveryArticle> ShopDeliveryArticles { get; set; }
    }
}
