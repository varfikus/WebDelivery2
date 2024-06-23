using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using WebDelivery2.Models;
using WebDelivery2.Extensions;
using System.Diagnostics;

namespace WebDelivery2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var customers = context.Customers.ToList();
                ViewBag.Customers = customers;

                var drivers = context.Drivers.ToList();
                ViewBag.Drivers = drivers;

                var orders = context.Orders.ToList();
                ViewBag.Orders = orders;
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                if (model.UserType == "Driver")
                {
                    Driver driver = DriverExtensions.CreateDriver(model);
                    context.Drivers.Add(driver);
                    context.SaveChanges();

                    return RedirectToAction("ProfileDriver", new { id = driver.Id });
                }
                else
                {
                    Customer customer = CustomerExtensions.CreateCustomer(model);
                    context.Customers.Add(customer);
                    context.SaveChanges();

                    Address address = AddressExtensions.CreateAddress(model, customer.Id);
                    context.Addresses.Add(address);
                    context.SaveChanges();

                    return RedirectToAction("ProfileCustomer", new { id = customer.Id });
                }
            }
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var customer = context.Customers
                    .FirstOrDefault(c => c.Email == model.Email && c.Password == model.Password);

                if (customer != null)
                {
                    LoadViewData(context);
                    return View("ProfileCustomer", customer);
                }

                var driver = context.Drivers
                    .FirstOrDefault(d => d.Email == model.Email && d.Password == model.Password);

                if (driver != null)
                {
                    LoadViewData(context);
                    return View("ProfileDriver", driver);
                }

                ViewBag.ErrorMessage = "Invalid email or password";
                return View("Index");
            }
        }

        public IActionResult ProfileCustomer(int id)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var customer = context.Customers.FirstOrDefault(c => c.Id == id);
                if (customer == null)
                {
                    return NotFound();
                }

                LoadViewData(context);
                return View("ProfileCustomer", customer);
            }
        }

        [HttpGet]
        public IActionResult ProfileDriver(int id)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var driver = context.Drivers.FirstOrDefault(c => c.Id == id);
                if (driver == null)
                {
                    return NotFound();
                }

                LoadViewData(context);
                return View("ProfileDriver", driver);
            }
        }
        
        [HttpPost]
        public IActionResult TakeOrder(int id, int driverId)
        {
            OrderExtensions.TakeOrder(id, driverId);

            return RedirectToAction("ProfileDriver", new { id = driverId });
        }

        [HttpPost]
        public IActionResult AcceptOrder(int id, int customerId)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var order = context.Orders.FirstOrDefault(o => o.Id == id);
                if (order != null)
                {
                    var driverid = order.DriverId;
                    var customerid = customerId;
                    OrderExtensions.AcceptOrder(id, customerId);
                    return RedirectToAction("DriverRating", new { driverId = driverid, customerId = customerid });
                }
                else
                {
                    return NotFound("Order not found.");
                }
            }
        }

        [HttpGet]
        public IActionResult DriverRating(int driverId, int customerId)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var driver = context.Drivers.FirstOrDefault(d => d.Id == driverId);
                if (driver != null)
                {
                    ViewBag.CustomerId = customerId;
                    return View(driver);
                }
                else
                {
                    return NotFound("Driver not found.");
                }
            }
        }

        [HttpPost]
        public IActionResult SubmitRating(int driverId, decimal rating, int customerId)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var driver = context.Drivers.FirstOrDefault(d => d.Id == driverId);
                if (driver == null)
                {
                    return BadRequest("Invalid DriverId. Driver does not exist.");
                }

                driver.Rating = (driver.Rating + rating) / 2;
                context.SaveChanges();

                return RedirectToAction("ProfileCustomer", "Home", new { id = customerId });
            }
        }

        public IActionResult CreateOrder(int customerId)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                if (customer.Money < 0)
                {
                    LoadViewData(context);
                    return View("ProfileCustomer", customer);
                }

                ViewBag.CustomerId = customerId;

                var driversList = context.Drivers.ToList();
                ViewBag.Drivers = driversList;

                var addresses = context.Addresses.ToList();
                ViewBag.Addresses = addresses;

                return View();
            }
        }

        public IActionResult Money(int customerId)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                ViewBag.CustomerId = customerId;
            }

            return View();
        }

        public IActionResult Order()
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var orders = context.Orders.ToList();
                ViewBag.Orders = orders;

                var drivers = context.Drivers.ToList();
                ViewBag.Drivers = drivers;
            }

            return View();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
