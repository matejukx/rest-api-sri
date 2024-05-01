using FluentValidation;

namespace rest_api_sri.Models.Commands;

public class CreateEmployeeDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Job { get; set; }
}

public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();
        RuleFor(x => x.LastName)
            .NotEmpty();
        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Now - TimeSpan.FromDays(18 * 365))
            .WithMessage("Employee must be at least 18 years old!");
        RuleFor(x => x.Job)
            .NotEmpty()
            .WithMessage("Employee must have a job!");
    }
}