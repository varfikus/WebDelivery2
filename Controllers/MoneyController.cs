using Microsoft.AspNetCore.Mvc;
using WebDelivery2.Extensions;
using WebDelivery2.Models;

namespace WebDelivery2.Controllers
{
    public class MoneyController : Controller
    {
        private readonly ILogger<MoneyController> _logger;

        public MoneyController(ILogger<MoneyController> logger)
        {
            _logger = logger;
        }

        public IActionResult Money(int customerId)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                ViewBag.CustomerId = customerId;
            }

            return View();
        }

        [HttpGet]
        public IActionResult AddMoney(int customerid)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                ViewBag.CustomerId = customerid;
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddMoney(int CustomerId, decimal Money)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var customerExists = context.Customers.Any(c => c.Id == CustomerId);
                if (!customerExists)
                {
                    return BadRequest("Invalid CustomerId. Customer does not exist.");
                }

                CustomerExtensions.AddMoney(CustomerId, Money);

                return RedirectToAction("ProfileCustomer", "Home", new { id = CustomerId });
            }
        }
    }
}
