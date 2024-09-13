using Battleships.Web.API.Controllers;
using Battleships.Web.DataAccess.DTOs;
using Battleships.Web.Service.GameService;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Battleships.Web.Tests.Controllers
{
    public class TestGameController
    {
        [Fact]
        public async Task Get_OnSuccess_ReturnSuccessCode200()
        {
            // Arrange
            var mockGameService = new Mock<IGameServiceRepository>();
            var mockLogger = new Mock<ILogger<GameController>>();

            // Act
            var sut = new GameController(mockGameService.Object, mockLogger.Object);
            var result = (OkObjectResult)sut.GetGrid();

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Get_OnSuccess_Invoke_Service()
        {
            // Arrange
            var mockGameService = new Mock<IGameServiceRepository>();
            var mockLogger = new Mock<ILogger<GameController>>();

            mockGameService.Setup(service => service.GetPlayerGrid()).Returns(new GameResponse());

            // Act
            var sut = new GameController(mockGameService.Object, mockLogger.Object);
            var result = sut.GetGrid();

            // Assert
            mockGameService.Verify(service => service.GetPlayerGrid(), Times.Once());
        }

        [Fact]
        public async Task Get_OnSuccess_Return_Grid()
        {
            // Arrange
            var mockGameService = new Mock<IGameServiceRepository>();
            var mockLogger = new Mock<ILogger<GameController>>();

            mockGameService.Setup(service => service.GetPlayerGrid()).Returns(new GameResponse());

            // Act
            var sut = new GameController(mockGameService.Object, mockLogger.Object);
            var result = sut.GetGrid();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeOfType<GameResponse>();
        }
    }

}
