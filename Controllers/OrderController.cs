using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebDelivery2.Extensions;
using WebDelivery2.Models;

namespace WebDelivery2.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }
        public IActionResult Order()
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var orders = context.Orders.ToList();
                ViewBag.Orders = orders;

                var drivers = context.Drivers.ToList();
                ViewBag.Drivers = drivers;

                var addresses = context.Addresses.ToList();
                ViewBag.Addresses = addresses;
            }

            return View();
        }

        [HttpGet]
        public IActionResult CreateOrder(int customerid)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var customer = context.Customers.FirstOrDefault(c => c.Id == customerid);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                if (customer.Money < 0)
                {
                    LoadViewData(context);
                    return RedirectToAction("ProfileCustomer", "Home", new { id = customerid });
                }

                ViewBag.CustomerId = customerid;

                var drivers = context.Drivers.ToList();
                ViewBag.Drivers = drivers;

                var addresses = context.Addresses.ToList();
                ViewBag.Addresses = addresses;

                return View();
            }
        }

        [HttpPost]
        public IActionResult CreateOrder(Order model, int CustomerId, string DeliveryType, string city, string street, string postcode, string country, string house, decimal price)
        {
                using (DeliveryContext context = new DeliveryContext())
                {
                    var customerExists = context.Customers.Any(c => c.Id == CustomerId);
                    if (!customerExists)
                    {
                        return BadRequest("Invalid CustomerId. Customer does not exist.");
                    }

                    var addressModel = new Address
                    {
                        City = city,
                        Street = street,
                        Country = country,
                        House = house,
                    };

                    OrderExtensions.CreateOrder(model, CustomerId, DeliveryType, addressModel, price);

                    return RedirectToAction("ProfileCustomer", "Home", new { id = CustomerId });
                }
        }

        [HttpGet]
        public IActionResult CalculatePrice(string country, string city, string street, int customerId)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                int price = 20; 

                var address = context.Addresses.FirstOrDefault(a => a.CustomerId == customerId);

                if (address != null)
                {
                    if (address.Country != country)
                    {
                        price = 200;
                    }
                    else if (address.City != city)
                    {
                        price = 100;
                    }
                    else if (address.Street != street)
                    {
                        price = 50;
                    }
                }

                return Json(new { price = price });
            }
        }

        private void LoadViewData(DeliveryContext context)
        {
            var addresses = context.Addresses.ToList();
            ViewBag.Addresses = addresses;

            var orders = context.Orders.ToList();
            ViewBag.Orders = orders;

            var drivers = context.Drivers.ToList();
            ViewBag.Drivers = drivers;

            var customers = context.Customers.ToList();
            ViewBag.Customers = customers;
        }
    }
}
