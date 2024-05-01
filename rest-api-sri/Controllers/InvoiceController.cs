using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rest_api_sri.Infrastructure;
using rest_api_sri.Models;
using rest_api_sri.Models.Commands;
using RiskFirst.Hateoas;

namespace rest_api_sri.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoiceController(
    AppDbContext dbContext,
    ILinksService linksService,
    IValidator<CreateInvoiceDto> createValidator,
    IValidator<UpdateInvoiceDto> updateValidator) : ControllerBase
{
    [HttpGet(Name = "GetAllInvoices")]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetAllInvoices()
    {
        var invoices = await dbContext.Invoices.Select(i => i.ToDto()).ToListAsync();
        foreach (var invoice in invoices)
        {
            await linksService.AddLinksAsync(invoice);
        }
        return Ok(invoices);
    }
    
    [HttpGet("{id:long}", Name = "GetInvoice")]
    public async Task<ActionResult<Invoice>> GetInvoice(long id)
    {
        var invoice = await dbContext.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound(id);
        }
        var result = invoice.ToDto();
        await linksService.AddLinksAsync(result);

        return Ok(result);
    }
    
    [HttpPost(Name = "CreateInvoice")]
    public async Task<ActionResult<Invoice>> CreateInvoice(CreateInvoiceDto createInvoiceDto)
    {
        var validationResult = await createValidator.ValidateAsync(createInvoiceDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var invoice = new Invoice
        {
            InvoiceDate = createInvoiceDto.InvoiceDate,
            Amount = createInvoiceDto.Amount,
            EmployeeId = createInvoiceDto.EmployeeId,
            CustomerName = createInvoiceDto.CustomerName,
            PaymentStatus = createInvoiceDto.PaymentStatus
        };
        dbContext.Invoices.Add(invoice);
        await dbContext.SaveChangesAsync();
        
        var result = invoice.ToDto();
        await linksService.AddLinksAsync(result);
        return CreatedAtRoute("GetInvoice", new { id = invoice.Id }, result);
    }
    
    [HttpPut("{id:long}", Name = "UpdateInvoice")]
    public async Task<ActionResult<Invoice>> UpdateInvoice(long id, UpdateInvoiceDto updateInvoiceDto)
    {
        var invoice = await dbContext.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound(id);
        }
        
        var validationResult = await updateValidator.ValidateAsync(updateInvoiceDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        invoice.InvoiceDate = updateInvoiceDto.InvoiceDate ?? invoice.InvoiceDate;
        invoice.Amount = updateInvoiceDto.Amount ?? invoice.Amount;
        invoice.EmployeeId = updateInvoiceDto.EmployeeId ?? invoice.EmployeeId;
        await dbContext.SaveChangesAsync();
        
        var result = invoice.ToDto();
        await linksService.AddLinksAsync(result);
        
        return Ok(result);
    }
    
    [HttpDelete("{id:long}", Name = "DeleteInvoice")]
    public async Task<ActionResult> DeleteInvoice(long id)
    {
        var invoice = await dbContext.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound(id);
        }
        dbContext.Invoices.Remove(invoice);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpGet("{id:long}/employee", Name = "GetInvoiceEmployee")]
    public async Task<ActionResult<Employee>> GetInvoiceEmployee(long id)
    {
        var invoice = await dbContext.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound(id);
        }
        var employee = await dbContext.Employees.FindAsync(invoice.EmployeeId);
        if (employee == null)
        {
            return NotFound(invoice.EmployeeId);
        }
        var result = employee.ToDto();
        await linksService.AddLinksAsync(result);

        return Ok(result);
    }
    
    [HttpPut("{id:long}/assign-employee/{employeeId:long}", Name = "AssignEmployeeToInvoice")]
    public async Task<ActionResult<Invoice>> AssignEmployeeToInvoice(long id, long employeeId)
    {
        var invoice = await dbContext.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound(id);
        }
        var employee = await dbContext.Employees.FindAsync(employeeId);
        if (employee == null)
        {
            return NotFound(employeeId);
        }
        invoice.EmployeeId = employeeId;
        await dbContext.SaveChangesAsync();
        
        var result = invoice.ToDto();
        await linksService.AddLinksAsync(result);
        
        return Ok(result);
    }
    
    [HttpPut("{id:long}/remove-employee", Name = "RemoveEmployeeFromInvoice")]
    public async Task<ActionResult<Invoice>> RemoveEmployeeFromInvoice(long id)
    {
        var invoice = await dbContext.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound(id);
        }
        invoice.EmployeeId = null;
        await dbContext.SaveChangesAsync();
        
        var result = invoice.ToDto();
        await linksService.AddLinksAsync(result);
        
        return Ok(result);
    }

}