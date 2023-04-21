using RapidPay.Models;
using RapidPay.Repositories;

namespace RapidPay.Services
{
    public interface ICardManagementService
    {
        public Task<Response> CreateCardAsync(long cardNumber);
        public Task<Response> GetCardAsync();
        public Task<Response> PayAsync(decimal paymentAmount);
    }

    public class CardManagementService : ICardManagementService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IUFEService _UFEService;

        public CardManagementService(ICardRepository cardRepository, IUFEService UFEService)
        {
            _cardRepository = cardRepository;
            _UFEService = UFEService;
        }

        public async Task<Response> CreateCardAsync(long cardNumber)
        {
            Response response = new Response();
            var isValidCardNumber = ValidateCardNumber(cardNumber);
            if (!isValidCardNumber)
            {
                response.Error = "Cardnumber is not correct, lenght should be of 15 digits";
                return response;
            }

            var cardInDB = await _cardRepository.GetCardAsync();
            if (cardInDB != null)
            {
                response.Error = "Card is already created";
                return response;
            }

            response.Result = await _cardRepository.AddCardAsync(cardNumber);

            return response;
        }

        public async Task<Response> PayAsync(decimal paidAmount)
        {
            Response response = new Response();
            var cardInDB = await _cardRepository.GetCardAsync();
            if (cardInDB == null)
            {
                response.Error = "Card has not been created";
                return response;
            }

            var lastFee = await _UFEService.GetLastFee();

            var newBalance = cardInDB.Balance + (paidAmount * (1 + lastFee));

            response.Result = await _cardRepository.UpdateCardAsync(newBalance);

            return response;
        }

        public async Task<Response> GetCardAsync()
        {
            Response response = new Response();
            var result = await _cardRepository.GetCardAsync();
            if (result == null)
            {
                response.Error = "Card has not been created";
                return response;
            }

            response.Result = result.Balance;
            return response;
        }

        private bool ValidateCardNumber(long cardNumber)
        {
            if (cardNumber.ToString().Length != 15)
            {
                return false;
            }
            return true;
        }
    }
}
