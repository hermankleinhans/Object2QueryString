using Object2QueryString;
using Test;

var personFilter = new PersonFilter() { 
    Firstname = "Herman", 
    LastName = "Kleinhans", 
    Age = 37, 
    Address = new Address { 
        Street = "19 Bloekom Avenue", 
        City = "Robertson", 
        Province = "Western Cape", 
        PostalCode = "6705" },
    Roles = new List<Role> { Role.Admin, Role.DataPrivacyLead }
};

var result = personFilter.ToQueryString();

Console.WriteLine(result);
Console.Read();