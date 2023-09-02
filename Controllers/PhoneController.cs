using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dashboard.Data;
using Dashboard.Models;

namespace Dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PhoneController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Cat")]
        public async Task<JsonResult> BritishCatData()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://catfact.ninja/fact");
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            return new JsonResult(content);
        }



        [HttpGet("GetAllPhone")]
        public JsonResult GetPhone()
        {
            var PhoneData = _context.ProductDetails.ToList();
            if(PhoneData == null)
                return new JsonResult("NotFound");

            return new JsonResult(Ok(PhoneData));
        }

        [HttpPost("GetAllPhone/{id}")]
        public JsonResult GetPhone(int id)
        {
            var PhoneData = _context.ProductDetails.SingleOrDefault(p=> p.Id == id);
            if (PhoneData == null)
                return new JsonResult("NotFound");

            return new JsonResult(Ok(PhoneData));
        }

        [HttpDelete("DeletePhone/{id}")]
        public JsonResult DeletePhone(int id)
        {
            var PhoneData = _context.ProductDetails.SingleOrDefault(p => p.Id == id);
            _context.ProductDetails.Remove(PhoneData);
            _context.SaveChanges();
            if (PhoneData == null)
                return new JsonResult("NotFound");

            return new JsonResult(Ok(PhoneData));
        }

        [HttpPut("AddNewPhone")]
        public JsonResult AddNewPhone(ProductDetails productDetails)
        {
            _context.ProductDetails.Add(productDetails);
            _context.SaveChanges();
        
            return new JsonResult(Ok(productDetails));
        }
    }
}
