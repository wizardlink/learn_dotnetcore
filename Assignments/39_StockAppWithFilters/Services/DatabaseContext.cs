using Microsoft.EntityFrameworkCore;
using Models.Entities.Order;

namespace Services;

public class DatabaseContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<BuyOrder> BuyOrder { get; set; }

    public virtual DbSet<SellOrder> SellOrder { get; set; }
}
