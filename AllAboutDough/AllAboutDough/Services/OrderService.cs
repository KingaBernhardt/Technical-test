using AllAboutDough.Repositories;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
            string[] neededRowsInOrderSpecification = new string[rowsInOrderSpecification.Length - 1];
            for (int i = 1; i < rowsInOrderSpecification.Length - 1; i++)
            {
                neededRowsInOrderSpecification[i - 1] += rowsInOrderSpecification[i];
            }
            return neededRowsInOrderSpecification;
        }

        public void CreateUpdatedCsv(List<string> orderDates, List<string> toppings, List<bool> isVegetarian, string orderSpecification)
        {
            orderDates = GetOrderDates(orderSpecification);
            toppings = ConcatToppingsToString(orderSpecification);
            isVegetarian = DecideBooleanValue(orderSpecification);
            using (StreamWriter sw = File.CreateText("../../../orderUpdated.csv"))
            {
                for (int i = 0; i < toppings.Count; i++)
                {
                    var line = String.Format("{0},{1},{2}", orderDates[i], toppings[i], isVegetarian[i]);
                    sw.WriteLine(line);
                }
            }
        }

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

        public List<string> GetOrderDates(string orderSpecification)
        {
            List<DateTime> orderDates = new List<DateTime>();
            List<string> orderDatesString = new List<string>();
            string[][] partInOrderSpecification = SplitCsvRowsIntoParts(orderSpecification);
            for (int i = 0; i < partInOrderSpecification.Length - 1; i++)
            {
                orderDatesString.Add(partInOrderSpecification[i][0]);
                //orderDates.Add(Convert.ToDateTime(partInOrderSpecification[i][0]));
            }
            //return orderDates;
            return orderDatesString;
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

        public List<string> ConcatToppingsToString(string orderSpecification)
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
            toppings.ToList();
            return toppings.ToList();
        }
        public List<bool> DecideBooleanValue(string orderSpecification)
        {
            List<bool> isVega = new List<bool>();
            List<string> toppings = ConcatToppingsToString(orderSpecification);
            string temp = "";
            for (int i = 0; i < toppings.Count; i++)
            {
                if ((CollectNonVegetarianToppings(GetToppings(orderSpecification)).Any(toppings[i].Contains)))
                {
                    isVega.Add(false);
                }
                else
                {
                    isVega.Add(true);
                }
                temp = "";
            }
            return isVega;
        }

        /*public void AddField(string orderSpecification)
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
            List<string> toppings = ConcatToppingsToString(orderSpecification);
        
            for (int i = 0; i < toppings.Count; i++)
            {
                Orders orders = new Orders()
                {
                    OrderDate = orderDates[i],
                    Topping = toppings[i],
                    IsToppingVegetarian = !(CollectNonVegetarianToppings(GetToppings(orderSpecification)).Any(toppings[i].Contains))
                };
                orderRepository.Create(orders);
            }
        }*/
    }
}
