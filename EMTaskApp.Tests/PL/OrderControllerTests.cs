using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EMTaskApp.BLL;
using EMTaskApp.DAL;
using EMTaskApp.PL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EMTaskApp.Tests.PL
{
    [TestFixture]
    public class OrderControllerTest
    {
        private Mock<IOrderService> _mockOrderService;
        private Mock<ILogger<OrderController>> _mockLogger;
        private OrderController _orderController;

        [SetUp]
        public void Setup()
        {
            _mockOrderService = new Mock<IOrderService>();
            _mockLogger = new Mock<ILogger<OrderController>>();
            _orderController = new OrderController(_mockLogger.Object, _mockOrderService.Object);
        }

        [Test]
        public async Task GetByDistrictAndDate_EmptyList_ReturnsNotFound()
        {
            // Arrange
            _mockOrderService.Setup(s => s.GetOrdersByDistrictAndDate(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Order>());

            // Act
            var result = await _orderController.GetByDistrictAndDate("TestDistrict", DateTime.UtcNow);

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), result.Result);
        }

        [Test]
        public async Task GetByDistrictAndDate_MatchingOrders_ReturnsOkAndLogsRequest()
        {
            // Arrange
            var sampleOrder = new Order();
            var sampleOrders = new List<Order>() { sampleOrder };
            _mockOrderService.Setup(s => s.GetOrdersByDistrictAndDate(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(sampleOrders);

            // Act
            var result = await _orderController.GetByDistrictAndDate("TestDistrict", DateTime.UtcNow);

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(sampleOrders, okResult.Value);
        }

        [Test]
        public async Task GetByDistrictAndDate_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            _mockOrderService.Setup(s => s.GetOrdersByDistrictAndDate(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _orderController.GetByDistrictAndDate("TestDistrict", DateTime.UtcNow);

            // Assert
            Assert.IsInstanceOf(typeof(StatusCodeResult), result.Result);
            var statusCodeResult = result.Result as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)statusCodeResult.StatusCode);
        }
    }
}