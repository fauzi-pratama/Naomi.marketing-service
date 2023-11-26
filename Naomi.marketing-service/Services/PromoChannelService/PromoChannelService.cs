using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Entities;
using static Naomi.marketing_service.Models.Request.ChannelMaterialRequest;

namespace Naomi.marketing_service.Services.PromoChannelService
{
    public class PromoChannelService : IPromoChannelService
    {
        private readonly DataDbContext _dbContext;
        public PromoChannelService(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region GetData
        public async Task<Tuple<List<PromotionChannel>, int>> GetPromotionChannel(string? searchName, int pageNo, int pageSize)
        {
            List<PromotionChannel> promoChannels = new();
            int Skip = (pageNo - 1) * pageSize;

            if (!string.IsNullOrEmpty(searchName))
            {
                promoChannels = await _dbContext.PromotionChannel.Where(x => x.ChannelName!.Trim().ToUpper().Contains(searchName.Trim().ToUpper())).ToListAsync() ?? new List<PromotionChannel>();
            }
            else
            {
                promoChannels = await _dbContext.PromotionChannel.ToListAsync() ?? new List<PromotionChannel>();
            }

            int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(promoChannels.Count) / Convert.ToDouble(pageSize)));
            promoChannels = promoChannels.OrderBy(x => x.ChannelName).Skip(Skip).Take(pageSize).ToList();
            return new Tuple<List<PromotionChannel>, int>(promoChannels ?? new List<PromotionChannel>(), totalPages);
        }
        #endregion

        #region InsertData
        public async Task<Tuple<PromotionChannel, string>> InsertPromotionChannel(PromotionChannel newPromoChannel)
        {
            bool isExist = _dbContext.PromotionChannel.Any(x => x.ChannelName!.Trim().ToUpper() == newPromoChannel.ChannelName!.Trim().ToUpper());
            if (isExist)
                return new Tuple<PromotionChannel, string>(new PromotionChannel(), string.Format("Promotion Channel name {0} is already exist", newPromoChannel.ChannelName));

            try
            {
                newPromoChannel.CreatedDate = DateTime.UtcNow;
                newPromoChannel.CreatedBy = newPromoChannel.Username;
                newPromoChannel.UpdatedDate = DateTime.UtcNow;
                newPromoChannel.UpdatedBy = newPromoChannel.Username;
                newPromoChannel.ActiveFlag = true;

                // Add entry to the DB context
                _dbContext.PromotionChannel.Add(newPromoChannel);
                await _dbContext.SaveChangesAsync();

                return new Tuple<PromotionChannel, string>(newPromoChannel, "Data has been saved");
            }
            catch (Exception ex)
            {
                return new Tuple<PromotionChannel, string>(new PromotionChannel(), ex.Message);
            }
        }
        #endregion

        #region UpdateData
        #endregion
    }
}
