using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMTaskApp.DAL
{
    public class Order
    {
        
        public Guid Id { get; set; }
        public float Weight { get; set; }
        public string District { get; set; }
        public DateTime Date { get; set; }
    }
}