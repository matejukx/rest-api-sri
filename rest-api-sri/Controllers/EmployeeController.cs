using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rest_api_sri.Infrastructure;
using rest_api_sri.Models;
using rest_api_sri.Models.Commands;
using RiskFirst.Hateoas;

namespace rest_api_sri.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController(
    AppDbContext dbContext, 
    ILinksService linksService,
    IValidator<CreateEmployeeDto> createValidator,
    IValidator<UpdateEmployeeDto> updateValidator) : ControllerBase
{
    [HttpPost("seed")]
    public async Task<IActionResult> SeedTestData()
    {
        var employees = new List<Employee>
        {
            new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1980, 1, 1),
                Job = "Developer"
            },
            new Employee
            {
                FirstName = "Jane",
                LastName = "Smith",
                BirthDate = new DateTime(1985, 2, 2),
                Job = "Designer"
            }
        };
        
        dbContext.Employees.AddRange(employees);
        await dbContext.SaveChangesAsync();
        
        var invoices = new List<Invoice>
        {
            new Invoice
            {
                InvoiceDate = new DateTime(2021, 1, 1),
                Amount = 100.0,
                PaymentStatus = "Pending",
                EmployeeId = employees[0].Id,
                CustomerName = "ABC Corp."
            },
            new Invoice
            {
                InvoiceDate = new DateTime(2021, 2, 2),
                Amount = 200.0,
                PaymentStatus = "Paid",
                EmployeeId = employees[1].Id,
                CustomerName = "ACME Inc."
            }
        };
        
        dbContext.Invoices.AddRange(invoices);
        await dbContext.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpGet(Name = "GetAllEmployees")]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
    {
        var employees = await dbContext.Employees.Select(e => e.ToDto()).ToListAsync();
        foreach (var employee in employees)
        {
            await linksService.AddLinksAsync(employee);
        }
        return Ok(employees);
    }
    
    [HttpGet("{id:long}", Name = "GetEmployee")]
    public async Task<ActionResult<Employee>> GetEmployee(long id)
    {
        var employee = await dbContext.Employees
            .Include(x => x.Invoices)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (employee == null)
        {
            return NotFound(id);
        }
        var result = employee.ToDto();
        await linksService.AddLinksAsync(result);
        foreach (var invoice in result.Invoices)
        {
            await linksService.AddLinksAsync(invoice);
        }

        return Ok(result);
    }
    
    [HttpPost(Name = "CreateEmployee")]
    public async Task<ActionResult<Employee>> CreateEmployee(CreateEmployeeDto createEmployeeDto, CancellationToken token)
    {
        var validationResult = await createValidator.ValidateAsync(createEmployeeDto, token);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var employee = new Employee
        {
            FirstName = createEmployeeDto.FirstName,
            LastName = createEmployeeDto.LastName,
            BirthDate = createEmployeeDto.BirthDate,
            Job = createEmployeeDto.Job
        };
        dbContext.Employees.Add(employee);
        await dbContext.SaveChangesAsync();
        return CreatedAtRoute("GetEmployee", new { id = employee.Id }, employee);
    }
    
    [HttpPut("{id:long}", Name = "UpdateEmployee")]
    public async Task<ActionResult<Employee>> UpdateEmployee(long id, UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = await dbContext.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound(id);
        }
        
        var validationResult = await updateValidator.ValidateAsync(updateEmployeeDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        employee.FirstName = updateEmployeeDto.FirstName ?? employee.FirstName;
        employee.LastName = updateEmployeeDto.LastName ?? employee.LastName;
        employee.BirthDate = updateEmployeeDto.BirthDate ?? employee.BirthDate;
        employee.Job = updateEmployeeDto.Job ?? employee.Job;
        await dbContext.SaveChangesAsync();
        
        var result = employee.ToDto();
        await linksService.AddLinksAsync(result);
        
        return Ok(result);
    }
    
    [HttpDelete("{id:long}", Name = "DeleteEmployee")]
    public async Task<ActionResult<Employee>> DeleteEmployee(long id)
    {
        var employee = await dbContext.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound(id);
        }
        dbContext.Employees.Remove(employee);
        await dbContext.SaveChangesAsync();
        
        
        return NoContent();
    }
    
    [HttpGet("{id:long}/invoices", Name = "GetEmployeeInvoices")]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetEmployeeInvoices(long id)
    {
        var employee = await dbContext.Employees.Include(e => e.Invoices).FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            return NotFound(id);
        }
        var invoices = employee.Invoices.Select(i => i.ToDto()).ToList();
        foreach (var invoice in invoices)
        {
            await linksService.AddLinksAsync(invoice);
        }
        return Ok(invoices);
    }

}