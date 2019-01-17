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
    public class OrdersController : Controller
    {
        private readonly StoreContext _context;

        public OrdersController(StoreContext context)
        {
            _context = context;
        }

        // GET: Orders
        public ActionResult Index(int page = 1)
        {
            var orders = _context.Order.Include(o => o.User);//.Skip((page - 1) *10).Take(page *  10);
            var p = _context.ProductOrders.Include(x => x.OrderId);
            List<ShowOrderView> lstOrderProduct = new List<ShowOrderView>();
            foreach (var item in orders)
            {
                var orderProducts =_context.ProductOrders.Where(x => x.OrderId == item.ID);
                var lst = new List<Product>();
                foreach (var singleProd in orderProducts)
                {
                    lst.Add(_context.Product.First(t => t.ID == singleProd.ProductId));

                }
                // Extecd the orders into this
                var orderprods = new ShowOrderView(item, lst);
                lstOrderProduct.Add(orderprods);
            }
            return View(lstOrderProduct.ToList()) ;
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            Order order = _context.Order.Find(id);
            if (order == null)
            {
                return new NotFoundResult();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.userID = new SelectList(_context.User, "ID", "firstName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,userID,orderDate,creditCardNum,amount")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Order.Add(order);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userID = new SelectList(_context.User, "ID", "firstName", order.userID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            Order order = _context.Order.Find(id);
            if (order == null)
            {
                return new NotFoundResult();

            }
            ViewBag.userID = new SelectList(_context.User, "ID", "firstName", order.userID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("ID,userID,orderDate,creditCardNum,amount")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(order).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userID = new SelectList(_context.User, "ID", "firstName", order.userID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            Order order = _context.Order.Find(id);
            if (order == null)
            {
                return new NotFoundResult();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = _context.Order.Find(id);
            _context.Order.Remove(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }



       

        public ActionResult UserReports()
        {
            return View("UserReport", new List<UserOrderReport>());
        }

        public ActionResult ProductsReports()
        {
            return View("ProductsReport", new List<OrderProdectsCount>());
        }
        


        public JsonResult GetUsersPerOrder()
        {
            var userOrders = new List<string>();
            var orders = _context.Order.ToList();
            var _usersOrder = new List<UserOrderReport>();

            foreach (var item in orders)
            {
                if (!userOrders.Contains(item.userID.ToString()))
                {
                    userOrders.Add(item.userID.ToString());
                    _usersOrder.Add(new UserOrderReport
                    {
                        UserName = _context.User.First(u => u.ID == item.userID).firstName,
                        Count = orders.Count(x => x.userID == item.userID),
                    });
                }
            }
            return Json(_usersOrder);
        }

        public JsonResult GetProdectsPerOrder()
        {
            var productsPerOrders = new SortedDictionary<int, int>();

            var products = _context.ProductOrders;

            foreach (var item in products)
            {
                if (!productsPerOrders.ContainsKey(item.ProductId))
                {
                    productsPerOrders[item.ProductId] = 0;
                }
                productsPerOrders[item.ProductId] += item.CountOfProducts;
            }

            var productsCount = new List<OrderProdectsCount>();
            foreach (var item in productsPerOrders)
            {
                productsCount.Add(new OrderProdectsCount()
                {
                    ProductName = _context.Product.First(k => k.ID == item.Key).name,
                    Count = item.Value
                });
            }

            return Json(productsCount);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult Search()
        {
            var name = Request.Form["txtname"];
            var orders = from p in _context.Order
                         select p;
            if (!String.IsNullOrEmpty(name))
            {
                orders = orders.Where(s => s.User.firstName.Contains(name) || s.User.lastName.Contains(name));
            }

            return View("Index", orders);
        }
    }
}
