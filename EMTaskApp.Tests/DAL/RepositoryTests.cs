using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;  

using EMTaskApp.DAL;
using Microsoft.Extensions.Logging;
using Moq;

namespace EMTaskApp.Tests.DAL
{
    [TestFixture]
    public class OrderRepositoryTests
    {
        private const string TEST_FILE_PATH = "TestOrders.json";

        private Mock<ILogger<OrderRepository>> _mockLogger;
        private OrderRepository _orderRepository;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<OrderRepository>>();
            _orderRepository = new OrderRepository(_mockLogger.Object);
        }

        [Test]
        public async Task ReadFile_EmptyFile_ReturnsEmptyList()
        {
            // Arrange
            File.WriteAllText(TEST_FILE_PATH, string.Empty);
            _orderRepository.FilePath = TEST_FILE_PATH;

            // Act
            var orders = await _orderRepository.ReadFile();

            // Assert
            Assert.IsEmpty(orders);
        }

        [Test]
        public async Task ReadFile_ValidJson_ReturnsDeserializedOrders()
        {
            // Arrange
            var sampleOrders = new List<Order>() { new Order() };
            File.WriteAllText(TEST_FILE_PATH, JsonSerializer.Serialize(sampleOrders));

            // Act
            var orders = await _orderRepository.ReadFile();

            //Assert
            Assert.IsNotEmpty(orders);
            Assert.IsInstanceOf<Order>(orders.FirstOrDefault());
        }

        [Test]
        public async Task ReadFile_InvalidJson_ReturnsEmptyList_LogsError()
        {
            // Arrange
            File.WriteAllText(TEST_FILE_PATH, "Invalid JSON");
            _orderRepository.FilePath = TEST_FILE_PATH;

            // Act
            var orders = await _orderRepository.ReadFile();

            // Assert
            Assert.IsEmpty(orders);
        }

        [Test]
        public async Task ReadFile_FileNotFound_ReturnsEmptyList_LogsError()
        {
            // Arrange
            File.Move(TEST_FILE_PATH, "TestOrders_temp.json");
            _orderRepository.FilePath = TEST_FILE_PATH;

            // Act
            var orders = await _orderRepository.ReadFile();

            // Assert
            Assert.IsEmpty(orders);

            // Restore
            File.Move("TestOrders_temp.json", TEST_FILE_PATH);
        }
    }
}