using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WebDelivery2.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int? DriverId { get; set; }

    public int DeliveryaddressId { get; set; }

    public int PickupaddressId { get; set; }

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime Date { get; set; }

    public OrderStatus Status { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual Address Deliveryaddress { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual Address Pickupaddress { get; set; }

    public void TakeOrder(int id, int driverId)
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

    public void AcceptOrder(int id, int customerId)
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

    public void Count(int id)
    {
        using (DeliveryContext context = new DeliveryContext())
        {
            var order = context.Orders.FirstOrDefault(o => o.Id == id);
            if(order != null)
            {
                if(order.Pickupaddress.Country != order.Deliveryaddress.Country)
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
