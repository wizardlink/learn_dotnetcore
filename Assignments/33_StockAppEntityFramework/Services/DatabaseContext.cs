using Microsoft.EntityFrameworkCore;
using Models.Entities.Order;

namespace Services;

public class DatabaseContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<BuyOrder> BuyOrder { get; set; }

    public DbSet<SellOrder> SellOrder { get; set; }
}
