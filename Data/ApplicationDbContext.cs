using DemoMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoMvc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) :base(options)
        {

        }
        public DbSet<Student> Students {get;set;}

        public DbSet<Employee> Employees {get;set;}

        public DbSet<DemoMvc.Models.Faculty> Faculty {get;set;} = default;
    }
}