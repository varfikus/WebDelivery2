﻿using WebDelivery2.Models;
using WebDelivery2.Extensions;
using System.Diagnostics.Metrics;

namespace WebDelivery2.Extensions
{
    public class OrderExtensions
    {
        public static void CreateOrder(Order model, int Customerid, string DeliveryType, Address modelA, decimal Price, float Weight)
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
                    Weight = Weight,
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
                    var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);
                    var driver = context.Drivers.FirstOrDefault(d => d.Id == order.DriverId);
                    decimal amount = order.Price;

                    customer.Money -= amount;
                    driver.Money += amount / 2;

                    context.Orders.Remove(order);
                    context.SaveChanges();
                }
            }
        }
    }
}
