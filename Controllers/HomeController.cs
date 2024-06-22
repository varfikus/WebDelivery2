using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebDelivery2.Models;

namespace WebDelivery2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User model)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                if (model.UserType == "Driver")
                {
                    Driver driver = Driver.CreateDriver(model);
                    context.Drivers.Add(driver);
                    context.SaveChanges();

                    return RedirectToAction("ProfileDriver", "Home", new { id = driver.Id });
                }
                else
                {
                    Customer customer = Customer.CreateCustomer(model);
                    context.Customers.Add(customer);
                    context.SaveChanges();

                    Address address = Address.CreateAddress(model, customer.Id);
                    context.Addresses.Add(address);
                    context.SaveChanges();

                    return RedirectToAction("ProfileCustomer", "Home", new { id = customer.Id });
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
                    var addresses = context.Addresses.ToList();
                    ViewBag.Addresses = addresses;

                    var orders = context.Orders.ToList();
                    ViewBag.Orders = orders;

                    var drivers = context.Drivers.ToList();
                    ViewBag.Drivers = drivers;

                    return View("ProfileCustomer", customer);
                }

                var driver = context.Drivers
                    .FirstOrDefault(d => d.Email == model.Email && d.Password == model.Password);

                if (driver != null)
                {
                    var addresses = context.Addresses.ToList();
                    ViewBag.Addresses = addresses;

                    var orders = context.Orders.ToList();
                    ViewBag.Orders = orders;

                    var customers = context.Customers.ToList();
                    ViewBag.Customers = customers;

                    return View("ProfileDriver", driver);
                }

                ViewBag.ErrorMessage = "Неверный пароль или почта";
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

                var addresses = context.Addresses.ToList();
                ViewBag.Addresses = addresses;

                var orders = context.Orders.ToList();
                ViewBag.Orders = orders;

                var drivers = context.Drivers.ToList();
                ViewBag.Drivers = drivers;

                return View("ProfileCustomer", customer);
            }
        }

        public IActionResult ProfileDriver(int id)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var driver = context.Drivers.FirstOrDefault(c => c.Id == id);
                if (driver == null)
                {
                    return NotFound();
                }

                var addresses = context.Addresses.ToList();
                ViewBag.Addresses = addresses;

                var orders = context.Orders.ToList();
                ViewBag.Orders = orders;

                var customers = context.Customers.ToList();
                ViewBag.Customers = customers;

                return View("ProfileDriver", driver);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
