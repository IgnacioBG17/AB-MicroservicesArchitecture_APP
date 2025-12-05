using Ecommerce.Services.RewardAPI.Data;
using Ecommerce.Services.RewardAPI.Message;
using Ecommerce.Services.RewardAPI.Models;
using Ecommerce.Services.RewardAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.EmailAPI.Services
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<AppDbContext> _dboptions;

        public RewardService(DbContextOptions<AppDbContext> dboptions)
        {
            _dboptions = dboptions;
        }
        public async Task UpdateRewards(RewardsMessage rewardsMessage)
        {
            try
            {
                Rewards rewards = new()
                {
                    OrderId = rewardsMessage.OrderId,
                    RewardsActivity = rewardsMessage.RewardsActivity,
                    UserId = rewardsMessage.UserId,
                    RewardsDate = DateTime.Now
                };

                await using var _db = new AppDbContext(_dboptions);
                await _db.Rewards.AddAsync(rewards);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
