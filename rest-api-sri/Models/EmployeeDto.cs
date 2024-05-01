using RiskFirst.Hateoas.Models;

namespace rest_api_sri.Models;

public class EmployeeDto : LinkContainer
{
    public EmployeeDto(long id, string firstName, string lastName, DateTime birthDate, string job, List<InvoiceDto> invoiceDtos)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Job = job;
        Invoices = invoiceDtos;
    }

    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Job { get; set; }
    public List<InvoiceDto> Invoices { get; set; }
}