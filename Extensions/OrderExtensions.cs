using WebDelivery2.Models;
using WebDelivery2.Extensions;

namespace WebDelivery2.Extensions
{
    public class OrderExtensions
    {
        public static void CreateOrder(Order model, int Customerid, string DeliveryType, Address modelA, decimal Price)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                Address address = AddressExtensions.CreateAddress(modelA);
                context.Addresses.Add(address);
                context.SaveChanges();

                var customerAddress = context.Addresses.FirstOrDefault(c => c.CustomerId == Customerid);
                if (customerAddress == null)
                {
                    throw new Exception("Customer address not found.");
                }

                int deliveryAddressId = 0;
                int pickupAddressId = 0;

                if (DeliveryType == "To")
                {
                    deliveryAddressId = address.Id;
                    pickupAddressId = customerAddress.Id;
                }
                else if (DeliveryType == "From")
                {
                    pickupAddressId = address.Id;
                    deliveryAddressId = customerAddress.Id;
                }

                var order = new Order
                {
                    CustomerId = Customerid,
                    Date = DateTime.Now,
                    Status = OrderStatus.pending,
                    Description = model.Description,
                    PickupaddressId = pickupAddressId,
                    DeliveryaddressId = deliveryAddressId,
                    Price = Price
                };

                context.Orders.Add(order);
                context.SaveChanges();
            }
        }

        public static void TakeOrder(int id, int driverId)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var order = context.Orders.FirstOrDefault(o => o.Id == id);
                if (order != null)
                {
                    if (order.Status == OrderStatus.pending)
                    {
                        order.Status = OrderStatus.intransit;
                        order.DriverId = driverId;
                    }
                    else if (order.Status == OrderStatus.intransit)
                    {
                        order.Status = OrderStatus.delivered;
                    }
                }
                context.SaveChanges();
            }
        }

        public static void AcceptOrder(int id, int customerId)
        {
            using (DeliveryContext context = new DeliveryContext())
            {
                var order = context.Orders.FirstOrDefault(o => o.CustomerId == customerId && o.Id == id && o.Status == OrderStatus.delivered);
                if (order != null)
                {
                    context.Orders.Remove(order);
                    context.SaveChanges();
                }
            }
        }

        public static void Count(int id)
        {
            
            using (DeliveryContext context = new DeliveryContext())
            {
                var order = context.Orders.FirstOrDefault(o => o.Id == id);
                if (order != null)
                {
                    if (order.Pickupaddress.Country != order.Deliveryaddress.Country)
                    {
                        order.Price = 200;
                    }
                    else if (order.Pickupaddress.City != order.Deliveryaddress.City)
                    {
                        order.Price = 100;
                    }
                    else if (order.Pickupaddress.Street != order.Deliveryaddress.Street)
                    {
                        order.Price = 50;
                    }
                    else
                    {
                        order.Price = 20;
                    }
                }
            }
            
        }
    }
}
