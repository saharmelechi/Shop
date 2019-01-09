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
                var g =_context.ProductOrders.First(x => x.OrderId == item.ID);
                var lst = _context.Product.Where(t => t.ID == g.ProductId);
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



       

        public ActionResult Reports()
        {
            return View("Report", new List<UserOrderReport>());
        }

        public ActionResult ProductsReports()
        {
            return View("ProductsReport", new List<UserOrderReport>());
        }

        [HttpPost]
        public ActionResult ApplyReport()
        {
            var result = from order in
                 (
                     from o in _context.Order
                     select new
                     {
                         Order = o,
                         UserName = o.userID
                     }
                 )
                         orderby order.Order.userID
                         group order by order.Order.userID into g
                         select new
                         {
                             User = g.Key,
                             UserName = "",
                             Count = g.Count()
                         };

            var users = _context.User.ToList();
            List<UserOrderReport> userOrderReports = new List<UserOrderReport>();
            foreach (var item in result)
            {
                var user = users.Where(x => x.ID == item.User).FirstOrDefault();
                userOrderReports.Add(new Models.UserOrderReport { Count = item.Count, UserName = (user.firstName + " " + user.lastName) });

            }

            return View("Report", userOrderReports);
        }


        public JsonResult GetUsersPerOrder()
        {
            List<UserOrderPear> userOrders = new List<UserOrderPear>();
            var users = _context.User.ToList();

            foreach (var item in users)
            {
                userOrders.Add(
                    new UserOrderPear
                    {
                        Name = item.firstName + " " + item.lastName,
                        NumOfOrders = item.Orders.Count()
                    });

            }
            return Json(userOrders);
        }
        public JsonResult GetProdectsPerOrder()
        {
            List<UserOrderPear> productsPerOrders = new List<UserOrderPear>();

            var products = _context.Product.ToList();

            foreach (var item in products)
            {
                productsPerOrders.Add(
                    new UserOrderPear
                    {
                        Name = item.name,
                        NumOfOrders = item.Orders.Count()
                    });

            }
            return Json(productsPerOrders);
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

    public class UserOrderPear
    {
        public string Name { get; set; }
        public int NumOfOrders { get; set; }
    }
}
