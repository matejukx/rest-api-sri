using Microsoft.EntityFrameworkCore;
using rest_api_sri.Models;

namespace rest_api_sri.Infrastructure;

public class AppDbContext : DbContext
{
   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
   {
   }

   public DbSet<Employee> Employees { get; set; }
   
   public DbSet<Invoice> Invoices { get; set; }
}