using RiskFirst.Hateoas.Models;

namespace rest_api_sri.Models;

public class InvoiceDto : LinkContainer
{
    public InvoiceDto(long id, DateTime invoiceDate, string customerName, double amount, string paymentStatus, long? employeeId)
    {
        Id = id;
        InvoiceDate = invoiceDate;
        CustomerName = customerName;
        Amount = amount;
        PaymentStatus = paymentStatus;
        EmployeeId = employeeId;
    }

    public long Id { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string CustomerName { get; set; }
    public double Amount { get; set; }
    public string PaymentStatus { get; set; }
    public long? EmployeeId { get; set; }
}