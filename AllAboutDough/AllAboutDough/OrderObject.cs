using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllAboutDough
{
    public class OrderTransferObject
    {
        public List<Order> Orders { get; set; }
    }

    public class Order
    {
        public string OrderDate { get; set; }
        public List<string> Toppings { get; set; }
    }
}
