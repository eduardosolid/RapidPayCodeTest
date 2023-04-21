using Microsoft.EntityFrameworkCore;
using RapidPay.Models;
using System;

namespace RapidPay.Repositories
{
    public interface ICardRepository
    {
        public Task<bool> AddCardAsync(long cardNumber);
        public Task<Card> GetCardAsync();
        public Task<bool> UpdateCardAsync(decimal newBalance);
    }
    public class CardRepository : ICardRepository
    {
        public async Task<bool> AddCardAsync(long cardNumber)
        {
            using (var context = new RapidPayDBContext())
            {
                context.Cards.Add(new Card() { 
                     Id = Guid.NewGuid(),
                      Balance = 0,
                       Number = cardNumber
                });
                await context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<Card> GetCardAsync()
        {
            using (var context = new RapidPayDBContext())
            {
                var card = await context.Cards.AsNoTracking().FirstOrDefaultAsync();
                return card;
            }
        }

        public async Task<bool> UpdateCardAsync(decimal newBalance)
        {
            using (var context = new RapidPayDBContext())
            {
                var card = context.Cards.FirstOrDefault();
                card.Balance = newBalance;
                await context.SaveChangesAsync();
            }
            return true;
        }
    }
}
