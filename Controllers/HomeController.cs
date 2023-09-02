using Dashboard.Data;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

		private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            var Name = HttpContext.User.Identity.Name;


            //CookieOptions options = new CookieOptions();
            //options.Expires = DateTime.Now.AddMinutes(10);
            //Response.Cookies.Append("Name", Name, options);

            //HttpContext.Session.SetString("Name", Name);

            TempData["Name"] = Name;
            ViewBag.Name = Name;
            
            var product = context.Products.ToList();

            return View(product);
        }

        public IActionResult PaymentAccept()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PaymentAccept(PaymentAccept paymentAccept)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult UpdateProducts(Products products)
        {
            context.Products.Update(products);

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var product = context.Products.SingleOrDefault(x => x.Id == id);
            return View(product);
        }

        public IActionResult Delete(int id)
		{
			var product = context.Products.SingleOrDefault(p => p.Id == id);
			if (product != null)
			{
				context.Products.Remove(product);
				context.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		public IActionResult AddProduct(Products product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

		public IActionResult AddProductDetails(ProductDetails productDetails)
		{
			context.ProductDetails.Add(productDetails);
			context.SaveChanges();
			return RedirectToAction("ProductDetails");
		}

		public IActionResult CreateNewProduct(Products product) 
        {
            context.Products.Add(product);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult GetProductDetails(int id)
        {
			var ProductDetails = context.ProductDetails.Where(predicate => predicate.ProductId == id).ToList();
			ViewBag.ProductDetails = ProductDetails;
			return RedirectToAction("ProductDetails");
		}

        [HttpPost]
		public IActionResult ProductDetails(int id)
		{
			var ProductDetails = context.ProductDetails.Where(p => p.ProductId == id).ToList();
			var product = context.Products.ToList();
			ViewBag.ProductDetails = ProductDetails;
			return View(product);
		}
		public IActionResult ProductDetails() 
        {
            //1-Cookies
            //ViewBag.Name = Request.Cookies["Name"];
            //2-Session
            //ViewBag.Name = HttpContext.Session.GetString("name");
            //3-TempData
            ViewBag.Name = TempData["Name"];

			var product = context.Products.ToList();
            var ProductDetails = context.ProductDetails.ToList();
            ViewBag.ProductDetails = ProductDetails;
			return View(product);
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
    }
}