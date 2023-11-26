using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Entities;
using static Naomi.marketing_service.Models.Request.PromotionStatusRequest;
using static Naomi.marketing_service.Models.Response.PromotionStatusResponse;

namespace Naomi.marketing_service.Services.PromoStatusService
{
    public class PromoStatusService : IPromoStatusService
    {
        private readonly DataDbContext _dbContext;
        public PromoStatusService(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region GetDate
        public async Task<List<PromotionStatus>> GetPromotionStatus(Guid id)
        {
            List<PromotionStatus> promotions = new();

            if (id == Guid.Empty)
            {
                promotions = await _dbContext.PromotionStatus.ToListAsync() ?? new List<PromotionStatus>();
            }
            else
            {
                promotions = await _dbContext.PromotionStatus.Where(x => x.Id == id).ToListAsync() ?? new List<PromotionStatus>();
            }

            return promotions;
        }

        public async Task<PromotionStatus> GetPromotionStatusByIdAsync(Guid id)
        {
            return await _dbContext.PromotionStatus.Where(x => x.Id == id).FirstOrDefaultAsync() ?? new PromotionStatus();
        }

        public async Task<PromotionStatus> GetPromotionStatusByNameAsync(string statusName)
        {
            return await _dbContext.PromotionStatus.Where(x => x.PromotionStatusName!.Trim().ToUpper().Contains(statusName.Trim().ToUpper()) || 
                                                               x.PromotionStatusKey!.Trim().ToUpper().Contains(statusName.Trim().ToUpper()))
                                                   .FirstOrDefaultAsync() ?? new PromotionStatus();
        }

        public async Task<List<RespondPromotionStatusCount>> GetPromotionStatusCount()
        {
            List<PromotionHeader> viewTable = await _dbContext.PromotionHeader.ToListAsync() ?? new List<PromotionHeader>();
            List<PromotionStatus> statuses = await _dbContext.PromotionStatus.ToListAsync() ?? new List<PromotionStatus>();
            List<RespondPromotionStatusCount> counts = new();

            foreach (PromotionStatus status in statuses)
            {
                RespondPromotionStatusCount count = new()
                {
                    StatusId = status.Id,
                    StatusKey = status.PromotionStatusKey,
                    StatusName = status.PromotionStatusName,
                    StatusCount = viewTable.Count(x => x.PromotionStatusId == status.Id)
                };

                counts.Add(count);
            }

            return counts;
        }
        #endregion

        #region InsertData
        public async Task<Tuple<PromotionStatus, string>> InsertPromotionStatus(PromotionStatus newPromoStatus)
        {
            bool isExist = _dbContext.PromotionStatus.Any(x => x.PromotionStatusKey!.Trim().ToUpper() == newPromoStatus.PromotionStatusKey!.Trim().ToUpper());
            if (isExist)
                return new Tuple<PromotionStatus, string>(new PromotionStatus(), string.Format("Promotion Status key {0} is already exist", newPromoStatus.PromotionStatusKey));

            try
            {
                newPromoStatus.CreatedDate = DateTime.UtcNow;
                newPromoStatus.CreatedBy = newPromoStatus.Username;
                newPromoStatus.UpdatedDate = DateTime.UtcNow;
                newPromoStatus.UpdatedBy = newPromoStatus.Username;
                newPromoStatus.ActiveFlag = true;

                // Add entry to the DB context.
                _dbContext.PromotionStatus.Add(newPromoStatus);
                await _dbContext.SaveChangesAsync();

                return new Tuple<PromotionStatus, string>(newPromoStatus, "Data has been saved");
            }
            catch (Exception ex)
            {
                return new Tuple<PromotionStatus, string>(new PromotionStatus(), ex.Message);
            }
        }
        #endregion

        #region UpdateData
        #endregion
    }
}
