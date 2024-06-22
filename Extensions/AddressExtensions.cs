using WebDelivery2.Models;
using WebDelivery2.Extensions;

namespace WebDelivery2.Extensions
{
    public class AddressExtensions
    {
        public static Address CreateAddress(User model, int customerId)
        {
            return new Address
            {
                CustomerId = customerId,
                City = model.City,
                Street = model.Street,
                Country = model.Country,
                House = model.House
            };
        }
        public static Address CreateAddress(Address model)
        {
            return new Address
            {
                City = model.City,
                Street = model.Street,
                Country = model.Country,
                House = model.House
            };
        }
    }
}
