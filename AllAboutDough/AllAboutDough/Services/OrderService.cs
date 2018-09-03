using AllAboutDough.Repositories;
using System;
using System.Collections.Generic;
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

        public List<string> GetOrderDates(string orderSpecification)
        {
            List<string> orderDates = new List<string>();
            string[][] partInOrderSpecification = SplitCsvRowsIntoParts(orderSpecification);
            string[] firstElementInRow = SplitCsvIntoRows(orderSpecification);
            for (int i = 0; i < partInOrderSpecification.Length-1; i++)
            {
                string[] neededRowsInOrderSpecification = partInOrderSpecification[i];

                for (int j = 0; j < neededRowsInOrderSpecification.Length; j++)
                {
                    if (!orderDates.Contains(neededRowsInOrderSpecification[0]))
                    {
                        orderDates.Add(neededRowsInOrderSpecification[0]);
                    }
                }
            }
            return orderDates;
        }
    }
}
