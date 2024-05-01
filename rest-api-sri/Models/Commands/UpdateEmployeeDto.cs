using FluentValidation;

namespace rest_api_sri.Models.Commands;

public class UpdateEmployeeDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Job { get; set; }
}

public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
{
    public UpdateEmployeeDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .When(x => x.FirstName != null);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .When(x => x.LastName != null);
        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Now - TimeSpan.FromDays(18 * 365))
            .WithMessage("Employee must be at least 18 years old!")
            .When(x => x.BirthDate.HasValue);
        RuleFor(x => x.Job)
            .NotEmpty()
            .WithMessage("Employee must have a job!")
            .When(x => x.Job != null);
    }
}