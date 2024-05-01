using FluentValidation;

namespace rest_api_sri.Models.Commands;

public class CreateInvoiceDto
{
    public DateTime InvoiceDate { get; set; }
    public string CustomerName { get; set; }
    public double Amount { get; set; }
    public string PaymentStatus { get; set; } = "Pending";
    public long? EmployeeId { get; set; }
}

public class CreateInvoiceDtoValidator : AbstractValidator<CreateInvoiceDto>
{
    public CreateInvoiceDtoValidator()
    {
        RuleFor(x => x.InvoiceDate)
            .NotEmpty();
        RuleFor(x => x.CustomerName)
            .NotEmpty();
        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0!");
        RuleFor(x => x.PaymentStatus)
            .NotEmpty();
    }
}