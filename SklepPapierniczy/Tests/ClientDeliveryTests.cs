using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using SklepPapierniczy.Controllers;
using SklepPapierniczy.Models;
using SklepPapierniczy.Data;

namespace SklepPapierniczy.Tests
{
    public static class DbContextMockerClientDelivery
    {
        public static SklepPapierniczyDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<SklepPapierniczyDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryClientDeliveryDatabase")
                .Options;

            var dbContext = new SklepPapierniczyDbContext(options);

            return dbContext;
        }
    }

    public class ClientDeliveriesControllerTests
    {
        [Fact]
        public void ClientDeliveryExists_ShouldReturnTrueIfExists()
        {
            var context = DbContextMockerClientDelivery.GetInMemoryContext();
            var controller = new ClientDeliveriesController(context);

            var clientDelivery = new ClientDelivery
            {
                Id = 1,
                Producent = "Test Producent",
                DeliveryTime = DateTime.Now,
                DeliveryDate = DateTime.Now,
                Status = "Delivered",
                Summary = 12.00M
            };
            context.ClientDeliveries.Add(clientDelivery);
            context.SaveChanges();

            var result = controller.ClientDeliveryExists(clientDelivery.Id);

            Assert.True(result);
        }

        [Fact]
        public void ClientDeliveryExists_ShouldReturnFalseIfNotExists()
        {
            var context = DbContextMocker.GetInMemoryContext();
            var controller = new ClientDeliveriesController(context);

            var result = controller.ClientDeliveryExists(999);

            Assert.False(result);
        }
    }
}
