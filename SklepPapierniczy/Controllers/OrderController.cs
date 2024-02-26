using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SklepPapierniczy.Data;
using Microsoft.EntityFrameworkCore;
using SklepPapierniczy.Models;
using NuGet.Configuration;

namespace SklepPapierniczy.Controllers
{
    public class OrderController : Controller
    {
        private readonly SklepPapierniczyDbContext _context;

        public OrderController(SklepPapierniczyDbContext context)
        {
            _context = context;
        }

        // GET: Shop
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Summary()
        {
            var cart = GetCartFromCookie();
            decimal summary = 0;
            cart.Status = "PAID";
            cart.DeliveryDate = DateTime.Now;
            cart.DeliveryTime = DateTime.Now.AddDays(7);
            cart.Producent = "Paper World";

            foreach(ShopDeliveryArticle item in cart.Articles)
            {
                summary += item.Quantity * item.Article.Price;
            }

            cart.Summary = summary;

            var newShopDelivery = new ShopDelivery
            {
                Producent = cart.Producent,
                Status = cart.Status,
                DeliveryDate = cart.DeliveryDate,
                DeliveryTime = cart.DeliveryTime,
                Summary = summary
            };

            _context.ShopDeliveries.Add(newShopDelivery);
            _context.SaveChanges();

            foreach (ShopDeliveryArticle item in cart.Articles)
            {
                _context.ShopDeliveryArticles.Add(new ShopDeliveryArticle
                {
                    ArticleId = item.ArticleId,
                    Quantity = item.Quantity,
                    ShopDeliveryId = newShopDelivery.Id // Set the foreign key here
                });
            }

            _context.SaveChanges();
            SaveCartToCookie(new ShopDelivery());

            return View("Details", cart);
        }

        // GET: Shop/Products/5
        public IActionResult Products(int? categoryId)
        {
            if (categoryId == null)
            {
                return NotFound();
            }

            var category = _context.Categories
                .Include(c => c.Articles)
                .FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Cart()
        {
            var cart = GetCartFromCookie();
            if(cart.Articles == null)
            {
                cart.Articles = new List<ShopDeliveryArticle>();
            }
            else
            {
                decimal summary = 0.0M;
                foreach (ShopDeliveryArticle item in cart.Articles)
                {
                    summary += item.Quantity * (item.Article.Price);
                }
                cart.Summary = summary;
            }
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int articleId, int quantity)
        {
            var cart = GetCartFromCookie();

            var article = _context.Articles.FirstOrDefault(a => a.Id == articleId);

            if (cart.Articles == null)
            {
                cart.Articles = new List<ShopDeliveryArticle>();
            }

            var existingItem = cart.Articles.FirstOrDefault(item => item.ArticleId == articleId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Articles.Add(new ShopDeliveryArticle
                {
                    ArticleId = article.Id,
                    ShopDeliveryId = cart.Id,
                    Quantity = 1,
                    Article = article
                });
            }

            SaveCartToCookie(cart);

            return RedirectToAction("Cart");
        }

        public IActionResult Purchase()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateCart(int articleId, int quantity)
        {
            var cart = GetCartFromCookie();

            var existingItem = cart.Articles.FirstOrDefault(item => item.ArticleId == articleId);
            if (existingItem != null)
            {
                if (quantity <= 0)
                {
                    cart.Articles.Remove(cart.Articles.FirstOrDefault(item => item.ArticleId == articleId));
                }
                else
                {
                    existingItem.Quantity = quantity;
                }
            }

            SaveCartToCookie(cart);

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int articleId)
        {
            var cart = GetCartFromCookie();

            var itemToRemove = cart.Articles.FirstOrDefault(item => item.ArticleId == articleId);
            if (itemToRemove != null)
            {
                cart.Articles.Remove(itemToRemove);
            }

            SaveCartToCookie(cart);

            return RedirectToAction("Cart");
        }

        private ShopDelivery GetCartFromCookie()
        {
            var cartCookie = Request.Cookies["Cart"];

            if (cartCookie != null)
            {
                return JsonConvert.DeserializeObject<ShopDelivery>(cartCookie);
            }
            else
            {
                return new ShopDelivery();
            }
        }

        private void SaveCartToCookie(ShopDelivery cart)
        {
            var cartCookie = JsonConvert.SerializeObject(cart);
            Response.Cookies.Append("Cart", cartCookie, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7)
            });
        }
    }
}
