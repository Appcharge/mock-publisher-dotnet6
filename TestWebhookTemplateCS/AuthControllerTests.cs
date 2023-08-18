using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebhookTemplateCS.controllers.auth.methods;
using Xunit.Abstractions;

namespace TestWebhookTemplateCS
{
    public class AuthControllerTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Mock<ISecretsService> _mockSecretsService;
        private readonly Mock<IFacebookAuthService> _mockFacebookAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _mockSecretsService = new Mock<ISecretsService>();
            _mockFacebookAuthService = new Mock<IFacebookAuthService>();
            _controller = new AuthController(_mockSecretsService.Object, _mockFacebookAuthService.Object);

            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task PlayerAuth_ValidAuth_ReturnsValidAuthResponse()
        {
            // Arrange
            _mockSecretsService.Setup(x => x.getFacebookSecret()).Returns("TestSecret");
            
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"AuthMethod\":\"facebook\",\"Token\":\"TestToken\",\"AppId\":\"TestAppId\"}"));
            _controller.ControllerContext.HttpContext.Request.Body = memoryStream;
            
            // Mock FacebookAuth
            _mockFacebookAuthService.Setup(f => f.Authenticate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new AuthResult(true, "1234567890"));

            // Act
            var result = await _controller.PlayerAuth();
            
            _testOutputHelper.WriteLine(result.Value.ToString());

            // Assert
            var response = Assert.IsType<AuthResponse>(result.Value);

            Assert.Equal("valid", response.Status);
            Assert.Equal("<player id>", response.PublisherPlayerId);
        }

        [Fact]
        public async Task PlayerAuth_InvalidAuth_ReturnsBadRequest()
        {
            // Arrange
            _mockSecretsService.Setup(x => x.getFacebookSecret()).Returns("TestSecret");

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"AuthMethod\":\"unknown\",\"Token\":\"TestToken\",\"AppId\":\"TestAppId\"}"));
            _controller.ControllerContext.HttpContext.Request.Body = memoryStream;

            // Act
            var result = await _controller.PlayerAuth();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("login failed", badRequestResult.Value);
        }
    }
}
