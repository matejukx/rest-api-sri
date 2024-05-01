using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rest_api_sri.Models;

public class Invoice
{
    [Key]
    public long Id { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string CustomerName { get; set; }
    public double Amount { get; set; }
    public string PaymentStatus { get; set; }
    
    
    [ForeignKey("Employee")]
    public long? EmployeeId { get; set; }
    
    public Employee? Employee { get; set; }
    
    public InvoiceDto ToDto() => new(Id, InvoiceDate, CustomerName, Amount, PaymentStatus, EmployeeId);
}