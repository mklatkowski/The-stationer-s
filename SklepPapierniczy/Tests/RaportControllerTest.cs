using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SklepPapierniczy.Controllers;
using SklepPapierniczy.Data;
using SklepPapierniczy.Models;
using Xunit;

namespace SklepPapierniczy.Tests
{
    public class RaportControllerTests
    {
        [Fact]
        public void GetSellArticles_ShouldReturnCorrectData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SklepPapierniczyDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemorySellDatabase")
                .Options;

            using (var context = new SklepPapierniczyDbContext(options))
            {
                var controller = new RaportController(context);

                var article1 = new Article { Id = 1, Name = "Article1", Quantity = 10, Price = 20.0M, CategoryId = 1 };
                var article2 = new Article { Id = 2, Name = "Article2", Quantity = 15, Price = 25.0M, CategoryId = 1 };

                var delivery1 = new ClientDelivery { Id = 1, Producent = "PaperWorld", DeliveryDate=DateTime.Now, DeliveryTime=DateTime.Now,  Status = "Delivered" };
                var delivery2 = new ClientDelivery { Id = 2, Producent = "PaperWorld", DeliveryDate = DateTime.Now, DeliveryTime = DateTime.Now, Status = "Delivered" };

                context.Articles.AddRange(article1, article2);
                context.ClientDeliveries.AddRange(delivery1, delivery2);
                context.SaveChanges();

                context.ClientDeliveryArticles.AddRange(
                    new ClientDeliveryArticle { ClientDeliveryId = delivery1.Id, ArticleId = article1.Id, Quantity = 5 },
                    new ClientDeliveryArticle { ClientDeliveryId = delivery1.Id, ArticleId = article2.Id, Quantity = 10 },
                    new ClientDeliveryArticle { ClientDeliveryId = delivery2.Id, ArticleId = article2.Id, Quantity = 8 }
                );

                context.SaveChanges();

                var result = controller.GetSellArticles();

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);

                var sellArticle1 = result.FirstOrDefault(a => a.Id == article1.Id);
                Assert.NotNull(sellArticle1);
                Assert.Equal(5, sellArticle1.Quantity);

                var sellArticle2 = result.FirstOrDefault(a => a.Id == article2.Id);
                Assert.NotNull(sellArticle2);
                Assert.Equal(18, sellArticle2.Quantity);
            }
        }


        [Fact]
        public void GetArticleList_ShouldReturnListOfArticles()
        {
            var options = new DbContextOptionsBuilder<SklepPapierniczyDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryArticleDatabase")
                .Options;

            using (var context = new SklepPapierniczyDbContext(options))
            {
                var controller = new RaportController(context);

                var article1 = new Article { Id = 1, Name = "Article1", Quantity = 10, Price = 20.0M, CategoryId = 1 };
                var article2 = new Article { Id = 2, Name = "Article2", Quantity = 15, Price = 25.0M, CategoryId = 1 };

                context.Articles.AddRange(article1, article2);
                context.SaveChanges();

                var result = controller.GetArticleList();

                Assert.NotNull(result);
                Assert.IsType<List<Article>>(result);

                Assert.Equal(2, result.Count);
            }
        }
    }
}
