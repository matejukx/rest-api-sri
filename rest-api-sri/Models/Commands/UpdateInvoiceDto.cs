using FluentValidation;

namespace rest_api_sri.Models.Commands;

public class UpdateInvoiceDto
{
    public DateTime? InvoiceDate { get; set; }
    public string? CustomerName { get; set; }
    public double? Amount { get; set; }
    public string? PaymentStatus { get; set; } = "Pending";
    public long? EmployeeId { get; set; }
}

public class UpdateInvoiceDtoValidator : AbstractValidator<UpdateInvoiceDto>
{
    public UpdateInvoiceDtoValidator()
    {
        RuleFor(x => x.InvoiceDate).NotEmpty().When(x => x.InvoiceDate.HasValue);
        RuleFor(x => x.CustomerName).NotEmpty().When(x => x.CustomerName != null);
        RuleFor(x => x.Amount).NotEmpty().GreaterThan(0)
            .WithMessage("Amount must be greater than 0!").When(x => x.Amount.HasValue);
        RuleFor(x => x.PaymentStatus).NotEmpty().When(x => x.PaymentStatus != null);
    }
}