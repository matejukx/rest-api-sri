using System.ComponentModel.DataAnnotations;

namespace rest_api_sri.Models;

public class Employee
{
    [Key]
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Job { get; set; }
    
    public List<Invoice> Invoices { get; set; }
    
    public EmployeeDto ToDto() => new(Id, FirstName, LastName, BirthDate, Job, Invoices.Select(i => i.ToDto()).ToList());
}