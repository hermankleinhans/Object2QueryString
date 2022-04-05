namespace Test
{
    public class PersonFilter
    {
        public string Firstname { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public Address Address { get; set; }

        public IEnumerable<Role> Roles { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }
    }

   public enum Role
    {
        Admin,
        DataPrivacyLead
    }
}
