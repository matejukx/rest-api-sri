using Microsoft.EntityFrameworkCore;
using rest_api_sri.Models;

namespace rest_api_sri.Infrastructure;

public interface IInvoiceContext
{
    DbSet<Invoice> Invoices { get; set; }
}