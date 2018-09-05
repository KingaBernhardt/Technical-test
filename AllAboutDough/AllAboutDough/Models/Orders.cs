using System;
using System.ComponentModel.DataAnnotations;

namespace AllAboutDough
{
    public class Orders
    {
        private Guid orderId;
        [Key]
        public Guid OrderId
        {
            get; set;
        }
        public DateTime OrderDate { get; set; }
        public string Topping { get; set; }
        public bool IsToppingVegetarian { get; set; }

        public Orders()
        {
        }
    }
}
