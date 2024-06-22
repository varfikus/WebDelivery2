namespace WebDelivery2.Models
{
    public class User
    {
        public string UserType { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LicenseNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
