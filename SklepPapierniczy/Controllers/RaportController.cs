using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using SklepPapierniczy.Data;
using SklepPapierniczy.Models;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SklepPapierniczy.Controllers
{
    public class RaportController : Controller
    {
        private readonly SklepPapierniczyDbContext _context;
 
        public RaportController(SklepPapierniczyDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> MagazineStatus()
        {
            var articles = await _context.Articles.Include(a => a.Category).ToListAsync();
            return View(articles);
        }

        public IActionResult GeneratePdf()
        {
            var articles = _context.Articles.Include(a => a.Category).ToList();

            // Tworzymy nazwę pliku z obecną datą i czasem
            string fileName = $"MagazineStatusRaport_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            TempData["SuccessRaport"] = "Added";

            return new ViewAsPdf("MagazineStatusPDF", articles)
            {
                FileName = fileName
            };
        }

        public IActionResult GeneratePdfSell()
        {
            var articles = GetSellArticles();

            string fileName = $"SellRaport_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            TempData["SuccessRaport"] = "Added";
            return new ViewAsPdf("SellRaportPDF", articles)
            {
                FileName = fileName
            };
        }

        public IActionResult GeneratePdfDeliveries()
        {
            var deliveries = _context.ClientDeliveries
                .Include(cd => cd.Articles)
                    .ThenInclude(a => a.Article);

            string fileName = $"DeliveriesRaport_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            TempData["SuccessRaport"] = "Added";
            return new ViewAsPdf("DeliveriesPDF", deliveries)
            {
                FileName = fileName
            };
        }


        public List<Article> GetSellArticles()
        {
            var salesData = _context.ClientDeliveryArticles
                .GroupBy(cda => cda.ArticleId)
                .Select(group => new
                {
                    ArticleId = group.Key,
                    TotalQuantity = group.Sum(cda => cda.Quantity)
                })
                .ToList();

            var articleData = new List<Article>();  
            foreach (var sale in salesData)
            {
                var article = _context.Articles.Find(sale.ArticleId);
                if (article != null)
                {
                    article.Quantity = sale.TotalQuantity;
                }
                articleData.Add(article);
            }

            return articleData;
        }

        public IActionResult SellRaport()
        {
            var articles = GetSellArticles();
            return View(articles);
        }

        public IActionResult Deliveries()
        {
            var deliveries = _context.ClientDeliveries
                .Include(cd => cd.Articles)
                    .ThenInclude(a => a.Article);
            return View(deliveries);
        }

        public IActionResult Index()
        {
            return View();
        }

        public List<Article> GetArticleList()
        {
            return _context.Articles.ToList();
        }
    }
}
