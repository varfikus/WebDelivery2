    using System;
using System.Collections.Generic;

namespace WebDelivery2.Models;

public partial class Address
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string House { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Order> OrderDeliveryaddresses { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderPickupaddresses { get; set; } = new List<Order>();

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
