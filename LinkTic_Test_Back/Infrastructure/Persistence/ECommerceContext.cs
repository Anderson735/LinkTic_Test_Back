using LinkTic_Test_Back.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LinkTic_Test_Back.Infrastructure.Persistence
{
    public class ECommerceContext : DbContext
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
       
    }
}
