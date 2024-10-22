using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMTaskApp.BLL;
using EMTaskApp.DAL;
using Moq;

namespace EMTaskApp.Tests.BLL
{
    [TestFixture]
    public class OrderServiceTest
    {
        private Mock<IRepository<Order>> _mockRepository;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IRepository<Order>>();
            _orderService = new OrderService(_mockRepository.Object);
        }

        [Test]
        public async Task GetOrdersByDistrictAndDate_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.ReadFile()).ReturnsAsync(new List<Order>());

            // Act
            var orders = await _orderService.GetOrdersByDistrictAndDate("TestDistrict", DateTime.UtcNow);

            // Assert
            Assert.IsEmpty(orders);
        }

        [Test]
        public async Task GetOrdersByDistrictAndDate_NoMatchingOrders_ReturnsEmptyList()
        {
            // Arrange
            var sampleOrders = new List<Order>()
            {
                new Order { District = "District1", Date = DateTime.UtcNow.AddDays(-1) },
                new Order { District = "District2", Date = DateTime.UtcNow }
            };
            _mockRepository.Setup(r => r.ReadFile()).ReturnsAsync(sampleOrders);

            // Act
            var orders = await _orderService.GetOrdersByDistrictAndDate("NonExistingDistrict", DateTime.UtcNow);

            // Assert
            Assert.IsEmpty(orders);
        }

        [Test]
        public async Task GetOrdersByDistrictAndDate_MatchingOrders_ReturnsFilteredList()
        {
            var date = DateTime.UtcNow;
            // Arrange
            var matchingOrder = new Order { District = "TestDistrict", Date = date};
            var sampleOrders = new List<Order>()
            {
                matchingOrder,
                new Order { District = "District2", Date = DateTime.UtcNow.AddDays(-1) }
            };
            _mockRepository.Setup(r => r.ReadFile()).ReturnsAsync(sampleOrders);

            // Act
            var orders = await _orderService.GetOrdersByDistrictAndDate("TestDistrict", date);

            // Assert
            Assert.AreEqual(1, orders.Count);
            Assert.AreEqual(matchingOrder, orders.FirstOrDefault());
        }

        [Test]
        public async Task GetOrdersByDistrictAndDate_OrdersWithinWindow_ReturnsFilteredList()
        {
            // Arrange
            var matchingOrder = new Order { District = "TestDistrict", Date = DateTime.UtcNow.AddMinutes(15) };
            var sampleOrders = new List<Order>()
            {
                matchingOrder,
                new Order { District = "TestDistrict", Date = DateTime.UtcNow.AddHours(1) },
                new Order { District = "District2", Date = DateTime.UtcNow.AddDays(-1) }
            };
            _mockRepository.Setup(r => r.ReadFile()).ReturnsAsync(sampleOrders);

            // Act
            var orders = await _orderService.GetOrdersByDistrictAndDate("TestDistrict", DateTime.UtcNow);

            // Assert
            Assert.AreEqual(1, orders.Count);
            Assert.AreEqual(matchingOrder, orders.FirstOrDefault());
        }
    }
}
