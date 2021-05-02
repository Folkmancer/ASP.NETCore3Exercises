using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly SportsStoreDbContext context;

        public EFOrderRepository(SportsStoreDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Order> Orders => context.Orders
            .Include(order => order.Lines)
            .ThenInclude(line => line.Product);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(line => line.Product));

            if (order.OrderId == 0)
            {
                context.Orders.Add(order);
            }

            context.SaveChanges();
        }
    }
}