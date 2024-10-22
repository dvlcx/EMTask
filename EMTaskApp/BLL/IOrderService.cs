using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMTaskApp.DAL;

namespace EMTaskApp.BLL
{
    public interface IOrderService
    {
        public Task<List<Order>> GetOrdersByDistrictAndDate(string cityDistrict, DateTime firstDeliveryDateTime); 
    }
}