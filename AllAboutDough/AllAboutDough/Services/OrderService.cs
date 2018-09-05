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

        public OrderService(OrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public string[] SplitCsvIntoRows(string orderSpecification)
        {
            string[] rowsInOrderSpecification = orderSpecification.Split("\r\n");
            string[] neededRowsInOrderSpecification = new string[rowsInOrderSpecification.Length - 1];
            for (int i = 1; i < rowsInOrderSpecification.Length - 1; i++)
            {
                neededRowsInOrderSpecification[i - 1] += rowsInOrderSpecification[i];
            }
            return neededRowsInOrderSpecification;
        }
        //string a = input.Substring(0, 18);
        //string b = input.Substring(19, neededRowsinOrderSpecification.Length())

        public string[][] SplitCsvRowsIntoParts(string orderSpecification)
        {
            string[] neededRowsInOrderSpecification = SplitCsvIntoRows(orderSpecification);
            string[][] partInOrderSpecification = new string[neededRowsInOrderSpecification.Length][];
            for (int i = 0; i < neededRowsInOrderSpecification.Length - 1; i++)
            {
                partInOrderSpecification[i] = neededRowsInOrderSpecification[i].Split(",");
            }
            return partInOrderSpecification;
        }

        public List<DateTime> GetOrderDates(string orderSpecification)
        {
            List<DateTime> orderDates = new List<DateTime>();
            string[][] partInOrderSpecification = SplitCsvRowsIntoParts(orderSpecification);
            for (int i = 0; i < partInOrderSpecification.Length - 1; i++)
            {
                orderDates.Add(Convert.ToDateTime(partInOrderSpecification[i][0]));
            }
            return orderDates;
        }

        public List<string> GetToppings(string orderSpecification)
        {
            List<string> pizzaToppings = new List<string>();
            string[][] partsInOrderSpecification = SplitCsvRowsIntoParts(orderSpecification);
            for (int i = 0; i < partsInOrderSpecification.Length - 1; i++)
            {
                for (int j = 0; j < partsInOrderSpecification[i].Length; j++)
                {
                    if (!pizzaToppings.Contains(partsInOrderSpecification[i][j]))
                    {
                        pizzaToppings.Add(partsInOrderSpecification[i][j]);
                    }
                }
                pizzaToppings.Remove(partsInOrderSpecification[i][0]);
            }
            return pizzaToppings;
        }

        public List<string> CollectNonVegetarianToppings(List<string> pizzaToppings)
        {
            List<string> nonVegaToppings = new List<string>();
            foreach (var toppingItem in pizzaToppings)
            {
                if (toppingItem.Equals("salami") || toppingItem.Equals("ham") || toppingItem.Equals("anchovies"))
                {
                    nonVegaToppings.Add(toppingItem);
                }
            }
            return nonVegaToppings;
        }

        public string[] ConcatToppingsToString(string orderSpecification)
        {
            string[][] partsInOrderSpecification = SplitCsvRowsIntoParts(orderSpecification);
            string[] toppings = new string[partsInOrderSpecification.GetLength(0) - 1];
            string temp = "";
            for (int i = 0; i < partsInOrderSpecification.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < partsInOrderSpecification[i].Length; j++)
                {
                    temp += partsInOrderSpecification[i][j] + " ";
                    toppings[i] = temp;
                }
                temp = "";
            }
            return toppings;
        }

        public void AddField(string orderSpecification)
        {
            Orders orders = new Orders();
            foreach (var item in GetOrderDates(orderSpecification))
            {
                orders.OrderDate = item;
            }
            foreach (var item in ConcatToppingsToString(orderSpecification))
            {
                orders.Topping = item;
            }
            orderRepository.Create(orders);
        }

        public void CreateEntity(string orderSpecification)
        {
            List<DateTime> orderDates = GetOrderDates(orderSpecification);
            string[] toppings = ConcatToppingsToString(orderSpecification);
        
            for (int i = 0; i < toppings.Length; i++)
            {
                Orders orders = new Orders()
                {
                    OrderDate = orderDates[i],
                    Topping = toppings[i],
                    IsToppingVegetarian = !(CollectNonVegetarianToppings(GetToppings(orderSpecification)).Any(toppings[i].Contains))
                };
                orderRepository.Create(orders);
            }
        }
    }
}
