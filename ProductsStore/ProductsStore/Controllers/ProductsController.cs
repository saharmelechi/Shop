using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductsStore.Models;

namespace ProductsStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,name,price,description,image")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,name,price,description,image")] Product product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ID == id);
        }

        public ActionResult ProductDetails(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            Product product = _context.Product.Find(id);
            if (product == null)
            {
                return new NotFoundResult();
            }
            return View("ProductDetails", product);
        }

        public ActionResult AddToCart(int id)
        {
            if ((HttpContext.Session.GetInt32(Globals.CART_SESSION_KEY) ?? 0) != 0)
            {
                Cart cart = (Cart)HttpContext.Session.GetString(Globals.CART_SESSION_KEY);
                Product p = _context.Product.Where(x => x.ID == id).FirstOrDefault();

                cart.Products.Add(p);

                cart.TotalAmount = 0;
                for (int i = 0; i < cart.Products.Count(); i++)
                {
                    cart.TotalAmount += cart.Products[i].price;
                }

                HttpContext.Session.SetString(Globals.CART_SESSION_KEY, cart);

                return View("Cart", cart);
            }
            return RedirectToAction("index", "Home");

        }

        public ActionResult Cart()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString(Globals.CART_SESSION_KEY)))
            {
                Cart cart = (Cart)HttpContext.Session.GetString(Globals.CART_SESSION_KEY);

                cart.TotalAmount = 0;
                for (int i = 0; i < cart.Products.Count(); i++)
                {
                    cart.TotalAmount += cart.Products[i].price;
                }

                HttpContext.Session.SetString(Globals.CART_SESSION_KEY, cart);

                return View("Cart", cart);
            }
            return RedirectToAction("index", "Home");

        }

        public ActionResult Checkout()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString(Globals.CART_SESSION_KEY)))
            {
                Cart cart = (Cart)HttpContext.Session.GetString(Globals.CART_SESSION_KEY);

                cart.TotalAmount = 0;
                for (int i = 0; i < cart.Products.Count(); i++)
                {
                    cart.TotalAmount += cart.Products[i].price;
                }

                HttpContext.Session.SetString(Globals.CART_SESSION_KEY, cart);

                Checkout ch = new Models.Checkout();
                ch.TotalAmount = cart.TotalAmount;
                return View("Checkout", ch);
            }
            return RedirectToAction("index", "Home");

        }


        private Cart GetCart()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString(Globals.CART_SESSION_KEY)))
            {
                Cart cart = (Cart)HttpContext.Session.GetString(Globals.CART_SESSION_KEY);

                cart.TotalAmount = 0;
                for (int i = 0; i < cart.Products.Count(); i++)
                {
                    cart.TotalAmount += cart.Products[i].price;
                }


                return cart;
            }

            return null;
        }
        [HttpPost]
        public ActionResult Checkout(Checkout ch)
        {
            if (!ValidateCardLength(ch.CreditCard))
            {
                ViewBag.IsError = true;
                return View();
            }

            DateTime dt = new DateTime(ch.Year, ch.Month, 1);
            if (dt < DateTime.Now)
            {
                ViewBag.CardExpiresError = true;
                return View();
            }

            Cart c = GetCart();
            Order o = new Order();
            o.creditCardNum = ch.CreditCard;
            o.orderDate = DateTime.Now;
            var id = ((User)Globals.getConnectedUser(HttpContext.Session)).ID;
            User u = _context.User.Where(x => x.ID == id).FirstOrDefault();
            o.userID = u.ID;
            o.User = new User();
            o.User = u;
            _context.Order.Add(o);
            Cart cart = (Cart)HttpContext.Session.GetString(Globals.CART_SESSION_KEY);
            foreach (var item in cart.Products)
            {

                var prd = _context.Product.Where(x => x.ID == item.ID).FirstOrDefault();
                if (prd == null) continue;
                ProductOrders po = new ProductOrders(prd.ID, o.ID, prd.count);
                //o.Products.Add(po);

                // Add the Prod as row in the DB
                _context.ProductOrders.Add(po);
            }

            _context.SaveChanges();

            HttpContext.Session.SetString(Globals.CART_SESSION_KEY, new Cart());


            return View("OrderSuccess");

        }


        private bool ValidateCardLength(string cardNum)
        {
            string completeUrl = "https://secure.ftipgw.com/ArgoFire/validate.asmx/ValidCardLength?cardnumber=" + cardNum;

            // Create a request for the URL.         
            WebRequest request = WebRequest.Create(completeUrl);

            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            //Get the response.
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();

                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                XmlDocument xm = new XmlDocument();
                xm.LoadXml(responseFromServer);

                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();
                return bool.Parse(xm.InnerText);

            }
            catch (Exception e)
            {
                return false;
            }

            
        }

    }
}
