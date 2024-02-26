using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SklepPapierniczy.Data;
using SklepPapierniczy.Models;

namespace SklepPapierniczy.Controllers
{
    public class ClientDeliveriesController : Controller
    {
        private readonly SklepPapierniczyDbContext _context;

        public ClientDeliveriesController(SklepPapierniczyDbContext context)
        {
            _context = context;
        }

        // GET: ClientDeliveries
        public async Task<IActionResult> Index()
        {
            var clientDeliveries = await _context.ClientDeliveries.ToListAsync();

            return View(clientDeliveries);
        }
        // GET: ClientDeliveries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientDelivery = await _context.ClientDeliveries
                .Include(cd => cd.Articles)
                    .ThenInclude(a => a.Article)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (clientDelivery == null)
            {
                return NotFound();
            }

            return View(clientDelivery);
        }

        // GET: ClientDeliveries/Create
        public IActionResult Create()
        {
            ViewBag.ArticleList = _context.Articles.ToList();
            return View();
        }

        // POST: ClientDeliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientDelivery clientDelivery)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientDelivery);
                await _context.SaveChangesAsync();

                //if (selectedArticleIds != null && articleQuantities != null)
                //{
                //    for (int i = 0; i < selectedArticleIds.Count; i++)
                //    {
                //        var clientDeliveryArticle = new ClientDeliveryArticle
                //        {
                //            ClientDeliveryId = clientDelivery.Id,
                //            ArticleId = selectedArticleIds[i],
                //            Quantity = articleQuantities[i]
                //        };
                //        _context.Add(clientDeliveryArticle);
                //    }

                //    await _context.SaveChangesAsync();
                //}

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ArticleList = _context.Articles.ToList();
            return View(clientDelivery);
        }


        // GET: ClientDeliveries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientDelivery = await _context.ClientDeliveries.FindAsync(id);
            if (clientDelivery == null)
            {
                return NotFound();
            }
            return View(clientDelivery);
        }

        // POST: ClientDeliveries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Producent,DeliveryTime,DeliveryDate,Status,Summary")] ClientDelivery clientDelivery)
        {
            if (id != clientDelivery.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientDelivery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientDeliveryExists(clientDelivery.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(clientDelivery);
        }

        // GET: ClientDeliveries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientDelivery = await _context.ClientDeliveries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientDelivery == null)
            {
                return NotFound();
            }

            return View(clientDelivery);
        }

        // POST: ClientDeliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientDelivery = await _context.ClientDeliveries.FindAsync(id);
            if (clientDelivery != null)
            {
                _context.ClientDeliveries.Remove(clientDelivery);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public bool ClientDeliveryExists(int id)
        {
            return _context.ClientDeliveries.Any(e => e.Id == id);
        }
    }
}
