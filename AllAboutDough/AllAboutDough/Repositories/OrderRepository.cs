using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllAboutDough.Repositories
{
    public class OrderRepository
    {
        private OrderDbContext orderDbContext;

        public void OrderDbContext(OrderDbContext orderDbContext)
        {
            this.orderDbContext = orderDbContext;
        }

        public void Create(Orders orders)
        {
            orderDbContext.Orders.Add(orders);
            orderDbContext.SaveChanges();
        }

        public void Delete(Orders orders)
        {
            orderDbContext.Orders.Remove(orders);
            orderDbContext.SaveChanges();
        }

        public List<Orders> Read()
        {
            return orderDbContext.Orders.ToList();
        }

        public void Update(Orders matrix)
        {
            orderDbContext.Orders.Update(matrix);
            orderDbContext.SaveChanges();
        }
    }
}
