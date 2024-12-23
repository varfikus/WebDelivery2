using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebDelivery2.Models;

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

    public string Status { get; set; }

    public float Weight { get; set; }   

    public virtual Customer Customer { get; set; }

    public virtual Address Deliveryaddress { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual Address Pickupaddress { get; set; } 
}
