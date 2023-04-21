using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Models;
using RapidPay.Services;

namespace RapidPay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardManagementController : ControllerBase
    {
        private readonly ILogger<CardManagementController> _logger;
        private readonly ICardManagementService _cardManagementService;

        public CardManagementController(ILogger<CardManagementController> logger,
            ICardManagementService cardManagementService)
        {
            _logger = logger;
            _cardManagementService = cardManagementService;
        }

        [HttpPost("Card")]
        [Authorize]
        public async Task<IActionResult> CreateCard([FromBody] Card card)
        {
            var result =  await _cardManagementService.CreateCardAsync(card.Number);

            return result.Error == null ? Created("", result.Result) : Conflict(result.Error);
        }

        [HttpPost("Pay")]
        [Authorize]
        public async Task<IActionResult> PayWithCard([FromBody] decimal amount)
        {
            var result = await _cardManagementService.PayAsync(amount);

            return result.Error == null ? Ok(result.Result) : Conflict(result.Error);
        }

        [HttpGet("Balance")]
        [Authorize]
        public async Task<IActionResult> GetCardBalance()
        {
            var result = await _cardManagementService.GetCardAsync();

            return result.Error == null ? Ok(result.Result) : Conflict(result.Error);
        }
    }
}