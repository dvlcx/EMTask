using EMTaskApp.DAL;

namespace EMTaskApp.BLL
{
    public class OrderService : IOrderService
    {
        private const double HALF_HOUR = 0.5d;
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        public async Task<List<Order>> GetOrdersByDistrictAndDate(string cityDistrict, DateTime firstDeliveryDateTime) 
        {
            List<Order> data = await this._orderRepository.ReadFile();

            var result = data.Where(x => x.District == cityDistrict && x.Date >= firstDeliveryDateTime && x.Date <= firstDeliveryDateTime.AddHours(HALF_HOUR)).ToList();

            return result;
        }
    }
}