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
    }
}
