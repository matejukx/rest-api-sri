using FluentValidation;
using Microsoft.EntityFrameworkCore;
using rest_api_sri.Infrastructure;
using rest_api_sri.Models;
using rest_api_sri.Models.Commands;
using RiskFirst.Hateoas;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("EmployeeDb"));

builder.Services.AddAuthorization();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddScoped<IValidator<CreateEmployeeDto>, CreateEmployeeDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateEmployeeDto>, UpdateEmployeeDtoValidator>();
builder.Services.AddScoped<IValidator<CreateInvoiceDto>, CreateInvoiceDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateInvoiceDto>, UpdateInvoiceDtoValidator>();

builder.Services.AddLinks(config =>
{
    config.AddPolicy<EmployeeDto>(policy =>
    
        policy.RequireSelfLink()
            .RequireRoutedLink("all", "GetAllEmployees", _ => new { })
            .RequireRoutedLink("delete", "DeleteEmployee", x => new { id = x.Id })
            .RequireRoutedLink("update", "UpdateEmployee", x => new { id = x.Id }));
    
    config.AddPolicy<InvoiceDto>(policy =>
        policy.RequireSelfLink()
            .RequireRoutedLink("all", "GetAllInvoices", _ => new { })
            .RequireRoutedLink("delete", "DeleteInvoice", x => new { id = x.Id })
            .RequireRoutedLink("update", "UpdateInvoice", x => new { id = x.Id })
            .RequireRoutedLink("employee", "GetEmployeeForInvoice", x => new { invoiceId = x.Id }));
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();


app.Run();
