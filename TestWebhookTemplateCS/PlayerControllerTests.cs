using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestWebhookTemplateCS
{
    public class PlayerControllerTests
    {
        private readonly PlayerController _controller;
        private readonly Mock<ControllerContext> _mockControllerContext;

        public PlayerControllerTests()
        {
            _controller = new PlayerController();
            _mockControllerContext = new Mock<ControllerContext>();
            _controller.ControllerContext = _mockControllerContext.Object;
        }

        [Fact]
        public async Task PlayerUpdateBalanceEndpoint_ValidPayload_ReturnsCorrectResponse()
        {
            // Arrange
            var payload = new PublisherPayload
            {
                appChargePaymentId = "TestPaymentId",
                purchaseDateAndTimeUtc = DateTime.Now,
                gameId = "TestGameId",
                playerId = "TestPlayerId",
                bundleName = "TestBundleName",
                bundleId = "TestBundleId",
                sku = "TestSku",
                priceInCents = 100,
                currency = "USD",
                priceInDollar = 1.0f,
                action = "TestAction",
                actionStatus = "TestActionStatus",
                products = new List<Product>
                {
                    new Product
                    {
                        amount = 10,
                        productId = new ProductId
                        {
                            sku = "ProductSku",
                            name = "ProductName"
                        },
                        sku = "ProductSku",
                        name = "ProductName"
                    }
                },
                tax = 0.1,
                subTotal = 0.9
            };
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload)));

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Body = memoryStream;

            // Act
            var result = await _controller.PlayerUpdateBalanceEndpoint();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(DateTime.Now.Date, result.purchaseTime.Date); // Only comparing the Date for simplicity
            Assert.Equal("<PURCHASE-ID>", result.publisherPurchaseId);
        }

        [Fact]
        public async Task PlayerUpdateBalanceEndpoint_InvalidPayload_ThrowsException()
        {
            // Arrange
            var invalidPayload = "{ invalid json }";
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidPayload)));

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Body = memoryStream;

            // Act & Assert
            await Assert.ThrowsAsync<JsonException>(() => _controller.PlayerUpdateBalanceEndpoint());
        }
    }
}
