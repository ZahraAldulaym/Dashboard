using Dashboard.Data;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;

namespace Dashboard.Controllers.Shopping
{
    public class ShoppingController : Controller
    {

		private readonly ApplicationDbContext _context;

        public ShoppingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ProductDetails(int id)
        {
            var ProductDetails = _context.ProductDetails.Where(p => p.ProductId == id).ToList();
            return View(ProductDetails);
        }
        
        [Authorize]
        public IActionResult Checkout(int id) 
        {
            var user = HttpContext.User.Identity.Name;
            var productDetails = _context.ProductDetails.SingleOrDefault(p => p.ProductId == id);
            var cart = new Cart()
            {
                IdCustomer = user,
                ProductId = productDetails.ProductId,
                Color = productDetails.Color,
                Image = productDetails.Image,
                Price = productDetails.Price,
                Total = productDetails.Price * (15 / 100) + productDetails.Price,
				ProductName = productDetails.ProductName,
                Tax = 0.15
            };

            _context.Cart.Add(cart);
            _context.SaveChanges();

            return View(cart);
        }

      
        public async Task<IActionResult> Invoice(int id)
        {
            var user = HttpContext.User.Identity.Name;
            var cart = _context.Cart.FirstOrDefault(p => p.ProductId == id);
            var invoice = new Invoice()
            {
                CustomerId = user,
                ProductId = cart.ProductId,
                Price = cart.Price,
                Total = cart.Price * (15 / 100) + cart.Price,
                ProductName = cart.ProductName,
                Tax = (float)cart.Tax
            };

            _context.Invoice.Add(invoice);
            _context.SaveChanges();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Invoice", "zahra25x@gmail.com")); //sender
            message.To.Add(MailboxAddress.Parse("zahraaldulaym@outlook.com")); //recieved
            message.Subject = "تمت عملية الشراء";
            message.Body = new TextPart("plain")
            {
                Text = "شكرا لتسوقك .. " +
                "تم تنفيذ عملية الشراء"
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587);
                    client.Authenticate("zahra25x@gmail.com", "qdvckroeafrgicip");
                    await client.SendAsync(message); //sending message
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            };

            return View(invoice);
        }

        public IActionResult Index()
        {
            var Product = _context.Products.ToList();
            return View(Product);
        }
    }
}
