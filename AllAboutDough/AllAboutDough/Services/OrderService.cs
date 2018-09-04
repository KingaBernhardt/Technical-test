using AllAboutDough.Repositories;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AllAboutDough.Services
{
    public class OrderService
    {
        private OrderRepository orderRepository;

        public OrderService()
        {
        }

        public OrderService(OrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public string[] SplitCsvIntoRows(string orderSpecification)
        {
            string[] rowsInOrderSpecification = orderSpecification.Split("\r\n");
            string[] neededRowsInOrderSpecification = new string[rowsInOrderSpecification.Length-1];
            for (int i = 1; i < rowsInOrderSpecification.Length-1; i++)
            {
                neededRowsInOrderSpecification[i - 1] += rowsInOrderSpecification[i];
            }
            return neededRowsInOrderSpecification;
        }

        public string[][] SplitCsvRowsIntoParts(string orderSpecification)
        {
            string[] neededRowsInOrderSpecification = SplitCsvIntoRows(orderSpecification);
            string[][] partInOrderSpecification = new string[neededRowsInOrderSpecification.Length][];
            for (int i = 0; i < neededRowsInOrderSpecification.Length-1; i++)
            {
                partInOrderSpecification[i] = neededRowsInOrderSpecification[i].Split(",");
            }
            return partInOrderSpecification;
        }

        public List<DateTime> GetOrderDates(string orderSpecification)
        {
            List<DateTime> orderDates = new List<DateTime>();
            string[][] partInOrderSpecification = SplitCsvRowsIntoParts(orderSpecification);
            for (int i = 0; i < partInOrderSpecification.Length-1; i++)
            {
                orderDates.Add(Convert.ToDateTime(partInOrderSpecification[i][0]));
            }
            return orderDates;
        }
    }
}
