using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using VAT_TODOLIST.Models;

namespace VAT_TODOLIST.ApplicationDBContext
{
    public class AppUserDb : DbContext
    {
        public AppUserDb(DbContextOptions<AppUserDb> options) : base(options)
        {

        }

        public DbSet<VATTodoModel> VatTodoDB { get; set; }
    }
}
