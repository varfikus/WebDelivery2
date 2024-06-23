using WebDelivery2.Models;

namespace WebDelivery2.Extensions
{
    public class CustomerExtensions
    {
        public static Customer CreateCustomer(User model)
        {
            return new Customer
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email,
                Phone = model.Phone,
                Password = model.Password
            };
        }

        public static void AddMoney(int customerId, decimal moneyAmount)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var customer = context.Customers.FirstOrDefault(o => o.Id == customerId);
                if (customer != null)
                {
                    customer.Money += moneyAmount;
                }
                context.SaveChanges();
            }
        }

        public static void TakeMoney(int customerId, decimal moneyAmount)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var customer = context.Customers.FirstOrDefault(o => o.Id == customerId);
                if (customer != null)
                {
                    customer.Money -= moneyAmount;
                }
                context.SaveChanges();
            }
        }
    }
}
