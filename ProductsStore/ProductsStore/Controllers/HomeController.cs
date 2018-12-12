using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsStore.Models;

namespace ProductsStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly StoreContext _context;

        public HomeController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVm homeVm = new HomeVm();
            homeVm.Products = _context.Product.ToList();

            return View(homeVm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult GetTopSaleProduct()
        {
            var products = _context.Product;
            List<int> users = new List<int>();
            //if (Session["User"] != null && Session["Admin"] == null)
            //TODO: Understand what the code do and fix it.
            if(false)
            {
                User use = null;// (User)Session["User"];
                users = null;// listusers(use);

                var result1 = (from a in _context.Order
                               join b in users on a.ID equals b
                               select a).Distinct();

                var result2 = (from a in _context.Product
                               join b in result1 on a.ID equals b.ID
                               select a).Distinct();


                var c = (from product in result2
                         select new
                         {
                             numberOfOrders = product.Orders.Count(),
                             product = product
                         });
                var list = c.ToList();


                Product p = new Product();
                int lastNum = 0;
                foreach (var item in list)
                {
                    if (item.numberOfOrders > lastNum)
                    {
                        lastNum = item.numberOfOrders;
                        p = item.product;
                    }
                }
                Product p1 = new Product();
                p1.name = p.name;
                p1.price = p.price;
                p1.description = p.description;
                p1.image = p.image;
                return Json(p1);
            }


            else
            {
                var c = (from product in products
                         select new
                         {
                             numberOfOrders = product.Orders.Count(),
                             product = product
                         });
                var list = c.ToList();


                Product p = new Product();
                int lastNum = 0;
                p = list[0].product;
                foreach (var item in list)
                {
                    if (item.numberOfOrders > lastNum)
                    {
                        lastNum = item.numberOfOrders;
                        p = item.product;
                    }
                }
                Product prod = new Product();
                prod.name = p.name;
                prod.price = p.price;
                prod.description = p.description;
                prod.image = p.image;
                return Json(prod);
            }
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user.email == "y@y.com" && user.pass == "12345")
            {
                Session["Admin"] = true;
                Session["User"] = new User();
                return RedirectToAction("Index", "Products");
            }
            else
            {
                var userInfo = db.Users.Where(s => s.email == user.email.Trim() && s.pass == user.pass.Trim()).FirstOrDefault();


                if (userInfo != null)
                {
                    Session["User"] = userInfo;
                    Session["Cart"] = new Cart();
                }
                return RedirectToAction("Index", "Home");
            }



        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return View("Login");
        }

        public ActionResult Register()
        {
            ViewBag.Message = "Register Page";

            return View("Register");
        }

        [HttpPost]
        public ActionResult Search()
        {
            var prodName = Request.Form["prodName"];
            var fromPrice = Request.Form["txtFromPrice"];
            var txtToPrice = Request.Form["txtToPrice"];

            var products = from p in _context.Product
                           select p;
            if (!String.IsNullOrEmpty(prodName))
            {
                products = products.Where(s => s.name.Contains(prodName));
            }

            if (!String.IsNullOrEmpty(fromPrice))
            {
                var fPrice = int.Parse(fromPrice);
                products = products.Where(s => s.price > fPrice);
            }

            if (!string.IsNullOrEmpty(txtToPrice))
            {
                var fPrice = int.Parse(txtToPrice);
                products = products.Where(s => s.price < fPrice);
            }

            HomeVm homeVm = new HomeVm();
            homeVm.Products = products.ToList();

            homeVm.TopSale = getMostSale();
            return View("Index", homeVm);

        }

        public Product getMostSale()
        {
            var a = (from product in _context.Product
                     select new
                     {
                         numberOfOrders = product.Orders.Count(),
                         product = product
                     });
            var list = a.ToList();


            Product p = new Product();
            int lastNum = 0;
            foreach (var item in list)
            {
                if (item.numberOfOrders > lastNum)
                {
                    lastNum = item.numberOfOrders;
                    p = item.product;
                }
            }

            return p;
        }
    }
}
