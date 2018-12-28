using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductsStore.Models;

namespace ProductsStore.Controllers
{
    public class ProductOrdersController : Controller
    {
        private readonly StoreContext _context;

        public ProductOrdersController(StoreContext context)
        {
            _context = context;
        }

        // GET: ProductOrders
        public async Task<IActionResult> Index()
        {
            var storeContext = _context.ProductOrders.Include(p => p._order).Include(p => p._product);
            return View(await storeContext.ToListAsync());
        }

        // GET: ProductOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productOrders = await _context.ProductOrders
                .Include(p => p._order)
                .Include(p => p._product)
                .FirstOrDefaultAsync(m => m.poID == id);
            if (productOrders == null)
            {
                return NotFound();
            }

            return View(productOrders);
        }

        // GET: ProductOrders/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Order, "ID", "creditCardNum");
            ViewData["ProductId"] = new SelectList(_context.Product, "ID", "description");
            return View();
        }

        // POST: ProductOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("poID,ProductId,OrderId,CountOfProducts")] ProductOrders productOrders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productOrders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Order, "ID", "creditCardNum", productOrders.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ID", "description", productOrders.ProductId);
            return View(productOrders);
        }

        // GET: ProductOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productOrders = await _context.ProductOrders.FindAsync(id);
            if (productOrders == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Order, "ID", "creditCardNum", productOrders.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ID", "description", productOrders.ProductId);
            return View(productOrders);
        }

        // POST: ProductOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("poID,ProductId,OrderId,CountOfProducts")] ProductOrders productOrders)
        {
            if (id != productOrders.poID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productOrders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductOrdersExists(productOrders.poID))
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
            ViewData["OrderId"] = new SelectList(_context.Order, "ID", "creditCardNum", productOrders.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ID", "description", productOrders.ProductId);
            return View(productOrders);
        }

        // GET: ProductOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productOrders = await _context.ProductOrders
                .Include(p => p._order)
                .Include(p => p._product)
                .FirstOrDefaultAsync(m => m.poID == id);
            if (productOrders == null)
            {
                return NotFound();
            }

            return View(productOrders);
        }

        // POST: ProductOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productOrders = await _context.ProductOrders.FindAsync(id);
            _context.ProductOrders.Remove(productOrders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductOrdersExists(int id)
        {
            return _context.ProductOrders.Any(e => e.poID == id);
        }
    }
}
