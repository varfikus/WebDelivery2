using WebDelivery2.Models;

namespace WebDelivery2.Extensions
{
    public class DriverExtensions
    {
        public static Driver CreateDriver(User model)
        {
            return new Driver
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email,
                Phone = model.Phone,
                Licensenumber = model.LicenseNumber,
                Password = model.Password
            };
        }
    }
}
