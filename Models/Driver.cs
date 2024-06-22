using System;
using System.Collections.Generic;

namespace WebDelivery2.Models;

public partial class Driver
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public decimal Money { get; set; }

    public string Licensenumber { get; set; } = null!;

    public decimal Rating { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
