using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace EMTaskApp.DAL
{
    public class OrderRepository  : IRepository<Order>
    {
        private readonly ILogger<OrderRepository> _logger;
        public string FilePath { get; set; } = "DAL/Data.json"; //defenitely not the best way to do this, but for the sake of tests...
        
        public OrderRepository(ILogger<OrderRepository> logger)
        {
            _logger = logger;
        }

        public async Task<List<Order>> ReadFile()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    _logger.LogError($"Deserialization error: File not Found: {FilePath}");
                    return new List<Order>();
                }
                using FileStream stream = File.OpenRead(FilePath);
                return await JsonSerializer.DeserializeAsync<List<Order>>(stream) ?? new List<Order>();
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Deserialization error: {ex.Message}");
                return new List<Order>();
            }
        }
    }
}