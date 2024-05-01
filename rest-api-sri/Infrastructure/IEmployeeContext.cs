using Microsoft.EntityFrameworkCore;
using rest_api_sri.Models;

namespace rest_api_sri.Infrastructure;

public interface IEmployeeContext 
{ 
    DbSet<Employee> Employees { get; set; }
}