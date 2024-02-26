using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SklepPapierniczy.Controllers;
using SklepPapierniczy.Models;
using SklepPapierniczy.Data;

namespace SklepPapierniczy.Tests
{
    public static class DbContextMocker
    {
        public static SklepPapierniczyDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<SklepPapierniczyDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryArticleDatabase")
                .Options;

            var dbContext = new SklepPapierniczyDbContext(options);

            return dbContext;
        }
    }

    public class ArticlesControllerTests
    {
        [Fact]
        public void ArticleExists_ShouldReturnTrueIfExists()
        {
            var context = DbContextMocker.GetInMemoryContext();
            var controller = new ArticlesController(context);

            var article = new Article { Id = 502, Name = "Test Article", Quantity = 10, Price = 20.0M, CategoryId = 1 };
            context.Articles.Add(article);
            context.SaveChanges();

            var result = controller.ArticleExists(article.Id);

            Assert.True(result);
        }

        [Fact]
        public void ArticleExists_ShouldReturnFalseIfNotExists()
        {
            var context = DbContextMocker.GetInMemoryContext();
            var controller = new ArticlesController(context);

            var result = controller.ArticleExists(999);

            Assert.False(result);
        }
    }
}
