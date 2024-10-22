using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMTaskApp.BLL;
using EMTaskApp.DAL;
using Microsoft.AspNetCore.Mvc;

namespace EMTaskApp.PL
{
    [Route("api/v1/[controller]")]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            this._orderService = orderService;
        }
        
        [HttpGet]
        public async Task<ActionResult<Order>> GetByDistrictAndDate(string district, DateTime date)
        {
            try
            {
                var result = await this._orderService.GetOrdersByDistrictAndDate(district, date);  
                _logger.LogInformation($"API used at: {DateTime.Now} by {Request?.HttpContext.Connection.RemoteIpAddress} with arguments: district = {district}, date = {date}");      
                    if (result.Count == 0)
                    {
                        return NotFound();
                    }
                return Ok(result);         
            }
            catch (Exception e)
            {
                _logger.LogError($"API throwed 500 at: {DateTime.Now} by {Request?.HttpContext.Connection.RemoteIpAddress} with arguments: district = {district}, date = {date}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}