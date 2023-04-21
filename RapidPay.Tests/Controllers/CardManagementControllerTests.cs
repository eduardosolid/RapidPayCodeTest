using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RapidPay.Controllers;
using RapidPay.Models;
using RapidPay.Services;
using Xunit.Sdk;

namespace RapidPay.Tests.Controllers
{
    [TestClass]
    public class CardManagementControllerTests
    {
        private static Mock<ILogger<CardManagementController>> _loggerMock;
        private static Mock<ICardManagementService> _cardManagementServiceMock;

        private CardManagementController _target;

        [TestInitialize]
        public void BeforeEach()
        {
            _loggerMock = new Mock<ILogger<CardManagementController>>();
            _cardManagementServiceMock = new Mock<ICardManagementService>();

            _target = new CardManagementController(_loggerMock.Object, _cardManagementServiceMock.Object);
        }


        [TestMethod]
        public async Task GetCardBalance_Return200Ok_WhenCardHasBeenCreatedAndCardIsReturned()
        {
            // arrange
            _cardManagementServiceMock.Setup(x => x.GetCardAsync()).ReturnsAsync(new Response() { Result = "", Error = null });

            // act
            var result = await _target.GetCardBalance();

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _cardManagementServiceMock.Verify(x => x.GetCardAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetCardBalance_Returns409Conflict_WhenSomeError()
        {
            // arrange
            _cardManagementServiceMock.Setup(x => x.GetCardAsync()).ReturnsAsync(new Response() { Result = null, Error = "Some error" });

            // act
            var result = await _target.GetCardBalance();

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ConflictObjectResult));
            _cardManagementServiceMock.Verify(x => x.GetCardAsync(), Times.Once);
        }
    }
}