using System;
using System.Collections.Generic;

namespace WebDelivery2.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public decimal Money { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

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
